USE HR;
GO

-- run both the select statements along with any JOIN statement 
-- to see all result tables together in the output window 
-- to have a convinient relationship visualisation
SELECT * FROM Departments;
SELECT * FROM Employees;

-- left join: all records from the left table with all the matched records from the right table
SELECT * FROM Employees
--                       this is left             this is right
LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id;

-- right join: all records from the right table with all the matched records from the left table
SELECT * FROM Employees
--                        this is left             this is right
RIGHT JOIN Departments ON Employees.DepartmentId = Departments.Id;

-- inner join: all records that have matching values in both tables
SELECT * FROM Employees
INNER  JOIN Departments ON Employees.DepartmentId = Departments.Id;

-- full join: all records when there is a match in either left or right table
SELECT * FROM Employees
FULL JOIN Departments ON Employees.DepartmentId = Departments.Id;

-- cross join: each row from the 1st table with all the rows of another table
SELECT * FROM Employees
CROSS JOIN Departments;

-- syntax features

-- use short local table names: em - Employees, dp - Departments
SELECT * FROM Employees em
INNER JOIN Departments dp ON em.DepartmentId = dp.Id;

-- you can also use short local table names in the select statement
SELECT em.Id, em.FirstName, em.LastName, dp.Name FROM Employees em
INNER JOIN Departments dp ON em.DepartmentId = dp.Id;

-- use table column aliases when you want to make them more informative
SELECT e.Id, e.FirstName, e.LastName, d.Name as 'DepartmentName', d.Description as 'DepartmentDescription'
FROM Employees e
INNER JOIN Departments d ON e.DepartmentId = d.Id;