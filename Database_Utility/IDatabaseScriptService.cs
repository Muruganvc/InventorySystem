namespace Database_Utility;
public interface IDatabaseScriptService
{
    List<DatabaseBackupResponse> GenerateFullDatabaseScript(string UserName);
    List<DatabaseBackupResponse> ReadCsv(string filePath);
}


