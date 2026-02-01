-- 0.1 Create & use database
CREATE DATABASE MiniShopDB;
USE MiniShopDB;

-- 1) Core tables
CREATE TABLE dbo.Customers (
    Id INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName  NVARCHAR(50) NOT NULL,
    Email     NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE dbo.Categories (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.Products (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    CategoryId INT NOT NULL,
    CONSTRAINT FK_Products_Categories
        FOREIGN KEY (CategoryId) REFERENCES dbo.Categories(Id)
);

CREATE TABLE dbo.Orders (
    Id INT PRIMARY KEY IDENTITY,
    CustomerId INT NOT NULL,
    OrderDate DATE NOT NULL,
    CONSTRAINT FK_Orders_Customers
        FOREIGN KEY (CustomerId) REFERENCES dbo.Customers(Id)
);

CREATE TABLE dbo.OrderItems (
    Id INT PRIMARY KEY IDENTITY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    CONSTRAINT FK_OrderItems_Orders
        FOREIGN KEY (OrderId) REFERENCES dbo.Orders(Id),
    CONSTRAINT FK_OrderItems_Products
        FOREIGN KEY (ProductId) REFERENCES dbo.Products(Id)
);
GO

-- 2) Seed data
INSERT INTO dbo.Customers (FirstName, LastName, Email) VALUES
('Nikos','Papadopoulos','nikos@mail.com'),
('Maria','Ioannou','maria@mail.com'),
('Giorgos','Kosta','giorgos@mail.com'),
('Eleni','Dimitriou','eleni@mail.com');   -- θα τη χρησιμοποιήσουμε για LEFT JOIN (χωρίς orders)

INSERT INTO dbo.Categories (Name) VALUES
('Electronics'),('Books'),('Coffee');

INSERT INTO dbo.Products (Name, Price, CategoryId) VALUES
('Laptop', 999.99, 1),
('Mouse',  19.90,  1),
('SQL Book', 29.50, 2),
('Espresso Beans', 12.00, 3),
('Mug', 8.50, 3);

INSERT INTO dbo.Orders (CustomerId, OrderDate) VALUES
(1, '2026-01-05'),
(1, '2026-01-10'),
(2, '2026-01-11'),
(3, '2026-01-12');

-- Order 1 (Nikos): Laptop x1, Mouse x2
INSERT INTO dbo.OrderItems (OrderId, ProductId, Quantity) VALUES
(1, 1, 1),
(1, 2, 2);

-- Order 2 (Nikos): SQL Book x1, Mug x1
INSERT INTO dbo.OrderItems (OrderId, ProductId, Quantity) VALUES
(2, 3, 1),
(2, 5, 1);

-- Order 3 (Maria): Espresso Beans x3
INSERT INTO dbo.OrderItems (OrderId, ProductId, Quantity) VALUES
(3, 4, 3);

-- Order 4 (Giorgos): Mouse x1, Mug x2
INSERT INTO dbo.OrderItems (OrderId, ProductId, Quantity) VALUES
(4, 2, 1),
(4, 5, 2);


--join exercises
--Exercise A (2 tables)
select c.FirstName , c.LastName
from Customers c
inner join Orders o
on o.CustomerId = c.Id;
--Exercise B (LEFT JOIN)
select c.FirstName , c.LastName , o.Id
from Customers c
left join Orders o
on o.CustomerId = c.Id;
--Exercise C (3 tables)
select o.Id,p.Name,oi.Quantity
from Orders o
inner join OrderItems oi
on oi.OrderId=o.Id
inner join Products p
on oi.ProductId=p.Id;
--Exercise D (4 tables)
select o.Id,p.Name,oi.Quantity,c.Name
from Orders o
inner join OrderItems oi
on oi.OrderId=o.Id
inner join Products p
on oi.ProductId=p.Id
inner join Categories c
on p.CategoryId = c.Id;
--Exercise E (φίλτρο με JOIN + WHERE)
select o.Id,p.Name,oi.Quantity
from Orders o
inner join OrderItems oi
on oi.OrderId=o.Id
inner join Products p
on oi.ProductId=p.Id
inner join Categories c
on p.CategoryId = c.Id
where c.Name='Coffee';
--Exercise F – Customers χωρίς Orders
select cu.firstName , cu.LastName
from Customers cu
left join Orders o 
on o.CustomerId=cu.Id
where o.Id is null;
--join&groupby exercises
--Exercise 1 — Πόσες παραγγελίες έχει κάθε customer
SELECT 
    cu.Id,
    cu.FirstName,
    cu.LastName,
    COUNT(o.Id) AS OrdersCount
FROM Customers cu
LEFT JOIN Orders o 
    ON o.CustomerId = cu.Id
GROUP BY cu.Id, cu.FirstName, cu.LastName;
--Exercise 2 — Σύνολο τεμαχίων ανά customer
SELECT 
    cu.Id,
    cu.FirstName,
    cu.LastName,
    SUM(oi.Quantity) AS TotalItems
FROM Customers cu
LEFT JOIN Orders o 
    ON o.CustomerId = cu.Id
LEFT JOIN OrderItems oi 
    ON oi.OrderId = o.Id
GROUP BY 
    cu.Id, cu.FirstName, cu.LastName;
--Exercise 3 — Τζίρος ανά κατηγορία
SELECT 
    ca.Name AS CategoryName,
    SUM(oi.Quantity * p.Price) AS Revenue
FROM Products p
INNER JOIN OrderItems oi 
    ON oi.ProductId = p.Id
INNER JOIN Categories ca
    ON p.CategoryId = ca.Id
GROUP BY ca.Name;
