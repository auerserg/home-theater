using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeTheater.API;
using HomeTheater.Helper;

namespace HomeTheater
{
    public partial class FormAuth : Form
    {
        private const string TEXT_LOGIN = "E-mail";
        private const string TEXT_PASSWORD = "Пароль";
        private const int DELAY = 100;

        public FormAuth()
        {
            InitializeComponent();
        }

        private FormMain FormParent => Owner as FormMain;

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            var login = textLogin.Text;
            var password = textPassword.Text;
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password)) return;
            buttonEnter.Visible = false;
            AnimationButtonEnter();
            Task.Run(() =>
            {
                var status = false;
                try
                {
                    status = Server.Instance.LogedIn(login, password);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }

                if (status)
                {
                    DB.Instance.OptionSet("Login", login);
                    DB.Instance.OptionSet("Password", password);
                    Invoke(new Action(() =>
                    {
                        buttonEnter.Visible = true;
                        FormParent.StatusMessageAuthSucs();
                        Close();
                        FormParent.SyncAsync(true);
                    }));
                }
                else
                {
                    Invoke(new Action(() =>
                    {
                        buttonEnter.Visible = true;
                        FormParent.StatusMessageAuthError();
                    }));
                }
            });
        }

        private async void AnimationButtonEnter()
        {
            progressAnimation.Visible = true;
            while (buttonEnter.Visible == false)
            {
                while (progressAnimation.Value < progressAnimation.Maximum)
                {
                    progressAnimation.Value++;
                    await Task.Delay(DELAY);
                }

                progressAnimation.Value = 0;
            }

            progressAnimation.Visible = false;
        }

        private void FormAuth_Load(object sender, EventArgs e)
        {
            textLogin.Text = DB.Instance.OptionGet("Login");
            textPassword.Text = DB.Instance.OptionGet("Password");
            textLogin_Leave(sender, e);
            textPassword_Leave(sender, e);
            buttonEnter.Visible = true;
        }

        #region Базовые обработчики полей

        private void textLogin_Enter(object sender, EventArgs e)
        {
            if (textLogin.Text == TEXT_LOGIN) textLogin.Text = "";
        }

        private void textLogin_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textLogin.Text)) textLogin.Text = TEXT_LOGIN;
        }

        private void textLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textPassword.Focus();
                e.Handled = true;
            }
        }

        private void textPassword_Enter(object sender, EventArgs e)
        {
            if (textPassword.Text == TEXT_PASSWORD) textPassword.Text = "";
        }

        private void textPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textPassword.Text)) textPassword.Text = TEXT_PASSWORD;
        }

        private void textPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonEnter_Click(null, null);
                e.Handled = true;
            }
        }

        #endregion

        #region Ссылки

        private void linkLabelReg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Server.Instance.getURLRegister);
        }

        private void linkLabelForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Server.Instance.getURLForgon);
        }

        #endregion
    }
}