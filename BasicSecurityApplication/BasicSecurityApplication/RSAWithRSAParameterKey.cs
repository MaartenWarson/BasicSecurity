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
    class RSAWithRSAParameterKey
    {
        string path;
        string username = "";
        string receiver = "";
        string filenamePrivate = "";
        string filenamePublic = "";

        public RSAWithRSAParameterKey()
        {
            username = MainWindow.username;
            receiver = MainWindow.receiver;
            SetPathAndFileName();
        }

        public RSAWithRSAParameterKey(string name)
        {
            username = name;
            SetPathAndFileName();
        }

        private void SetPathAndFileName()
        {
            path = Application.StartupPath + @"\Keys";
            filenamePrivate = path + @"\Private_" + username + ".xml";
            filenamePublic = path + @"\Public_" + username + ".xml";
        }

        //GENEREERT PUBLIC & PRIVATE KEY, EN SLAAT DEZE OP
        public void GeneratePrivatePublicKeys()
        {

            //RSACryptoServiceProvider voert asymmetrische encryptie en decryptie uit met behulp van het RSA - algoritme
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048)) //2048 is de sleutellengte
            {
                GenerateFiles();

                rsa.PersistKeyInCsp = false; //False -> We gaan deze niet opslaan in container (maar in XML-file)

                //Als keys al bestaan (om een of andere rede) => verwijderen!
                if (File.Exists(filenamePublic))
                {
                    File.Delete(filenamePublic);
                }

                if (File.Exists(filenamePrivate))
                {
                    File.Delete(filenamePrivate);
                }

                //PUBLIC KEY MAKEN EN OPSLAAN
                File.WriteAllText(filenamePublic, rsa.ToXmlString(false)); //False -> Maakt enkel public key

                //PRIVATE KEY MAKEN EN OPSLAAN
                File.WriteAllText(filenamePrivate, rsa.ToXmlString(true)); //True -> Maakt public en private key
            }
        }

        //FILES VOOR KEYS MAKEN
        private void GenerateFiles()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.Create(filenamePrivate).Close(); //Tekstbestand maken en onmiddellijk sluiten
            File.Create(filenamePublic).Close(); //Tekstbestand maken en onmiddellijk sluiten
        }

        //1) NEEMT BYTE-ARRAY VAN TE ENCRYPTEREN DATA
        //2) ENCRYPTEERT DATA
        //3) GEEFT GEENCRYPTEERDE DATA (cipherbytes)
        public byte[] EncryptData(byte[] dataToEncrypt)
        {
            byte[] cipherbytes;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048)) //2048 = keysize
            {
                rsa.PersistKeyInCsp = false; //Wordt niet in Container gestoken
                rsa.FromXmlString(File.ReadAllText(path + "/Public_" + receiver + ".xml")); //Public key van ontvanger gebruiken

                cipherbytes = rsa.Encrypt(dataToEncrypt, false); //data encrypteren
            }

            return cipherbytes;
        }

        //1) NEEMT BYTE-ARRAY VAN TE DECRYPTEREN DATA
        //2) DECRYPTEERT DATA
        //3) GEEFT GEDECRYPTEERDE DATA (plain)
        public byte[] DecryptData(byte[] dataToEncrypt)
        {
            byte[] plain;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(filenamePrivate)); //Eigen private key gebruiken

                plain = rsa.Decrypt(dataToEncrypt, false);
            }

            return plain;
        }

        public byte[] EncryptHashedData(byte[] hashedDataToEncrypt)
        {
            byte[] cipherbytes;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(path + "/Private_" + username + ".xml")); //Private key van metzelf gebruiken

                cipherbytes = rsa.Encrypt(hashedDataToEncrypt, false);
            }

            return cipherbytes;
        }

        public byte[] DecryptHashedData(byte[] dataToEncrypt, string sender)
        {
            byte[] plain;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                string publicKeySender = path + "/Private_" + sender + ".xml"; //Public key van sender gebruiken
                rsa.FromXmlString(File.ReadAllText(publicKeySender)); 

                plain = rsa.Decrypt(dataToEncrypt, false); //HIER KRIJG IK DE FOUTMELDING DAT DE SLEUTEL NIET BESTAAT
            }

            return plain;
        }

        private string DefineSender(string filename)
        {
            int startIndex = filename.LastIndexOf(@"\") + 14; //Vanaf plaats 13 begint de naam van de zender
            filename = filename.Substring(startIndex);
            string sender = "";

            bool matchWithUnderscore = false;
            int count = 0;

            while (!matchWithUnderscore)
            {
                if (filename[count] != '_')
                {
                    sender += filename[count];
                    count++;
                }
                else
                {
                    matchWithUnderscore = true;
                }
            }

            return sender;
        }
    }
}
