using HomeTheater.Helper;
using HomeTheater.Serial;
using System;
using System.Windows.Forms;

namespace HomeTheater
{
    public partial class FormAuth : Form
    {
        FormMain mainForm;
        public FormAuth(FormMain mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void TextBoxEmail_Enter(object sender, EventArgs e)
        {
            if (textBoxLogin.Text == "E-mail")
            {
                textBoxLogin.Text = "";
            }
        }

        private void TextBoxEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxLogin.Text))
            {
                textBoxLogin.Text = "E-mail";
            }
        }

        private void TextBoxPassword_Enter(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "Пароль")
            {
                textBoxPassword.Text = "";
            }
        }

        private void TextBoxPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                textBoxPassword.Text = "Пароль";
            }
        }

        private void ButtonEnter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxLogin.Text) || string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                return;
            }
            if (APIServer.Instance.LogedIn(textBoxLogin.Text, textBoxPassword.Text))
            {
                DB.Instance.setOption("Login", textBoxLogin.Text);
                DB.Instance.setOption("Password", textBoxPassword.Text);
                this.Close();
                this.mainForm.Sync(true);
            }
        }

        private void LinkLabelReg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(APIServer.Instance.getURLRegister());
        }

        private void LinkLabelForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(APIServer.Instance.getURLForgon());
        }

        private void FormAuth_Load(object sender, EventArgs e)
        {
            textBoxLogin.Text = DB.Instance.getOption("Login");
            textBoxPassword.Text = DB.Instance.getOption("Password");
            TextBoxEmail_Leave(sender, e);
            TextBoxPassword_Leave(sender, e);
        }
    }
}
