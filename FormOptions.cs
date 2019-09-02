using HomeTheater.Helper;
using System;
using System.Windows.Forms;

namespace HomeTheater
{
    public partial class FormOptions : Form
    {
        public FormOptions()
        {
            InitializeComponent();
        }

        private void CheckBoxUseProxy_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FormOptions_Load(object sender, EventArgs e)
        {
            textBoxDownloadDir.Text = DB.Instance.getOption("DownloadDir");
            textBoxNameFiles.Text = DB.Instance.getOption("NameFiles");
            numericUpDownSimultaneousDownloads.Value = int.Parse(DB.Instance.getOption("SimultaneousDownloads"));
            checkBoxUseProxy.Checked = DB.Instance.getOption("proxy.Use") == "1";
            if (checkBoxUseProxy.Checked)
            {
                textBoxProxyAddress.Text = DB.Instance.getOption("proxy.Host");
                textBoxProxyPort.Text = DB.Instance.getOption("proxy.Port");
            }
        }

        private void CheckBoxUseProxy_CheckedChanged_1(object sender, EventArgs e)
        {
            textBoxProxyAddress.Enabled = textBoxProxyPort.Enabled = (sender as CheckBox).Checked;
        }

        private void FormOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateSettings();
        }
        private void UpdateSettings()
        {
            DB.Instance.setOption("DownloadDir", textBoxDownloadDir.Text);
            DB.Instance.setOption("NameFiles", textBoxNameFiles.Text);
            DB.Instance.setOption("SimultaneousDownloads", numericUpDownSimultaneousDownloads.Value.ToString());
            DB.Instance.setOption("proxy.Use", checkBoxUseProxy.Checked ? "1" : "0");
            DB.Instance.setOption("proxy.Host", textBoxProxyAddress.Text);
            DB.Instance.setOption("proxy.Port", textBoxProxyPort.Text);
        }

        private void ButtonDownloadFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBoxDownloadDir.Text = fbd.SelectedPath;
                }
            }
        }
    }
}
