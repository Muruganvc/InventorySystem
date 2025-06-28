using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
namespace Database_Utility;

public class DatabaseScriptService : IDatabaseScriptService
{
    private readonly IConfiguration _configuration;
    public DatabaseScriptService(IConfiguration configuration) => _configuration = configuration;
    public List<DatabaseBackupResponse> GenerateFullDatabaseScript(string userName)
    {
        string? connectionString = _configuration["ConnectionStrings:DefaultConnection"];
        string? outputDirectory = _configuration["appSetting:backUpPath"];

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("❌ Error: Connection string is missing in configuration.");

        if (string.IsNullOrWhiteSpace(outputDirectory))
            throw new Exception("❌ Error: Backup path is missing in configuration.");

        var builder = new SqlConnectionStringBuilder(connectionString);
        string dbName = builder.InitialCatalog;

        var status = BackUp(connectionString, dbName, outputDirectory);

        var fileName = LogAction(userName, DateTime.Now, status.fileName, status.message);
        var response = ReadCsv(fileName);
        return response;
    }

    private (string message, string fileName, bool status) BackUp(string connectionString, string dbName, string outputDirectory)
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = Path.Combine(outputDirectory, $"Backup_{dbName}_{timestamp}.sql");
        try
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            var server = new Server(new ServerConnection(new SqlConnection(connectionString)));
            var database = server.Databases[dbName];
            var schemaName = "dbo";

            // Collect only valid user tables (non-system and has at least one column)
            var tableList = database.Tables.Cast<Table>()
                .Where(t => !t.IsSystemObject && t.Schema == schemaName && t.Columns.Count > 0)
                .ToList();

            // Dependency walker to sort tables based on FK dependencies
            var dependencyWalker = new DependencyWalker(server);
            var collection = new UrnCollection();
            foreach (var table in tableList)
            {
                collection.Add(table.Urn);
            }

            var tree = dependencyWalker.DiscoverDependencies(collection, DependencyType.Parents);
            var sortedList = dependencyWalker.WalkDependencies(tree);

            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                // Step 1: Script Table Schema Only
                var schemaOptions = new ScriptingOptions
                {
                    ScriptSchema = true,
                    ScriptData = false,
                    IncludeHeaders = true,
                    Indexes = true,
                    DriAll = true,
                    SchemaQualify = true,
                    NoCommandTerminator = false,
                    ScriptDrops = false
                };

                foreach (var node in sortedList)
                {
                    if (node.Urn.Type == "Table")
                    {
                        var tableName = node.Urn.GetAttribute("Name");
                        var tableSchema = node.Urn.GetAttribute("Schema");

                        var table = database.Tables[tableName, tableSchema];

                        if (table != null && table.Columns.Count > 0)
                        {
                            var schemaScript = table.Script(schemaOptions);
                            foreach (var line in schemaScript)
                                writer.WriteLine(line);
                        }
                    }
                }

                // Step 2: Script Table Data Only
                var dataOptions = new ScriptingOptions
                {
                    ScriptSchema = false,
                    ScriptData = true,
                    SchemaQualify = true,
                    NoCommandTerminator = false
                };

                foreach (var node in sortedList)
                {
                    if (node.Urn.Type == "Table")
                    {
                        var tableName = node.Urn.GetAttribute("Name");
                        var tableSchema = node.Urn.GetAttribute("Schema");
                        var table = database.Tables[tableName, tableSchema];
                        if (table != null && table.Columns.Count > 0)
                        {
                            var dataScript = table.EnumScript(dataOptions);
                            foreach (var line in dataScript)
                                writer.WriteLine(line);
                        }
                    }
                }
            }
            return (message: "success", fileName, true);
        }
        catch (Exception ex)
        {
            return (message: ex.Message, fileName, false);
        }
    }

    private string LogAction(string name, DateTime date, string FileName, string Status)
    {
        string? fileName =$"{_configuration["appSetting:backUpHistory"]}" ;

        bool fileExists = File.Exists(fileName);

        string? directory = Path.GetDirectoryName(fileName);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (StreamWriter writer = new(fileName, append: true))
        {
            if (!fileExists)
            {
                writer.WriteLine("Creator,Date,FileName,Status");
            }
            string formattedDate = date.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"{Escape(name)},{formattedDate},{Escape(FileName)},{Status}";
            writer.WriteLine(line);
        }
        return fileName;
    }
    private string Escape(string input)
    {
        if (input.Contains(",") || input.Contains("\"") || input.Contains("\n"))
        {
            return $"\"{input.Replace("\"", "\"\"")}\"";
        }
        return input;
    }

    public List<DatabaseBackupResponse> ReadCsv(string filePath)
    {
        var actions = new List<DatabaseBackupResponse>();
        using (var reader = new StreamReader(filePath))
        {
            bool isFirstLine = true;
            int i = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                
                // Skip the header
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }
                var parts = ParseCsvLine(line);
                i++;
                actions.Add(new DatabaseBackupResponse
                {
                    BackupNo = i,
                    Creator = parts[0],
                    BackUpDate = Convert.ToDateTime(parts[1]),
                    FileName = parts[2],
                    Status = parts[3]
                });
            }
        }
        return actions;
    }

    private static string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        bool inQuotes = false;
        string current = "";

        foreach (char c in line)
        {
            if (c == '\"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (c == ',' && !inQuotes)
            {
                result.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }
        result.Add(current);
        return result.ToArray();
    }

     
}