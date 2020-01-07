using HomeTheater.Helper;
using HomeTheater.Serial;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HomeTheater
{
    public partial class FormMain : Form
    {
        const string ERROR_AUTH = "Пользователь не авторизирован!";
        List<SerialSeason> Serials = new List<SerialSeason>();
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            DB.Load();
        }

        public void АвторизацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAuth form = new FormAuth(this);
            form.Owner = this;
            form.ShowDialog();
        }

        private void НастройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOptions form = new FormOptions();
            form.Owner = this;
            form.ShowDialog();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            if (APIServer.Instance.isLogedIn())
            {
                Sync();
            }
            else
            {
                setStatusMessage(ERROR_AUTH);
                АвторизацияToolStripMenuItem_Click(null, null);
            }
        }

        public void Sync(bool list = false, bool serials = false, bool playlists = false, bool videos = false)
        {
            if (!APIServer.Instance.isLogedIn())
            {
                setStatusMessage(ERROR_AUTH);
                return;
            }
            Serials = APIServer.Instance.getPause(list);

            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = Serials.Count;
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Value = 0;
            for (var i = 0; i < Serials.Count; i++)
            {
                setStatusMessage("Парситься: " + (!string.IsNullOrEmpty(Serials[i].TitleRU) ? Serials[i].TitleRU : Serials[i].serialUrl));
                toolStripProgressBar1.Value = i + 1;
                Serials[i].syncPage(serials);
                Serials[i].syncPlayer(playlists);
                Serials[i].syncPlaylists(videos);
            }
            toolStripProgressBar1.Visible = false;
            setStatusMessage(Serials.Count.ToString() + " сериалов");
        }

        private void UpdateListsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sync(true);
        }

        private void UpdateSerialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sync(false, true);
        }

        private void UpdatePlaylistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sync(false, false, true);
        }

        private void UpdateVideosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sync(false, false, false, true);
        }
        public void setStatusMessage(string message)
        {
            toolStripStatusLabel1.Text = message;
        }

        private void FormMain_ResizeEnd(object sender, EventArgs e)
        {
            int width = statusStripMain.Width - 14;
            toolStripStatusLabel1.Width = Convert.ToInt32(Math.Floor(width * 0.75));
            toolStripProgressBar1.Width = Convert.ToInt32(Math.Floor(width * 0.25));
        }
    }
}
