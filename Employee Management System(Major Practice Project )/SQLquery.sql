CREATE TABLE Departments (
 Id INT PRIMARY KEY IDENTITY,
 Name NVARCHAR(100),
 Location NVARCHAR(100),
 ManagerId INT
);
CREATE TABLE Employees (
 Id INT PRIMARY KEY IDENTITY,
 FirstName NVARCHAR(50),
 LastName NVARCHAR(50),
 Email NVARCHAR(100) UNIQUE,
 Phone NVARCHAR(20),
 DepartmentId INT,
 Salary DECIMAL(18,2),
 HireDate DATE,
 IsActive BIT DEFAULT 1,
 FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
);
CREATE TABLE Projects (
 Id INT PRIMARY KEY IDENTITY,
 Name NVARCHAR(200),
 StartDate DATE,
 EndDate DATE,
 Budget DECIMAL(18,2)
);
CREATE TABLE EmployeeProjects (
 EmployeeId INT,
 ProjectId INT,
 Role NVARCHAR(50),
 PRIMARY KEY (EmployeeId, ProjectId),
 FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
 FOREIGN KEY (ProjectId) REFERENCES Projects(Id)
);
CREATE TABLE EmployeeSalaryHistory (
    Id INT IDENTITY PRIMARY KEY,
    EmployeeId INT NOT NULL,
    OldSalary DECIMAL(18,2) NOT NULL,
    NewSalary DECIMAL(18,2) NOT NULL,
    ChangedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
);

INSERT INTO Employees
(
    FirstName,
    LastName,
    Email,
    Phone,
    DepartmentId,
    Salary,
    HireDate,
    IsActive
)
VALUES
(
    'John',
    'Doe',
    'john.doe@test.com',
    NULL,
    1,
    1200.00,
    GETDATE(),
    1
);
INSERT INTO Departments (Name, Location, ManagerId)
VALUES ('IT', 'Athens', NULL);

CREATE OR ALTER PROCEDURE dbo.sp_DepartmentTotalsById
    @DepartmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.Id AS DepartmentId,
        d.Name AS DepartmentName,

        COUNT(e.Id) AS TotalEmployeeCount,
        SUM(CASE WHEN e.IsActive = 1 THEN 1 ELSE 0 END) AS ActiveEmployeeCount,
        SUM(CASE WHEN e.IsActive = 0 THEN 1 ELSE 0 END) AS InactiveEmployeeCount,

        COALESCE(SUM(CASE WHEN e.IsActive = 1 THEN e.Salary ELSE 0 END), 0) AS ActiveTotalSalary,
        COALESCE(AVG(CASE WHEN e.IsActive = 1 THEN e.Salary END), 0) AS ActiveAverageSalary
    FROM Departments d
    LEFT JOIN Employees e
        ON e.DepartmentId = d.Id
    WHERE d.Id = @DepartmentId
    GROUP BY d.Id, d.Name;
END
GO
