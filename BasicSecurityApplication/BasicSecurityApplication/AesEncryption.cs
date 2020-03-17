using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicSecurityApplication
{
    class AesEncryption
    {
        //SYMMETRISCHE SESSION KEY MAKEN
        public byte[] GenerateRandomNumber(int length)
        {
            //RNGCryptoServiceProviver is hetzelfde als Random, maar 'complexer' voor betere 'randomness'
            using (RNGCryptoServiceProvider randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[length];
                randomNumberGenerator.GetBytes(randomNumber); //Random waarde in byte-array zetten

                return randomNumber;
            }
        }

        //ENCRYPTEREN
        public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv) //iv = initialization vector
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC; //Voordat ieder tekstblok wordt versleuteld, wordt het combineerd met het vorige blok (XOR)
                aes.Padding = PaddingMode.PKCS7; //De PKCS7-padding string bestaat uit een opeenvolging van bytes, die elk gelijk is aan het totaal aantal toegevoegde padding bytes
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream()) //Gebruikt streams als opslag ipv bv een harde schijf
                {
                    //Maakt een stream waarop cryptografie toegepast kan worden
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                    //aes.CreateEncryptor() => Creëert een symmetrisch AES-encryptor-object mbv de huidige sleutel en IV

                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length); // 0 -> De byte-offset in buffer om te beginnen met het kopiëren van bytes naar de huidige stream
                    cryptoStream.FlushFinalBlock(); //Update de gegevensbron en wist de buffer

                    return memoryStream.ToArray();
                }
            }
        }

        //DECRYPTEREN
        public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    byte[] decryptBytes = memoryStream.ToArray();
                    return decryptBytes;
                }
            }
        }
    }
}
