using Microsoft.Data.SqlClient;
using System.Text;

namespace Stock_Maintenance_System_Api.Common;

public interface IDatabaseScriptService
{
    string GenerateScript(string connectionString, string dbName);
}

public class DatabaseScriptService: IDatabaseScriptService
{
    private readonly IConfiguration _configuration;
    public DatabaseScriptService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateScript(string connectionString, string dbName)
    {
        return connectionString;
    }
}
