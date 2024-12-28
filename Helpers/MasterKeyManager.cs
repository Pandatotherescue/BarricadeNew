using System;
using System.IO;
using System.Security.Cryptography;

namespace BarricadeNew.Helpers;

public static class MasterKeyManager
{
    private static readonly string KeyFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "BarricadeNew",
        "master.key"
    );

    // Retrieve or generate the master key
    public static byte[] GetMasterKey()
    {
        if (File.Exists(KeyFilePath))
        {
            var key = File.ReadAllBytes(KeyFilePath);
            return EnsureCorrectKeyLength(key);
        }

        var newKey = GenerateMasterKey();
        SaveMasterKey(newKey);
        return newKey;
    }

    // Ensure the key is exactly 32 bytes long
    private static byte[] EnsureCorrectKeyLength(byte[] key)
    {
        if (key.Length == 32)
            return key;

        if (key.Length > 32)
            return key[..32]; // Trim key if it's too long

        // Pad the key with zero bytes if it's too short
        var paddedKey = new byte[32];
        Array.Copy(key, paddedKey, key.Length);
        return paddedKey;
    }

    // Generate a new 256-bit key
    private static byte[] GenerateMasterKey()
    {
        using var rng = RandomNumberGenerator.Create();
        var key = new byte[32]; // 256 bits
        rng.GetBytes(key);
        return key;
    }

    // Save the master key securely
    private static void SaveMasterKey(byte[] key)
    {
        var folderPath = Path.GetDirectoryName(KeyFilePath);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath!);

        File.WriteAllBytes(KeyFilePath, key);
        Console.WriteLine(key.Length);
    }
}