using HomeTheater.Helper;
using HomeTheater.Serial;
using System;
using System.Threading.Tasks;
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
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;
            buttonEnter.Enabled = false;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                return;
            }
            AnimatedButtonEnter();
            Task.Run(() =>
            {
                bool status = APIServer.Instance.LogedIn(login, password);
                if (status)
                {
                    DB.Instance.OptionSet("Login", login);
                    DB.Instance.OptionSet("Password", password);
                    Invoke(new Action(() =>
                    {
                        buttonEnter.Enabled = true;
                        buttonEnter.Text = "Войти";
                        this.mainForm.setStatusMessage(FormMain.SUCS_AUTH);
                        this.Close();
                        this.mainForm.SyncAsync(true);
                    }));
                }
                else
                    Invoke(new Action(() =>
                    {
                        buttonEnter.Enabled = true;
                        buttonEnter.Text = "Войти";
                        this.mainForm.setStatusMessage(FormMain.ERROR_AUTH);
                    }));
            });
        }
        private async void AnimatedButtonEnter()
        {
            string[] animation = new[] { "|", "/", "-", "\\" };
            while (!buttonEnter.Enabled)
                for (int i = 0; i < animation.Length; i++)
                {
                    buttonEnter.Text = animation[i];
                    await Task.Delay(100);
                }
            buttonEnter.Text = "Войти";
        }
        private void LinkLabelReg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(APIServer.Instance.getURLRegister);
        }

        private void LinkLabelForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(APIServer.Instance.getURLForgon);
        }

        private void FormAuth_Load(object sender, EventArgs e)
        {
            textBoxLogin.Text = DB.Instance.OptionGet("Login");
            textBoxPassword.Text = DB.Instance.OptionGet("Password");
            buttonEnter.Text = "Войти";
            TextBoxEmail_Leave(sender, e);
            TextBoxPassword_Leave(sender, e);
        }

        private void textBoxLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxPassword.Focus();
                e.Handled = true;
            }
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ButtonEnter_Click(null, null);
                e.Handled = true;
            }
        }
    }
}
