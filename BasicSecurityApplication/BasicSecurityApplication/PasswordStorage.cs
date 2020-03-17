using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace BasicSecurityApplication
{
    class PasswordStorage
    {
        //SALT GENEREREN
        public static string GenerateSalt()
        {
            using (RNGCryptoServiceProvider randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[32];
                randomNumberGenerator.GetBytes(salt);

                return Convert.ToBase64String(salt);
            }
        }

        //HASH GENEREN
        public static string HashPassword(byte[] toBeHashed, byte[] salt, int numberOfRounds)
        {
            using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds))
            {
                return Convert.ToBase64String(rfc2898.GetBytes(32));
            }
        }
    }
}

/*OPMERKINGEN
 * 
 * RNGCryptoServiceProvider: Hier wordt een Random Number Generator gebruikt die heel willekeurig is. We gebruiken
 * RNGCryptoServiceProvider (in plaats van bv. Random) omdat de getallen hier zo random mogelijk moeten zijn. Random is
 * 'simpeler' en volstaat voor de meeste programma's! RNGCryptoServiceProvider is complexer en beter voor beveiliging. Ook
 * heeft deze klasse een GetBytes-methode waarbij een random waarde in de byte-array gezet wordt.
 * Het converten van het bekomen byte-array naar ToBase64String zet de bytes om naar karakters en gehele getallen.
 * 
 * Rfc2898DeriveBytes bevat methoden voor het maken van sleutels/IV met behulp van een wachtwoord en salt.
 * Hier: 
 * - toBeHashed = password en salt = salt
 * - numberOfRounds geeft het aantal iteraties aan dat gedaan moeten worden
 * - 32 geeft het aantal pseudo-willekeurige bytes om te genereren waarop een byte-array gevuld wordt met pseudo-willekeurige
 * keys.
 * - Die bekomen byte wordt omgezet naar een string die teruggegeven wordt.
 * */
