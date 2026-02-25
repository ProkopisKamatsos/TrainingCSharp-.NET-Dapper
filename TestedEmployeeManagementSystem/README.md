# Employee Management System

## 📌 Overview

This project is a **C# Console Application** that implements a complete **Employee Management System** using **ADO.NET**, **Dapper**, and **SQL Server**.

It was developed as a major practice project to simulate real-world employee, department, and project management scenarios using a layered architecture, **with a strong focus on clean code and unit testing**.

---

## 🎯 Purpose

The main goals of this project are to:

- Build a database-driven console application
- Perform **CRUD operations** using **Dapper**
- Apply the **Repository** and **Service** patterns
- Work directly with **SQL Server**
- Manage entity relationships in a real-world scenario
- Write **unit tests** for business logic and repositories
- Practice **mocking dependencies** and **test isolation**

---

## 🧠 Concepts Practiced

- ADO.NET database connections
- Dapper and Dapper Plus
- Repository pattern
- Service layer abstraction
- SQL Server integration
- Console menu-based navigation
- One-to-many relationships (Employee–Department)
- Many-to-many relationships (Employee–Project)
- Soft delete implementation
- **Unit Testing with xUnit**
- **Arrange–Act–Assert (AAA) pattern**
- **Mocking dependencies with Moq**
- **In-memory (Fake) repositories for unit testing**
- Separation of concerns & testable design

---

## 🛠 Tech Stack

- **C#**
- **.NET Console Application**
- **ADO.NET**
- **Dapper / Dapper Plus**
- **SQL Server**
- **Microsoft.Data.SqlClient**
- **xUnit**
- **Moq**

---

## 📂 Project Structure

### Application Project
- **Models** – Entity definitions
- **Data** – Database connection factory
- **Repositories** – Data access logic (Dapper + SQL)
- **Services** – Business logic & validation
- **Program.cs** – Application entry point
- **SQLquery.sql** – Database schema and setup

### Test Project (`EmployeeManagement.Tests`)
- **Services**
  - Unit tests for `EmployeeService`
  - Business rules & validation tests
  - Mocked dependencies using Moq
- **Repositories**
  - Repository unit tests using **Fake/In-Memory repositories**
  - No database dependency
- **TestHelpers**
  - Mock data builders
  - Fake repository implementations

---

## ⚙️ Features

- Create, read, update, and deactivate employees
- Manage departments
- Manage projects
- Assign employees to projects
- View employees with department details
- Search employees using filters
- Salary update with history tracking
- Bulk employee import
- Interactive console-based menu
- **Fully unit-tested service layer**
- **Repository tests without database dependency**

---

## 🧪 Testing Strategy

This project follows **unit testing best practices**:

- **Service layer tests**
  - Dependencies are mocked using **Moq**
  - Focus on business logic and validation rules
- **Repository tests**
  - Implemented using **Fake/In-Memory repositories**
  - No SQL Server or database required
- Tests follow the **Arrange–Act–Assert (AAA)** pattern
- Edge cases and invalid inputs are covered

All tests can be executed using:

```bash
dotnet test
