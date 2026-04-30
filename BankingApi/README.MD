# 🏦 Banking REST API

## 📌 Overview

This project is an **ASP.NET Core Web API** that implements a secure, enterprise-level **Banking Platform REST API** using **Dapper**, **SQL Server**, and **JWT Authentication**.

The platform allows customers to register, open bank accounts, and perform financial transactions — deposits, withdrawals, and transfers — through authenticated API endpoints.

The application supports resource-level authorization, SQL transactions for atomicity, structured logging, global exception handling, input validation, and follows a clean layered architecture.

---

## 🎯 Purpose

The main goals of this project are to:

- Build a **secure REST API** for a banking platform
- Implement **JWT Authentication** with claims-based user identification
- Apply **resource-level authorization** — customers can only manage their own accounts
- Enforce **business rules** across the full transaction lifecycle
- Guarantee **atomicity** using SQL transactions — all or nothing
- Follow a clean layered architecture with **Controllers**, **Services**, and **Repositories**
- Practice realistic enterprise-level API development patterns

---

## 🧠 Concepts Practiced

- ASP.NET Core Web API architecture
- SQL Server integration with raw SQL queries
- Dapper for data access (no ORM overhead)
- Repository pattern with interface abstraction
- Service layer for business logic
- DTOs for request/response separation (never exposing models directly)
- FluentValidation for input validation
- JWT token generation with claims
- JWT-based authentication and authorization
- Resource-level authorization (ownership checks)
- SQL transactions for atomic banking operations
- Soft delete pattern using `IsActive`
- Immutable audit log for transactions (no UpdatedAt, no deletes)
- Global exception handling via custom middleware
- Structured logging with Serilog

---

## 🛠 Tech Stack

| Technology | Usage |
|---|---|
| **C# / .NET 10** | Core language and runtime |
| **ASP.NET Core Web API** | API framework |
| **SQL Server** | Relational database |
| **Dapper** | Lightweight data access |
| **JWT Bearer Authentication** | Stateless authentication |
| **BCrypt.Net** | Password hashing |
| **FluentValidation** | Input validation |
| **Serilog** | Structured logging |
| **Postman** | API testing |

---

## 📂 Project Structure

```
BankingApi/
├── Controllers/
│   ├── AuthController.cs
│   ├── AccountsController.cs
│   └── TransactionsController.cs
├── Services/
│   ├── Interfaces/
│   │   ├── IAuthService.cs
│   │   ├── IAccountService.cs
│   │   └── ITransactionService.cs
│   ├── AuthService.cs
│   ├── AccountService.cs
│   └── TransactionService.cs
├── Repositories/
│   ├── Interfaces/
│   │   ├── ICustomerRepository.cs
│   │   ├── IAccountRepository.cs
│   │   └── ITransactionRepository.cs
│   ├── CustomerRepository.cs
│   ├── AccountRepository.cs
│   └── TransactionRepository.cs
├── Models/
│   ├── Customer.cs
│   ├── Account.cs
│   └── Transaction.cs
├── DTOs/
│   ├── Auth/
│   │   ├── RegisterDto.cs
│   │   ├── LoginDto.cs
│   │   └── AuthResponseDto.cs
│   ├── Account/
│   │   ├── CreateAccountDto.cs
│   │   └── AccountResponseDto.cs
│   └── Transaction/
│       ├── DepositDto.cs
│       ├── WithdrawDto.cs
│       ├── TransferDto.cs
│       └── TransactionResponseDto.cs
├── Validators/
│   ├── RegisterDtoValidator.cs
│   ├── LoginDtoValidator.cs
│   ├── CreateAccountDtoValidator.cs
│   ├── DepositDtoValidator.cs
│   ├── WithdrawDtoValidator.cs
│   └── TransferDtoValidator.cs
├── Middleware/
│   └── ExceptionMiddleware.cs
├── Data/
│   └── SqlConnectionFactory.cs
├── Program.cs
└── appsettings.json
```

---

## ⚙️ Features

### 🔑 Authentication
- Register new customers with hashed passwords using **BCrypt**
- Login with email and password
- JWT token generation with claims: `CustomerId`, `Name`, `Email`
- Token expiry: 8 hours
- Protected endpoints using **Bearer Token Authentication**
- Duplicate emails are rejected at registration
- Login returns the same error message for wrong email and wrong password — preventing user enumeration attacks

### 🏦 Account Management
- Customers can open `Checking` or `Savings` accounts
- Accounts always start with a balance of **0€**
- Soft delete — accounts are never permanently deleted (`IsActive = 0`)
- Accounts with remaining balance cannot be closed
- Customers can only view and manage their own accounts

### 💸 Transaction Management
- **Deposit** — add funds to your own account
- **Withdraw** — remove funds from your own account
- **Transfer** — send funds from your account to any other account
- Transactions are **immutable** — no updates, no deletes, ever
- Failed operations are recorded with `Status = 'Failed'`
- All balance changes happen inside **SQL transactions** — atomicity guaranteed
- Customers can view the full transaction history of their own accounts

---

## 🗃 Database Design

### Tables

| Table | Description |
|---|---|
| `Customers` | Registered users of the platform |
| `Accounts` | Bank accounts owned by customers |
| `Transactions` | Immutable record of all financial operations |

### Transaction Logic

| Type | FromAccountId | ToAccountId |
|---|---|---|
| Deposit | NULL | ✅ |
| Withdrawal | ✅ | NULL |
| Transfer | ✅ | ✅ |

