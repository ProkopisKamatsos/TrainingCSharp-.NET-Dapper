# Employee Management System MVC

## 📌 Overview

This project is an **ASP.NET Core MVC Web Application** that implements a complete **Employee Management System** using **Entity Framework Core**, **SQL Server**, and **ASP.NET Identity**.

It was developed as a web-based evolution of a previous console application project, with the goal of simulating a more realistic internal business system for managing employees, departments, projects, assignments, and salary records through a secure and user-friendly interface.

---

## 🎯 Purpose

The main goals of this project are to:

- Build a database-driven **MVC web application**
- Perform full **CRUD operations** through a browser-based interface
- Implement **authentication** using **Individual Accounts**
- Apply **role-based authorization**
- Manage real-world entity relationships
- Create a more realistic and professional internal management system

---

## 🧠 Concepts Practiced

- ASP.NET Core MVC architecture
- Entity Framework Core
- SQL Server integration
- ASP.NET Identity authentication
- Role-based authorization
- CRUD operations with Razor Views
- ViewModels and model binding
- One-to-many relationships (Employee–Department)
- Many-to-many relationships (Employee–Project)
- Search and filtering
- Soft delete implementation
- Salary update history tracking
- Business rule validation
- Responsive UI with Bootstrap

---

## 🛠 Tech Stack

- **C#**
- **ASP.NET Core MVC**
- **.NET 10**
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Identity**
- **Bootstrap**
- **Razor Views**

---

## 📂 Project Structure

- **Controllers** – MVC controllers for application features
- **Models** – Entity models and ViewModels
- **Views** – Razor pages for the UI
- **Data** – ApplicationDbContext and database configuration
- **Areas/Identity** – Authentication and Identity functionality
- **wwwroot** – Static assets such as CSS, JavaScript, and libraries
- **Program.cs** – Application startup and service configuration
- **appsettings.json** – Configuration and connection strings

---

## ⚙️ Features

- Register and login with **Individual Accounts**
- Role-based access with **Admin**, **Manager**, and authenticated user restrictions
- Create, read, update, and deactivate employees
- Manage departments
- Manage projects
- Assign employees to projects with roles
- Remove employee-project assignments
- Search employees by:
  - Name
  - Department
  - Salary range
  - Active status
- View employee salary history
- Update salaries with history tracking
- View department totals report
- Soft delete for employees using `IsActive`
- Validation for:
  - Unique email
  - Unique department name
  - Unique project name
  - Salary rules
  - Date rules
  - Assignment rules

---

## 🔐 Authorization

The application includes authentication and authorization features:

- Only authenticated users can access the management system
- Some actions are restricted based on role
- **Admin** users have full control
- **Manager** users can perform selected management actions
- Read-only access is available on general secured pages for authenticated users

---

## 🗃 Database Design

The system is based on the following core entities:

- **Departments**
- **Employees**
- **Projects**
- **EmployeeProjects**
- **EmployeeSalaryHistories**

### Relationships

- One **Department** has many **Employees**
- One **Employee** belongs to one **Department**
- Many **Employees** can belong to many **Projects**
- One **Employee** can have many **Salary History** records

---

## ✅ Business Rules Implemented

- Employees cannot be edited if they are inactive
- Inactive employees cannot receive salary updates
- Departments cannot be deleted if they contain employees
- Duplicate employee emails are not allowed
- Duplicate department names are not allowed
- Duplicate project names are not allowed
- Employees cannot be assigned twice to the same project
- Salary values cannot be negative
- Hire dates cannot be in the future
- Project end date cannot be earlier than start date

---

## 🚀 How to Run

1. Clone the repository
2. Open the project in **Visual Studio**
3. Update the connection string in `appsettings.json`
4. Run the following commands in **Package Manager Console**:

```powershell
Add-Migration InitialCreate
Update-Database
