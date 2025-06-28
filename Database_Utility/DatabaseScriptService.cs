using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
using System.Xml.Linq;
using SMO = Microsoft.SqlServer.Management.Smo;

namespace Database_Utility;

public class DatabaseScriptService : IDatabaseScriptService
{
    public string GenerateFullDatabaseScript(string connectionString, string outputDirectory)
    {
        var success = BackUp(connectionString, outputDirectory);
        return success ? "Backup completed successfully." : "Backup failed.";
    }
    public bool BackUp(string connectionString, string outputDirectory)
    {
       
        try
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            string dbName = builder.InitialCatalog;

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = Path.Combine(outputDirectory, $"Backup_{dbName}_{timestamp}.sql");

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
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"❌ Error during backup: {ex.Message}");
            return false;
        }
    }
}