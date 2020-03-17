namespace BasicSecurityApplication
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnShowSaltHash = new System.Windows.Forms.Button();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.comboboxUsers = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.lblStatusDecrypt = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.lblOpened = new System.Windows.Forms.Label();
            this.btnCheckHash = new System.Windows.Forms.Button();
            this.lblHashResult = new System.Windows.Forms.Label();
            this.btnOpenSteganography = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnShowSaltHash
            // 
            this.btnShowSaltHash.Location = new System.Drawing.Point(345, 370);
            this.btnShowSaltHash.Name = "btnShowSaltHash";
            this.btnShowSaltHash.Size = new System.Drawing.Size(176, 23);
            this.btnShowSaltHash.TabIndex = 0;
            this.btnShowSaltHash.Text = "Show Salt and Hash (User)";
            this.btnShowSaltHash.UseVisualStyleBackColor = true;
            this.btnShowSaltHash.Click += new System.EventHandler(this.btnShowSaltHash_Click);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(344, 351);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(94, 13);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "Logged in as user:";
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(12, 21);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(75, 23);
            this.btnLoadFile.TabIndex = 2;
            this.btnLoadFile.Text = "Load File";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(345, 399);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(85, 23);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Log out";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(436, 399);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(85, 23);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Status: ";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(49, 5);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(72, 13);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "No file loaded";
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Enabled = false;
            this.btnEncrypt.Location = new System.Drawing.Point(144, 82);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(121, 23);
            this.btnEncrypt.TabIndex = 9;
            this.btnEncrypt.Text = "Encrypt and Send File";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // comboboxUsers
            // 
            this.comboboxUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboboxUsers.FormattingEnabled = true;
            this.comboboxUsers.Location = new System.Drawing.Point(15, 83);
            this.comboboxUsers.Name = "comboboxUsers";
            this.comboboxUsers.Size = new System.Drawing.Size(121, 21);
            this.comboboxUsers.TabIndex = 10;
            this.comboboxUsers.SelectedIndexChanged += new System.EventHandler(this.comboboxUsers_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(258, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Select the user you want to send an encrypted file to:";
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Enabled = false;
            this.btnDecrypt.Location = new System.Drawing.Point(302, 83);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(82, 23);
            this.btnDecrypt.TabIndex = 13;
            this.btnDecrypt.Text = "Decrypt File";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // lblStatusDecrypt
            // 
            this.lblStatusDecrypt.AutoSize = true;
            this.lblStatusDecrypt.Location = new System.Drawing.Point(299, 64);
            this.lblStatusDecrypt.Name = "lblStatusDecrypt";
            this.lblStatusDecrypt.Size = new System.Drawing.Size(154, 13);
            this.lblStatusDecrypt.TabIndex = 14;
            this.lblStatusDecrypt.Text = "You have no files in your inbox.";
            // 
            // btnOpen
            // 
            this.btnOpen.Enabled = false;
            this.btnOpen.Location = new System.Drawing.Point(390, 83);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(82, 23);
            this.btnOpen.TabIndex = 15;
            this.btnOpen.Text = "Open File";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtContent
            // 
            this.txtContent.BackColor = System.Drawing.SystemColors.Window;
            this.txtContent.ForeColor = System.Drawing.Color.Gray;
            this.txtContent.Location = new System.Drawing.Point(12, 137);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ReadOnly = true;
            this.txtContent.Size = new System.Drawing.Size(255, 285);
            this.txtContent.TabIndex = 16;
            this.txtContent.Text = "After opening a file, the content of this file will be shown here.";
            // 
            // lblOpened
            // 
            this.lblOpened.AutoSize = true;
            this.lblOpened.Location = new System.Drawing.Point(15, 121);
            this.lblOpened.Name = "lblOpened";
            this.lblOpened.Size = new System.Drawing.Size(0, 13);
            this.lblOpened.TabIndex = 17;
            // 
            // btnCheckHash
            // 
            this.btnCheckHash.Enabled = false;
            this.btnCheckHash.Location = new System.Drawing.Point(302, 111);
            this.btnCheckHash.Name = "btnCheckHash";
            this.btnCheckHash.Size = new System.Drawing.Size(82, 23);
            this.btnCheckHash.TabIndex = 18;
            this.btnCheckHash.Text = "Check Hash";
            this.btnCheckHash.UseVisualStyleBackColor = true;
            this.btnCheckHash.Click += new System.EventHandler(this.btnCheckHash_Click);
            // 
            // lblHashResult
            // 
            this.lblHashResult.AutoSize = true;
            this.lblHashResult.Location = new System.Drawing.Point(390, 116);
            this.lblHashResult.Name = "lblHashResult";
            this.lblHashResult.Size = new System.Drawing.Size(0, 13);
            this.lblHashResult.TabIndex = 19;
            // 
            // btnOpenSteganography
            // 
            this.btnOpenSteganography.Location = new System.Drawing.Point(299, 224);
            this.btnOpenSteganography.Name = "btnOpenSteganography";
            this.btnOpenSteganography.Size = new System.Drawing.Size(101, 23);
            this.btnOpenSteganography.TabIndex = 20;
            this.btnOpenSteganography.Text = "Steganography";
            this.btnOpenSteganography.UseVisualStyleBackColor = true;
            this.btnOpenSteganography.Click += new System.EventHandler(this.btnOpenSteganography_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(299, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Click on the Steganography-button to hide ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(299, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "a message in an image.";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(391, 116);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(37, 13);
            this.lblResult.TabIndex = 23;
            this.lblResult.Text = "Result";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 427);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOpenSteganography);
            this.Controls.Add(this.lblHashResult);
            this.Controls.Add(this.btnCheckHash);
            this.Controls.Add(this.lblOpened);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.lblStatusDecrypt);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboboxUsers);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.btnShowSaltHash);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Encrypt and Decrypt";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnShowSaltHash;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.ComboBox comboboxUsers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Label lblStatusDecrypt;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Label lblOpened;
        private System.Windows.Forms.Button btnCheckHash;
        private System.Windows.Forms.Label lblHashResult;
        private System.Windows.Forms.Button btnOpenSteganography;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblResult;
    }
}