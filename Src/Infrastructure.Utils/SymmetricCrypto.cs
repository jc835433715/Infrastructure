using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Utils
{
    /// <summary>
    /// 对称加密
    /// </summary>
    public class SymmetricCrypto
    {
        /// <summary>
        /// 加密算法名称
        /// </summary>
        public static class AlgorithmName
        {
            /// <summary>
            /// DESCryptoServiceProvider
            /// </summary>
            public const string DESCryptoServiceProvider = "DES";
            /// <summary>
            /// DES
            /// </summary>
            public const string DES = "System.Security.Cryptography.DES";
            /// <summary>
            /// TripleDESCryptoServiceProvider
            /// </summary>
            public const string TripleDESCryptoServiceProvider = "TripleDES";
            /// <summary>
            /// TripleDES
            /// </summary>
            public const string TripleDES = "System.Security.Cryptography.TripleDES";
            /// <summary>
            /// RC2CryptoServiceProvider
            /// </summary>
            public const string RC2CryptoServiceProvider = "System.Security.Cryptography.RC2";
            /// <summary>
            /// RijndaelManaged
            /// </summary>
            public const string RijndaelManaged = "System.Security.Cryptography.Rijndael";
            /// <summary>
            /// AesCryptoServiceProvider
            /// </summary>
            public const string AesCryptoServiceProvider = "System.Security.Cryptography.AesCryptoServiceProvider";
            /// <summary>
            /// AesManaged
            /// </summary>
            public const string AesManaged = "System.Security.Cryptography.AesManaged";
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="algorithmName">加密算法名称</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        public SymmetricCrypto(string algorithmName, string key, string iv)
        {
            SymmetricAlgorithm provider = SymmetricAlgorithm.Create(algorithmName);

            provider.Key = Encoding.Unicode.GetBytes(key);
            provider.IV = Encoding.Unicode.GetBytes(iv);
            encryptor = provider.CreateEncryptor();
            decryptor = provider.CreateDecryptor();
        }

        /// <summary>
        /// 构造方法,采用TripleDES加密算法
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        public SymmetricCrypto(string key, string iv) : this(AlgorithmName.TripleDESCryptoServiceProvider, key, iv) { }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="clearBuffer">明文</param>
        /// <returns>密文</returns>
        public byte[] Encrypt(byte[] clearBuffer)
        {
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(clearBuffer, 0, clearBuffer.Length);
            cryptoStream.FlushFinalBlock();

            return encryptedStream.ToArray();
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="clearText">明文</param>
        /// <returns>密文</returns>
        public string Encrypt(string clearText)
        {
            byte[] clearBuffer = Encoding.Unicode.GetBytes(clearText);

            return Convert.ToBase64String(Encrypt(clearBuffer));
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedBuffer">密文</param>
        /// <returns>明文</returns>
        public byte[] Decrypt(byte[] encryptedBuffer)
        {
            MemoryStream clearStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(clearStream, decryptor, CryptoStreamMode.Write);

            cryptoStream.Write(encryptedBuffer, 0, encryptedBuffer.Length);
            cryptoStream.FlushFinalBlock();

            return clearStream.ToArray();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedText">密文</param>
        /// <returns>明文</returns>
        public string Decrypt(string encryptedText)
        {
            byte[] encryptedBuffer = Convert.FromBase64String(encryptedText);

            return Encoding.Unicode.GetString(Decrypt(encryptedBuffer));
        }

        /// <summary>
        /// 加密,采用TripleDES加密算法
        /// </summary>
        /// <param name="clearText">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>密文</returns>
        public static string Encrypt(string clearText, string key, string iv)
        {
            SymmetricCrypto helper = new SymmetricCrypto(key, iv);

            return helper.Encrypt(clearText);
        }

        /// <summary>
        /// 解密,采用TripleDES加密算法
        /// </summary>
        /// <param name="encryptedText">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encryptedText, string key, string iv)
        {
            SymmetricCrypto helper = new SymmetricCrypto(key, iv);

            return helper.Decrypt(encryptedText);
        }

        private ICryptoTransform encryptor;
        private ICryptoTransform decryptor;
    }
}
