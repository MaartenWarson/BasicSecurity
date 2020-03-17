using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BasicSecurityApplication
{
    class Hash
    {
        //BOODSCHAP HASHEN
        //public static string HashMessage(string toBeHashed, int numberOfRounds)
        //{
        //    using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(toBeHashed, numberOfRounds))
        //    {
        //        return Convert.ToBase64String(rfc2898.GetBytes(32));
        //    }
        //}

        public static string ToMD5Hash(string source)
        {
            StringBuilder sb = new StringBuilder();

            using (MD5 md5 = MD5.Create())
            {
                byte[] md5HashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(source));

                foreach (byte b in md5HashBytes)
                {
                    sb.Append(b.ToString("X2")); //Print byte al hexadecimaal
                }
            }

            return sb.ToString();
        }
    }
}
