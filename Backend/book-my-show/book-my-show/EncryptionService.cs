using System.Security.Cryptography;
using System.Text;

namespace book_my_show
{
    public class EncryptionService
    {
        private readonly byte[] key; // 32 bytes for AES-256
        private readonly byte[] iv;  // 16 bytes for AES block size

        //public EncryptionService(string encryptionKey)
        //{
        //    // Convert the string key to byte array
        //    key = Encoding.UTF8.GetBytes(encryptionKey);
        //    iv = new byte[16]; // Initialize IV with zero, can be random as per your security needs
        //}

        //public EncryptionService(byte[] key, byte[] iv)
        //{
        //    this.key = key;
        //    this.iv = iv;
        //}
        public EncryptionService(IConfiguration configuration)
        {
            key = Convert.FromBase64String(configuration["Encryption:Key"]);
            iv = Convert.FromBase64String(configuration["Encryption:IV"]);
        }

        public string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        //private readonly byte[] _key = Encoding.UTF8.GetBytes("WnAN8cST9pMSGx+akihNHQ==");

        //public string Encrypt(string plainText)
        //{

        //    using (var aes = Aes.Create())
        //    {
        //        aes.Key = _key;
        //        aes.GenerateIV();
        //        var iv = aes.IV;

        //        using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
        //        using (var ms = new MemoryStream())
        //        {
        //            ms.Write(iv, 0, iv.Length);
        //            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        //            using (var sw = new StreamWriter(cs))
        //            {
        //                sw.Write(plainText);
        //            }
        //            return Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //}

        //public string Decrypt(string cipherText)
        //{


        //    var fullCipher = Convert.FromBase64String(cipherText);
        //    using (var aes = Aes.Create())
        //    {
        //        var key = Encoding.UTF8.GetBytes("WnAN8cST9pMSGx+akihNHQ==");
        //        aes.Key = key;
        //        aes.IV = new byte[16]; // Use a zero IV for simplicity (not secure for production)

        //        using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
        //        using (var ms = new MemoryStream(fullCipher))
        //        {
        //            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        //            using (var sr = new StreamReader(cs))
        //            {
        //                return sr.ReadToEnd();
        //            }
        //        }
        //    }
        //}
    }
}

