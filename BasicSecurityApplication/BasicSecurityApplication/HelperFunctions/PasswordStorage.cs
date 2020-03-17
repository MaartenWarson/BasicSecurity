using System;
using System.Security.Cryptography;

namespace ufoTransfer.HelperFunctions
{
    public class PasswordStorage
    {
        public static byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[32];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public static byte[] HashPassword(byte[] toBeHashed, byte[] Salt, int NumberOfRounds)
        {
            using(var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, Salt, NumberOfRounds))
            {
                return rfc2898.GetBytes(32);
            }
        }
    }
}