### Key Constraints
- `Balance >= 0` — enforced at database level
- `Amount > 0` — enforced at database level
- `FromAccountId OR ToAccountId` must not both be NULL
- `AccountType` must be `Checking` or `Savings`
- `Email` is unique per customer
- `AccountNumber` is unique per account

### Indexes
- `IX_Accounts_CustomerId` — fast lookup of customer accounts
- `IX_Transactions_FromAccountId` — fast lookup of outgoing transactions
- `IX_Transactions_ToAccountId` — fast lookup of incoming transactions
- `IX_Transactions_CreatedAt` — chronological ordering

---

## ✅ Business Rules

### Authentication
- Passwords are never stored as plain text — hashed using **BCrypt**
- Only active customers (`IsActive = 1`) can log in
- Duplicate emails are rejected during registration
- Login error messages never reveal whether the email exists or not

### Accounts
- Customers can only access their own accounts
- Accounts always open with a balance of 0€
- An account with remaining balance cannot be deactivated
- Soft delete is used — `IsActive = 0`

### Transactions
- Withdrawal and Transfer require sufficient balance
- Transfer `FromAccountId` must belong to the authenticated customer
- Transfer `ToAccountId` can belong to any customer
- Cannot transfer to the same account
- All balance updates and transaction inserts happen atomically inside a SQL transaction
- Transactions are never modified or deleted — immutable audit log

---

## 🔐 Authorization

### Resource-Level Authorization

Every protected operation checks that the resource belongs to the authenticated customer **before** executing any business logic. The `CustomerId` is always extracted from the **JWT token** — never from the request body or URL parameters.

```csharp
private int GetCustomerId() =>
    int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
```

This means a customer cannot access, modify, or perform transactions on accounts that do not belong to them — even if they know the account ID.

---

## 🔗 Endpoints

### 🔑 Auth

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `POST` | `/api/auth/register` | ❌ | Register a new customer |
| `POST` | `/api/auth/login` | ❌ | Login and receive JWT token |

### 🏦 Accounts

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `GET` | `/api/accounts` | ✅ | Get all accounts of the authenticated customer |
| `GET` | `/api/accounts/{accountId}` | ✅ | Get a specific account (owner only) |
| `POST` | `/api/accounts` | ✅ | Open a new account |
| `DELETE` | `/api/accounts/{accountId}` | ✅ | Deactivate an account (owner only, balance must be 0) |

### 💸 Transactions

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `GET` | `/api/transactions/{accountId}` | ✅ | Get transaction history of an account (owner only) |
| `POST` | `/api/transactions/deposit` | ✅ | Deposit funds into your account |
| `POST` | `/api/transactions/withdraw` | ✅ | Withdraw funds from your account |
| `POST` | `/api/transactions/transfer` | ✅ | Transfer funds to any account |

---

## ✔️ Validation

All inputs are validated using **FluentValidation** before reaching the service layer.

| DTO | Rules |
|---|---|
| `RegisterDto` | FirstName/LastName not empty, Email valid format, Password ≥ 8 chars, PhoneNumber optional |
| `LoginDto` | Email valid format, Password not empty |
| `CreateAccountDto` | AccountType must be `Checking` or `Savings` |
| `DepositDto` | Amount > 0 |
| `WithdrawDto` | Amount > 0 |
| `TransferDto` | Amount > 0, FromAccountId ≠ ToAccountId |

---

## 🌊 Request Flow

```
HTTP Request
    ↓
ExceptionMiddleware       ← catches all unhandled exceptions, returns clean JSON
    ↓
JWT Authentication        ← validates Bearer token
    ↓
Authorization             ← checks [Authorize] attribute
    ↓
FluentValidation          ← validates request body
    ↓
Controller                ← extracts CustomerId from JWT, calls Service
    ↓
Service                   ← business logic + ownership check
    ↓
Repository                ← Dapper SQL queries
    ↓
SQL Server
```

---

## 🧪 Testing

The API is tested using **Postman**.

For protected endpoints, include the JWT token in the request header:

```http
Authorization: Bearer YOUR_TOKEN_HERE
```

### Suggested Testing Flow

1. `POST /api/auth/register` — Create a customer account
2. `POST /api/auth/login` — Get JWT token
3. `POST /api/accounts` — Open a Checking account
4. `POST /api/accounts` — Open a Savings account
5. `POST /api/transactions/deposit` — Deposit funds
6. `POST /api/transactions/withdraw` — Withdraw funds
7. `POST /api/transactions/transfer` — Transfer between accounts
8. `GET /api/transactions/{accountId}` — View transaction history
9. `GET /api/accounts/{accountId}` — Verify balances
10. *(Register a second customer and test cross-account authorization)*
11. `GET /api/accounts/{id}` with foreign token → expect `403 Forbidden`
12. `POST /api/transactions/transfer` from foreign account → expect `403 Forbidden`

---

## 🔒 Security Highlights

- Passwords hashed with **BCrypt** — never stored as plain text
- JWT tokens expire after **8 hours**
- `CustomerId` always extracted from JWT — never trusted from client input
- Uniform error messages on login — no user enumeration possible
- Resource-level ownership checks on every protected operation
- Database-level constraints as a last line of defense (`CHECK`, `UNIQUE`, `FK`)
- Global exception middleware returns clean JSON — no stack traces exposed to clients