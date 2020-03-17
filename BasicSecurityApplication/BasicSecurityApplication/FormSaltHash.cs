using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicSecurityApplication
{
    public partial class FormSaltHash : Form
    {
        public FormSaltHash()
        {
            InitializeComponent();
        }

        private void FormSaltHash_Load(object sender, EventArgs e)
        {
            txtUser.Text = FormLogin.username;
            txtSalt.Text = FormLogin.salt;
            txtHash.Text = FormLogin.hashedPassword;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
