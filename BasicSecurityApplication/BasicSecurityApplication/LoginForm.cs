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
    public partial class LoginForm : Form
    {
        public string path = Application.StartupPath + "/Login System";
        public bool userExists = false;

        public LoginForm()
        {
            InitializeComponent();
        }

        /* Al dan niet aanmaken van pad en/of folder */
        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(path + "/user.login") && File.ReadAllText(path + "/user.login") != "")
            {
                
            }
        }

        /* btnLogin enablen als in beide textboxes iets geschreven staat */
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            btnLogin.Enabled = (txtUsername.Text.Trim() != "" && txtPassword.Text.Trim() != "");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            userExists = false;

            string username = "";
            string password = "";

            StreamReader input = File.OpenText(path + "/user.login");
            string line = input.ReadLine();

            while (line != null && userExists != true)
            {
                if (line == txtUsername.Text)
                {
                    userExists = true;
                    username = line;
                }

                password = input.ReadLine();
                line = input.ReadLine();
            }

            if (!userExists)
            {
                MessageBox.Show("You haven't created this user yet.", "User not detected");
                return;
            }

            string[] credentials = File.ReadAllLines(path + "/user.login");

            if (txtPassword.Text != password)
            {
                MessageBox.Show("The password is incorrect", "Incorrect password");
                return;
            }

            MessageBox.Show("You are now signed in as " + username + "!");
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm rf = new RegisterForm(this);
            rf.ShowDialog();
        }
    }
}
