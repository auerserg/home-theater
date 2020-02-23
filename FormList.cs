using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeTheater.API;
using HomeTheater.API.Serial;
using HomeTheater.Helper;
using HomeTheater.UI;

// ReSharper disable All

namespace HomeTheater
{
    public partial class FormList : Form
    {
        private int currentOrderColumn;
        private bool currentOrderInverted;
        public bool firstRun = true;
        private Dictionary<int, Season> Serials;
        public CountDownTimer timer = new CountDownTimer();

        private FormMain MainParent
        {
            get => this.MdiParent as FormMain;
        }

        #region listDownload

        private void listDownload_ClientSizeChanged(object sender, System.EventArgs e)
        {
            int width = 0;
            for (int i = 0; i < listDownload.Columns.Count; i++)
                width += listDownload.Columns[i].Width;
            int widthParent = listDownload.Width - 25;
            if (0 < widthParent)
            {
                listDownload.BeginUpdate();
                for (int i = 0; i < listDownload.Columns.Count; i++)
                    if (0 < listDownload.Columns[i].Width)
                    {
                        int widthItem = listDownload.Columns[i].Width * widthParent / width;
                        if (0 < widthItem)
                            listDownload.Columns[i].Width = widthItem;
                    }

                listDownload.EndUpdate();
            }
        }

        #endregion

        private void splitContainerMainList_Panel2_ClientSizeChanged(object sender, EventArgs e)
        {
            if (pictureSeasonImage.Width * 2.5 < splitContainerMainList.Panel2.Width)
            {
                panelTexts.Location = new Point(130, 0);
                panelTexts.Width = splitContainerMainList.Panel2.Width * 2 / 3;
                labelDescription.Height = splitContainerMainList.Panel2.Height - 190 * 1 - 5;
            }
            else
            {
                panelTexts.Location = new Point(0, 190);
                panelTexts.Width = splitContainerMainList.Panel2.Width;
                labelDescription.Height = splitContainerMainList.Panel2.Height - 190 * 2 - 5;
            }
        }

        private void linkLabelTitleFULL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureSeasonImage_Click(null, null);
        }

        private void pictureSeasonImage_Click(object sender, EventArgs e)
        {
            Process.Start(pictureSeasonImage.Tag.ToString());
        }

