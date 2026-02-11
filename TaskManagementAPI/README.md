# Task Management REST API 

## 📌 Overview

This project is a **C# ASP.NET Core Web API** that implements a complete **Task Management System** using **Dapper** and **SQL Server**.

It was developed as a major practice project to simulate a real-world backend REST API, focusing on clean architecture, data access patterns, and proper API design.

---

## 🎯 Purpose

The main goals of this project are to:

- Build a **RESTful Web API** with ASP.NET Core
- Perform full **CRUD operations** using **Dapper**
- Apply **Repository** and **Service** patterns
- Work directly with **SQL Server**
- Practice API validation, error handling, and HTTP status codes
- Prepare for real-world backend interview scenarios

---

## 🧠 Concepts Practiced

- RESTful API design
- ASP.NET Core Web API
- Model binding and DTO usage
- Input validation with data annotations
- Repository pattern for data access
- Service layer for business logic
- Dependency Injection
- Global error handling middleware
- Logging with ILogger
- Async / await programming
- SQL Server integration
- Foreign key constraints and relational data modeling
- LINQ for filtering and data transformations
- One-to-many relationships (User–Tasks, Task–Comments)
- Many-to-many relationships (Tasks–Categories)
- Swagger / OpenAPI documentation

---

## 🛠 Tech Stack

- **C#**
- **ASP.NET Core Web API**
- **.NET**
- **Dapper**
- **SQL Server-LINQ**
- **Microsoft.Data.SqlClient**
- **Swagger / OpenAPI**

---


## 📂 Project Structure

- **Models** – Entity definitions
- **Data** – Database connection factory
- **Repositories** – Data access logic
- **Services** – Business logic
- **Program.cs** – Application entry point
- **SQLquery_TaskManagementAPI.sql** – Database schema and queries

---

## ⚙️ Features
- **👤 Users** –
Register new users
Login validation
Get user profile
Update user profile
Deactivate users
- **✅ Tasks** –
Create, read, update, and delete tasks
Assign tasks to users
Filter tasks by user
Filter tasks by status
Search tasks by keyword
Task status and priority management
- **🗂 Categories** –
Create, read, update, and delete categories
- **💬 Comments** –
Add comments to tasks
View comments per task
Delete comments


