namespace Database_Utility
{
    public class DatabaseBackupResponse
    {
        public int BackupNo { get; set; }
        public string Creator { get; set; } = string.Empty;
        public DateTime BackUpDate { get; set; }
        public string  FileName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
