using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Sales.Services
{
    public class DbConnectionFactory(IConfiguration configuration)
    {
        private readonly string _connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "DefaultConnection was not found."
            );

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
