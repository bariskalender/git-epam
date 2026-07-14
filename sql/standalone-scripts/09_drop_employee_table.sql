USE HR;
GO

-- drop is similar to delete but applies to database objects instead of table records
-- we can drop tables, databases, constraints, views, and other database objects.

-- remove Employees table
DROP TABLE Employees;
GO

-- tell the database engine to run the following commands against the master database, since we are removing HR
USE master;
GO

-- remove HR database
DROP DATABASE HR;
GO