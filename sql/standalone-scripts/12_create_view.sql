USE HR;
GO

-- create a view that returns only employees with the salary greater than 200
CREATE VIEW dbo.HighSalaryEmployees
AS
	SELECT * FROM Employees
	WHERE Salary > 200;
GO

-- now we can reuse this view without having to write it again
SELECT * FROM dbo.HighSalaryEmployees;

-- we can even query it as a table
SELECT FirstName, LastName
FROM dbo.HighSalaryEmployees
WHERE DepartmentId = 3;