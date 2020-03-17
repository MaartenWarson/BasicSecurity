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
    public partial class FormLogin : Form
    {
        public string path = Application.StartupPath + "/Login System";
        bool userExists;
        public static string salt = "";
        public static string hashedPassword = "";
        int userCount;
        public static string username;
        string password;
        bool maxUsersReached = false;
        public const int maxUsers = 5;
        public bool userCreated = false;
        private StreamReader reader;

        public FormLogin()
        {
            InitializeComponent();
            reader = null;
        }

        //FOLDER EN/OF PAD BESTAAN NIET? => AANMAKEN BIJ LADEN VAN SCHERM
        private void FormLogin_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(path + "/user.login"))
            {
                File.Create(path + "/user.login").Close();
            }

            //5 (of meer) aangemaakte gebruikers? => Disablet de REGISTER-knop en schrijft note
            CountUsers();
            if (maxUsersReached)
            {
                btnRegister.Enabled = false;
                lblNote.Visible = true;
                lblNote2.Visible = true;
            }
        }

        //LOGIN-BUTTON ENABLEN BIJ INVULLEN VAN USERNAME & PASSWORD
        private void txtUsername_TextChanged_1(object sender, EventArgs e)
        {
            btnLogin.Enabled = (txtUsername.Text.Trim() != "" && txtPassword.Text.Trim() != "");
        }

        //INLOGGEN DOOR KLIK OP LOGIN-BUTTON
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            userExists = false; //Terug op false zetten voor het geval het nog op true staat
            username = txtUsername.Text;
            password = txtPassword.Text;
            userExists = false;

            string hashControl = "";

            //Zoeken naar gebruikersnaam in login-file (om vervolgens in te loggen)
            try
            {
                //user.login-file openen en eerste lijn lezen
                reader = File.OpenText(path + "/user.login");
                string line = reader.ReadLine();
                char separator = ',';
                string[] words;

                while (line != null && !userExists)
                {
                    words = line.Split(separator);

                    //Username-check
                    if (words[0] == username)
                    {
                        userExists = true;

                        //Salt en byte opslaan en omzetten naar byte-array
                        salt = words[1];
                        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
                        hashedPassword = words[2];
                        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                        hashControl = PasswordStorage.HashPassword(passwordBytes, saltBytes, 50000); //Maakt nieuwe hash om te controleren
                                                                                                     //of deze overeenkomt met die wat er werkelijk staat
                    }
                    else
                    {
                        line = reader.ReadLine();
                    }
                }
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

            //labelopmaak aanpassen
            lblNotification.ForeColor = Color.Red;
            lblNotification.Font = new Font(lblNotification.Font, FontStyle.Bold);

            if (!userExists)
            {
                lblNotification.Text = "This user does not exist.";
                return;
            }

            if (hashedPassword != hashControl)
            {
                lblNotification.Text = "The password is incorrect.";
                return;
            }
            else
            {
                MainWindow mainWindow = new MainWindow(this);
                mainWindow.Show();

                this.Hide();
            }
        }

        //NIEUWE USER REGISTREREN
        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            //Registratievenster opent
            FormRegister rf = new FormRegister(this);
            rf.ShowDialog();

            //Als user succesvol gemaakt is (in registratievenster), verschijnt deze tekst in label
            if (userCreated)
            {
                lblNotification.ForeColor = Color.Green;
                lblNotification.Font = new Font(lblNotification.Font, FontStyle.Bold);
                lblNotification.Text = "The user is created successfully!";
            }

            //Als max aantal users bereikt is, verschijnt deze tekst
            CountUsers();
            if (maxUsersReached)
            {
                btnRegister.Enabled = false;
                lblNote.Visible = true;
                lblNote2.Visible = true;
            }

            //Textboxes worden terug leeggemaakt
            txtUsername.Clear();
            txtPassword.Clear();
        }

        //GEBRUIKERS IN LOGIN-FILE TELLEN
        private void CountUsers()
        {
            try
            {
                reader = File.OpenText(path + "/user.login");
                userCount = 0;

                string line = reader.ReadLine();

                while (line != null)
                {
                    userCount++;
                    line = reader.ReadLine();
                }

                if (userCount >= maxUsers)
                {
                    maxUsersReached = true;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Close the user.login-file to continue.", "Close user.login-file.");
            }
            finally
            {
                reader.Close();
            }
        }
    }
}
