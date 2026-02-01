CREATE DATABASE AdoNetPractice;
GO
USE AdoNetPractice;
GO
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
GO
INSERT INTO Users (Name, Email) VALUES
('Alex', 'alex@test.com'),
('Maria', 'maria@test.com'),
('Nikos', 'nikos@test.com');
