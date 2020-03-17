using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicSecurityApplication
{
    public partial class MainWindow : Form
    {
        FormLogin loginForm;
        OpenFileDialog dialogWindow = new OpenFileDialog();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        StreamReader reader;
        public static string username = "";
        public static string receiver = "";
        public static string[] usersArray = new string[FormLogin.maxUsers];
        string path; //Pad naar user.login
        string loadedFilename = "";
        string filenameToDecrypt = "";
        string myInbox;
        string hashedInbox;
        bool docLoaded = false;
        bool receiverSelected = false;
        bool hashIsEqual;
        FormSteganography formStega = new FormSteganography();
        bool formStegaOpen = false;
        string hashedMessage = "";

        //ZO KAN loginForm GEBRUIKT WORDEN
        public MainWindow(FormLogin lf)
        {
            loginForm = lf;
            InitializeComponent();

            path = loginForm.path; //Pad naar user.login
            username = FormLogin.username;

            lblResult.Visible = false;
        }

        //BIJ LADEN VAN MAINWINDOW
        private void MainWindow_Load(object sender, EventArgs e)
        {
            lblUser.Text = "Logged in as user: " + username;

            //Controleren of inbox leeg is of niet
            myInbox = Application.StartupPath + @"\Users\" + username + @"\Inbox\";

            //Als er een document in Inbox zit, worden deze knoppen enabled
            if (Directory.GetFiles(myInbox).Length > 0)
            {
                btnOpen.Enabled = true;
                btnDecrypt.Enabled = true;
                lblStatusDecrypt.Text = "You have files in your inbox.";
            }

            //Als in HashedInbox een document zit (een geëncrypteerde hash), wordt deze button enabled
            hashedInbox = Application.StartupPath + @"\Users\" + username + @"\HashedInbox\";
            if (Directory.GetFiles(hashedInbox).Length > 0)
            {
                btnCheckHash.Enabled = true;
            }

            //usersArray opvullen
            FillUsersArray();

            //Combobox vullen met users
            for (int i = 0; i < usersArray.Length; i++)
            {
                if (usersArray[i] != null && usersArray[i] != username) //user kan niet naar zichzelf sturen
                {
                    comboboxUsers.Items.Add(usersArray[i]);
                }
            }
        }


        //ARRAY MET USERS (usersArray) VULLEN OM INHOUD VAN COMBOBOX TE BEPALEN
        private void FillUsersArray()
        {
            string[] words = null;
            string[] defenitiveArray = new string[FormLogin.maxUsers * 3]; //Iedere users heeft 3 velden

            //Gegevens uit doc halen en in words zetten
            try
            {
                reader = File.OpenText(path + "/user.login");

                char separator = ',';
                string line = reader.ReadLine();

                while (line != null)
                {
                    words = line.Split(separator); //Users zitten op plaats 0, 3, 6, 9, 12
                    line = reader.ReadLine();

                    //Defenitieve array opvullen
                    int count = 0;
                    for (int i = 0; i < defenitiveArray.Length; i++)
                    {
                        if (defenitiveArray[i] == null && count < 3) //Anders gaat deze buiten bereik van words
                        {
                            defenitiveArray[i] = words[count];
                            count++;
                        }
                    }
                }
                reader.Close();
            }
            catch (IOException)
            {
                MessageBox.Show("Close the user.login-file before logging in.", "Close user.login-file.");
                return;
            }
            finally
            {
                reader.Close();
            }

            //Array met enkel users opvullen
            int teller = 0;
            for (int i = 0; i < defenitiveArray.Length; i += 3)
            {
                usersArray[teller] = defenitiveArray[i];
                teller++;
            }
        }

        //WAARDE IN COMBOBOX AANKLIKKEN
        private void comboboxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            receiver = comboboxUsers.Text;
            receiverSelected = true;

            if (docLoaded)
            {
                btnEncrypt.Enabled = true;
            }
        }

        //TOONT SALT- en HASH-gegevens
        private void btnShowSaltHash_Click(object sender, EventArgs e)
        {
            FormSaltHash fsh = new FormSaltHash();
            fsh.Show();
        }

        //FILE INLADEN
        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            docLoaded = false;

            string startdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialogWindow.InitialDirectory = startdir;
            dialogWindow.Filter = "Text|*.txt|All|*.*";

            if (dialogWindow.ShowDialog() == DialogResult.OK)
            {
                loadedFilename = dialogWindow.FileName;

                //Enkel de naam van het bestand tonen (niet het volledige pad)
                int startIndex = loadedFilename.LastIndexOf(@"\") + 1;
                lblStatus.Text = loadedFilename.Substring(startIndex) + " is loaded.";
                docLoaded = true;

                if (receiverSelected)
                {
                    btnEncrypt.Enabled = true;
                }
            }
        }

        //GELADE FILE ENCRYPTEREN, HASHEN EN VERSTUREN
        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            //Gelade file omzetten naar byte-array
            byte[] dataToEncrypt = File.ReadAllBytes(loadedFilename);

            //Objecten aanmaken die gebruikt moeten worden
            RSAWithRSAParameterKey rsaParams = new RSAWithRSAParameterKey();
            HybridEncryption hybrid = new HybridEncryption();

            //Filename voor encrypted data maken
            int startIndex = loadedFilename.LastIndexOf(@"\") + 1;
            string folderReceiver = Application.StartupPath + @"\Users\" + receiver + @"\Inbox\";
            string encryptedFilename = folderReceiver + "Encr_From" + username + "_" + loadedFilename.Substring(startIndex);

            //ZOWEL DE DATA-, SESSION KEY- EN IV-FILE WORDEN HIERONDER GEHAALD UIT HET ENCRYPTEDPACK
            //Data wordt geencrypteerd + in Inbox van receiver geplaatst
            EncryptedPacket encryptedBlock = hybrid.EncryptData(dataToEncrypt, rsaParams);
            File.WriteAllBytes(encryptedFilename, encryptedBlock.encryptedData);

            //Filename voor session key maken
            string keyFilename = Application.StartupPath + @"\Users\" + receiver + @"\Cryptodata\" + "Encrypted Session Key";
            File.WriteAllBytes(keyFilename, encryptedBlock.encryptedSessionKey);

            //Filename voor iv maken
            string ivFilename = Application.StartupPath + @"\Users\" + receiver + @"\Cryptodata\" + "IV";
            File.WriteAllBytes(ivFilename, encryptedBlock.iv);

            //Hash maken van de oorspronkelijke file
            hashedMessage = Hash.ToMD5Hash(File.ReadAllText(loadedFilename));

            //Hash encrypteren
            EncryptHash(hashedMessage);

            MessageBox.Show($"You have successfully signed, encrypted and sent the file to {receiver}!", "Succesfully hashed, encrypted and sent!");
        }

        //File decrypteren
        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            RSAWithRSAParameterKey rsaParams = new RSAWithRSAParameterKey();
            HybridEncryption hybrid = new HybridEncryption();

            //Session key en IV in string-variabelen zetten + omzetten naar byte-arrays
            string encryptedSessionKeyFile = Application.StartupPath + @"\Users\" + username + @"\Cryptodata\Encrypted Session Key";
            string ivFile = Application.StartupPath + @"\Users\" + username + @"\CryptoData\IV";

            byte[] encryptedSessionKey = null;
            byte[] iv = null;
            try
            {
                encryptedSessionKey = File.ReadAllBytes(encryptedSessionKeyFile);
                iv = File.ReadAllBytes(ivFile);

                //Dialoogvenster openen om een te decrypteren file te openen
                openFileDialog.InitialDirectory = myInbox;
                openFileDialog.Filter = "Text|*.txt|All|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                        filenameToDecrypt = openFileDialog.FileName;
                        byte[] dataToDecrypt = File.ReadAllBytes(filenameToDecrypt); //file omzetten naar byte-array
                        byte[] decryptedData = hybrid.DecryptData(encryptedSessionKey, dataToDecrypt, iv, rsaParams);

                        //Filename van gedecrypteerde boodschap maken
                        string myFolder = Application.StartupPath + @"\Users\" + username + @"\Inbox\";
                        int startIndex = filenameToDecrypt.LastIndexOf(@"\") + 6; //+6 Zodat de naam begint vanaf de "From"
                        int indexUnderscore = filenameToDecrypt.LastIndexOf("_") + 1;
                        string decryptedFilename = myFolder + "Decr_" + filenameToDecrypt.Substring(startIndex);

                        //Gedecrypteerde data in bestand schrijven
                        File.WriteAllBytes(decryptedFilename, decryptedData);

                        MessageBox.Show("You have successfully decrypted the received file!", "Successfully decrypted!");
                }
            }
            catch(FileNotFoundException)
            {
                MessageBox.Show("You have no files to decrypt yet. You were able to click this button because you appear to have a hidden file in your inbox.", "Warning");
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                MessageBox.Show("You can not decrypt this file.", "Not possible to decrypt!");
                return;
            }
        }

        //HASH CHECKEN
        private void btnCheckHash_Click(object sender, EventArgs e)
        {
            //Dialoogvenster aanklikken om een geëncrypteerde hash te openen
            dialogWindow.InitialDirectory = hashedInbox;
            dialogWindow.Filter = "Text|*.txt|All|*.*";
            try
            {
                if (dialogWindow.ShowDialog() == DialogResult.OK)
                {
                    string hashFileToDecrypt = dialogWindow.FileName;
                    string receivedFrom = DefineSender(hashFileToDecrypt); //Bepalen van wie de geëncrypteerde hash komt

                    RSAWithRSAParameterKey rsa = new RSAWithRSAParameterKey();
                    byte[] dataToDecrypt = File.ReadAllBytes(hashFileToDecrypt); //file omzetten naar byte-array
                    byte[] decryptedHashArray = rsa.DecryptHashedData(dataToDecrypt, receivedFrom);
                    string decryptedHash = Encoding.UTF8.GetString(decryptedHashArray);

                    MessageBox.Show("Choose a decrypted file to compare the hash with.", "Choose file.");
                    hashIsEqual = ChooseFileToCompare(decryptedHash);

                    DetermineLabel(hashIsEqual);
                }
            }
            catch (IOException)
            {
                MessageBox.Show($"Close {dialogWindow.FileName} before continueing.", $"Close {dialogWindow.FileName}.");
            }
            catch (Exception)
            {
                MessageBox.Show("This is not a valid enrcypted hash.", "Warning");
            }
            finally
            {
                reader.Close();
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dialogWindow.InitialDirectory = myInbox;
            dialogWindow.Filter = "Text|*.txt|All|*.*";

            try
            {
                if (dialogWindow.ShowDialog() == DialogResult.OK)
                {
                    StreamReader reader = new StreamReader(dialogWindow.FileName, Encoding.Default);
                    txtContent.ForeColor = Color.Black;
                    txtContent.Text = reader.ReadToEnd();

                    int startIndex = dialogWindow.FileName.LastIndexOf(@"\") + 1;
                    lblOpened.Text = "You have opened " + dialogWindow.FileName.Substring(startIndex);
                }
            }
            catch (IOException)
            {
                MessageBox.Show($"Close {dialogWindow.FileName} before continueing.", $"Close {dialogWindow.FileName}.");
            }
            finally
            {
                reader.Close();
            }
        }

        private void EncryptHash(string hashedMessageToEncrypt)
        {
            byte[] hashedMessageBytes = Encoding.UTF8.GetBytes(hashedMessageToEncrypt); //hashed message omzetten naar byte-array

            //Objecten aanmaken die gebruikt moeten worden
            RSAWithRSAParameterKey rsa = new RSAWithRSAParameterKey();
            HybridEncryption hybrid = new HybridEncryption();
            byte[] encryptedHash = rsa.EncryptHashedData(hashedMessageBytes); //Hashed message encrypteren

            //Filename voor encrypted hash maken
            int startIndex = loadedFilename.LastIndexOf(@"\") + 1;
            string folderReceiver = Application.StartupPath + @"\Users\" + receiver + @"\HashedInbox\";
            string encryptedFilename = folderReceiver + "EncrHash_From" + username + "_" + loadedFilename.Substring(startIndex);

            //Geëncrypteerde hashed message in HashedInbox-map plaatsen
            File.WriteAllBytes(encryptedFilename, encryptedHash);
        }

        private string DefineSender(string filename)
        {
            string sender = ""; 
           
            int startIndex = filename.LastIndexOf(@"\") + 14; //Vanaf plaats 13 begint de naam van de zender
            filename = filename.Substring(startIndex);

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

        private bool ChooseFileToCompare(string hash)
        {
            string hashedMessage = "";

            //Opent nieuw dialoogvenster om file aan te klikken om de hash mee te vergelijken
            openFileDialog.InitialDirectory = myInbox;
            openFileDialog.Filter = "Text|*.txt|All|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] toHashMessage = Encoding.UTF8.GetBytes(File.ReadAllText(openFileDialog.FileName));
                hashedMessage = Hash.ToMD5Hash(File.ReadAllText(openFileDialog.FileName)); //File hashen
            }

            if (hashedMessage == hash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DetermineLabel(bool hashIsEqual)
        {
            lblResult.Visible = true;
            lblResult.Font = new Font(lblHashResult.Font, FontStyle.Bold);

            if (hashIsEqual)
            {
                lblResult.ForeColor = Color.Green;
                lblResult.Text = "Hash matches";
            }
            else
            {
                lblResult.ForeColor = Color.Red;
                lblResult.Text = "Hash does not match";
            }
        }

        //UITLOGGEN
        private void btnLogout_Click(object sender, EventArgs e)
        {
            FormLogin lf = new FormLogin();
            lf.Show();

            if (formStegaOpen)
            {
                formStega.Close();
            }
            
            this.Close();
        }

        //PROGRAMMA AFSLUITEN
        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        //STEGANOGRAFIE-VENSTER OPENEN
        private void btnOpenSteganography_Click(object sender, EventArgs e)
        {
            formStega.Show();
            formStegaOpen = true;
        }
    }
}