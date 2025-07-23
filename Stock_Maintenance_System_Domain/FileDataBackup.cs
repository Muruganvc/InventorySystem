namespace InventorySystem_Domain;
public class FileDataBackup
{
    public int FileDataBackupId { get; set; }
    public string TableName { get; set; } = string.Empty;
    public byte[] FileData { get; set; } = Array.Empty<byte>();
    public int UniqueId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
}