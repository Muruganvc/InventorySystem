namespace Database_Utility;
public interface IDatabaseScriptService
{
    List<DatabaseBackupResponse> GenerateFullDatabaseScript(string UserName, string historyFileName);
    List<DatabaseBackupResponse> ReadCsv(string filePath);
}


