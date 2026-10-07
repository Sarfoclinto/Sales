using Microsoft.Data.SqlClient;
using Sales.Extensions;
using Sales.Models;
using Sales.Services;

namespace Sales.Repository
{
    public class EmployeeRepo(DbConnectionFactory db)
    {
        public async Task<bool> EmployeeIdExistAsync(int empID)
        {
            try
            {
                using SqlConnection connection = db.CreateConnection();
                await connection.OpenAsync();
                const string sql = """
                    SELECT 1 FROM EMPLOYEES WHERE EMPLOYEEID = @EMPLOYEEID;
                    """;
                using SqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("@EMPLOYEEID", empID);
                object? res = await command.ExecuteScalarAsync();
                if (res == null) return false;
                else return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to check Id: {ex.Message}");
                Utilities.Pause();
                return false;
            }
        }

        public async Task<bool> InsertEmployeeAsync(EmployeeCreate emp)
        {
            try
            {
                using SqlConnection connection = db.CreateConnection();
                await connection.OpenAsync();
                const string sql = """
                    INSERT INTO Employees
                        (FirstName, LastName, Email, Department, Salary, HireDate, ManagerID)
                    VALUES
                        (@FirstName, @LastName, @Email, @Department, @Salary, @HireDate, @ManagerID)
                    """;
                using SqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("@FirstName", emp.FirstName);
                command.Parameters.AddWithValue("@LastName", emp.LastName);
                command.Parameters.AddWithValue("@Email", emp.Email);
                command.Parameters.AddWithValue("@Department", emp.Department);
                command.Parameters.AddWithValue("@Salary", emp.Salary);
                command.Parameters.AddWithValue("@HireDate", emp.HireDate);
                command.Parameters.AddWithValue("@ManagerID", emp.ManagerId);
                return (await command.ExecuteNonQueryAsync()) > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to insert record: {ex.Message}");
                Utilities.Pause();
                return false;
            }
        }

        public async Task<List<Employee>> GetAllEmployeeAsync()
        {
            try
            {
                List<Employee> employees = [];
                using SqlConnection connection = db.CreateConnection();
                await connection.OpenAsync();
                const string sql = @"
                    SELECT 
                	    emp.EmployeeID,
                	    emp.FirstName,
                	    emp.LastName,
                	    emp.Email,
                	    emp.Department,
                	    emp.Salary,
                	    emp.HireDate,
                	    case 
                		    when emp.ManagerID is null then 0
                		    else emp.ManagerID
                	    end as ManagerID,
                	    emp.IsActive,
                	    case
                		    when emp.ManagerID is null then 'No Manager'
                		    else CONCAT(man.FirstName, ' ',man.LastName) 
                	    end as ManagerName 
                    FROM Employees emp
                    left join 
                    Employees man 
                    on emp.ManagerID = man.EmployeeID
                    ";
                using SqlCommand command = new(sql, connection);
                var reader = await command.ExecuteReaderAsync();
                while(await reader.ReadAsync())
                {
                    employees.Add(new Employee()
                    {
                        FirstName = reader.GetStringSafe("FirstName")!,
                        LastName = reader.GetStringSafe("LastName")!,
                        Email = reader.GetStringSafe("Email")!,
                        Department = reader.GetStringSafe("Department")!,
                        Salary = reader.GetDecimalSafe("Salary") ?? 0,
                        HireDate = reader.GetDateTimeSafe("HireDate") ?? DateTime.Now,
                        ManagerId = reader.GetIntSafe("ManagerID") ?? 0,
                        ManagerName = reader.GetStringSafe("ManagerName")!
                    });
                }
                return employees;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to get employees: {ex.Message}");
                Utilities.Pause();
                return [];
            }
        }
    }
}
