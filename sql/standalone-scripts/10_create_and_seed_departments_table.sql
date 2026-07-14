USE HR;
GO

-- create Departments table
CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY(1,1), 
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