using System.Security.Cryptography;
using System.Text;

namespace BarricadeNew.Helpers;

public class MasterKeyManager
{
    private static readonly byte[] Salt = Encoding.UTF8.GetBytes("YourUniqueSaltValueHere"); // Sørg for, at denne er unik og konstant

    public static byte[] DeriveKey(string masterKey)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(masterKey, Salt, 100000); // 100000 iterations for security
        return pbkdf2.GetBytes(32); // AES-256 kræver 32 bytes
    }
}