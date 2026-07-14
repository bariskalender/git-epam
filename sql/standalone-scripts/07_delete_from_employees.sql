USE HR;
GO

-- delete a single record from Employees table
DELETE FROM Employees
WHERE Id = 1;

-- multiple conditions
DELETE FROM Employees
WHERE Id = 1 OR Id > 3; -- AND, OR ...

-- delete by multiple ids
DELETE FROM Employees
WHERE Id IN (1, 2);

-- delete all
-- use with extreme caution =)
DELETE FROM Employees;