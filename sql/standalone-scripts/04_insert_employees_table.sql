USE HR;
GO

-- insert a single value while populating all columns
INSERT INTO Employees (FirstName, LastName, Email, PhoneNumber, Salary, DepartmentId)
VALUES('Caryn', 'Mitchell', 'm.f@protonmail.edu', '1-683-204-3142', 100, 5);
GO

-- insert a single value while populating all columns
-- this one works the same as prevoius because we don't have to specify 
-- the column names if we are populating all of them
INSERT INTO Employees
VALUES('Caryn', 'Mitchell', 'm.f@protonmail.edu', '1-683-204-3142', 100, 5);
GO

-- insert a single record while only populating specific columns
INSERT INTO Employees (FirstName, LastName)
VALUES('Caryn', 'Mitchell');
GO

-- insert multiple values while populating all columns
INSERT INTO Employees
VALUES
	('Tyler', 'Herring', 'el@google.couk', '(234) 887-3617', 200, 1),
	('Omar', 'Adkins', 'd.m@yahoo.net', '1-517-314-5525', 300, 2),
	('Erich', 'Casey', 'sit@yahoo.couk', '(283) 850-5763', 400, 2),
	('Vanna', 'Herring', 'u.p.c@aol.net', '(346) 556-9451', 500, NULL);
GO

-- insert multiple values while only populating specific columns
INSERT INTO Employees (FirstName, LastName)
VALUES
	('Tyler', 'Herring'),
	('Omar', 'Adkins'),
	('Erich', 'Casey'),
	('Vanna', 'Herring');
GO