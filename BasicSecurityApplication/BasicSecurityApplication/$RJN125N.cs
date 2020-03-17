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
    public partial class FormDelete : Form
    {
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

        }
    }
}
