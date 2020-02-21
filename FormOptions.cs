using System;
using System.Windows.Forms;
using HomeTheater.Helper;
using Convert = Microsoft.JScript.Convert;

namespace HomeTheater
{
    public partial class FormOptions : Form
    {
        private void checkUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            textProxyAddress.Enabled = numericProxyPort.Enabled = checkUseProxy.Checked;
        }

        private void buttonDownloadDir_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    textDownloadDir.Text = fbd.SelectedPath;
            }
        }


        #region Form

        public FormOptions()
        {
            InitializeComponent();
        }

        private void FormOptions_Shown(object sender, EventArgs e)
        {
            textDownloadDir.Text = DB.Instance.OptionGet("DownloadDir");
            textNameFiles.Text = DB.Instance.OptionGet("NameFiles");
            numericSimultaneousDownloads.Value = Convert.ToInt32(DB.Instance.OptionGet("SimultaneousDownloads"));
            numericTimer.Value = Convert.ToInt32(DB.Instance.OptionGet("Timer"));
            checkUseProxy.Checked = DB.Instance.OptionGet("proxy.Use") == "1";
            if (checkUseProxy.Checked)
            {
                textProxyAddress.Text = DB.Instance.OptionGet("proxy.Host");
                numericProxyPort.Value = Convert.ToInt32(DB.Instance.OptionGet("proxy.Port"));
            }

            checkUseProxy_CheckedChanged(null, null);
        }

        private void FormOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            var changeTimer = DB.Instance.OptionGet("Timer") != numericTimer.Value.ToString();
            DB.Instance.OptionSetAsync("DownloadDir", textDownloadDir.Text);
            DB.Instance.OptionSetAsync("NameFiles", textNameFiles.Text);
            DB.Instance.OptionSetAsync("SimultaneousDownloads", numericSimultaneousDownloads.Value.ToString());
            DB.Instance.OptionSetAsync("Timer", numericTimer.Value.ToString());
            DB.Instance.OptionSetAsync("proxy.Use", checkUseProxy.Checked ? "1" : "0");
            DB.Instance.OptionSetAsync("proxy.Host", textProxyAddress.Text);
            DB.Instance.OptionSetAsync("proxy.Port", numericProxyPort.Value.ToString());
            if (changeTimer)
                (MdiParent as FormMain)?.List.LoadTimer((int) numericTimer.Value);
        }

        #endregion

        #region Tags

        private void textNameFiles_Insert(string insertText = "")
        {
            var selectionIndex = textNameFiles.SelectionStart;
            textNameFiles.Text = textNameFiles.Text.Insert(selectionIndex, insertText);
            textNameFiles.SelectionStart = selectionIndex + insertText.Length;
        }

        private void labelTagType_Click(object sender, EventArgs e)
        {
            textNameFiles_Insert("{Type}");
        }

        private void labelTagCollection_Click(object sender, EventArgs e)
        {
            textNameFiles_Insert("{Сollection}");
        }

        private void labelTagSerialName_Click(object sender, EventArgs e)
        {
            textNameFiles_Insert("{SerialName}");
        }

        private void labelTagSeason_Click(object sender, EventArgs e)
        {
            textNameFiles_Insert("{Season}");
        }

        private void labelTagEpisode_Click(object sender, EventArgs e)
        {
            textNameFiles_Insert("{Episode}");
        }

        private void labelTagTranslate_Click(object sender, EventArgs e)
        {
            textNameFiles_Insert("{Translate}");
        }

        private void labelTagOriginalName_Click(object sender, EventArgs e)
        {
            textNameFiles_Insert("{OriginalName}");
        }

        private void labelTagFormat_Click(object sender, EventArgs e)
        {
            textNameFiles_Insert("{Format}");
        }

        #endregion
    }
}