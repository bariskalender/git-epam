USE HR;
GO

-- update a single record in the Employees table
UPDATE Employees
SET 
	FirstName = 'NewName',
	LastName = 'NewLastName'
WHERE Id = 1;

-- multiple conditions
UPDATE Employees
SET 
	FirstName = 'NewName',
	LastName = 'NewLastName'
WHERE Id = 1 OR Id > 3; -- AND, OR ...

-- update by multiple ids
UPDATE Employees
SET 
	FirstName = 'NewName',
	LastName = 'NewLastName'
WHERE Id IN (1, 2, 5);

-- update all
-- use with extreme caution =)
UPDATE Employees
SET 
	FirstName = 'NewName',
	LastName = 'NewLastName';