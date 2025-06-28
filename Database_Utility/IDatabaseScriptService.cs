namespace Database_Utility;
public interface IDatabaseScriptService
{
    string GenerateFullDatabaseScript(string connectionString, string dbName, string outputDirectory);
}


