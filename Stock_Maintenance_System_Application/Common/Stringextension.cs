using System.Security.Cryptography;
using System.Text;

namespace InventorySystem_Application.Common
{
    public static class StringExtensions
    {
        public static string Decrypt(this string encryptedText, string secretKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

            if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
                throw new ArgumentException("Key must be 16, 24, or 32 bytes long for AES.", nameof(secretKey));

            byte[] cipherBytes = Convert.FromBase64String(encryptedText);

            using var aes = Aes.Create();
            using var ms = new MemoryStream(cipherBytes);

            byte[] iv = new byte[16];
            ms.Read(iv, 0, iv.Length);

            aes.Key = keyBytes;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cs);

            return reader.ReadToEnd();
        }
    }
}
