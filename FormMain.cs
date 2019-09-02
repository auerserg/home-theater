using HomeTheater.Helper;
using HomeTheater.Serial;
using System;
using System.Windows.Forms;

namespace HomeTheater
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            DB.Load();
        }

        private void АвторизацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAuth form = new FormAuth(this);
            form.ShowDialog();
        }

        private void НастройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOptions form = new FormOptions();
            form.ShowDialog();
        }
        public void SyncSerials()
        {
            Console.WriteLine("Start SyncSerials");
            if (!APIServer.Instance.isLogedIn())
            {
                АвторизацияToolStripMenuItem_Click(null, null);
                return;
            }
            APIServer.Instance.getPause();

            APIServer.Instance.getSidebar();
            APIServer.Instance.getSidebar("pop");
            APIServer.Instance.getSidebar("newest");
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            SyncSerials();
        }
    }
}
