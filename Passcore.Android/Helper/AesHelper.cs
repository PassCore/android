using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Passcore.Android.Helper
{
    class AesHelper
    {
        private static byte[] GetKey(string pwd)
            => Sha256Helper.Compute(pwd);

        public static byte[] AES_IV = Encoding.UTF8.GetBytes("0000000000000000");

        public static string Encrypt(string pwd, string data)
        {
            using AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider();
            aesAlg.Key = GetKey(pwd);
            aesAlg.IV = AES_IV;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using MemoryStream msEncrypt = new MemoryStream();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(data);
            }
            byte[] bytes = msEncrypt.ToArray();
            return Convert.ToBase64String(bytes);
        }

        public static string Decrypt(string pwd, string data)
        {
            byte[] inputBytes = Convert.FromBase64String(data);

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = GetKey(pwd);
                aesAlg.IV = AES_IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using MemoryStream msEncrypt = new MemoryStream(inputBytes);
                using CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srEncrypt = new StreamReader(csEncrypt);
                return srEncrypt.ReadToEnd();
            }
        }
    }
}