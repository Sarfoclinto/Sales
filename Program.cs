using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sales.Repository;
using Sales.Services;

class Program
{
    public static async void Main()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();

        services.AddSingleton(configuration);
        services.AddSingleton<DbConnectionFactory>();

        services.AddTransient<EmployeeRepo>();
        services.AddTransient<EmployeeServices>();

        using ServiceProvider serviceProvider = services.BuildServiceProvider();

        List<string> options = ["Employees", "Customers", "Suppliers", "Categories", "Products", "Orders", "Payments"];
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("                             SHELL STORE");
            Console.WriteLine("--------------------------------------------------------------------------");
            for(int i = 0; i < 7; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            Console.WriteLine("0. Quit");
            if(int.TryParse(Console.ReadLine(),out int opt))
            {
                if(opt >= 0 && opt <= 7)
                {
                    switch (opt)
                    {
                        case 1:
                            EmployeeServices empSvc = serviceProvider.GetRequiredService<EmployeeServices>();
                            await empSvc.Init();
                            Utilities.Pause();
                            break;
                        case 2:
                            Console.WriteLine("Customers");
                            Utilities.Pause();
                            break;
                        case 3:
                            Console.WriteLine("Suppliers");
                            Utilities.Pause();
                            break;
                        case 4:
                            Console.WriteLine("Categories");
                            Utilities.Pause();
                            break;
                        case 5:
                            Console.WriteLine("Products");
                            Utilities.Pause();
                            break;
                        case 6:
                            Console.WriteLine("Orders");
                            Utilities.Pause();
                            break;
                        case 7:
                            Console.WriteLine("Payments");
                            Utilities.Pause();
                            break;
                        case 0:
                            Utilities.Pause("Thank you for using SHELL STORE. Press any key to quit");
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
}