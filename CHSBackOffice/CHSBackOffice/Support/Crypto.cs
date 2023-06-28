using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CHSBackOffice.Support
{
    class Crypto
    {
        static string _password = "rcSuperPassw0rd";
        static string _salt = "aq0dGDsflPKRPPS";

        internal static string Encrypt(string text)
        {
            try
            {
                return Encrypt<RijndaelManaged>(text, _password, _salt);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            return "";
        }

        internal static string Decrypt(string encrypted)
        {
            try
            {
                return Decrypt<RijndaelManaged>(encrypted, _password, _salt);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            return "";
        }

        static string Encrypt<T>(string value, string password, string salt)
            where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T();
            algorithm.Padding = PaddingMode.PKCS7;

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            var transform = algorithm.CreateEncryptor(rgbKey, rgbIV);

            using (var buffer = new MemoryStream())
            {
                using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (var writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        writer.Write(value);
                    }
                }

                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        static string Decrypt<T>(string text, string password, string salt)
            where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new RijndaelManaged();
            algorithm.Padding = PaddingMode.PKCS7;

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);

            using (var buffer = new MemoryStream(Convert.FromBase64String(text)))
            {
                using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                {
                    using (var reader = new StreamReader(stream, Encoding.Unicode))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

    }
}
