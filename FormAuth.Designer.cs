namespace HomeTheater
{
    partial class FormAuth
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
            this.textLogin = new System.Windows.Forms.TextBox();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.buttonEnter = new System.Windows.Forms.Button();
            this.linkLabelReg = new System.Windows.Forms.LinkLabel();
            this.linkLabelForgot = new System.Windows.Forms.LinkLabel();
            this.progressAnimation = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // textLogin
            // 
            this.textLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textLogin.Location = new System.Drawing.Point(12, 12);
            this.textLogin.Name = "textLogin";
            this.textLogin.Size = new System.Drawing.Size(216, 20);
            this.textLogin.TabIndex = 0;
            this.textLogin.Text = "E-mail";
            this.textLogin.Enter += new System.EventHandler(this.textLogin_Enter);
            this.textLogin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textLogin_KeyDown);
            this.textLogin.Leave += new System.EventHandler(this.textLogin_Leave);
            // 
            // textPassword
            // 
            this.textPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPassword.Location = new System.Drawing.Point(12, 38);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(216, 20);
            this.textPassword.TabIndex = 1;
            this.textPassword.Text = "Пароль";
            this.textPassword.Enter += new System.EventHandler(this.textPassword_Enter);
            this.textPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textPassword_KeyDown);
            this.textPassword.Leave += new System.EventHandler(this.textPassword_Leave);
            // 
            // buttonEnter
            // 
            this.buttonEnter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEnter.Location = new System.Drawing.Point(12, 64);
            this.buttonEnter.Name = "buttonEnter";
            this.buttonEnter.Size = new System.Drawing.Size(216, 23);
            this.buttonEnter.TabIndex = 2;
            this.buttonEnter.Text = "Войти";
            this.buttonEnter.UseVisualStyleBackColor = true;
            this.buttonEnter.Click += new System.EventHandler(this.buttonEnter_Click);
            // 
            // linkLabelReg
            // 
            this.linkLabelReg.AutoSize = true;
            this.linkLabelReg.Location = new System.Drawing.Point(12, 90);
            this.linkLabelReg.Name = "linkLabelReg";
            this.linkLabelReg.Size = new System.Drawing.Size(72, 13);
            this.linkLabelReg.TabIndex = 3;
            this.linkLabelReg.TabStop = true;
            this.linkLabelReg.Text = "Регистрация";
            this.linkLabelReg.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelReg_LinkClicked);
            // 
            // linkLabelForgot
            // 
            this.linkLabelForgot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelForgot.AutoSize = true;
            this.linkLabelForgot.Location = new System.Drawing.Point(137, 90);
            this.linkLabelForgot.Name = "linkLabelForgot";
            this.linkLabelForgot.Size = new System.Drawing.Size(91, 13);
            this.linkLabelForgot.TabIndex = 6;
            this.linkLabelForgot.TabStop = true;
            this.linkLabelForgot.Text = "Забыли пароль?";
            this.linkLabelForgot.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelForgot_LinkClicked);
            // 
            // progressAnimation
            // 
            this.progressAnimation.Location = new System.Drawing.Point(15, 64);
            this.progressAnimation.Maximum = 10;
            this.progressAnimation.Name = "progressAnimation";
            this.progressAnimation.Size = new System.Drawing.Size(213, 23);
            this.progressAnimation.TabIndex = 7;
            // 
            // FormAuth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 113);
            this.Controls.Add(this.linkLabelForgot);
            this.Controls.Add(this.linkLabelReg);
            this.Controls.Add(this.buttonEnter);
            this.Controls.Add(this.textPassword);
            this.Controls.Add(this.textLogin);
            this.Controls.Add(this.progressAnimation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAuth";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Авторизация";
            this.Load += new System.EventHandler(this.FormAuth_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textLogin;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Button buttonEnter;
        private System.Windows.Forms.LinkLabel linkLabelReg;
        private System.Windows.Forms.LinkLabel linkLabelForgot;
        private System.Windows.Forms.ProgressBar progressAnimation;
    }
}