using Microsoft.Data.SqlClient;
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
    }
}
