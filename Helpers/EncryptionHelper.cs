using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BarricadeNew.Helpers;

public class EncryptionHelper
{
    // Encrypt a plain text string with the provided key
    public static string Encrypt(string plainText, byte[] key)
    {
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentException("Plaintext cannot be null or empty.", nameof(plainText));

        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();
                
        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream();
        memoryStream.Write(aes.IV, 0, aes.IV.Length); // Store IV at the start of the stream
                
        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cryptoStream))
        {
            writer.Write(plainText);
        }

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    // Decrypt an encrypted string with the provided key
    public static string Decrypt(string cipherText, byte[] key)
    {
        if (string.IsNullOrEmpty(cipherText))
            throw new ArgumentException("Ciphertext cannot be null or empty.", nameof(cipherText));

        var cipherBytes = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        using var memoryStream = new MemoryStream(cipherBytes);

        // Read IV from the start of the stream
        var iv = new byte[aes.BlockSize / 8];
        memoryStream.Read(iv, 0, iv.Length);
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream, Encoding.UTF8);

        return reader.ReadToEnd();
    }
}