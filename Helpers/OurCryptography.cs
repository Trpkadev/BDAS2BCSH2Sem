﻿using System.Security.Cryptography;
using System.Text;

namespace BCSH2BDAS2.Helpers;

public sealed class OurCryptography
{
    private static readonly Lazy<OurCryptography> _instance = new(() => new OurCryptography());

    private OurCryptography()
    {
        using Aes aes = Aes.Create();
        aes.GenerateKey();
        Key = aes.Key;
        aes.GenerateIV();
        IV = aes.IV;
    }

    public static OurCryptography Instance { get; } = _instance.Value;
    private byte[] IV { get; init; }
    private byte[] Key { get; init; }

    public static string EncryptHash(string text)
    {
        byte[] data = Encoding.UTF8.GetBytes(text);
        byte[] hash = SHA256.HashData(data);
        return BitConverter.ToString(hash).Replace("-", "");
    }

    public int DecryptId(string encryptedId)
    {
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream ms = new(Convert.FromBase64String(encryptedId));
        using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
        byte[] decryptedBytes = new byte[sizeof(int)];
        _ = cs.Read(decryptedBytes, 0, decryptedBytes.Length);
        return BitConverter.ToInt32(decryptedBytes, 0);
    }

    public string EncryptId(string? plainText)
    {
        if (plainText == null)
            return string.Empty;
        return EncryptIdInner(Encoding.UTF8.GetBytes(plainText));
    }

    public string EncryptId(int id)
    {
        return EncryptIdInner(BitConverter.GetBytes(id));
    }

    private string EncryptIdInner(byte[] bytes)
    {
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
        cs.Write(bytes, 0, bytes.Length);
        cs.FlushFinalBlock();
        return Convert.ToBase64String(ms.ToArray());
    }
}