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
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // 16-byte IV for AES
                new Random().NextBytes(aesAlg.IV);  // Generate random IV for each encryption
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    byte[] iv = aesAlg.IV;
                    byte[] encrypted = msEncrypt.ToArray();

                    // Combine IV and encrypted text to send both to the other side
                    byte[] combined = new byte[iv.Length + encrypted.Length];
                    Array.Copy(iv, 0, combined, 0, iv.Length);
                    Array.Copy(encrypted, 0, combined, iv.Length, encrypted.Length);

                    return Convert.ToBase64String(combined);
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                byte[] combined = Convert.FromBase64String(cipherText);

                // Extract the IV from the combined data
                byte[] iv = new byte[16];
                Array.Copy(combined, 0, iv, 0, iv.Length);

                // Extract the actual encrypted message
                byte[] encryptedMessage = new byte[combined.Length - iv.Length];
                Array.Copy(combined, iv.Length, encryptedMessage, 0, encryptedMessage.Length);

                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedMessage))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}