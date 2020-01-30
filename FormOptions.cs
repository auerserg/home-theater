using System;
using System.Windows.Forms;
using HomeTheater.Helper;

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
            textBoxDownloadDir.Text = DB.Instance.OptionGet("DownloadDir");
            textBoxNameFiles.Text = DB.Instance.OptionGet("NameFiles");
            numericUpDownSimultaneousDownloads.Value = int.Parse(DB.Instance.OptionGet("SimultaneousDownloads"));
            checkBoxUseProxy.Checked = DB.Instance.OptionGet("proxy.Use") == "1";
            if (checkBoxUseProxy.Checked)
            {
                textBoxProxyAddress.Text = DB.Instance.OptionGet("proxy.Host");
                textBoxProxyPort.Text = DB.Instance.OptionGet("proxy.Port");
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
            DB.Instance.OptionSet("DownloadDir", textBoxDownloadDir.Text);
            DB.Instance.OptionSet("NameFiles", textBoxNameFiles.Text);
            DB.Instance.OptionSet("SimultaneousDownloads", numericUpDownSimultaneousDownloads.Value.ToString());
            DB.Instance.OptionSet("proxy.Use", checkBoxUseProxy.Checked ? "1" : "0");
            DB.Instance.OptionSet("proxy.Host", textBoxProxyAddress.Text);
            DB.Instance.OptionSet("proxy.Port", textBoxProxyPort.Text);
        }

        private void ButtonDownloadFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    textBoxDownloadDir.Text = fbd.SelectedPath;
            }
        }
    }
}