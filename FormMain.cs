using System;
using System.Diagnostics;
using System.Windows.Forms;
using HomeTheater.API;
using HomeTheater.Helper;

namespace HomeTheater
{
    public partial class FormMain : Form
    {
        public const string ERROR_AUTH = "Пользователь не авторизирован!";
        public const string SUCS_AUTH = "Пользователь авторизировался";
        public FormList List;
        public Stopwatch Timer = new Stopwatch();

        public FormMain()
        {
            InitializeComponent();
        }

        public void SyncAsync(bool list = false, bool serials = false, bool playlists = false,
            bool videos = false)
        {
            _ = List.SyncAsync(list, serials, playlists, videos);
        }

        private void statusMain_ClientSizeChanged(object sender, EventArgs e)
        {
            var width = 0;
            for (var i = 0; i < statusMain.Items.Count; i++)
                width += statusMain.Items[i].Width;
            var widthParent = statusMain.Width - 40 - 100;
            statusTimer.Width = 100;
            for (var i = 0; i < statusMain.Items.Count; i++)
                if ("statusTimer" != statusMain.Items[i].Name)
                    statusMain.Items[i].Width = widthParent * statusMain.Items[i].Width / width;
        }

        #region Методы Формы

        private Form prepareMDI(Form form, bool show = true)
        {
            form.MdiParent = this;
            form.ControlBox = false;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowIcon = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(6);
            form.Activated += Form_Activated;
            if (show)
                form.Show();
            return form;
        }

        private void Form_Activated(object sender, EventArgs e)
        {
            (sender as Form).WindowState = FormWindowState.Maximized;
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            DB.Load();
            statusMain_ClientSizeChanged(null, null);
            List = prepareMDI(new FormList()) as FormList;

            if (Server.Instance.isLogedIn())
            {
                SyncAsync();
            }
            else
            {
                StatusMessageAuthError();
                авторизацияToolStripMenuItem_Click(null, null);
            }
        }

        private void FormMain_MdiChildActivate(object sender, EventArgs e)
        {
            xToolStripMenuItem.Visible = !(ActiveMdiChild is FormList);
        }

        #endregion

        #region StatusMessage

        public void StatusMessageAuthSucs()
        {
            StatusMessageSet(SUCS_AUTH);
        }

        public void StatusMessageAuthError()
        {
            StatusMessageSet(ERROR_AUTH);
        }

        public void StatusTimerSet(double est, double timer = 0)
        {
            var time = TimeSpan.FromMilliseconds(est).ToString(@"hh\:mm\:ss");
            var time2 = 0 < timer ? " / " + TimeSpan.FromMilliseconds(timer).ToString(@"hh\:mm\:ss") : "";

            statusTimer.Text = time + time2;
        }

        public void StatusMessageSet(string status, object arg0 = null, object arg1 = null, object arg2 = null,
            object arg3 = null)
        {
            statusLabelMessage.Text = string.Format(status, arg0, arg1, arg2, arg3);
        }

        public void StatusProgressReset(int Maximum = 0)
        {
            statusProgress.Visible = true;
            statusTimer.Visible = true;
            Timer.Reset();
            Timer.Start();
            statusProgress.Value = 0;
            if (0 < Maximum)
            {
                statusProgress.Minimum = 0;
                statusProgress.Maximum = Maximum;
            }
        }

        public void StatusProgressStep()
        {
            if (statusProgress.Value < statusProgress.Maximum)
                statusProgress.Value++;
            double est = (statusProgress.Maximum - statusProgress.Value) * Timer.ElapsedMilliseconds /
                         statusProgress.Value;
            StatusTimerSet(est, Timer.ElapsedMilliseconds);
        }

        public void StatusProgressEnd()
        {
            Timer.Stop();
            statusProgress.Visible = false;
            statusTimer.Visible = false;
        }

        #endregion

        #region Меню

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = null;
            foreach (var childForm in MdiChildren)
                if (childForm is FormOptions)
                {
                    form = childForm as FormOptions;
                    break;
                }

            if (null != form)
                form.Focus();
            else
                prepareMDI(new FormOptions());
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormAbout();
            form.Owner = this;
            form.ShowDialog();
        }

        public void авторизацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormAuth();
            form.Owner = this;
            form.ShowDialog();
        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form)
                ActiveMdiChild.Close();
        }

        #endregion
    }
}