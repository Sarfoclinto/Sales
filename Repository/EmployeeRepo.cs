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
        public async Task<Employee?> GetAllEmployeeByIdAsync(int Id)
        {
            if (Id == 0) return null;
            try
            {
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
                    WHERE emp.EmployeeID = @Id;
                    ";
                using SqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("@Id", Id);
                var reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows) return null;
                Employee emp = new();
                while(await reader.ReadAsync())
                {
                    emp.EmployeeId = reader.GetIntSafe("EmployeeID") ?? 0;
                    emp.FirstName = reader.GetStringSafe("FirstName")!;
                    emp.LastName = reader.GetStringSafe("LastName")!;
                    emp.Email = reader.GetStringSafe("Email")!;
                    emp.Department = reader.GetStringSafe("Department")!;
                    emp.Salary = reader.GetDecimalSafe("Salary") ?? 0;
                    emp.HireDate = reader.GetDateTimeSafe("HireDate") ?? DateTime.Now;
                    emp.ManagerId = reader.GetIntSafe("ManagerID") ?? 0;
                    emp.ManagerName = reader.GetStringSafe("ManagerName")!;
                    emp.IsActive = reader.GetBoolSafe("IsActive") ?? false;
                    
                }
                return emp;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to get employees: {ex.Message}");
                Utilities.Pause();
                return null;
            }
        }
        public async Task<List<Employee>> GetEmployeeAsync()
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
                        EmployeeId = reader.GetIntSafe("EmployeeID") ?? 0,
                        FirstName = reader.GetStringSafe("FirstName")!,
                        LastName = reader.GetStringSafe("LastName")!,
                        Email = reader.GetStringSafe("Email")!,
                        Department = reader.GetStringSafe("Department")!,
                        Salary = reader.GetDecimalSafe("Salary") ?? 0,
                        HireDate = reader.GetDateTimeSafe("HireDate") ?? DateTime.Now,
                        ManagerId = reader.GetIntSafe("ManagerID") ?? 0,
                        ManagerName = reader.GetStringSafe("ManagerName")!,
                        IsActive = reader.GetBoolSafe("IsActive") ?? false
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
        public async Task<Employee?> UpdateEmployeeAsync(int empId, EmployeeUpdate emp)
        {
            try
            {
                if(empId == 0)
                {
                    Console.WriteLine("Invalid Employee");
                    return null;
                }
                Employee? employee = await GetAllEmployeeByIdAsync(empId);
                if(employee == null || employee.EmployeeId == 0)
                {
                    Console.WriteLine("Employee not found");
                    return null;
                }

                using SqlConnection connection = db.CreateConnection();
                await connection.OpenAsync();
                const string updateText = @"
                    UPDATE EMPLOYEES SET
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Department = @Department,
                        Salary = @Salary,
                        HireDate = @HireDate,
                        ManagerId = @ManagerId
                    WHERE EmployeeId = @EmployeeId
                ";
                using SqlCommand command = new(updateText, connection);
                command.Parameters.AddWithValue("@EmployeeId", empId);
                command.Parameters.AddWithValue("@FirstName", !string.IsNullOrEmpty(emp.FirstName) ?  emp.FirstName: employee.FirstName);
                command.Parameters.AddWithValue("@LastName", !string.IsNullOrEmpty(emp.LastName) ? emp.LastName : employee.LastName);
                command.Parameters.AddWithValue("@Email", !string.IsNullOrEmpty(emp.Email) ? emp.Email : employee.Email);
                command.Parameters.AddWithValue("@Department", !string.IsNullOrEmpty(emp.Department) ? emp.Department : employee.Department);
                command.Parameters.AddWithValue("@Salary", emp.Salary == 0 ? employee.Salary : emp.Salary);
                command.Parameters.AddWithValue("@HireDate", emp.HireDate == default ? employee.HireDate : emp.HireDate);
                command.Parameters.AddWithValue("@ManagerID", emp.ManagerId == 0 ? employee.ManagerId : emp.ManagerId);
                int row = await command.ExecuteNonQueryAsync();
                if (row > 0) return await GetAllEmployeeByIdAsync(empId);
                else
                {
                    Console.WriteLine("Failed to update employee. Try Again");
                    return null;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred whiles updating: {ex.Message}");
                return null;
            }
        }
    }
}
