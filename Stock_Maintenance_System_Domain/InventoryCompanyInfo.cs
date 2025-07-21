namespace InventorySystem_Domain;
public class InventoryCompanyInfo
{
    public int InventoryCompanyInfoId { get; set; }
    public string InventoryCompanyInfoName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public string GstNumber { get; set; } = string.Empty;
    public string ApiVersion { get; set; } = string.Empty;
    public string UiVersion { get; set; } = string.Empty;
    public byte[] QcCode { get; set; } = Array.Empty<byte>();
    public string Email { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string BankBranchName { get; set; } = string.Empty;
    public string BankAccountNo { get; set; } = string.Empty;
    public string BankBranchIFSC { get; set; } = string.Empty;
}
