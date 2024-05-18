using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.CryptographyHelper
{
    public class CryptographyHelper
    {
        public static string SHA256WithRSAEncrypt(string data, string privateKey)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            var dataToEncrypt = encoding.GetBytes(data);

            RSACryptoServiceProvider provider = RSAKey.ImportPrivateKey(privateKey);
            byte[] hashBytes = SHA256.HashData(dataToEncrypt);
            byte[] signData = provider.SignHash(hashBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signData);
        }
        public static string Sha1WithRSAEncrypt1(string data, string privateKey)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            var dataToEncrypt = encoding.GetBytes(data);

            RSACryptoServiceProvider provider = RSAKey.ImportPrivateKey(privateKey);

            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] signData = provider.SignData(dataToEncrypt, sh);

            return Convert.ToBase64String(signData);
        }

        public static bool Verify(string content, string signature, string publicKey)
        {
            UTF8Encoding encoding = new ();
            byte[] contentData = encoding.GetBytes(content);
            byte[] data = Convert.FromBase64String(signature);

            RSACryptoServiceProvider provider = RSAKey.ImportPublicKey(publicKey);
            SHA1 sh = new SHA1CryptoServiceProvider();

            return provider.VerifyData(contentData, sh, data);
        }
    }
}
