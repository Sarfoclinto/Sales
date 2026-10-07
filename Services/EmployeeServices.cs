using Sales.Models;
using Sales.Repository;

namespace Sales.Services
{
    internal class EmployeeServices(EmployeeRepo repo)
    {
        public async Task Init()
        {
            while (true)
            {
                Console.Clear();
                List<string> str = [
                    "--------------------------------------------------------------------------",
                    "                             EMPLOYEES",
                    "--------------------------------------------------------------------------"
                    ];
                Utilities.DisplayHeader(str);
                int len = Utilities.DisplayBasicOptions();
                Console.Write(">> ");
                if (int.TryParse(Console.ReadLine(), out int opt))
                {
                    if (opt >= 0 && opt <= len)
                    {
                        switch (opt)
                        {
                            case 1:
                                await CreateService();
                                Utilities.Pause();
                                break;
                            case 2:
                                await ViewAllService();
                                Utilities.Pause();
                                break;
                            case 3:
                                await UpdateService();
                                Utilities.Pause();
                                break;
                            case 4:
                                await DeleteService();
                                Utilities.Pause();
                                break;
                            case 0:
                                break;
                        }
                        if (opt == 0) break;
                    }
                    else
                    {
                        Console.WriteLine($@"Invalid option '{opt}' ");
                        Utilities.Pause();
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                    Utilities.Pause();
                    continue;
                }
            }
        }
        private async Task CreateService()
        {
            Console.Clear();
            List<string> str = [
                "--------------------------------------------------------------------------",
                "                             CREATE (Type 'Quit' at any point to quit)",
                "--------------------------------------------------------------------------"
                ];
            Utilities.DisplayHeader(str);
            Employee emp = new();

            Console.Write("Enter first name: ");
            emp.FirstName = Console.ReadLine()!;
            if (emp.FirstName.Equals("quit", StringComparison.CurrentCultureIgnoreCase)) return;
            
            Console.Write("Enter last name: ");
            emp.LastName = Console.ReadLine()!;
            if (emp.LastName.Equals("quit", StringComparison.CurrentCultureIgnoreCase)) return;
            

            Console.Write("Enter email: ");
            emp.Email = Console.ReadLine()!;
            if (emp.Email.Equals("quit", StringComparison.CurrentCultureIgnoreCase)) return;
            

            Console.Write("Enter department: ");
            emp.Department = Console.ReadLine()!;
            if (emp.Department.Equals("quit", StringComparison.CurrentCultureIgnoreCase)) return;
            

            // salary
            while (true)
            {
                Console.Write("Enter salary: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal salary))
                {
                    if(salary <= 0)
                    {
                        Console.WriteLine("Salary can't be less or equal to 0");
                        continue;
                    }
                    else
                    {
                        emp.Salary = salary;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid amount");
                    continue;
                }
            }

            // manger id
            while (true)
            {
                Console.Write("Enter Manager ID: ");
                if (int.TryParse(Console.ReadLine(), out int managerId))
                {
                    if(managerId <= 0)
                    {
                        Console.WriteLine("Manager ID can't be less or equal to 0");
                        continue;
                    }
                    else
                    {
                        // check if managerid in DB
                        if(!(await repo.EmployeeIdExistAsync(managerId)))
                        {
                            Console.WriteLine("Invalid Manager ID");
                            continue;
                        }
                        emp.ManagerId = managerId;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Manager ID");
                    continue;
                }
            }

            // Insert into database
            bool inserted = await repo.InsertEmployeeAsync(emp);

            Console.WriteLine(
            inserted
                ? "Employee created successfully."
                : "Employee could not be created."
            );
        }
        private async Task ViewAllService()
        {
            List<Employee> employees = await repo.GetEmployeeAsync();
            if(employees.Count == 0) Utilities.Pause();
            Console.Clear();
            List<string> str = [
                "--------------------------------------------------------------------------",
                "                             EMPLOYEES LIST",
                "--------------------------------------------------------------------------"
                ];
            Utilities.DisplayHeader(str);
            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.EmployeeId}. {emp.FirstName} {emp.LastName}\t\t  GHC {emp.Salary:N2}\t\t  Mng: {emp.ManagerId}. {emp.ManagerName}");              
            }
        }        
        private async Task UpdateService() 
        {
            Console.Clear();
            List<string> str = [
                "--------------------------------------------------------------------------",
                "                             Update (Type 'Quit' at any point to quit)",
                "--------------------------------------------------------------------------"
                ];
            Utilities.DisplayHeader(str);
            List<Employee> employees = await repo.GetEmployeeAsync();
            if(employees.Count == 0)
            {
                Console.WriteLine("No Employee found");
                return;
            }
            foreach(Employee emp in employees)
            {
                Console.WriteLine($"ID: ({emp.EmployeeId})\tName: {emp.FirstName} {emp.LastName}\t\t({emp.Department})");
            }
            while (true)
            {
                Console.WriteLine("Enter the ID of employee to update");
                Console.Write(">> ");
                string opt = Console.ReadLine()!;
                if (opt.Equals("quit", StringComparison.CurrentCultureIgnoreCase)) return;
                else
                {
                    if(int.TryParse(opt,out int optVal))
                    {
                        Employee? employee = employees.Find((em) => em.EmployeeId == optVal);
                        if(employee == null || employee.EmployeeId == 0)
                        {
                            Console.WriteLine("Invalid Employee ID or Employee not found");
                            continue;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("------------------------------------------------------------------------------------------------");
                            Console.WriteLine($"Updating User ({employee.FirstName} {employee.LastName}) (Type 'Quit' at any point to quit)");
                            Console.WriteLine($"\t\t---------------------");
                            Console.WriteLine($"\t\tCurrent User Details:");
                            Console.WriteLine($"\t\t---------------------");
                            Console.WriteLine($"ID: {employee.EmployeeId}");
                            Console.WriteLine($"FullName: {employee.FirstName} {employee.LastName}");
                            Console.WriteLine($"Department: {employee.Department}");
                            Console.WriteLine($"Salary: GHC {employee.Salary:N2}");
                            Console.WriteLine($"Manager: ({employee.ManagerId}) {employee.ManagerName}");
                            Console.WriteLine($"IsActive: ({employee.IsActive})");
                            Console.WriteLine("-------------------------------------------------------------------------------------------------");
                            EmployeeUpdate emp = new();

                            Console.Write("Enter first name: ");
                            emp.FirstName = Console.ReadLine()!;
                            if (emp.FirstName.Equals("quit", StringComparison.CurrentCultureIgnoreCase)) return;
                            else if (string.IsNullOrEmpty(emp.FirstName)) Console.WriteLine($"Old data will be used");

                            Console.Write("Enter last name: ");
                            emp.LastName = Console.ReadLine()!;
                            if (emp.LastName.Equals("quit", StringComparison.CurrentCultureIgnoreCase)) return;
                            else if (string.IsNullOrEmpty(emp.LastName)) Console.WriteLine($"Old data will be used");


                            Console.Write("Enter email: ");
                            emp.Email = Console.ReadLine()!;
                            if (emp.Email.Equals("quit", StringComparison.CurrentCultureIgnoreCase)) return;
                            else if (string.IsNullOrEmpty(emp.Email)) Console.WriteLine($"Old data will be used");


                            Console.Write("Enter department: ");
                            emp.Department = Console.ReadLine()!;
                            if (emp.Department.Equals("quit", StringComparison.CurrentCultureIgnoreCase)) return;
                            else if (string.IsNullOrEmpty(emp.Department)) Console.WriteLine($"Old data will be used");


                            // salary
                            while (true)
                            {
                                Console.Write("Enter salary: ");
                                string? salaryInput = Console.ReadLine();
                                if (string.IsNullOrEmpty(salaryInput))
                                {
                                    Console.WriteLine($"Old data will be used"); 
                                    break;
                                }
                                else if (decimal.TryParse(salaryInput, out decimal salary))
                                {
                                    if (salary <= 0)
                                    {
                                        Console.WriteLine("Salary can't be less or equal to 0");
                                        continue;
                                    }
                                    else
                                    {
                                        if(salary < employee.Salary || salary > employee.Salary)
                                        {
                                            Console.WriteLine($"Are you sure you want to {(salary > employee.Salary ? "increase" : "decrease")} salary to GHC {salary:N2}?");
                                            Console.WriteLine("Y. Yes");
                                            Console.WriteLine("N. No");
                                            Console.WriteLine(">> ");
                                            string? ans = Console.ReadLine();
                                            if (ans == null || ans.ToLower().Equals("no") || ans.ToLower().Equals("n")) continue;
                                            else
                                            {
                                                emp.Salary = salary;
                                                break;
                                            }
                                        }
                                        else { Console.WriteLine("Value is the same"); break; };                                       
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid amount");
                                    continue;
                                }
                            }

                            // manager id
                            while (true)
                            {
                                Console.Write("Enter Manager ID: ");
                                string? managerIdInput = Console.ReadLine();
                                if (string.IsNullOrEmpty(managerIdInput))
                                {
                                    Console.WriteLine($"Old data will be used");
                                    break;
                                }
                                else if (int.TryParse(managerIdInput, out int managerId))
                                {
                                    if (managerId <= 0)
                                    {
                                        Console.WriteLine("Manager ID can't be less or equal to 0");
                                        continue;
                                    }
                                    else
                                    {
                                        // check if managerid in DB
                                        if (!(await repo.EmployeeIdExistAsync(managerId)))
                                        {
                                            Console.WriteLine("Invalid Manager ID");
                                            continue;
                                        }
                                        bool isSetting = employee.EmployeeId == 0;
                                        if (managerId != employee.ManagerId)
                                        {
                                            Console.WriteLine($"Are you sure you want to {(isSetting ? "set" : "change")} employee's manager?");
                                            Console.WriteLine("Y. Yes");
                                            Console.WriteLine("N. No");
                                            Console.WriteLine(">> ");
                                            string? ans = Console.ReadLine();
                                            if (ans == null || ans.ToLower().Equals("no") || ans.ToLower().Equals("n")) continue;
                                            else
                                            {
                                                emp.ManagerId = managerId;
                                                break;
                                            }
                                        }
                                        else { Console.WriteLine("Value is the same"); break; }                                        
                                    }
                                } 
                                else
                                {
                                    Console.WriteLine("Invalid Manager ID");
                                    continue;
                                }
                            }
                            Employee? newRecord = await repo.UpdateEmployeeAsync(optVal, emp);
                            if(newRecord != null)
                            {
                                Console.WriteLine($"\t\t---------------------");
                                Console.WriteLine($"\t\tUpdated User Details:");
                                Console.WriteLine($"\t\t---------------------");
                                Console.WriteLine($"ID: {newRecord.EmployeeId}");
                                Console.WriteLine($"FullName: {newRecord.FirstName} {newRecord.LastName}");
                                Console.WriteLine($"Email: {newRecord.Email}");
                                Console.WriteLine($"Department: {newRecord.Department}");
                                Console.WriteLine($"Salary: GHC {newRecord.Salary:N2}");
                                Console.WriteLine($"Manager: ({newRecord.ManagerId}) {newRecord.ManagerName}");
                                Console.WriteLine($"IsActive: ({newRecord.IsActive})");
                                break;
                            }
                            else
                            {
                                Utilities.Pause();
                                continue;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid option '{opt}'");
                    }
                }
            }

        }
        private async Task DeleteService()
        {
            Console.Clear();
            List<string> str = [
                "--------------------------------------------------------------------------",
                "                             Delete",
                "--------------------------------------------------------------------------"
                ];
            Utilities.DisplayHeader(str);
            List<Employee> employees = await repo.GetEmployeeAsync();
            if (employees.Count == 0)
            {
                Console.WriteLine("No Employee found");
                return;
            }
            foreach (Employee emp in employees)
            {
                Console.WriteLine($"ID: ({emp.EmployeeId})\tName: {emp.FirstName} {emp.LastName}\t\t({emp.Department})");
            }
            while (true)
            {
                Console.WriteLine("Enter the ID of employee to delete");
                Console.Write(">> ");
                string opt = Console.ReadLine()!;
                if (opt.Equals("quit", StringComparison.CurrentCultureIgnoreCase)) return;
                else
                {
                    if (int.TryParse(opt, out int optVal))
                    {
                        Employee? employee = employees.Find((em) => em.EmployeeId == optVal);
                        if (employee == null || employee.EmployeeId == 0)
                        {
                            Console.WriteLine("Invalid Employee ID or Employee not found");
                            continue;
                        }
                        else
                        {
                            int rows = await repo.DeleteEmployeeAsync(optVal);
                            if(rows == 0)
                            {
                                Console.WriteLine("Failed to delete employee. Try again");
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Employee delete successfully");
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
