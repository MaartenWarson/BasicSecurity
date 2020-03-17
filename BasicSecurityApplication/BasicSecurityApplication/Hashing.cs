using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BasicSecurityApplication
{
    class Hashing
    {
        /* SHA256-hasing wordt hier gebruikt */

        public static string ComputeHash(string plainText, byte[] salt)
        {
            const int minSaltLength = 4;
            const int maxSaltLength = 16;

            byte[] saltBytes = null;

            if (salt != null)
            {
                saltBytes = salt;
            }
            else
            {
                Random rand = new Random();
                int saltLength = rand.Next(minSaltLength, maxSaltLength);
                saltBytes = new byte[saltLength];

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetNonZeroBytes(saltBytes);
                rng.Dispose();
            }

            byte[] plainData = ASCIIEncoding.UTF8.GetBytes(plainText);
            byte[] plainDataAndSalt = new byte[plainData.Length + saltBytes.Length];

            for (int i = 0; i < plainData.Length; i++)
            {
                plainDataAndSalt[i] = plainData[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                plainDataAndSalt[plainData.Length + i] = saltBytes[i];
            }

            SHA256Managed sha = new SHA256Managed();
            byte[] hashValue = sha.ComputeHash(plainDataAndSalt);
            sha.Dispose();

            byte[] result = new byte[hashValue.Length + saltBytes.Length];

            for (int i = 0; i < hashValue.Length; i++)
            {
                result[i] = hashValue[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                result[hashValue.Length + i] = saltBytes[i];
            }

            return Convert.ToBase64String(result);
        }

        public static bool Confirm(string plainText, string hashValue)
        {
            byte[] hashBytes = Convert.FromBase64String(hashValue);

            const int hashSize = 32; //voor SHA256
            byte[] saltBytes = new byte[hashBytes.Length - hashSize];

            for (int i = 0; i < saltBytes.Length; i++)
            {
                saltBytes[i] = hashBytes[hashSize + i];
            }

            string newHash = ComputeHash(plainText, saltBytes);

            return (hashValue == newHash);

            //KIJKEN VANAF 22:00
        }
    }
}
