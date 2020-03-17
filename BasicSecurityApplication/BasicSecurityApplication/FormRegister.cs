using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BasicSecurityApplication
{
    public partial class FormRegister : Form
    {
        FormLogin loginForm;
        public static string username;
        public static string password;
        string path;
        bool userExists;
        StreamReader reader;
        StreamWriter writer;

        //FormLogin KAN GEBRUIKT WORDEN BIJ OPENEN VAN FormRegister
        public FormRegister(FormLogin lf)
        {
            loginForm = lf;
            InitializeComponent();

            path = loginForm.path;

            //labelopmaak aanpassen
            lblNotification.ForeColor = Color.Red;
            lblNotification.Font = new Font(lblNotification.Font, FontStyle.Bold);
        }

        //CREATE-BUTTON ENABLEN ALS USERNAME EN PASSWORD ZIJN INGEVULD
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            btnCreateUser.Enabled = (txtUsername.Text.Trim() != "" && txtPassword.Text.Trim() != "");
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            username = txtUsername.Text;
            password = txtPassword.Text;
            userExists = false;

            char separator = ',';
            string[] words;

            //Controleren of user al bestaat
            try
            {
                reader = File.OpenText(path + "/user.login");
                string line = reader.ReadLine();

                while (line != null && !userExists)
                {
                    words = line.Split(separator);

                    if (words[0] == username)
                    {
                        userExists = true;
                    }
                    else
                    {
                        line = reader.ReadLine();
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Close the user.login-file before registering.", "Close user.login-file");
            }
            finally
            {
                reader.Close();
            }

            if (userExists)
            {
                lblNotification.Text = "This user already exists. Choose another username.";
                return;
            }
            else
            {
                CreateUser();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //NIEUWE USER MAKEN
        private void CreateUser()
        {
            bool containsSpace = false;

            try
            {
                writer = File.AppendText(path + "/user.login");

                //Username controleren en in user.login zetten
                for (int i = 0; i < username.Length; i++)
                {
                    if (username[i] == ' ')
                    {
                        lblNotification.Text = "Username can not contain a space.";
                        containsSpace = true;
                        return;
                    }
                }

                //Password controleren
                for (int i = 0; i < password.Length; i++)
                {
                    if (password[i] == ' ')
                    {
                        lblNotification.Text = "Password can not contain a space.";
                        containsSpace = true;
                        return;
                    }
                }

                if (!containsSpace)
                {
                    writer.Write(username + ",");
                }

                //Salt maken en in user.login zetten
                string salt = PasswordStorage.GenerateSalt();
                writer.Write(salt + ",");

                byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                //Password + salt hashen en in user.login zetten
                string hashedPassword = PasswordStorage.HashPassword(passwordBytes, saltBytes, 50000);
                writer.WriteLine(hashedPassword);
                
                loginForm.userCreated = true;

                //Public en private key van deze gebruiker genereren
                RSAWithRSAParameterKey rsaParams = new RSAWithRSAParameterKey(username);
                rsaParams.GeneratePrivatePublicKeys();

                CreateFolder(); //Folders voor de gebruiker maken
            }
            catch (IOException)
            {
                MessageBox.Show("Close the user.login-file before registering.", "Close user.login-file.");
                return;
            }
            finally
            {
                writer.Close();

                if (!containsSpace)
                {
                    this.Close();
                }
            }
        }

        private void CreateFolder()
        {
            string inbox = Application.StartupPath + @"\Users\" + username + @"\Inbox\";
            string cryptoFolder = Application.StartupPath + @"\Users\" + username + @"\Cryptodata\";
            string hashedInbox = Application.StartupPath + @"\Users\" + username + @"\HashedInbox\";
            string secretImage = Application.StartupPath + @"\Users\" + username + @"\SecretImage\";

            if (!Directory.Exists(inbox))
            {
                Directory.CreateDirectory(inbox);
            }

            if (!Directory.Exists(cryptoFolder))
            {
                Directory.CreateDirectory(cryptoFolder);
            }

            if (!Directory.Exists(hashedInbox))
            {
                Directory.CreateDirectory(hashedInbox);
            }

            if (!Directory.Exists(secretImage))
            {
                Directory.CreateDirectory(secretImage);
            }
        }
    }
}