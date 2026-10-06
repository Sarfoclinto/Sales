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
                Console.WriteLine("--------------------------------------------------------------------------");
                Console.WriteLine("                             EMPLOYEES");
                Console.WriteLine("--------------------------------------------------------------------------");
                int len = Utilities.DisplayBasicOptions();
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
                                Console.WriteLine("View");
                                Utilities.Pause();
                                break;
                            case 3:
                                Console.WriteLine("Update");
                                Utilities.Pause();
                                break;
                            case 4:
                                Console.WriteLine("Delete");
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
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("                             CREATE");
            Console.WriteLine("--------------------------------------------------------------------------");
            Employee emp = new();

            Console.Write("Enter first name: ");
            emp.FirstName = Console.ReadLine()!;
            
            Console.Write("Enter last name: ");
            emp.LastName = Console.ReadLine()!;

            Console.Write("Enter email: ");
            emp.Email = Console.ReadLine()!;

            Console.Write("Enter department: ");
            emp.Department = Console.ReadLine()!;

            // salary
            while (true)
            {
                Console.Write("Enter salary: ");
                if (double.TryParse(Console.ReadLine(), out double salary))
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
    }
}
