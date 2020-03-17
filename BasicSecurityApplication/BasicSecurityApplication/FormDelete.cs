using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicSecurityApplication
{
    public partial class FormDelete : Form
    {
        FormLogin lf = new FormLogin();
        StreamReader input;
        StreamWriter writer;
        string path = Application.StartupPath + "/Login System";
        int lineCounter = 0;
        string line = "";

        public FormDelete()
        {
            InitializeComponent();

            lbl1.ForeColor = Color.Red;
            lbl1.Font = new Font(lbl1.Font, FontStyle.Bold);
            lbl2.ForeColor = Color.Red;
            lbl2.Font = new Font(lbl2.Font, FontStyle.Bold);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            bool userExists = false;
            string salt = "";
            string hashedPassword = "";
            string hashControl = "";

            try
            {
                //user.login-file openen en lijn inlezen
                input = File.OpenText(path + "/user.login");
                line = input.ReadLine();
                char separator = ',';
                string[] words;

                //salt en hash bepalen van users in bestand
                while (line != null && !userExists)
                {
                    lineCounter++;
                    words = line.Split(separator);

                    if (words[0] == username)
                    {
                        userExists = true;

                        salt = words[1];
                        hashedPassword = words[2];

                        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
                        hashControl = PasswordStorage.HashPassword(passwordBytes, saltBytes, 50000); //Maakt nieuwe hash om te controleren
                    }
                    else
                    {
                        line = input.ReadLine();
                    }

                    input.Close();
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Close the 'user.login'-file before logging in.", "'user.login'-file is open.");
                return;
            }

            if (!userExists)
            {
                lbl1.Text = "This user does not exist.";
                lbl2.Text = "";
                return;
            }

            if (hashedPassword != hashControl)
            {
                lbl1.Text = "The password is incorrect.";
                lbl2.Text = "";
                return;
            }
            else
            {
                DeleteUser(username);

                FormRegister regForm = new FormRegister(lf);
                regForm.Show();

                this.Hide();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() != "" && txtPassword.Text.Trim() != "")
            {
                btnDelete.Enabled = true;
            }
        }

        private void DeleteUser(string username)
        {
            string line = null;
            string lineToDelete = username;

            StreamReader reader = new StreamReader(path + "/user.login");

            while (line != null)
            {
                if (line.Contains(lineToDelete))
                {
                    reader.Close();
                    writer = new StreamWriter(path + "/user.login");


                    writer.WriteLine("Test");
                    writer.Close();
                }

                reader = new StreamReader(path + "/user.login");
                line = reader.ReadLine();

            }


        }
    }
}
