namespace Sales.Models
{
    public class EmployeeCreate
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; } = DateTime.Now;
        public int ManagerId { get; set; }

    }
    public class EmployeeUpdate
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; } = DateTime.Now;
        public int ManagerId { get; set; }

    }
    public class Employee : EmployeeCreate
    {
        public int EmployeeId { get; set; }
        public string ManagerName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
