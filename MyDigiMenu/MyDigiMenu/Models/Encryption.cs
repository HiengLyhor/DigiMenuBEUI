using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MyDigiMenu.Models
{
    public class Encryption
    {
        private static string key = ConfigurationManager.AppSettings["MyEncryptKey"];
        // Method to encrypt text using AES
        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Key setup (UTF-8, like Java)
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Generate secure IV (16 bytes, matching Java's SecureRandom)
                byte[] iv = new byte[16];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(iv);
                }
                aesAlg.IV = iv;

                // Encrypt using CryptoStream (supports large data)
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                        csEncrypt.FlushFinalBlock(); // Ensure padding is applied
                    }

                    byte[] encryptedBytes = msEncrypt.ToArray();

                    // Combine IV + ciphertext (same as Java)
                    byte[] combined = new byte[iv.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(iv, 0, combined, 0, iv.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, combined, iv.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(combined);
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Key setup (UTF-8, like Java)
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Decode Base64 and split IV + ciphertext
                byte[] combined = Convert.FromBase64String(cipherText);
                byte[] iv = new byte[16];
                byte[] encryptedBytes = new byte[combined.Length - iv.Length];
                Buffer.BlockCopy(combined, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(combined, iv.Length, encryptedBytes, 0, encryptedBytes.Length);
                aesAlg.IV = iv;

                // Decrypt using CryptoStream (supports large data)
                using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd(); // Handles large text (e.g., JWTs)
                }
            }
        }
    }
}