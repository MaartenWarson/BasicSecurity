using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicSecurityApplication
{
    class HybridEncryption
    {
        private readonly AesEncryption aes = new AesEncryption();

        //ENCRYPTEER DATA
        //original = te encrypteren data
        public EncryptedPacket EncryptData(byte[] original, RSAWithRSAParameterKey rsaParams)
        {
            //Genereert session key
            byte[] sessionKey = aes.GenerateRandomNumber(32);

            //Maakt encryptedPacket en genereert iv
            EncryptedPacket encryptedPacket = new EncryptedPacket { iv = aes.GenerateRandomNumber(16) };

            //Encrypteert data met AES-sleutel
            encryptedPacket.encryptedData = aes.Encrypt(original, sessionKey, encryptedPacket.iv); //Session key en IV gebruiken voor encryptie van data

            //Encrypteert de session key met RSA
            encryptedPacket.encryptedSessionKey = rsaParams.EncryptData(sessionKey);

            return encryptedPacket;
        }

        //NEEMT GEENCRYPTEERD PAKKET (met geencrypteerde data, session key en iv)
        public byte[] DecryptData(byte[] encryptedSessionKey, byte[] encryptedData, byte[] iv, RSAWithRSAParameterKey rsaParams)
        {
            byte[] decryptedData = null;

            //Decrypteer AES-key met RSA + Decrypteren van session key met RSA
            byte[] decryptedSessionKey = rsaParams.DecryptData(encryptedSessionKey);

            //Decrypteer de data met de AES-key
            decryptedData = aes.Decrypt(encryptedData, decryptedSessionKey, iv);

            return decryptedData;
        }
    }
}
