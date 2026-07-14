USE HR;
GO

-- select every record and every column from Employees table
-- use the * (called wildcard) selector pattern
SELECT * FROM Employees;

-- select a limited amount (3) of the records and every column from Employees table
SELECT TOP(3) * FROM Employees;

-- select a limited percentage (50%) of the records and every column from Employees table
SELECT TOP 50 PERCENT * FROM Employees;

-- select every record but only specific columns from Employees table
SELECT FirstName, LastName FROM Employees;

-- select every record that follows a certain condition from Employees table
SELECT * FROM Employees
WHERE Id = 1; -- we can also use >, <, >=, <=, !=, etc.

SELECT * FROM Employees
WHERE FirstName = 'Caryn';

SELECT * FROM Employees
-- we can define comparison patterns for strings.
-- 'C%' means starts with C
-- '%C' means ends with C
-- '%C%' means has C anywhere
WHERE FirstName LIKE 'C%';

-- use multiple conditions
SELECT * FROM Employees
WHERE Id = 4 AND Salary > 100; -- we can also use OR, etc.

-- use a collection of values
SELECT * FROM Employees
WHERE Id IN (1, 3, 5);

-- order result records (descending)
SELECT * FROM Employees
WHERE Id IN (1, 2)
ORDER BY Salary DESC;

-- order result records (ascending)
SELECT * FROM Employees
WHERE Id IN (1, 2)
ORDER BY Salary ASC;

-- use a subquery in a WHERE statement
SELECT * FROM Employees
WHERE DepartmentId = (SELECT Id FROM Departments WHERE Name = 'Sales');