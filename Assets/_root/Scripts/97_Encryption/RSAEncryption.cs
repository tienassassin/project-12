/*
 * RSA Encryption
 * Ref from UnityCipher by TakuKobayashi
 * https://github.com/TakuKobayashi/UnityCipher
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

public class RSAEncryption {
    public static KeyValuePair<string, string> GenerateKeyPair(int keySize) {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize);
        string publicKey = rsa.ToXmlString(false);
        string privateKey = rsa.ToXmlString(true);
        return new KeyValuePair<string, string>(publicKey, privateKey);
    }

    public static string Encrypt(string plane, string publicKey) {
        byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(plane), publicKey);
        return Convert.ToBase64String(encrypted);
    }

    public static byte[] Encrypt(byte[] src, string publicKey) {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
            rsa.FromXmlString(publicKey);
            byte[] encrypted = rsa.Encrypt(src, false);
            return encrypted;
        }
    }

    public static string Decrypt(string encrypted, string privateKey) {
        byte[] decrypted = Decrypt(Convert.FromBase64String(encrypted), privateKey);
        return Encoding.UTF8.GetString(decrypted);
    }

    public static byte[] Decrypt(byte[] src, string privateKey) {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
            rsa.FromXmlString(privateKey);
            byte[] decrypted = rsa.Decrypt(src, false);
            return decrypted;
        }
    }
}