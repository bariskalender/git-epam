USE HR;
GO

-- alter database: add, remove, update data and log files, change compatibility, etc.
-- alter views, etc.

-- change table column data type
ALTER TABLE Employees
ALTER COLUMN FirstName NVARCHAR(100) NOT NULL;

-- add a new table column
ALTER TABLE Employees
ADD SocialSecurityNumber INT;

-- remove a table column 
ALTER TABLE Employees
DROP COLUMN SocialSecurityNumber;

-- rename: in order to rename columns and other database objects
--		   we have to use the system function: EXEC sp_rename. 
--		   We will cover this topic later since it is more advanced