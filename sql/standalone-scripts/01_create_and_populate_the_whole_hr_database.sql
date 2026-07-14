-- this script creates and populates the whole HR database with two tables: Employees and Departments.
-- use the Execute button or F5 key to run it

-- tell the database engine to run commands against the master database
USE master;
GO

-- create a new HR database
CREATE DATABASE HR;
GO

-- tell the database engine to run commands against the newely created HR database
USE HR;
GO

-- create Employees table
CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY(1,1), -- a primary key that is populated automatically by the database engine
    FirstName NVARCHAR(20) NOT NULL, 
    LastName NVARCHAR(20) NOT NULL, 
    Email NVARCHAR(25), 
    PhoneNumber NVARCHAR(25), 
    Salary INT,
	DepartmentId INT NULL -- a foreign key to relate Employees table to Departments table
);
GO

-- populate Employees table
INSERT INTO Employees
VALUES
	('Caryn', 'Mitchell', 'm.f@protonmail.edu', '1-683-204-3142', 100, 1),
	('Tyler', 'Herring', 'el@google.couk', '(234) 887-3617', 200, 1),
	('Omar', 'Adkins', 'd.m@yahoo.net', '1-517-314-5525', 300, 2),
	('Erich', 'Casey', 'sit@yahoo.couk', '(283) 850-5763', 400, 2),
	-- the last employee does not have a department id so that there would be a noticable difference when practicing JOINs
	('Vanna', 'Herring', 'u.p.c@aol.net', '(346) 556-9451', 500, NULL); 
GO

-- create Departments table
CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY(1,1), -- a primary key that is populated automatically by the database engine
    Name NVARCHAR(20) NOT NULL, 
    Description NVARCHAR(20) NULL
);
GO

-- populate Departments table
INSERT INTO Departments
VALUES
	('Sales', 'Sales department'),
	('Logistics', 'Logistics department'),
	('Warehouse', 'Warehouse department');
GO