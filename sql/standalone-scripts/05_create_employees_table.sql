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

-- same command but using the full table name
--	HR - database name
--	dbo - schema name
--	Employees - table name
CREATE TABLE HR.dbo.Employees (
    Id INT PRIMARY KEY IDENTITY(1,1), -- a primary key that is populated automatically by the database engine
    FirstName NVARCHAR(20) NOT NULL, 
    LastName NVARCHAR(20) NOT NULL, 
    Email NVARCHAR(25), 
    PhoneNumber NVARCHAR(25), 
    Salary INT,
	DepartmentId INT NULL -- a foreign key to relate Employees table to Departments table
);
GO