        private void выполнитьСейчасToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = SyncSilent();
        }

        #region Form

        public FormList()
        {
            InitializeComponent();
            menuMain.Visible = false;
        }

        public void LoadTimer(int time = 0)
        {
            if (0 == time)
                time = Convert.ToInt32(DB.Instance.OptionGet("Timer"));
            timer.SetTime(time);
            timer.Reset();
        }

        private void FormList_Load(object sender, EventArgs e)
        {
            timer.TimeChanged += () => toolStripSyncTimer.Text = timer.TimeLeftStr;
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            timer.CountDownFinished += () => SyncSilent();
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            LoadTimer();
        }

        private void FormList_Shown(object sender, System.EventArgs e)
        {
            LoadFormView();
            if (0 < columnSerialSiteUpdated.Width)
                listSerials.ListViewItemSorter = new ListViewItemComparer(columnSerialSiteUpdated.Index, false);
            listSerials_ClientSizeChanged(null, null);
            listDownload_ClientSizeChanged(null, null);
            splitContainerMainList_Panel2_ClientSizeChanged(null, null);
        }

        private void FormList_FormClosing(object sender, FormClosingEventArgs e)
        {
            var listViewSerialsDisplayIndex = new List<int>();
            var listViewSerialsWidth = new List<int>();
            for (var i = 0; i < listSerials.Columns.Count; i++)
            {
                listViewSerialsDisplayIndex.Add(listSerials.Columns[i].DisplayIndex);
                listViewSerialsWidth.Add(listSerials.Columns[i].Width);
            }

            var listViewDownloadWidth = new List<int>();
            for (var i = 0; i < listDownload.Columns.Count; i++)
                listViewDownloadWidth.Add(listDownload.Columns[i].Width);
            DB.Instance.OptionSet("listSerialsDisplayIndex",
                SimpleJson.SimpleJson.SerializeObject(listViewSerialsDisplayIndex));
            DB.Instance.OptionSet("listSerialsWidth", SimpleJson.SimpleJson.SerializeObject(listViewSerialsWidth));
            DB.Instance.OptionSet("listDownloadWidth",
                SimpleJson.SimpleJson.SerializeObject(listViewDownloadWidth));
            tokenSource.Cancel();
            timer.Dispose();
        }

        private void LoadFormView()
        {
            List<int> data = SimpleJson.SimpleJson.DeserializeObject<List<int>>(
                DB.Instance.OptionGet("listSerialsDisplayIndex"));
            int count = data.Count;
            if (listSerials.Columns.Count < count)
                count = listSerials.Columns.Count;
            for (int i = 0; i < count; i++)
                listSerials.Columns[i].DisplayIndex = data[i];
            data = SimpleJson.SimpleJson.DeserializeObject<List<int>>(DB.Instance.OptionGet("listSerialsWidth"));
            count = data.Count;
            if (listSerials.Columns.Count < count)
                count = listSerials.Columns.Count;
            for (int i = 0; i < count; i++)
                listSerials.Columns[i].Width = data[i];
            data = SimpleJson.SimpleJson.DeserializeObject<List<int>>(DB.Instance.OptionGet("listDownloadWidth"));
            count = data.Count;
            if (listDownload.Columns.Count < count)
                count = listDownload.Columns.Count;
            for (int i = 0; i < count; i++)
                listDownload.Columns[i].Width = data[i];

            if (0 < columnSerialTitleFull.Width)
                ToolStripMenuItemSerialTitleFull_Click(null, null);
            else if (0 < columnSerialTitle.Width)
                ToolStripMenuItemSerialTitle_Click(null, null);
            else if (0 < columnSerialTitleRU.Width)
                ToolStripMenuItemSerialTitleRU_Click(null, null);
            else if (0 < columnSerialTitleEN.Width)
                ToolStripMenuItemSerialTitleEN_Click(null, null);
            else if (0 < columnSerialTitleOriginal.Width) ToolStripMenuItemSerialTitleOriginal_Click(null, null);

            ToolStripMenuItemSerialCompilation.Checked = 0 < columnSerialCompilation.Width;
            ToolStripMenuItemSerialCountry.Checked = 0 < columnSerialCountry.Width;
            ToolStripMenuItemSerialGenre.Checked = 0 < columnSerialGenre.Width;
            ToolStripMenuItemSerialIMDB.Checked = 0 < columnSerialIMDB.Width;
            ToolStripMenuItemSerialKinoPoisk.Checked = 0 < columnSerialKinoPoisk.Width;
            ToolStripMenuItemSerialLimitation.Checked = 0 < columnSerialLimitation.Width;
            ToolStripMenuItemSerialMark.Checked = 0 < columnSerialMark.Width;
            ToolStripMenuItemSerialMarkCurrent.Checked = 0 < columnSerialMarkCurrent.Width;
            ToolStripMenuItemSerialMarkLast.Checked = 0 < columnSerialMarkLast.Width;
            ToolStripMenuItemSerialRelease.Checked = 0 < columnSerialRelease.Width;
            ToolStripMenuItemSerialSerialID.Checked = 0 < columnSerialSerialID.Width;
            ToolStripMenuItemSerialserialUrl.Checked = 0 < columnSerialserialUrl.Width;
            ToolStripMenuItemSerialSiteUpdated.Checked = 0 < columnSerialSiteUpdated.Width;
            ToolStripMenuItemSerialUserComments.Checked = 0 < columnSerialUserComments.Width;
            ToolStripMenuItemSerialUserViewsLastDay.Checked = 0 < columnSerialUserViewsLastDay.Width;
        }

        #region Синхронизация

        CancellationTokenSource tokenSource = new CancellationTokenSource();

        private void остановитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }

        private void CancelSync()
        {
            MainParent.StatusProgressEnd();
            MainParent.StatusMessageSet(Serials.Count + " сериалов");
            остановитьToolStripMenuItem.Visible = false;
            _ = LoadTableSerialsAsync();
        }

        public async Task SyncAsync(bool list = false, bool serials = false, bool playlists = false,
            bool videos = false, bool all = false)
        {
            timer.Stop();
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            остановитьToolStripMenuItem.Visible = true;
            MainParent.авторизацияToolStripMenuItem.Enabled = !firstRun;
            await Task.Run(() =>
            {
                try
                {
                    Invoke(new Action(() => MainParent.StatusMessageSet("Проверка авторизации")));
                    bool status = Server.Instance.isLogedIn();
                    if (!status)
                    {
                        Invoke(new Action(() => MainParent.StatusMessageAuthError()));
                        return;
                    }

                    if (token.IsCancellationRequested)
                    {
                        Invoke(new Action(() => CancelSync()));
                        return;
                    }

                    Invoke(new Action(() => MainParent.StatusMessageSet("Получение списка сериалов")));
                    Serials = Server.Instance.getPause(list);

                    if (token.IsCancellationRequested)
                    {
                        Invoke(new Action(() => CancelSync()));
                        return;
                    }

                    Invoke(new Action(() =>
                    {
                        MainParent.StatusMessageSet("Обновление таблицы");
                        _ = RefreshSerials();
                    }));
                    Task.Run(() =>
                    {
                        foreach (KeyValuePair<int, Season> item in Serials)
                            item.Value.SaveAsync();
                    });


                    #region Синхронизация Страниц

                    if (token.IsCancellationRequested)
                    {
                        Invoke(new Action(() => CancelSync()));
                        return;
                    }

                    Invoke(new Action(() => MainParent.StatusProgressReset(Serials.Count)));
                    bool first = true;
                    foreach (KeyValuePair<int, Season> item in Serials)
                    {
                        string title = item.Value.TitleFull;
                        if (string.IsNullOrEmpty(title))
                            title = string.Format(0 < item.Value.SeasonNum ? "{0} {1} сезон" : "{0}",
                                item.Value.TitleRU, item.Value.SeasonNum);
                        Invoke(new Action(() => MainParent.StatusMessageSet("Обработка страницы: " + title)));
                        try
                        {
                            bool forsed = first || serials && "notwatch" != item.Value.Type || all;
                            item.Value.syncPage(forsed);
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.Error(ex);
                        }

                        if (first)
                        {
                            Invoke(new Action(() => Server.Instance.Secure = item.Value.Secure));
                            first = false;
                        }

                        Invoke(new Action(() =>
                        {
                            item.Value.ToListViewItem();
                            MainParent.StatusProgressStep();
                        }));
                        if (token.IsCancellationRequested)
                        {
                            Invoke(new Action(() => CancelSync()));
                            return;
                        }
                    }

                    DB.Instance.SeasonClearOld(new List<int>(Serials.Keys));

                    #endregion

                    if (token.IsCancellationRequested)
                    {
                        Invoke(new Action(() => CancelSync()));
                        return;
                    }

                    #region Синхронизация Плейлистов

                    Invoke(new Action(() => MainParent.StatusProgressReset()));
                    foreach (KeyValuePair<int, Season> item in Serials)
                    {
                        string title = item.Value.TitleFull;
                        Invoke(new Action(() => MainParent.StatusMessageSet("Обработка плейлистов: " + title)));
                        try
                        {
                            item.Value.syncPlayer(
                                playlists && "watched" != item.Value.Type && "notwatch" != item.Value.Type || all);
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.Error(ex);
                        }

                        Invoke(new Action(() =>
                        {
                            //item.Value.ToListViewItem();
                            MainParent.StatusProgressStep();
                        }));
                        if (token.IsCancellationRequested)
                        {
                            Invoke(new Action(() => CancelSync()));
                            return;
                        }
                    }

                    #endregion

                    if (token.IsCancellationRequested)
                    {
                        Invoke(new Action(() => CancelSync()));
                        return;
                    }

                    #region Синхронизация Видео

                    Invoke(new Action(() => MainParent.StatusProgressReset()));
                    foreach (KeyValuePair<int, Season> item in Serials)
                    {
                        string title = item.Value.TitleFull;
                        Invoke(new Action(() => MainParent.StatusMessageSet("Обработка видео: " + title)));
                        if (all || ("watched" != item.Value.Type && "notwatch" != item.Value.Type))
                        {
                            try
                            {
                                item.Value.syncPlaylists(videos || all);
                            }
                            catch (Exception ex)
                            {
                                Logger.Instance.Error(ex);
                            }
                        }

                        Invoke(new Action(() =>
                        {
                            //item.value.tolistviewitem();
                            MainParent.StatusProgressStep();
                        }));
                        if (token.IsCancellationRequested)
                        {
                            Invoke(new Action(() => CancelSync()));
                            return;
                        }
                    }

                    Invoke(new Action(() =>
                    {
                        MainParent.StatusProgressEnd();
                        остановитьToolStripMenuItem.Visible = false;
                    }));

                    #endregion
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            }, token);
            DB.Instance.OptionSetAsync("firstLaunch", "");
            timer.Restart();
            await LoadTableSerialsAsync();
            _ = SyncUpdateList();
            firstRun = false;
            toolStripSyncTimer.Visible = true;
            остановитьToolStripMenuItem.Visible = false;
            обновитьToolStripMenuItem.Enabled = true;
            toolStripUpdate.Enabled = true;
            MainParent.авторизацияToolStripMenuItem.Enabled = true;
        }

        public async Task SyncSilent()
        {
            timer.Stop();
            выполнитьСейчасToolStripMenuItem.Visible = false;
            toolStripSyncTimer.Enabled = false;
            toolStripSyncTimer.Text = "--:--";
            toolStripSyncTimer.ToolTipText = "";
            await Task.Run(() =>
            {
                try
                {
                    Serials = Server.Instance.getPause(true);

                    Invoke(new Action(() => { _ = RefreshSerials(); }));
                    Task.Run(() =>
                    {
                        try
                        {
                            List<int> IDs = new List<int>();
                            bool first = true;
                            foreach (KeyValuePair<int, Season> item in Serials)
                            {
                                try
                                {
                                    item.Value.syncPage(first);
                                    item.Value.SaveAsync();
                                }
                                catch (Exception ex)
                                {
                                    Logger.Instance.Error(ex);
                                }

                                if (first)
                                {
                                    Invoke(new Action(() => Server.Instance.Secure = item.Value.Secure));
                                    first = false;
                                }

                                IDs.Add(item.Key);
                            }

                            DB.Instance.SeasonClearOld(IDs);
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.Error(ex);
                        }
                    });
                    var incList = DB.Instance.VideoGetTemp();
                    int maxind = 0;
                    foreach (KeyValuePair<int, Season> item in Serials)
                        if ("new" == item.Value.Type || "want" == item.Value.Type || incList.Contains(item.Key))
                            maxind++;
                    var silentTimer = new Stopwatch();
                    int ind = 0;
                    silentTimer.Start();
                    foreach (KeyValuePair<int, Season> item in Serials)
                    {
                        if ("new" == item.Value.Type || "want" == item.Value.Type || incList.Contains(item.Key))
                        {
                            try
                            {
                                item.Value.syncPlayer(true);
                                item.Value.syncPlaylists(true);
                            }
                            catch (Exception ex)
                            {
                                Logger.Instance.Error(ex);
                            }

                            ind++;
                            double estD = (maxind - ind) * silentTimer.ElapsedMilliseconds / ind;
                            string est = TimeSpan.FromMilliseconds(estD).ToString(@"\-mm\:ss");
                            Invoke(new Action(() => toolStripSyncTimer.Text = est));
                        }
                    }

                    silentTimer.Stop();
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            });

            timer.Restart();
            выполнитьСейчасToolStripMenuItem.Visible = true;
            toolStripSyncTimer.Enabled = true;
            toolStripSyncTimer.ToolTipText = "";
            await LoadTableSerialsAsync();
        }

        public async Task SyncUpdateList()
        {
            await Task.Run(() =>
            {
                try
                {
                    var option = DB.Instance.OptionGet("newIgnore");
                    List<int> newIgnore = string.IsNullOrEmpty(option)
                        ? new List<int>()
                        : SimpleJson.SimpleJson.DeserializeObject<List<int>>(option);
                    option = DB.Instance.OptionGet("oldIgnore");
                    List<int> oldIgnore = string.IsNullOrEmpty(option)
                        ? new List<int>()
                        : SimpleJson.SimpleJson.DeserializeObject<List<int>>(option);
                    option = DB.Instance.OptionGet("oldestIgnore");
                    List<int> oldestIgnore = string.IsNullOrEmpty(option)
                        ? new List<int>()
                        : SimpleJson.SimpleJson.DeserializeObject<List<int>>(option);

                    var NewSeasons = new List<int>();
                    var OldSeasons = new List<int>();
                    var OldestSeasons = new List<int>();
                    var OldInterval = Convert.ToInt32(DB.Instance.OptionGet("OldesDaysSeason"));

                    foreach (var item in Serials)
                    {
                        if (0 < item.Value.Seasons.Count)
                        {
                            var seeason = item.Value.SeasonNum;
                            foreach (var _item in item.Value.Seasons)
                            {
                                if (seeason < _item.Key)
                                {
                                    if (Serials.ContainsKey(_item.Value))
                                    {
                                        if (
                                            (
                                                (
                                                    "new" != item.Value.Type &&
                                                    "want" != item.Value.Type
                                                ) ||
                                                (
                                                    "new" == item.Value.Type &&
                                                    item.Value.MarkLast == item.Value.MarkCurrent
                                                )
                                            ) &&
                                            !oldIgnore.Contains(item.Key) &&
                                            !OldSeasons.Contains(item.Key) &&
#pragma warning disable CA1820 // Проверяйте наличие пустых строк, используя длину строки
                                            "" != Serials[_item.Value].MarkLast
#pragma warning restore CA1820 // Проверяйте наличие пустых строк, используя длину строки
                                        )
                                            OldSeasons.Add(item.Key);
                                    }
                                    else if (!newIgnore.Contains(_item.Value) &&
                                             !NewSeasons.Contains(_item.Value))
                                        NewSeasons.Add(_item.Value);
                                }
                            }
                        }

                        if ("nonew" == item.Value.Type &&
                            !oldestIgnore.Contains(item.Key) &&
                            !OldestSeasons.Contains(item.Key) &&
                            OldInterval < DateTime.UtcNow.Subtract(item.Value.SiteUpdated).TotalDays)
                            OldestSeasons.Add(item.Key);
                    }

                    if (0 < NewSeasons.Count || 0 < OldSeasons.Count || 0 < OldestSeasons.Count)
                        Invoke(new Action(() =>
                        {
                            try
                            {
                                MainParent.FormMain_ShowDiffUpdate(NewSeasons, OldSeasons, OldestSeasons);
                            }
                            catch (Exception ex)
                            {
                                Logger.Instance.Error(ex);
                            }
                        }));
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            });
        }

        public async Task LoadTableSerialsAsync(bool load = false)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (load)
                        foreach (KeyValuePair<int, Season> item in Serials)
                        {
                            item.Value.Load();
                        }

                    Invoke(new Action(() =>
                    {
                        MainParent.StatusProgressReset(Serials.Count);
                        MainParent.StatusMessageSet("Обновление таблицы...");
                    }));
                    foreach (KeyValuePair<int, Season> item in Serials)
                    {
                        Invoke(new Action(() =>
                        {
                            item.Value.ToListViewItem();
                            item.Value.ListViewItem.Group = listSerials.Groups["listViewGroup" + item.Value.Type];
                            if (item.Value.URL == pictureSeasonImage.Tag as string)
                            {
                                item.Value.ListViewItem.Selected = true;
                                item.Value.ListViewItem.Focused = true;
                            }

                            MainParent.StatusProgressStep();
                        }));
                    }

                    Invoke(new Action(() =>
                    {
                        MainParent.StatusProgressEnd();
                        MainParent.StatusMessageSet(Serials.Count + " сериалов");
                        listSerials_ClientSizeChanged(null, null);
                    }));
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            });
        }

        private async Task listSerials_updateAsync(Dictionary<int, Season> collection)
        {
            try
            {
                listSerials.Enabled = false;
                listSerials.BeginUpdate();
                listSerials.Items.Clear();
                foreach (KeyValuePair<int, Season> item in collection)
                {
                    item.Value.ListViewItem.Group = listSerials.Groups["listViewGroup" + item.Value.Type];
                    listSerials.Items.Add(item.Value.ToListViewItem());
                }

                listSerials.Sort();
                listSerials.EndUpdate();
                listSerials.Enabled = true;
                splitContainerMain.Enabled = true;
                await Task.Delay(10);
                listSerials_ClientSizeChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }
        }

        #endregion

        #endregion

        #region Menu

        #region Update

        private void cпискиСериаловToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _ = SyncAsync(true);
        }

        private void cтраницыСериаловToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _ = SyncAsync(false, true);
        }

        private void cпискиПлейлиствоToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _ = SyncAsync(false, false, true);
        }

        private void спискиВидеоToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _ = SyncAsync(false, false, false, true);
        }

        private void всеToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _ = SyncAsync(true, true, true, true);
        }

        #endregion

        #region View

        private void ToolStripMenuItemSerialTitleFull_Click(object sender, EventArgs e)
        {
            int width = columnSerialTitleFull.Width + columnSerialTitle.Width +
                        columnSerialTitleRU.Width + columnSerialTitleEN.Width +
                        columnSerialTitleOriginal.Width;
            if (0 >= columnSerialTitleFull.Width)
            {
                columnSerialTitleFull.Width = width;
                columnSerialTitleFull.DisplayIndex = firstVisibleColumnslistSerials();
            }

            ToolStripMenuItemSerialTitleFull.Checked = true;

            columnSerialSeason.Width = 0;
            columnSerialSeason.DisplayIndex = 0;

            ShowHideColumns(columnSerialTitle, ToolStripMenuItemSerialTitle, false);
            ShowHideColumns(columnSerialTitleRU, ToolStripMenuItemSerialTitleRU, false);
            ShowHideColumns(columnSerialTitleEN, ToolStripMenuItemSerialTitleEN, false);
            ShowHideColumns(columnSerialTitleOriginal, ToolStripMenuItemSerialTitleOriginal, false);
        }

        private void ToolStripMenuItemSerialTitle_Click(object sender, EventArgs e)
        {
            int width = columnSerialTitleFull.Width + columnSerialTitle.Width +
                        columnSerialTitleRU.Width + columnSerialTitleEN.Width +
                        columnSerialTitleOriginal.Width;

            ShowHideColumns(columnSerialTitleFull, ToolStripMenuItemSerialTitleFull, false);
            ShowHideColumns(columnSerialTitleRU, ToolStripMenuItemSerialTitleRU, false);
            ShowHideColumns(columnSerialTitleEN, ToolStripMenuItemSerialTitleEN, false);
            ShowHideColumns(columnSerialTitleOriginal, ToolStripMenuItemSerialTitleOriginal, false);

            if (0 >= columnSerialTitle.Width)
            {
                columnSerialTitle.Width = width;
                columnSerialTitle.DisplayIndex = firstVisibleColumnslistSerials();
            }

            ToolStripMenuItemSerialTitle.Checked = true;

            if (0 >= columnSerialSeason.Width)
                columnSerialSeason.Width = 25;
            columnSerialSeason.DisplayIndex = firstVisibleColumnslistSerials() + 1;
        }

        private void ToolStripMenuItemSerialTitleRU_Click(object sender, EventArgs e)
        {
            int width = columnSerialTitleFull.Width + columnSerialTitle.Width +
                        columnSerialTitleRU.Width + columnSerialTitleEN.Width +
                        columnSerialTitleOriginal.Width;

            ShowHideColumns(columnSerialTitleFull, ToolStripMenuItemSerialTitleFull, false);
            ShowHideColumns(columnSerialTitle, ToolStripMenuItemSerialTitle, false);
            ShowHideColumns(columnSerialTitleEN, ToolStripMenuItemSerialTitleEN, false);
            ShowHideColumns(columnSerialTitleOriginal, ToolStripMenuItemSerialTitleOriginal, false);

            if (0 >= columnSerialTitleRU.Width)
            {
                columnSerialTitleRU.Width = width;
                columnSerialTitleRU.DisplayIndex = firstVisibleColumnslistSerials();
            }

            ToolStripMenuItemSerialTitleRU.Checked = true;

            if (0 >= columnSerialSeason.Width)
                columnSerialSeason.Width = 25;
            columnSerialSeason.DisplayIndex = firstVisibleColumnslistSerials() + 1;
        }

        private void ToolStripMenuItemSerialTitleEN_Click(object sender, EventArgs e)
        {
            int width = columnSerialTitleFull.Width + columnSerialTitle.Width +
                        columnSerialTitleRU.Width + columnSerialTitleEN.Width +
                        columnSerialTitleOriginal.Width;

            ShowHideColumns(columnSerialTitleFull, ToolStripMenuItemSerialTitleFull, false);
            ShowHideColumns(columnSerialTitle, ToolStripMenuItemSerialTitle, false);
            ShowHideColumns(columnSerialTitleRU, ToolStripMenuItemSerialTitleRU, false);
            ShowHideColumns(columnSerialTitleOriginal, ToolStripMenuItemSerialTitleOriginal, false);

            if (0 >= columnSerialTitleEN.Width)
            {
                columnSerialTitleEN.Width = width;
                columnSerialTitleEN.DisplayIndex = firstVisibleColumnslistSerials();
            }

            ToolStripMenuItemSerialTitleEN.Checked = true;

            if (0 >= columnSerialSeason.Width)
                columnSerialSeason.Width = 25;
            columnSerialSeason.DisplayIndex = firstVisibleColumnslistSerials() + 1;
        }

        private void ToolStripMenuItemSerialTitleOriginal_Click(object sender, EventArgs e)
        {
            int width = columnSerialTitleFull.Width + columnSerialTitle.Width +
                        columnSerialTitleRU.Width + columnSerialTitleEN.Width +
                        columnSerialTitleOriginal.Width;

            ShowHideColumns(columnSerialTitleFull, ToolStripMenuItemSerialTitleFull, false);
            ShowHideColumns(columnSerialTitle, ToolStripMenuItemSerialTitle, false);
            ShowHideColumns(columnSerialTitleRU, ToolStripMenuItemSerialTitleRU, false);
            ShowHideColumns(columnSerialTitleEN, ToolStripMenuItemSerialTitleEN, false);

            if (0 >= columnSerialTitleOriginal.Width)
                columnSerialTitleOriginal.Width = width;
            columnSerialTitleOriginal.DisplayIndex = firstVisibleColumnslistSerials();
            ToolStripMenuItemSerialTitleOriginal.Checked = true;

            if (0 >= columnSerialSeason.Width)
                columnSerialSeason.Width = 25;
            columnSerialSeason.DisplayIndex = firstVisibleColumnslistSerials() + 1;
        }

        private void ToolStripMenuItemSerialSeasonID_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialSeasonID, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialSerialID_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialSerialID, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialserialUrl_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialserialUrl, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialGenre_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialGenre, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialCountry_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialCountry, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialRelease_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialRelease, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialIMDB_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialIMDB, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialKinoPoisk_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialKinoPoisk, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialLimitation_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialLimitation, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialUserComments_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialUserComments, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialUserViewsLastDay_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialUserViewsLastDay, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialCompilation_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialCompilation, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialMarkCurrent_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialMarkCurrent, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialMarkLast_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialMarkLast, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialMark_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialMark, sender as ToolStripMenuItem);
        }

        private void ToolStripMenuItemSerialSiteUpdated_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnSerialSiteUpdated, sender as ToolStripMenuItem);
        }

        #endregion

        private void ShowHideColumns(ColumnHeader column, ToolStripMenuItem menuItem, bool show = true)
        {
            _ShowHideColumns(column, menuItem, show);
            if (null != column)
                listSerials_ClientSizeChanged(null, null);
        }

        private void _ShowHideColumns(ColumnHeader column, ToolStripMenuItem menuItem, bool show = true)
        {
            if (null != menuItem)
                menuItem.Checked = show;

            if (null != column)
            {
                column.Width = show ? 0 < column.Width ? column.Width : 90 : 0;
                column.DisplayIndex = show ? listSerials.Columns.Count - 1 : 0;
            }
        }

        #endregion

        #region listSerials

        private List<bool> _getAutoSizeResizeColumnslistSerials()
        {
            List<bool> _data = new List<bool>();
            List<int> data = SimpleJson.SimpleJson.DeserializeObject<List<int>>(
                DB.Instance.OptionGet("listSerialsAutoSize"));
            for (int i = 0; i < listSerials.Columns.Count; i++)
                _data.Add(i < data.Count ? 0 < data[i] : false);

            return _data;
        }

        private int firstVisibleColumnslistSerials()
        {
            int result = listSerials.Columns.Count;
            for (int i = 0; i < listSerials.Columns.Count; i++)
                if (0 < listSerials.Columns[i].Width && result > listSerials.Columns[i].DisplayIndex)
                    result = listSerials.Columns[i].DisplayIndex;
            return result;
        }

        private void listSerials_ClientSizeChanged(object sender, System.EventArgs e)
        {
            List<bool> data = _getAutoSizeResizeColumnslistSerials();
            bool existItems = 0 < listSerials.Items.Count;
            int width = 0;
            int _width = 0;
            for (int i = 0; i < listSerials.Columns.Count; i++)
                if (0 < listSerials.Columns[i].Width)
                    if (data[i])
                    {
                        if (existItems)
                            listSerials.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                        _width += listSerials.Columns[i].Width;
                    }
                    else
                        width += listSerials.Columns[i].Width;

            int widthParent = listSerials.Width - _width - 25;
            if (0 < widthParent)
            {
                listSerials.BeginUpdate();
                for (int i = 0; i < listSerials.Columns.Count; i++)
                    if (0 < listSerials.Columns[i].Width && !data[i])
                    {
                        int widthItem = widthParent * listSerials.Columns[i].Width / width;
                        if (0 <= widthItem)
                            listSerials.Columns[i].Width = widthItem;
                    }

                listSerials.EndUpdate();
            }
        }

        private void listSerials_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (currentOrderColumn == e.Column)
            {
                currentOrderInverted = !currentOrderInverted;
            }
            else
            {
                currentOrderColumn = e.Column;
                currentOrderInverted = false;
            }

            listSerials.ListViewItemSorter = new ListViewItemComparer(currentOrderColumn, currentOrderInverted);
            listSerials.BeginUpdate();
            listSerials.Sort();
            listSerials.EndUpdate();
        }

        private void listSerials_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = (0 < listSerials.SelectedItems.Count) ? Convert.ToInt32(listSerials.SelectedItems[0].Tag) : 0;
            if (!Serials.ContainsKey(id))
            {
                //panelTexts.Visible = false;
                //labelDescription.Visible = false;
                return;
            }

            var item = Serials[id];
            if (item.URL == pictureSeasonImage.Tag as string)
                return;

            pictureSeasonImage.LoadAsync(item.Image);
            pictureSeasonImage.Tag = item.URL;
            linkLabelTitleFULL.Text =
                string.Format(
                    (item.TitleRU != item.TitleOriginal ? "{0}\n{1}" : "{0}") +
                    (0 < item.SeasonNum ? "\nСезон: {2}" : ""), item.TitleRU, item.TitleOriginal, item.SeasonNum);
            labelGenre.Text = item.Genre;
            labelCountry.Text = item.Country;
            labelRelease.Text = item.Release;
            labelIMDB.Text = item.IMDB.ToString();
            labelKinoPoisk.Text = item.KinoPoisk.ToString();
            labelMark.Text = item.Mark;
            labelCompilation.Text = item.Compilation;
            labelSiteUpdated.Text = (new DateTime() != item.SiteUpdated) ? item.SiteUpdated.ToString("dd.MM.yyyy") : "";
            labelDescription.Text = "    " + item.Description.Replace("\r\n", "\r\n    ");
            panelTexts.Visible = true;
            labelDescription.Visible = true;
        }

        private void listSerials_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || 0 == listSerials.SelectedItems.Count)
                return;

            var types = new List<string>();
            for (var i = 0; i < listSerials.SelectedItems.Count; i++)
            {
                int id = Convert.ToInt32(listSerials.SelectedItems[0].Tag);
                if (!Serials.ContainsKey(id))
                    continue;
                var item = Serials[id];
                if (!types.Contains(item.Type)) types.Add(item.Type);
            }

            отметитьНаПоследнейСерииToolStripMenuItem.Enabled = !(1 == types.Count && types.Contains("nonew"));
            хочуПосмотретьToolStripMenuItem.Enabled = !(1 == types.Count && types.Contains("want"));
            ужеПосмотрелToolStripMenuItem.Enabled = !(1 == types.Count && types.Contains("watched"));

            //#if DEBUG
            //                    Console.WriteLine("MouseClick Right {0:S}", item.URL);
            //#endif
            // UNDONE Фильтровать пункты контекстного меню
        }

        private void listSerials_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listSerials.SelectedItems.Count > 0)
            {
                int id = Convert.ToInt32(listSerials.SelectedItems[0].Tag);
                if (!Serials.ContainsKey(id))
                    return;
                var item = Serials[id];
#if DEBUG
                Console.WriteLine("MouseDoubleClick {0:S}", item.URL);
#endif
                // UNDONE Открывать окно сериала
            }
        }

        #region Контекстное меню

        private List<int> getSelectedIds()
        {
            var collection = new List<int>();
            if (0 < listSerials.SelectedItems.Count)
                for (var i = 0; i < listSerials.SelectedItems.Count; i++)
                {
                    int id = Convert.ToInt32(listSerials.SelectedItems[i].Tag);
                    if (!Serials.ContainsKey(id))
                        continue;
                    collection.Add(id);
                }

            return collection;
        }

        private async Task SyncSelectedSerialsAsync(bool serials = false, bool playlists = false, bool videos = false)
        {
            var collection = getSelectedIds();

            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            остановитьToolStripMenuItem.Visible = true;
            await Task.Run(() =>
            {
                try
                {
                    Invoke(new Action(() => MainParent.StatusProgressReset(collection.Count)));
                    // Синхронизация Страниц
                    foreach (var seasonID in collection)
                    {
                        var title = Serials[seasonID].TitleFull;
                        Invoke(new Action(() =>
                        {
                            MainParent.StatusMessageSet("Обработка страницы: " + title);
                            MainParent.StatusProgressStep();
                        }));
                        Serials[seasonID].syncPage(serials);
                        if (token.IsCancellationRequested)
                        {
                            Invoke(new Action(() => CancelSync()));
                            return;
                        }
                    }

                    if (token.IsCancellationRequested)
                    {
                        Invoke(new Action(() => CancelSync()));
                        return;
                    }

                    // Синхронизация Плейлистов
                    Invoke(new Action(() => MainParent.StatusProgressReset()));
                    foreach (var seasonID in collection)
                    {
                        var title = Serials[seasonID].TitleFull;
                        Invoke(new Action(() =>
                        {
                            MainParent.StatusMessageSet("Обработка плейлистов: " + title);
                            MainParent.StatusProgressStep();
                        }));
                        Serials[seasonID].syncPlayer(playlists);
                        if (token.IsCancellationRequested)
                        {
                            Invoke(new Action(() => CancelSync()));
                            return;
                        }
                    }

                    if (token.IsCancellationRequested)
                    {
                        Invoke(new Action(() => CancelSync()));
                        return;
                    }

                    // Синхронизация Видео
                    Invoke(new Action(() => MainParent.StatusProgressReset()));
                    foreach (var seasonID in collection)
                    {
                        var title = Serials[seasonID].TitleFull;
                        Invoke(new Action(() =>
                        {
                            MainParent.StatusMessageSet("Обработка видео: " + title);
                            MainParent.StatusProgressStep();
                        }));
                        Serials[seasonID].syncPlaylists(videos);
                        if (token.IsCancellationRequested)
                        {
                            Invoke(new Action(() => CancelSync()));
                            return;
                        }
                    }

                    Invoke(new Action(() =>
                    {
                        MainParent.StatusProgressEnd();
                        остановитьToolStripMenuItem.Visible = false;
                    }));
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            }, token);
            await LoadTableSerialsAsync(true);
            остановитьToolStripMenuItem.Visible = false;
            остановитьToolStripMenuItem.Enabled = true;
            toolStripUpdate.Enabled = true;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            _ = SyncSelectedSerialsAsync(true, true, true);
            contextMenuStripSerials.Close();
        }

        private async void страницуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await SyncSelectedSerialsAsync(true);
        }

        private async void плеерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await SyncSelectedSerialsAsync(false, true);
        }

        private async void видеоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await SyncSelectedSerialsAsync(false, false, true);
        }


        private async void MassChangeMarkAsync(string type)
        {
            var collection = getSelectedIds();
            await Task.Run(() =>
            {
                try
                {
                    Invoke(new Action(() => MainParent.StatusProgressReset()));
                    foreach (var seasonID in collection)
                    {
                        var title = Serials[seasonID].TitleFull;
                        Invoke(new Action(() =>
                        {
                            MainParent.StatusMessageSet("Обновление маркера: " + title);
                            MainParent.StatusProgressStep();
                        }));
                        if (Serials[seasonID].Type != type)
                            switch (type)
                            {
                                case "nonew":
                                    Serials[seasonID].MarkSetLast();
                                    break;
                                case "want":
                                    Serials[seasonID].MarkSeWantToSee();
                                    break;
                                case "watched":
                                    Serials[seasonID].MarkSetWatched();
                                    break;
                                case "notwatch":
                                    Serials[seasonID].MarkSetNotWatched();
                                    break;
                                case "delete":
                                    Serials[seasonID].MarkSetDelSee();
                                    break;
                            }
                    }

                    Invoke(new Action(() => MainParent.StatusProgressEnd()));
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            });
            _ = LoadTableSerialsAsync();
        }

        private void отметитьНаПоследнейСерииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MassChangeMarkAsync("nonew");
        }

        private void хочуПосмотретьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MassChangeMarkAsync("want");
        }

        private void ужеПосмотрелToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MassChangeMarkAsync("watched");
        }

        private void вЧерныйСписокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MassChangeMarkAsync("notwatch");
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MassChangeMarkAsync("delete");
        }

        #endregion

        #endregion

        #region Filter

        private void textFilter_TextChanged(object sender, EventArgs e)
        {
            // TODO Добавить дебоунсер
            _ = RefreshSerials();
        }

        private bool isFiltred = true;

        private async Task RefreshSerials()
        {
            await Task.Delay(50);
            if (2 < textFilter.Text.Length)
            {
                Dictionary<int, Season> data = new Dictionary<int, Season>();
                String[] words = textFilter.Text.Split(' ');
                foreach (var item in Serials)
                {
                    var result = true;
                    foreach (String i in words)
                    {
                        if (i.Length == 0) continue;
                        bool invert = i.StartsWith("-");
                        string _i = invert ? i.Substring(1) : i;
                        _i = _i.ToLower();

                        bool _result = item.Value.Title.ToLower().Contains(i) ||
                                       item.Value.Genre.ToLower().Contains(i) ||
                                       item.Value.ID.ToString() == i;

                        if (invert)
                            _result = !_result;

                        result = result && _result;
                    }

                    if (result)
                        data.Add(item.Key, item.Value);
                }

                await listSerials_updateAsync(data);
                isFiltred = true;
            }
            else if (isFiltred)
            {
                await listSerials_updateAsync(Serials);
                isFiltred = false;
            }
        }

        #endregion
    }
}