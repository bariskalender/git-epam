USE HR;
GO

-- create a value constraint of Employees table
-- we have to use alter since we are changing the table schema
ALTER TABLE Employees
--						  constraint name (CHK - convention for constraints that just check values)
WITH CHECK ADD CONSTRAINT CHK_DepartmentIdValid
-- check if the DepartmentId values being inserted or updated are equal to 1, 2 or 3
CHECK (([DepartmentId]=1 OR [DepartmentId]=2 OR [DepartmentId]=3))
GO

-- create a foreign key constraint that will check if the DepartmentId values being inserted or updated
-- exist in the Departments table (will only allow to use existing departments)

-- in order for this constraint to work we have to first drop the existing Employees table
-- and create a new one where DepartmantId column does to accept nulls because the constraint
-- is not going to be able to check NULLs - we cannot compare a NULL (nothing) to a real value
DROP TABLE IF EXISTS Employees;

-- create new Employees table
CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(20) NOT NULL, 
    LastName NVARCHAR(20) NOT NULL, 
    Email NVARCHAR(25), 
    PhoneNumber NVARCHAR(25), 
    Salary INT,
	DepartmentId INT NOT NULL -- does not accept nulls
);
GO

-- then we can run the following command
ALTER TABLE Employees
--						  constraint name (FK - convention for constraints on foreign key checks)
WITH CHECK ADD CONSTRAINT FK_Employees_Departments
-- DepartmentId - foreign key from the Employees table
-- Id - primary key from the Departments table
FOREIGN KEY (DepartmentId) REFERENCES Departments (Id)
GO