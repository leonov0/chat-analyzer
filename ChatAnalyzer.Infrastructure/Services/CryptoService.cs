﻿using System.Security.Cryptography;
using ChatAnalyzer.Domain.Interfaces;
using ChatAnalyzer.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace ChatAnalyzer.Infrastructure.Services;

public class CryptoService(IOptions<EncryptionOptions> options) : ICryptoService
{
    private readonly byte[] _iv = Convert.FromBase64String(options.Value.IV);
    private readonly byte[] _key = Convert.FromBase64String(options.Value.Key);

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);
        sw.Write(plainText);
        sw.Close();

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string encryptedText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(Convert.FromBase64String(encryptedText));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}