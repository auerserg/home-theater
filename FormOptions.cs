using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HomeTheater.API.Response;
using HomeTheater.Helper;
using Convert = Microsoft.JScript.Convert;

namespace HomeTheater
{
    public partial class FormOptions : Form
    {
        private const int DAY_SECONDS = 24 * 60 * 60;

        private void checkUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            textProxyAddress.Enabled = numericProxyPort.Enabled = checkUseProxy.Checked;
        }

        private void buttonDownloadDir_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(textDownloadDir.Text))
                    fbd.SelectedPath = textDownloadDir.Text;
                var result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    textDownloadDir.Text = fbd.SelectedPath;
            }
        }

        private void labelTimer_Click(object sender, EventArgs e)
        {
        }

        private void checkSilentUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkSilentUpdate00.Checked) checkSilentUpdateCache00.Checked = false;
            if (checkSilentUpdate01.Checked) checkSilentUpdateCache01.Checked = false;
            if (checkSilentUpdate02.Checked) checkSilentUpdateCache02.Checked = false;
            if (checkSilentUpdate10.Checked) checkSilentUpdateCache10.Checked = false;
            if (checkSilentUpdate11.Checked) checkSilentUpdateCache11.Checked = false;
            if (checkSilentUpdate12.Checked) checkSilentUpdateCache12.Checked = false;
            if (checkSilentUpdate20.Checked) checkSilentUpdateCache20.Checked = false;
            if (checkSilentUpdate21.Checked) checkSilentUpdateCache21.Checked = false;
            if (checkSilentUpdate22.Checked) checkSilentUpdateCache22.Checked = false;
            if (checkSilentUpdate30.Checked) checkSilentUpdateCache30.Checked = false;
            if (checkSilentUpdate31.Checked) checkSilentUpdateCache31.Checked = false;
            if (checkSilentUpdate32.Checked) checkSilentUpdateCache32.Checked = false;

            checkSilentUpdateCache00.Enabled = !checkSilentUpdate00.Checked;
            checkSilentUpdateCache01.Enabled = !checkSilentUpdate01.Checked;
            checkSilentUpdateCache02.Enabled = !checkSilentUpdate02.Checked;
            checkSilentUpdateCache10.Enabled = !checkSilentUpdate10.Checked;
            checkSilentUpdateCache11.Enabled = !checkSilentUpdate11.Checked;
            checkSilentUpdateCache12.Enabled = !checkSilentUpdate12.Checked;
            checkSilentUpdateCache20.Enabled = !checkSilentUpdate20.Checked;
            checkSilentUpdateCache21.Enabled = !checkSilentUpdate21.Checked;
            checkSilentUpdateCache22.Enabled = !checkSilentUpdate22.Checked;
            checkSilentUpdateCache30.Enabled = !checkSilentUpdate30.Checked;
            checkSilentUpdateCache31.Enabled = !checkSilentUpdate31.Checked;
            checkSilentUpdateCache32.Enabled = !checkSilentUpdate32.Checked;
        }

        private void buttonResetIgnore_Click(object sender, EventArgs e)
        {
            DB.Instance.OptionSetAsync("oldestIgnore", "[]");
            DB.Instance.OptionSetAsync("oldIgnore", "[]");
            DB.Instance.OptionSetAsync("newIgnore", "[]");
            buttonResetIgnore.Enabled = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            numericOldesDaysSeason.Enabled = checkoldestAllow.Checked;
        }


        #region Form

        public FormOptions()
        {
            InitializeComponent();
        }

        private void FormOptions_Shown(object sender, EventArgs e)
        {
            //Главная
            textDownloadDir.Text = DB.Instance.OptionGet("DownloadDir");
            textNameFiles.Text = DB.Instance.OptionGet("NameFiles");

            //Синхронизация
            numericCachenew.Value = Convert.ToInt32(DB.Instance.OptionGet("cacheTimeSerial_new")) / DAY_SECONDS;
            numericCachewant.Value = Convert.ToInt32(DB.Instance.OptionGet("cacheTimeSerial_want")) / DAY_SECONDS;
            numericCachenonew.Value = Convert.ToInt32(DB.Instance.OptionGet("cacheTimeSerial_nonew")) / DAY_SECONDS;
            numericCachewatched.Value = Convert.ToInt32(DB.Instance.OptionGet("cacheTimeSerial_watched")) / DAY_SECONDS;
            numericCachenone.Value = Convert.ToInt32(DB.Instance.OptionGet("cacheTimeSerial_none")) / DAY_SECONDS;


            pickerTimer.Value = DateTime.Parse("01.01.1753 " + DB.Instance.OptionGet("Timer"));
            numericOldesDaysSeason.Value = Convert.ToInt32(DB.Instance.OptionGet("OldesDaysSeason"));
            checkoldestAllow.Checked = "1" == DB.Instance.OptionGet("oldestAllow");
            checkoldAllow.Checked = "1" == DB.Instance.OptionGet("oldAllow");
            checknewAllow.Checked = "1" == DB.Instance.OptionGet("newAllow");
            checkStopTimer.Checked = "1" == DB.Instance.OptionGet("StopTimer");

            var checkUpdate = new updateCheck(DB.Instance.OptionGet("checkUpdate"));
            checkUpdate00.Checked = checkUpdate[0, 0];
            checkUpdate01.Checked = checkUpdate[0, 1];
            checkUpdate02.Checked = checkUpdate[0, 2];
            checkUpdate10.Checked = checkUpdate[1, 0];
            checkUpdate11.Checked = checkUpdate[1, 1];
            checkUpdate12.Checked = checkUpdate[1, 2];
            checkUpdate20.Checked = checkUpdate[2, 0];
            checkUpdate21.Checked = checkUpdate[2, 1];
            checkUpdate22.Checked = checkUpdate[2, 2];
            checkUpdate30.Checked = checkUpdate[3, 0];
            checkUpdate31.Checked = checkUpdate[3, 1];
            checkUpdate32.Checked = checkUpdate[3, 2];

            checkUpdate = new updateCheck(DB.Instance.OptionGet("checkSilentUpdate"));
            checkSilentUpdate00.Checked = checkUpdate[0, 0];
            checkSilentUpdate01.Checked = checkUpdate[0, 1];
            checkSilentUpdate02.Checked = checkUpdate[0, 2];
            checkSilentUpdate10.Checked = checkUpdate[1, 0];
            checkSilentUpdate11.Checked = checkUpdate[1, 1];
            checkSilentUpdate12.Checked = checkUpdate[1, 2];
            checkSilentUpdate20.Checked = checkUpdate[2, 0];
            checkSilentUpdate21.Checked = checkUpdate[2, 1];
            checkSilentUpdate22.Checked = checkUpdate[2, 2];
            checkSilentUpdate30.Checked = checkUpdate[3, 0];
            checkSilentUpdate31.Checked = checkUpdate[3, 1];
            checkSilentUpdate32.Checked = checkUpdate[3, 2];

            checkUpdate = new updateCheck(DB.Instance.OptionGet("checkSilentUpdateCache"));
            checkSilentUpdateCache00.Checked = checkUpdate[0, 0];
            checkSilentUpdateCache01.Checked = checkUpdate[0, 1];
            checkSilentUpdateCache02.Checked = checkUpdate[0, 2];
            checkSilentUpdateCache10.Checked = checkUpdate[1, 0];
            checkSilentUpdateCache11.Checked = checkUpdate[1, 1];
            checkSilentUpdateCache12.Checked = checkUpdate[1, 2];
            checkSilentUpdateCache20.Checked = checkUpdate[2, 0];
            checkSilentUpdateCache21.Checked = checkUpdate[2, 1];
            checkSilentUpdateCache22.Checked = checkUpdate[2, 2];
            checkSilentUpdateCache30.Checked = checkUpdate[3, 0];
            checkSilentUpdateCache31.Checked = checkUpdate[3, 1];
            checkSilentUpdateCache32.Checked = checkUpdate[3, 2];
            checkSilentUpdate_CheckedChanged(null, null);
            //Перевод
            listTranslateBlackList_Load();
            listTranslate_Load();
            //Сеть
            numericSimultaneousDownloads.Value = Convert.ToInt32(DB.Instance.OptionGet("SimultaneousDownloads"));
            checkUseProxy.Checked = DB.Instance.OptionGet("proxy.Use") == "1";
            if (checkUseProxy.Checked)
            {
                textProxyAddress.Text = DB.Instance.OptionGet("proxy.Host");
                numericProxyPort.Value = Convert.ToInt32(DB.Instance.OptionGet("proxy.Port"));
            }

            checkUseProxy_CheckedChanged(null, null);
            checkBox2_CheckedChanged(null, null);
            buttonResetIgnore.Enabled = false ||
                                        "[]" != DB.Instance.OptionGet("oldestIgnore") ||
                                        "[]" != DB.Instance.OptionGet("oldIgnore") ||
                                        "[]" != DB.Instance.OptionGet("newIgnore");
        }

        private void FormOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Главная
            DB.Instance.OptionSetAsync("DownloadDir", textDownloadDir.Text);
            DB.Instance.OptionSetAsync("NameFiles", textNameFiles.Text);
            //Синхронизация
            var changeTimer = DB.Instance.OptionGet("Timer") != pickerTimer.Value.ToString(DB.TIME_FORMAT);
            DB.Instance.OptionSetAsync("Timer", pickerTimer.Value.ToString(DB.TIME_FORMAT));
            DB.Instance.OptionSetAsync("cacheTimeSerial_new", (int) numericCachenew.Value * DAY_SECONDS);
            DB.Instance.OptionSetAsync("cacheTimeSerial_want", (int) numericCachewant.Value * DAY_SECONDS);
            DB.Instance.OptionSetAsync("cacheTimeSerial_nonew", (int) numericCachenonew.Value * DAY_SECONDS);
            DB.Instance.OptionSetAsync("cacheTimeSerial_watched", (int) numericCachewatched.Value * DAY_SECONDS);
            DB.Instance.OptionSetAsync("cacheTimeSerial_none", (int) numericCachenone.Value * DAY_SECONDS);

            DB.Instance.OptionSetAsync("oldestAllow", checkoldestAllow.Checked);
            DB.Instance.OptionSetAsync("oldAllow", checkoldAllow.Checked);
            DB.Instance.OptionSetAsync("newAllow", checknewAllow.Checked);
            DB.Instance.OptionSetAsync("StopTimer", checkStopTimer.Checked);
            DB.Instance.OptionSetAsync("OldesDaysSeason", (int) numericOldesDaysSeason.Value);

            var checkUpdate = new updateCheck();
            checkUpdate[0, 0] = checkUpdate00.Checked;
            checkUpdate[0, 1] = checkUpdate01.Checked;
            checkUpdate[0, 2] = checkUpdate02.Checked;
            checkUpdate[1, 0] = checkUpdate10.Checked;
            checkUpdate[1, 1] = checkUpdate11.Checked;
            checkUpdate[1, 2] = checkUpdate12.Checked;
            checkUpdate[2, 0] = checkUpdate20.Checked;
            checkUpdate[2, 1] = checkUpdate21.Checked;
            checkUpdate[2, 2] = checkUpdate22.Checked;
            checkUpdate[3, 0] = checkUpdate30.Checked;
            checkUpdate[3, 1] = checkUpdate31.Checked;
            checkUpdate[3, 2] = checkUpdate32.Checked;
            DB.Instance.OptionSetAsync("checkUpdate", checkUpdate.ToString());

            checkUpdate = new updateCheck();
            checkUpdate[0, 0] = checkSilentUpdate00.Checked;
            checkUpdate[0, 1] = checkSilentUpdate01.Checked;
            checkUpdate[0, 2] = checkSilentUpdate02.Checked;
            checkUpdate[1, 0] = checkSilentUpdate10.Checked;
            checkUpdate[1, 1] = checkSilentUpdate11.Checked;
            checkUpdate[1, 2] = checkSilentUpdate12.Checked;
            checkUpdate[2, 0] = checkSilentUpdate20.Checked;
            checkUpdate[2, 1] = checkSilentUpdate21.Checked;
            checkUpdate[2, 2] = checkSilentUpdate22.Checked;
            checkUpdate[3, 0] = checkSilentUpdate30.Checked;
            checkUpdate[3, 1] = checkSilentUpdate31.Checked;
            checkUpdate[3, 2] = checkSilentUpdate32.Checked;
            DB.Instance.OptionSetAsync("checkSilentUpdate", checkUpdate.ToString());

            checkUpdate = new updateCheck();
            checkUpdate[0, 0] = checkSilentUpdateCache00.Checked;
            checkUpdate[0, 1] = checkSilentUpdateCache01.Checked;
            checkUpdate[0, 2] = checkSilentUpdateCache02.Checked;
            checkUpdate[1, 0] = checkSilentUpdateCache10.Checked;
            checkUpdate[1, 1] = checkSilentUpdateCache11.Checked;
            checkUpdate[1, 2] = checkSilentUpdateCache12.Checked;
            checkUpdate[2, 0] = checkSilentUpdateCache20.Checked;
            checkUpdate[2, 1] = checkSilentUpdateCache21.Checked;
            checkUpdate[2, 2] = checkSilentUpdateCache22.Checked;
            checkUpdate[3, 0] = checkSilentUpdateCache30.Checked;
            checkUpdate[3, 1] = checkSilentUpdateCache31.Checked;
            checkUpdate[3, 2] = checkSilentUpdateCache32.Checked;
            DB.Instance.OptionSetAsync("checkSilentUpdateCache", checkUpdate.ToString());
            //Переводы
            DB.Instance.OptionSetAsync("TranslateBlackList",
                SimpleJson.SimpleJson.SerializeObject(listTranslateBlackList.Items));
            DB.Instance.OptionSetAsync("TranslateOrder", SimpleJson.SimpleJson.SerializeObject(listTranslate.Items));
            //Сеть
            DB.Instance.OptionSetAsync("SimultaneousDownloads", numericSimultaneousDownloads.Value.ToString());
            DB.Instance.OptionSetAsync("proxy.Use", checkUseProxy.Checked ? "1" : "0");
            DB.Instance.OptionSetAsync("proxy.Host", textProxyAddress.Text);
            DB.Instance.OptionSetAsync("proxy.Port", numericProxyPort.Value.ToString());


            if (changeTimer)
                (MdiParent as FormMain)?.List.LoadTimer(pickerTimer.Value);
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

        #region Управление переводами

        private void listTranslate_Load()
        {
            var List = new List<string>();
            var str = DB.Instance.OptionGet("TranslateOrder");
            if (!string.IsNullOrWhiteSpace(str))
                List = SimpleJson.SimpleJson.DeserializeObject<List<string>>(str);
            listTranslate.Items.Clear();
            foreach (var item in List)
                if (!listTranslateBlackList.Items.Contains(item) && !listTranslate.Items.Contains(item))
                    listTranslate.Items.Add(item);
            List = DB.Instance.TranslateGetRate();
            foreach (var item in List)
                if (!listTranslateBlackList.Items.Contains(item) && !listTranslate.Items.Contains(item))
                    listTranslate.Items.Add(item);
        }

        private void listTranslateBlackList_Load()
        {
            var List = new List<string>();
            var str = DB.Instance.OptionGet("TranslateBlackList");
            if (!string.IsNullOrWhiteSpace(str))
                List = SimpleJson.SimpleJson.DeserializeObject<List<string>>(str);
            listTranslateBlackList.Items.Clear();
            foreach (var item in List)
                if (!listTranslateBlackList.Items.Contains(item))
                    listTranslateBlackList.Items.Add(item);
        }

        private void buttonRefreh_Click(object sender, EventArgs e)
        {
            var List = DB.Instance.TranslateGetRate();
            listTranslate.BeginUpdate();
            listTranslate.Items.Clear();
            foreach (var item in List)
                if (!listTranslateBlackList.Items.Contains(item) && !listTranslate.Items.Contains(item))
                    listTranslate.Items.Add(item);
            listTranslate.EndUpdate();
        }

        private List<string> listTranslate_GetSelected()
        {
            var list = new List<string>();
            if (null != listTranslate.SelectedItems && 0 < listTranslate.SelectedItems.Count)
                foreach (string item in listTranslate.SelectedItems)
                    if (!list.Contains(item))
                        list.Add(item);
            return list;
        }

        private void listTranslate_MoveSelected(int direction)
        {
            var list = listTranslate_GetSelected();
            var minPositon = 0;
            var maxPositon = listTranslate.Items.Count - 1;
            var invert = 0 < direction;
            for (var i = invert ? list.Count - 1 : 0; invert ? i >= 0 : i < list.Count; i -= direction)
            {
                var item = list[i];
                var index = listTranslate.Items.IndexOf(item);
                var indexNew = index + direction;
                var need = invert ? maxPositon > index : minPositon < index;
                if (need)
                {
                    listTranslate.Items.Remove(item);
                    listTranslate.Items.Insert(indexNew, item);
                    listTranslate.SetSelected(indexNew, true);
                }

                if (invert)
                    maxPositon -= direction;
                else
                    minPositon -= direction;
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            listTranslate_MoveSelected(-1);
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            listTranslate_MoveSelected(1);
        }

        private void buttonToBlackList_Click(object sender, EventArgs e)
        {
            var list = listTranslate_GetSelected();
            foreach (var item in list)
            {
                listTranslateBlackList.Items.Add(item);
                listTranslate.Items.Remove(item);
            }
        }

        private void buttonFromBlackList_Click(object sender, EventArgs e)
        {
            if (null == listTranslateBlackList.SelectedItems || 0 == listTranslateBlackList.SelectedItems.Count)
                return;
            var list = new List<string>();
            foreach (string item in listTranslateBlackList.SelectedItems)
                if (!list.Contains(item))
                    list.Add(item);
            foreach (var item in list)
            {
                listTranslateBlackList.Items.Remove(item);
                listTranslate.Items.Add(item);
            }
        }

        #endregion
    }
}