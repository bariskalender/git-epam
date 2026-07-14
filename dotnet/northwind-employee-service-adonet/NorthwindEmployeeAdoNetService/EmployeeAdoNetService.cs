namespace NorthwindEmployeeAdoNetService;

/// <summary>
/// A service for interacting with the "Employees" table using ADO.NET.
/// </summary>
public sealed class EmployeeAdoNetService
{
    private readonly DbProviderFactory dbFactory;
    private readonly string connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeAdoNetService"/> class.
    /// </summary>
    /// <param name="dbFactory">The database provider factory used to create database connection and command instances.</param>
    /// <param name="connectionString">The connection string used to establish a database connection.</param>
    /// <exception cref="ArgumentNullException">Thrown when either <paramref name="dbFactory"/> or <paramref name="connectionString"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="connectionString"/> is empty or contains only white-space characters.</exception>
    public EmployeeAdoNetService(DbProviderFactory dbFactory, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(dbFactory);
        ArgumentNullException.ThrowIfNull(connectionString);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("The connection string cannot be empty or whitespace.", nameof(connectionString));
        }

        this.dbFactory = dbFactory;
        this.connectionString = connectionString;
    }

    /// <summary>
    /// Retrieves a list of all employees from the Employees table of the database.
    /// </summary>
    /// <returns>A list of Employee objects representing the retrieved employees.</returns>
    public IList<Employee> GetEmployees()
    {
        using var connection = this.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT EmployeeID,
                   LastName,
                   FirstName,
                   Title,
                   TitleOfCourtesy,
                   BirthDate,
                   HireDate,
                   Address,
                   City,
                   Region,
                   PostalCode,
                   Country,
                   HomePhone,
                   Extension,
                   Notes,
                   ReportsTo,
                   PhotoPath
            FROM Employees
            ORDER BY EmployeeID;
            """;

        using var reader = command.ExecuteReader();

        var employees = new List<Employee>();
        while (reader.Read())
        {
            employees.Add(MapEmployee(reader));
        }

        return employees;
    }

    /// <summary>
    /// Retrieves an employee with the specified employee ID.
    /// </summary>
    /// <param name="employeeId">The ID of the employee to retrieve.</param>
    /// <returns>The retrieved an <see cref="Employee"/> instance.</returns>
    /// <exception cref="EmployeeServiceException">Thrown if the employee is not found.</exception>
    public Employee GetEmployee(long employeeId)
    {
        using var connection = this.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT EmployeeID,
                   LastName,
                   FirstName,
                   Title,
                   TitleOfCourtesy,
                   BirthDate,
                   HireDate,
                   Address,
                   City,
                   Region,
                   PostalCode,
                   Country,
                   HomePhone,
                   Extension,
                   Notes,
                   ReportsTo,
                   PhotoPath
            FROM Employees
            WHERE EmployeeID = @employeeId;
            """;

        AddParameter(command, "@employeeId", employeeId);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            throw new EmployeeServiceException("Employee not found.");
        }

        return MapEmployee(reader);
    }

    /// <summary>
    /// Adds a new employee to Employee table of the database.
    /// </summary>
    /// <param name="employee">The  <see cref="Employee"/> object containing the employee's information.</param>
    /// <returns>The ID of the newly added employee.</returns>
    /// <exception cref="EmployeeServiceException">Thrown when an error occurs while adding the employee.</exception>
    public long AddEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        using var connection = this.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO Employees
            (
                LastName,
                FirstName,
                Title,
                TitleOfCourtesy,
                BirthDate,
                HireDate,
                Address,
                City,
                Region,
                PostalCode,
                Country,
                HomePhone,
                Extension,
                Notes,
                ReportsTo,
                PhotoPath
            )
            VALUES
            (
                @lastName,
                @firstName,
                @title,
                @titleOfCourtesy,
                @birthDate,
                @hireDate,
                @address,
                @city,
                @region,
                @postalCode,
                @country,
                @homePhone,
                @extension,
                @notes,
                @reportsTo,
                @photoPath
            );
            SELECT last_insert_rowid();
            """;

        AddParameter(command, "@lastName", employee.LastName);
        AddParameter(command, "@firstName", employee.FirstName);
        AddParameter(command, "@title", employee.Title);
        AddParameter(command, "@titleOfCourtesy", employee.TitleOfCourtesy);
        AddParameter(command, "@birthDate", employee.BirthDate);
        AddParameter(command, "@hireDate", employee.HireDate);
        AddParameter(command, "@address", employee.Address);
        AddParameter(command, "@city", employee.City);
        AddParameter(command, "@region", employee.Region);
        AddParameter(command, "@postalCode", employee.PostalCode);
        AddParameter(command, "@country", employee.Country);
        AddParameter(command, "@homePhone", employee.HomePhone);
        AddParameter(command, "@extension", employee.Extension);
        AddParameter(command, "@notes", employee.Notes);
        AddParameter(command, "@reportsTo", employee.ReportsTo);
        AddParameter(command, "@photoPath", employee.PhotoPath);

        object? result = command.ExecuteScalar();

        if (result is null || result == DBNull.Value)
        {
            throw new EmployeeServiceException("Inserting an employee failed.");
        }

        long newEmployeeId = Convert.ToInt64(result, System.Globalization.CultureInfo.InvariantCulture);
        if (newEmployeeId <= 0)
        {
            throw new EmployeeServiceException("Inserting an employee failed.");
        }

        return newEmployeeId;
    }

    /// <summary>
    /// Removes an employee from the the Employee table of the database based on the provided employee ID.
    /// </summary>
    /// <param name="employeeId">The ID of the employee to remove.</param>
    /// <exception cref="EmployeeServiceException"> Thrown when an error occurs while attempting to remove the employee.</exception>
    public void RemoveEmployee(long employeeId)
    {
        using var connection = this.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            DELETE FROM Employees
            WHERE EmployeeID = @employeeId;
            """;

        AddParameter(command, "@employeeId", employeeId);
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Updates an employee record in the Employee table of the database.
    /// </summary>
    /// <param name="employee">The employee object containing updated information.</param>
    /// <exception cref="EmployeeServiceException">Thrown when there is an issue updating the employee record.</exception>
    public void UpdateEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        using var connection = this.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            UPDATE Employees
            SET LastName = @lastName,
                FirstName = @firstName,
                Title = @title,
                TitleOfCourtesy = @titleOfCourtesy,
                BirthDate = @birthDate,
                HireDate = @hireDate,
                Address = @address,
                City = @city,
                Region = @region,
                PostalCode = @postalCode,
                Country = @country,
                HomePhone = @homePhone,
                Extension = @extension,
                Notes = @notes,
                ReportsTo = @reportsTo,
                PhotoPath = @photoPath
            WHERE EmployeeID = @employeeId;
            """;

        AddParameter(command, "@employeeId", employee.Id);
        AddParameter(command, "@lastName", employee.LastName);
        AddParameter(command, "@firstName", employee.FirstName);
        AddParameter(command, "@title", employee.Title);
        AddParameter(command, "@titleOfCourtesy", employee.TitleOfCourtesy);
        AddParameter(command, "@birthDate", employee.BirthDate);
        AddParameter(command, "@hireDate", employee.HireDate);
        AddParameter(command, "@address", employee.Address);
        AddParameter(command, "@city", employee.City);
        AddParameter(command, "@region", employee.Region);
        AddParameter(command, "@postalCode", employee.PostalCode);
        AddParameter(command, "@country", employee.Country);
        AddParameter(command, "@homePhone", employee.HomePhone);
        AddParameter(command, "@extension", employee.Extension);
        AddParameter(command, "@notes", employee.Notes);
        AddParameter(command, "@reportsTo", employee.ReportsTo);
        AddParameter(command, "@photoPath", employee.PhotoPath);

        int affectedRows = command.ExecuteNonQuery();
        if (affectedRows == 0)
        {
            throw new EmployeeServiceException("Employee is not updated.");
        }
    }

    private DbConnection CreateConnection()
    {
        var connection = this.dbFactory.CreateConnection();
        if (connection is null)
        {
            throw new InvalidOperationException("Failed to create a database connection.");
        }

        connection.ConnectionString = this.connectionString;
        return connection;
    }

    private static Employee MapEmployee(IDataRecord record)
    {
        var employee = new Employee(record.GetInt64(record.GetOrdinal("EmployeeID")))
        {
            LastName = GetRequiredString(record, "LastName"),
            FirstName = GetRequiredString(record, "FirstName"),
            Title = GetNullableString(record, "Title"),
            TitleOfCourtesy = GetNullableString(record, "TitleOfCourtesy"),
            BirthDate = GetNullableDateTime(record, "BirthDate"),
            HireDate = GetNullableDateTime(record, "HireDate"),
            Address = GetNullableString(record, "Address"),
            City = GetNullableString(record, "City"),
            Region = GetNullableString(record, "Region"),
            PostalCode = GetNullableString(record, "PostalCode"),
            Country = GetNullableString(record, "Country"),
            HomePhone = GetNullableString(record, "HomePhone"),
            Extension = GetNullableString(record, "Extension"),
            Notes = GetNullableString(record, "Notes"),
            ReportsTo = GetNullableInt64(record, "ReportsTo"),
            PhotoPath = GetNullableString(record, "PhotoPath"),
        };

        return employee;
    }

    private static void AddParameter(DbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }

    private static string GetRequiredString(IDataRecord record, string columnName)
    {
        int ordinal = record.GetOrdinal(columnName);
        return record.IsDBNull(ordinal) ? string.Empty : record.GetString(ordinal);
    }

    private static string? GetNullableString(IDataRecord record, string columnName)
    {
        int ordinal = record.GetOrdinal(columnName);
        return record.IsDBNull(ordinal) ? null : record.GetString(ordinal);
    }

    private static DateTime? GetNullableDateTime(IDataRecord record, string columnName)
    {
        int ordinal = record.GetOrdinal(columnName);
        return record.IsDBNull(ordinal) ? null : record.GetDateTime(ordinal);
    }

    private static long? GetNullableInt64(IDataRecord record, string columnName)
    {
        int ordinal = record.GetOrdinal(columnName);
        return record.IsDBNull(ordinal) ? null : record.GetInt64(ordinal);
    }
}