using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicSecurityApplication
{
    public partial class FormSteganography : Form
    {
        string imgName = "";
        string receiver = "";
        string secretImgInbox = "";
        Bitmap bmp = null;
        bool receiverSelected = false;
        bool messageEncoded = false;

        public FormSteganography()
        {
            InitializeComponent();

            lblStatus.ForeColor = Color.Green;
            lblStatus.Font = new Font(lblStatus.Font, FontStyle.Bold);
        }

        //LADEN VAN VENSTER
        private void FormSteganography_Load(object sender, EventArgs e)
        {
            //Controleren of inbox leeg is of niet
            secretImgInbox = Application.StartupPath + @"\Users\" + MainWindow.username + @"\SecretImage\";

            //Als er een document in SecretImage zit, wordt deze knoppen enabled
            if (Directory.GetFiles(secretImgInbox).Length > 0)
            {
                btnOpenSecretImage.Enabled = true;
            }

            //Combobox vullen met users
            for (int i = 0; i < MainWindow.usersArray.Length; i++)
            {
                if (MainWindow.usersArray[i] != null && MainWindow.usersArray[i] != MainWindow.username) //user kan niet naar zichzelf sturen
                {
                    comboboxUsers.Items.Add(MainWindow.usersArray[i]);
                }
            }
        }

        //ENCODE-BUTTON ENABLEN ALS ER TEKST GESCHREVEN IS
        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            btnEncode.Enabled = (txtMessage.Text.Trim() != "");
        }

        //AFBEELDING OPENEN
        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            //Dialoogvenster openen om afbeelding in te laden
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png, *.jpg) | *.png; *.jpg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imgName = openFileDialog.FileName.ToString();
                pictureBox.Image = Image.FromFile(imgName); //img tonen in picturebox

                lblStatus.Visible = true;
                lblStatus.Text = "You have opened an image successfully. Hide a message in it!";
                txtMessage.Enabled = true;
            }
        }

        private void btnOpenSecretImage_Click(object sender, EventArgs e)
        {
            //Dialoogvenster openen om afbeelding in te laden
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png, *.jpg) | *.png; *.jpg";
            openFileDialog.InitialDirectory = secretImgInbox;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imgName = openFileDialog.FileName.ToString();
                pictureBox.Image = Image.FromFile(imgName); //img tonen in picturebox

                lblStatus.Visible = true;
                lblStatus.Text = "You have opened a secret image successfully. Decode the message!";
                txtMessage.Enabled = true;
                btnDecode.Enabled = true;
                btnDecode.Enabled = true;
            }
        }

        //ENCODE
        private void btnEncode_Click(object sender, EventArgs e)
        {
            receiverSelected = false;

            bmp = (Bitmap)pictureBox.Image;
            string text = FormLogin.username + " said: " + txtMessage.Text;

            bmp = SteganographyHelper.EmbedText(text, bmp);

            //Melding
            lblStatus.Visible = true;
            lblStatus.Text = "Your text is hidden in the image successfully. Don't forget to send it!";

            txtMessage.Clear();
            messageEncoded = true;
            comboboxUsers.Enabled = true;

            if (receiverSelected && messageEncoded)
            {
                btnSend.Enabled = true;
            }
        }

        //DECODE
        private void btnDecode_Click(object sender, EventArgs e)
        {
            bmp = (Bitmap)pictureBox.Image;
            string decodedMessage = SteganographyHelper.ExtractText(bmp);

            txtMessage.Text = decodedMessage;
            btnDecode.Enabled = false;
            lblStatus.Text = "Message decoded successfully!";
        }

        //SEND GECODEERDE IMG
        private void btnSend_Click(object sender, EventArgs e)
        {
            //Filename van image maken
            int startIndex = imgName.LastIndexOf(@"\") + 1;
            string foldername = Application.StartupPath + @"\Users\" + receiver + @"\SecretImage\";
            string filename = foldername + imgName.Substring(startIndex);

            //Image opslaan
            Bitmap image = (Bitmap)bmp;
            image.Save(filename, ImageFormat.Png);

            //Label bepalen
            lblStatus.Visible = true;
            lblStatus.Text = $"Your encoded image is sent successfully to {receiver}!";
            comboboxUsers.Enabled = false;
            comboboxUsers.Text = null;
            btnSend.Enabled = false; //Terug disabled maken
        }

        private void comboboxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            receiver = comboboxUsers.Text;
            receiverSelected = true;

            if (receiverSelected && messageEncoded)
            {
                btnSend.Enabled = true;
            }
        }

        //EXIT
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
