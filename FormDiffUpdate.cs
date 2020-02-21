using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeTheater.API.Serial;
using HomeTheater.Helper;
using HomeTheater.UI;

namespace HomeTheater
{
    public partial class FormDiffUpdate : Form
    {
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        private Dictionary<int, Season> newSeason = new Dictionary<int, Season>();
        private Dictionary<int, Season> oldestSeason = new Dictionary<int, Season>();
        private Dictionary<int, Season> oldSeason = new Dictionary<int, Season>();

        public FormDiffUpdate(List<int> news, List<int> old, List<int> oldest)
        {
            InitializeComponent();
            UpdateNotice(news, old, oldest);
        }

        private Dictionary<int, Season> listLoadSeason(List<int> list)
        {
            var data = new Dictionary<int, Season>();
            if (null != list && 0 < list.Count)
                foreach (var item in list)
                    data.Add(item, new Season(item));
            return data;
        }

        public void UpdateNotice(List<int> news, List<int> old, List<int> oldest)
        {
            newSeason = listLoadSeason(news);
            oldSeason = listLoadSeason(old);
            oldestSeason = listLoadSeason(oldest);

            groupNew.Visible = 0 < newSeason.Count;
            groupOld.Visible = 0 < oldSeason.Count;
            groupOldest.Visible = 0 < oldestSeason.Count;
            panelMain_ClientSizeChanged(null, null);
            if (0 < newSeason.Count)
                listNew_Load();
            if (0 < oldSeason.Count)
                listOld_Load();
            if (0 < oldestSeason.Count)
                listOldest_Load();
        }

        private void listNew_Load()
        {
            listNew.ListViewItemSorter = new ListViewItemComparer(21, false);
            listNew.BeginUpdate();
            listNew.Items.Clear();
            foreach (var item in newSeason)
            {
                item.Value.ListViewItem.Group = listNew.Groups["listViewGroup" + item.Value.Type];
                listNew.Items.Add(item.Value.ToListViewItem());
            }

            listNew.Sort();
            listNew.EndUpdate();

            foreach (ListViewItem item in listNew.Items)
                item.Checked = true;

            Task.Run(() =>
            {
                try
                {
                    foreach (var item in newSeason)
                    {
                        item.Value.syncPage();
                        Invoke(new Action(() => item.Value.ToListViewItem()));
                        if (tokenSource.Token.IsCancellationRequested)
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            }, tokenSource.Token);
        }

        private void listOld_Load()
        {
            listOld.ListViewItemSorter = new ListViewItemComparer(21, false);
            listOld.BeginUpdate();
            listOld.Items.Clear();
            foreach (var item in oldSeason)
            {
                item.Value.ListViewItem.Group = listOld.Groups["listViewGroup" + item.Value.Type];
                listOld.Items.Add(item.Value.ToListViewItem());
            }

            listOld.Sort();
            listOld.EndUpdate();

            foreach (ListViewItem item in listOld.Items)
                item.Checked = true;
        }

        private void listOldest_Load()
        {
            listOldest.ListViewItemSorter = new ListViewItemComparer(21, false);
            listOldest.BeginUpdate();
            listOldest.Items.Clear();
            foreach (var item in oldestSeason)
                listOldest.Items.Add(item.Value.ToListViewItem());
            listOldest.Sort();
            listOldest.EndUpdate();

            foreach (ListViewItem item in listOldest.Items)
                item.Checked = true;
        }


        private void panelMain_ClientSizeChanged(object sender, EventArgs e)
        {
            if (0 < panelMain.Height)
            {
                var zoneVis = 0;
                if (groupNew.Visible)
                    zoneVis++;
                if (groupOld.Visible)
                    zoneVis++;
                if (groupOldest.Visible)
                    zoneVis++;
                if (0 < zoneVis)
                {
                    var itemH = panelMain.ClientSize.Height / zoneVis;
                    groupNew.Height = groupOld.Height = groupOldest.Height = itemH;
                }
            }

            if (0 < panelMain.Width)
                listOldest.Columns[0].Width = listOld.Columns[0].Width =
                    listNew.Columns[0].Width = listNew.ClientSize.Width - 100 - 100 - 140 - 20;
        }

        private void FormDiffUpdate_FormClosingAsync(object sender, FormClosingEventArgs e)
        {
            var newIgnore = new List<int>();
            var oldIgnore = new List<int>();
            var oldestIgnore = new List<int>();
            foreach (ListViewItem item in listNew.Items)
                if (!item.Checked)
                    newIgnore.Add((int) item.Tag);
            foreach (ListViewItem item in listOld.Items)
                if (!item.Checked)
                    oldIgnore.Add((int) item.Tag);
            foreach (ListViewItem item in listOldest.Items)
                if (!item.Checked)
                    oldestIgnore.Add((int) item.Tag);
            if (0 < newIgnore.Count || 0 < oldIgnore.Count || 0 < oldestIgnore.Count)
            {
                var closeMsg = MessageBox.Show("У вас есть не отмеченные сезоны!\nВы уверены что хотите их оставить?",
                    "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
                if (closeMsg == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            tokenSource.Cancel();
            _ = Task.Run(() =>
            {
                try
                {
                    foreach (var item in newSeason)
                        if (!newIgnore.Contains(item.Key))
                            item.Value.MarkSeWantToSee();
                    foreach (var item in oldestSeason)
                        if (!oldestIgnore.Contains(item.Key))
                            item.Value.MarkSetWatched();
                    foreach (var item in oldSeason)
                        if (!oldestIgnore.Contains(item.Key))
                            item.Value.MarkSetDelSee();
                    DB.Instance.OptionSetAsync("newIgnore", SimpleJson.SimpleJson.SerializeObject(newIgnore));
                    DB.Instance.OptionSetAsync("oldIgnore", SimpleJson.SimpleJson.SerializeObject(oldIgnore));
                    DB.Instance.OptionSetAsync("oldestIgnore", SimpleJson.SimpleJson.SerializeObject(oldestIgnore));
                    DB.Instance.OptionSetAsync("needListUpdate", "1");
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            });
        }
    }
}