USE HR;
GO

-- group employees by their DepartmentIds and counting how many employees per department
SELECT DepartmentId, COUNT(Id) AS 'EmployeeCount'   -- we can also use other aggregation methods, like SUM(), MAX(), AVG()
	FROM Employees
-- the column that is used for grouping (DepartmentId) should be always included in the SELECT statement.
-- If not included the query will result in an error
GROUP BY DepartmentId; 

-- group employees by their DepartmentIds while connecting two tables
-- and counting how many employees per department
SELECT dp.Name AS 'DepartmentName', COUNT(em.Id) AS 'EmployeeCount'
	-- the following FROM - WHERE combination works like a JOIN
	-- and establishes a relation between Employees and Departments
	FROM Employees em, Departments dp
	WHERE em.DepartmentId = dp.Id
GROUP BY dp.Name;

-- group employees by their DepartmentIds while connecting two tables
-- and counting total amount spent on salaries per department
SELECT dp.Name AS 'DepartmentName', SUM(em.Salary) AS 'TotalSalaries'
	-- the following FROM - WHERE combination works like a JOIN
	-- and establishes a relation between Employees and Departments
	FROM Employees em, Departments dp
	WHERE em.DepartmentId = dp.Id
GROUP BY dp.Name;

-- the same as previous but only take the departments where total salaries are greater than 500
SELECT dp.Name AS 'DepartmentName', SUM(em.Salary) AS 'TotalSalaries'
	FROM Employees em, Departments dp
	WHERE em.DepartmentId = dp.Id
GROUP BY dp.Name
-- HAVING statements is like WHERE but can be used to filter groups (after GROUP BY)
HAVING SUM(em.Salary) > 500;