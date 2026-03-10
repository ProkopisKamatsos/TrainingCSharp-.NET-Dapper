# Task Management REST API

## 📌 Overview

This project is an **ASP.NET Core Web API** that implements a secure **Task Management REST API** using **Dapper**, **SQL Server**, and **JWT Authentication**.

It was developed as an improved and more secure evolution of a previous CRUD-based API project, with the goal of applying proper **authentication** and **authorization** practices in a more realistic API architecture.

The application allows users to register, log in, manage their profile, and perform secured CRUD operations on their own tasks through authenticated API endpoints.

---

## 🎯 Purpose

The main goals of this project are to:

- Build a database-driven **REST API**
- Perform secured **CRUD operations**
- Implement **JWT Authentication**
- Apply **authorization** based on the authenticated user
- Enforce **ownership rules** on protected resources
- Follow a clean layered architecture with **Controllers**, **Services**, and **Repositories**
- Practice more realistic API development patterns

---

## 🧠 Concepts Practiced

- ASP.NET Core Web API architecture
- SQL Server integration
- Dapper for data access
- Repository pattern
- Service layer for business logic
- DTOs and model binding
- Data annotations validation
- JWT token generation
- JWT-based authentication
- Authorization with `[Authorize]`
- Claims-based user identification
- Ownership-based authorization
- Async/await throughout the application
- Swagger/OpenAPI testing
- Secure password hashing with BCrypt

---

## 🛠 Tech Stack

- **C#**
- **ASP.NET Core Web API**
- **.NET 10**
- **SQL Server**
- **Dapper**
- **JWT Bearer Authentication**
- **BCrypt**
- **Swagger / OpenAPI**

---

## 📂 Project Structure

- **Controllers** – API endpoints for authentication, users, and tasks
- **Models** – Core domain models
- **DTOs** – Request and response models
- **Repositories** – Data access layer with Dapper
- **Services** – Business logic layer
- **Data** – SQL connection factory and SQL script
- **Security** – JWT settings and token generator
- **Program.cs** – Application startup and dependency injection
- **appsettings.json** – Configuration and connection strings

---

## ⚙️ Features

### Authentication
- Register new users
- Login with email and password
- Password hashing using **BCrypt**
- JWT token generation
- Protected endpoints using **Bearer Token Authentication**

### User Management
- Get current authenticated user profile
- Update current authenticated user profile
- Deactivate current authenticated user account

### Task Management
- Create new tasks for the authenticated user
- Get all tasks belonging to the authenticated user
- Get a specific task by id only if it belongs to the authenticated user
- Update a task only if it belongs to the authenticated user
- Delete a task only if it belongs to the authenticated user

---

## 🔐 Authorization

The API includes authentication and authorization features:

- Public endpoints:
  - `POST /api/auth/register`
  - `POST /api/auth/login`

- Protected endpoints require a valid JWT token
- The authenticated user is identified through JWT claims
- Users can only access and manage **their own tasks**
- Users can only manage **their own profile**
- Account deletion is implemented as **deactivation** using `IsActive`

---

## 🗃 Database Design

The system is based on the following core entities:

- **Users**
- **Tasks**
- **Categories**
- **TaskCategories**
- **Comments**

### Relationships

- One **User** has many **Tasks**
- One **Task** belongs to one **User**
- Many **Tasks** can belong to many **Categories**
- One **Task** can have many **Comments**
- One **User** can create many **Comments**

> At the current stage of the project, the implemented secured modules are focused on **Authentication**, **Users**, and **Tasks**.

---

## ✅ Business Rules Implemented

- Passwords are never stored as plain text
- Only hashed passwords are stored using **BCrypt**
- Only active users can log in
- A user can only update their own profile
- A user can only deactivate their own account
- A user can only create tasks for themselves
- A user can only view their own tasks
- A user can only update their own tasks
- A user can only delete their own tasks
- Duplicate emails are not allowed
- Duplicate usernames are not allowed
- Task priority must be between `1` and `5`
- Task status must be one of:
  - `Pending`
  - `InProgress`
  - `Completed`
  - `Cancelled`
- `CompletedAt` is automatically managed based on task status
- User deactivation is implemented as **soft delete** using `IsActive`

---

## 🌐 API Endpoints Implemented So Far

### Auth
- `POST /api/auth/register`
- `POST /api/auth/login`

### Users
- `GET /api/users/me`
- `PUT /api/users/me`
- `DELETE /api/users/me`

### Tasks
- `POST /api/tasks`
- `GET /api/tasks/my`
- `GET /api/tasks/{id}`
- `PUT /api/tasks/{id}`
- `DELETE /api/tasks/{id}`

---

## 🧪 Testing

The API can be tested using:

- **Swagger UI**
- **Postman**
- **manual Authorization header testing**

For protected endpoints, include the JWT token in the request header:

```http
Authorization: Bearer YOUR_TOKEN_HERE
