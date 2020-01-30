using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeTheater.Helper;
using HomeTheater.Serial;

namespace HomeTheater
{
    public partial class FormMain : Form
    {
        public const string ERROR_AUTH = "Пользователь не авторизирован!";
        public const string SUCS_AUTH = "Пользователь авторизировался";

        private int currentOrderColumn;
        private bool currentOrderInverted;
        private List<SerialSeason> Serials = new List<SerialSeason>();

        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadFormMain();
        }

        private async void LoadFormMain()
        {
            DB._load();
            LoadFormView();
            listViewSerials_ClientSizeChanged(null, null);
            listViewDownload_ClientSizeChanged(null, null);
        }

        public void АвторизацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormAuth(this);
            form.Owner = this;
            form.ShowDialog();
        }

        private void НастройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormOptions();
            form.Owner = this;
            form.ShowDialog();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            if (APIServer.Instance.isLogedIn())
            {
                SyncAsync();
            }
            else
            {
                setStatusMessage(ERROR_AUTH);
                АвторизацияToolStripMenuItem_Click(null, null);
            }
        }

        public async Task SyncAsync(bool list = false, bool serials = false, bool playlists = false,
            bool videos = false)
        {
            setStatusMessage("Проверка авторизации");
            if (!APIServer.Instance.isLogedIn())
            {
                setStatusMessage(ERROR_AUTH);
                return;
            }

            setStatusMessage("Получение списка сериалов");
            Serials = await Task.Run(() => APIServer.Instance.getPause(list));
            setStatusMessage("Обновление таблицы");
            listViewSerials_updateAsync();
            SyncSerialsAsync(serials, playlists, videos);
        }

        private async Task SyncSerialsAsync(bool serials = false, bool playlists = false, bool videos = false)
        {
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Maximum = Serials.Count;
            await Task.Run(() =>
            {
                try
                {
                    /*
                    // Синхронизация Страниц
                    for (var i = 0; i < Serials.Count; i++)
                    {
                        string title = Serials[i].TitleFull;

                        Invoke(new Action(() => setStatusMessage("Обработка страницы: " + title)));
                        Serials[i].syncPage(i == 0 || serials && "watched" != Serials[i].Type);
                        if (i == 0)
                        {
                            string SecureMark = Serials[i].SecureMark;
                            Invoke(new Action(() => APIServer.Instance.secureMark = SecureMark));
                        }
                        Invoke(new Action(() =>
                        {
                            Serials[i].ToListViewItem();
                            toolStripProgressBar1.Value++;
                        }));
                    }
                    // Синхронизация Плейлистов
                    Invoke(new Action(() => toolStripProgressBar1.Value = 0));
                    for (var i = 0; i < Serials.Count; i++)
                    {
                        string title = Serials[i].TitleFull;

                        Invoke(new Action(() => setStatusMessage("Обработка плейлистов: " + title)));
                        Serials[i].syncPlayer(i == 0 || playlists && "watched" != Serials[i].Type);
                        Invoke(new Action(() =>
                        {
                            Serials[i].ToListViewItem();
                            toolStripProgressBar1.Value++;
                        }));
                    }
                    // Синхронизация Видео
                    Invoke(new Action(() => toolStripProgressBar1.Value = 0));
                    for (var i = 0; i < Serials.Count; i++)
                    {
                        string title = Serials[i].TitleFull;

                        Invoke(new Action(() => setStatusMessage("Обработка видео: " + title)));
                        Serials[i].syncPlaylists(i == 0 || videos && "watched" != Serials[i].Type);
                        Invoke(new Action(() =>
                        {
                            Serials[i].ToListViewItem();
                            toolStripProgressBar1.Value++;
                        }));
                    }
                    // Обновление таблицы
                    Invoke(new Action(() => toolStripProgressBar1.Value = 0));
                    for (var i = 0; i < Serials.Count; i++)
                    {
                        Invoke(new Action(() =>
                        {
                            Serials[i].ToListViewItem();
                            toolStripProgressBar1.Value++;
                        }));
                    }
                    */
                    for (var i = 0; i < Serials.Count; i++)
                    {
                        var title = Serials[i].TitleFull;

                        Invoke(new Action(() => setStatusMessage("Обработка страницы: " + title)));
                        Serials[i].syncPage(i == 0 || serials && "watched" != Serials[i].Type);
                        if (i == 0)
                        {
                            var SecureMark = Serials[i].SecureMark;
                            Invoke(new Action(() => APIServer.Instance.secureMark = SecureMark));
                        }

                        Invoke(new Action(() => setStatusMessage("Обработка плейлистов: " + title)));
                        Serials[i].syncPlayer(i == 0 || playlists && "watched" != Serials[i].Type);
                        Invoke(new Action(() => setStatusMessage("Обработка видео: " + title)));
                        Serials[i].syncPlaylists(i == 0 || videos && "watched" != Serials[i].Type);
                        Invoke(new Action(() =>
                        {
                            Serials[i].ToListViewItem();
                            toolStripProgressBar1.Value++;
                        }));
                    }

                    Invoke(new Action(() =>
                    {
                        toolStripProgressBar1.Visible = false;
                        setStatusMessage(Serials.Count + " сериалов");
                        resizeColumnsListViewSerials();
                    }));
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            });
        }

        private async Task listViewSerials_updateAsync()
        {
            try
            {
                listViewSerials.Items.Clear();
                for (var i = 0; i < Serials.Count; i++)
                {
                    var SeasonID = Serials[i].SeasonID.ToString();
                    Serials[i].ListViewItem.Group = listViewSerials.Groups["listViewGroup" + Serials[i].Type];
                    if (!listViewSerials.Items.ContainsKey(SeasonID))
                        listViewSerials.Items.Add(Serials[i].ToListViewItem());
                }

                resizeColumnsListViewSerials();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }
        }

        private void UpdateListsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SyncAsync(true);
        }

        private void UpdateSerialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SyncAsync(false, true);
        }

        private void UpdatePlaylistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SyncAsync(false, false, true);
        }

        private void UpdateVideosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SyncAsync(false, false, false, true);
        }

        public void setStatusMessage(string message)
        {
            toolStripStatusLabel1.Text = message;
        }

        private void resizeColumnsListViewDownload()
        {
            decimal width = 0;
            for (var i = 0; i < listViewDownload.Columns.Count; i++)
                width += listViewDownload.Columns[i].Width;
            decimal widthParent = listViewDownload.Width - 25;
            for (var i = 0; i < listViewDownload.Columns.Count; i++)
                if (0 < listViewDownload.Columns[i].Width)
                {
                    var widthItem =
                        decimal.ToInt32(Math.Floor(widthParent * listViewDownload.Columns[i].Width / width));
                    if (0 < widthItem)
                        listViewDownload.Columns[i].Width = widthItem;
                }
        }

        private List<bool> _getAutoSizeResizeColumnsListViewSerials()
        {
            var _data = new List<bool>();
            var data = SimpleJson.SimpleJson.DeserializeObject<List<int>>(
                DB.Instance.OptionGet("listViewSerialsAutoSize"));
            for (var i = 0; i < listViewSerials.Columns.Count; i++)
                _data.Add(i < data.Count ? 0 < data[i] : false);

            return _data;
        }

        private void resizeColumnsListViewSerials()
        {
            var data = _getAutoSizeResizeColumnsListViewSerials();
            var existItems = 0 < listViewSerials.Items.Count;
            decimal width = 0;
            decimal _width = 0;
            for (var i = 0; i < listViewSerials.Columns.Count; i++)
                if (0 < listViewSerials.Columns[i].Width)
                    if (data[i])
                    {
                        if (existItems)
                            listViewSerials.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                        _width += listViewSerials.Columns[i].Width;
                    }
                    else
                    {
                        width += listViewSerials.Columns[i].Width;
                    }

            var widthParent = listViewSerials.Width - _width - 25;

            for (var i = 0; i < listViewSerials.Columns.Count; i++)
                if (0 < listViewSerials.Columns[i].Width && !data[i])
                {
                    var widthItem = decimal.ToInt32(Math.Floor(widthParent * listViewSerials.Columns[i].Width / width));
                    if (0 <= widthItem)
                        listViewSerials.Columns[i].Width = widthItem;
                }
        }

        private int firstVisibleColumnsListViewSerials()
        {
            var result = listViewSerials.Columns.Count;
            for (var i = 0; i < listViewSerials.Columns.Count; i++)
                if (0 < listViewSerials.Columns[i].Width && result > listViewSerials.Columns[i].DisplayIndex)
                    result = listViewSerials.Columns[i].DisplayIndex;
            return result;
        }

        private void LoadFormView()
        {
            var data = SimpleJson.SimpleJson.DeserializeObject<List<int>>(
                DB.Instance.OptionGet("listViewSerialsDisplayIndex"));
            var count = data.Count;
            if (listViewSerials.Columns.Count < count)
                count = listViewSerials.Columns.Count;
            for (var i = 0; i < count; i++)
                listViewSerials.Columns[i].DisplayIndex = data[i];
            data = SimpleJson.SimpleJson.DeserializeObject<List<int>>(DB.Instance.OptionGet("listViewSerialsWidth"));
            count = data.Count;
            if (listViewSerials.Columns.Count < count)
                count = listViewSerials.Columns.Count;
            for (var i = 0; i < count; i++)
                listViewSerials.Columns[i].Width = data[i];
            data = SimpleJson.SimpleJson.DeserializeObject<List<int>>(DB.Instance.OptionGet("listViewDownloadWidth"));
            count = data.Count;
            if (listViewDownload.Columns.Count < count)
                count = listViewDownload.Columns.Count;
            for (var i = 0; i < count; i++)
                listViewDownload.Columns[i].Width = data[i];

            if (0 < columnHeaderSerialTitleFull.Width)
                ToolStripMenuItemSerialTitleFull_Click(null, null);
            else if (0 < columnHeaderSerialTitle.Width)
                ToolStripMenuItemSerialTitle_Click(null, null);
            else if (0 < columnHeaderSerialTitleRU.Width)
                ToolStripMenuItemSerialTitleRU_Click(null, null);
            else if (0 < columnHeaderSerialTitleEN.Width)
                ToolStripMenuItemSerialTitleEN_Click(null, null);
            else if (0 < columnHeaderSerialTitleOriginal.Width) ToolStripMenuItemSerialTitleOriginal_Click(null, null);

            ToolStripMenuItemSerialCompilation.Checked = 0 < columnHeaderSerialCompilation.Width;
            ToolStripMenuItemSerialCountry.Checked = 0 < columnHeaderSerialCountry.Width;
            ToolStripMenuItemSerialGenre.Checked = 0 < columnHeaderSerialGenre.Width;
            ToolStripMenuItemSerialIMDB.Checked = 0 < columnHeaderSerialIMDB.Width;
            ToolStripMenuItemSerialKinoPoisk.Checked = 0 < columnHeaderSerialKinoPoisk.Width;
            ToolStripMenuItemSerialLimitation.Checked = 0 < columnHeaderSerialLimitation.Width;
            ToolStripMenuItemSerialMark.Checked = 0 < columnHeaderSerialMark.Width;
            ToolStripMenuItemSerialMarkCurrent.Checked = 0 < columnHeaderSerialMarkCurrent.Width;
            ToolStripMenuItemSerialMarkLast.Checked = 0 < columnHeaderSerialMarkLast.Width;
            ToolStripMenuItemSerialRelease.Checked = 0 < columnHeaderSerialRelease.Width;
            ToolStripMenuItemSerialSerialID.Checked = 0 < columnHeaderSerialSerialID.Width;
            ToolStripMenuItemSerialserialUrl.Checked = 0 < columnHeaderSerialserialUrl.Width;
            ToolStripMenuItemSerialSiteUpdated.Checked = 0 < columnHeaderSerialSiteUpdated.Width;
            ToolStripMenuItemSerialUserComments.Checked = 0 < columnHeaderSerialUserComments.Width;
            ToolStripMenuItemSerialUserViewsLastDay.Checked = 0 < columnHeaderSerialUserViewsLastDay.Width;
        }

        private void SaveFormView()
        {
            var listViewSerialsDisplayIndex = new List<int>();
            var listViewSerialsWidth = new List<int>();
            for (var i = 0; i < listViewSerials.Columns.Count; i++)
            {
                listViewSerialsDisplayIndex.Add(listViewSerials.Columns[i].DisplayIndex);
                listViewSerialsWidth.Add(listViewSerials.Columns[i].Width);
            }

            var listViewDownloadWidth = new List<int>();
            for (var i = 0; i < listViewDownload.Columns.Count; i++)
                listViewDownloadWidth.Add(listViewDownload.Columns[i].Width);
            DB.Instance.OptionSet("listViewSerialsDisplayIndex",
                SimpleJson.SimpleJson.SerializeObject(listViewSerialsDisplayIndex));
            DB.Instance.OptionSet("listViewSerialsWidth", SimpleJson.SimpleJson.SerializeObject(listViewSerialsWidth));
            DB.Instance.OptionSet("listViewDownloadWidth",
                SimpleJson.SimpleJson.SerializeObject(listViewDownloadWidth));
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIconTray.Visible = false;
            SaveFormView();
        }

        private void ToolStripMenuItemSerialTitleFull_Click(object sender, EventArgs e)
        {
            var width = columnHeaderSerialTitleFull.Width + columnHeaderSerialTitle.Width +
                        columnHeaderSerialTitleRU.Width + columnHeaderSerialTitleEN.Width +
                        columnHeaderSerialTitleOriginal.Width;
            if (0 >= columnHeaderSerialTitleFull.Width)
            {
                columnHeaderSerialTitleFull.Width = width;
                columnHeaderSerialTitleFull.DisplayIndex = firstVisibleColumnsListViewSerials();
            }

            ToolStripMenuItemSerialTitleFull.Checked = true;

            columnHeaderSerialSeason.Width = 0;
            columnHeaderSerialSeason.DisplayIndex = 0;

            ShowHideColumns(columnHeaderSerialTitle, ToolStripMenuItemSerialTitle, false);
            ShowHideColumns(columnHeaderSerialTitleRU, ToolStripMenuItemSerialTitleRU, false);
            ShowHideColumns(columnHeaderSerialTitleEN, ToolStripMenuItemSerialTitleEN, false);
            ShowHideColumns(columnHeaderSerialTitleOriginal, ToolStripMenuItemSerialTitleOriginal, false);
        }

        private void ToolStripMenuItemSerialTitle_Click(object sender, EventArgs e)
        {
            var width = columnHeaderSerialTitleFull.Width + columnHeaderSerialTitle.Width +
                        columnHeaderSerialTitleRU.Width + columnHeaderSerialTitleEN.Width +
                        columnHeaderSerialTitleOriginal.Width;

            ShowHideColumns(columnHeaderSerialTitleFull, ToolStripMenuItemSerialTitleFull, false);
            ShowHideColumns(columnHeaderSerialTitleRU, ToolStripMenuItemSerialTitleRU, false);
            ShowHideColumns(columnHeaderSerialTitleEN, ToolStripMenuItemSerialTitleEN, false);
            ShowHideColumns(columnHeaderSerialTitleOriginal, ToolStripMenuItemSerialTitleOriginal, false);

            if (0 >= columnHeaderSerialTitle.Width)
            {
                columnHeaderSerialTitle.Width = width;
                columnHeaderSerialTitle.DisplayIndex = firstVisibleColumnsListViewSerials();
            }

            ToolStripMenuItemSerialTitle.Checked = true;

            if (0 >= columnHeaderSerialSeason.Width)
                columnHeaderSerialSeason.Width = 25;
            columnHeaderSerialSeason.DisplayIndex = firstVisibleColumnsListViewSerials() + 1;
        }

        private void ToolStripMenuItemSerialTitleRU_Click(object sender, EventArgs e)
        {
            var width = columnHeaderSerialTitleFull.Width + columnHeaderSerialTitle.Width +
                        columnHeaderSerialTitleRU.Width + columnHeaderSerialTitleEN.Width +
                        columnHeaderSerialTitleOriginal.Width;

            ShowHideColumns(columnHeaderSerialTitleFull, ToolStripMenuItemSerialTitleFull, false);
            ShowHideColumns(columnHeaderSerialTitle, ToolStripMenuItemSerialTitle, false);
            ShowHideColumns(columnHeaderSerialTitleEN, ToolStripMenuItemSerialTitleEN, false);
            ShowHideColumns(columnHeaderSerialTitleOriginal, ToolStripMenuItemSerialTitleOriginal, false);

            if (0 >= columnHeaderSerialTitleRU.Width)
            {
                columnHeaderSerialTitleRU.Width = width;
                columnHeaderSerialTitleRU.DisplayIndex = firstVisibleColumnsListViewSerials();
            }

            ToolStripMenuItemSerialTitleRU.Checked = true;

            if (0 >= columnHeaderSerialSeason.Width)
                columnHeaderSerialSeason.Width = 25;
            columnHeaderSerialSeason.DisplayIndex = firstVisibleColumnsListViewSerials() + 1;
        }

        private void ToolStripMenuItemSerialTitleEN_Click(object sender, EventArgs e)
        {
            var width = columnHeaderSerialTitleFull.Width + columnHeaderSerialTitle.Width +
                        columnHeaderSerialTitleRU.Width + columnHeaderSerialTitleEN.Width +
                        columnHeaderSerialTitleOriginal.Width;

            ShowHideColumns(columnHeaderSerialTitleFull, ToolStripMenuItemSerialTitleFull, false);
            ShowHideColumns(columnHeaderSerialTitle, ToolStripMenuItemSerialTitle, false);
            ShowHideColumns(columnHeaderSerialTitleRU, ToolStripMenuItemSerialTitleRU, false);
            ShowHideColumns(columnHeaderSerialTitleOriginal, ToolStripMenuItemSerialTitleOriginal, false);

            if (0 >= columnHeaderSerialTitleEN.Width)
            {
                columnHeaderSerialTitleEN.Width = width;
                columnHeaderSerialTitleEN.DisplayIndex = firstVisibleColumnsListViewSerials();
            }

            ToolStripMenuItemSerialTitleEN.Checked = true;

            if (0 >= columnHeaderSerialSeason.Width)
                columnHeaderSerialSeason.Width = 25;
            columnHeaderSerialSeason.DisplayIndex = firstVisibleColumnsListViewSerials() + 1;
        }

        private void ToolStripMenuItemSerialTitleOriginal_Click(object sender, EventArgs e)
        {
            var width = columnHeaderSerialTitleFull.Width + columnHeaderSerialTitle.Width +
                        columnHeaderSerialTitleRU.Width + columnHeaderSerialTitleEN.Width +
                        columnHeaderSerialTitleOriginal.Width;

            ShowHideColumns(columnHeaderSerialTitleFull, ToolStripMenuItemSerialTitleFull, false);
            ShowHideColumns(columnHeaderSerialTitle, ToolStripMenuItemSerialTitle, false);
            ShowHideColumns(columnHeaderSerialTitleRU, ToolStripMenuItemSerialTitleRU, false);
            ShowHideColumns(columnHeaderSerialTitleEN, ToolStripMenuItemSerialTitleEN, false);

            if (0 >= columnHeaderSerialTitleOriginal.Width)
                columnHeaderSerialTitleOriginal.Width = width;
            columnHeaderSerialTitleOriginal.DisplayIndex = firstVisibleColumnsListViewSerials();
            ToolStripMenuItemSerialTitleOriginal.Checked = true;

            if (0 >= columnHeaderSerialSeason.Width)
                columnHeaderSerialSeason.Width = 25;
            columnHeaderSerialSeason.DisplayIndex = firstVisibleColumnsListViewSerials() + 1;
        }

        private void ToolStripMenuItemSerialSeasonID_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialSeasonID, ToolStripMenuItemSerialSeasonID);
        }

        private void ToolStripMenuItemSerialSerialID_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialSerialID, ToolStripMenuItemSerialSerialID);
        }

        private void ToolStripMenuItemSerialserialUrl_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialserialUrl, ToolStripMenuItemSerialserialUrl);
        }

        private void ToolStripMenuItemSerialGenre_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialGenre, ToolStripMenuItemSerialGenre);
        }

        private void ToolStripMenuItemSerialCountry_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialCountry, ToolStripMenuItemSerialCountry);
        }

        private void ToolStripMenuItemSerialRelease_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialRelease, ToolStripMenuItemSerialRelease);
        }

        private void ToolStripMenuItemSerialIMDB_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialIMDB, ToolStripMenuItemSerialIMDB);
        }

        private void ToolStripMenuItemSerialKinoPoisk_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialKinoPoisk, ToolStripMenuItemSerialKinoPoisk);
        }

        private void ToolStripMenuItemSerialLimitation_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialLimitation, ToolStripMenuItemSerialLimitation);
        }

        private void ToolStripMenuItemSerialUserComments_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialUserComments, ToolStripMenuItemSerialUserComments);
        }

        private void ToolStripMenuItemSerialUserViewsLastDay_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialUserViewsLastDay, ToolStripMenuItemSerialUserViewsLastDay);
        }

        private void ToolStripMenuItemSerialCompilation_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialCompilation, ToolStripMenuItemSerialCompilation);
        }

        private void ToolStripMenuItemSerialMarkCurrent_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialMarkCurrent, ToolStripMenuItemSerialMarkCurrent);
        }

        private void ToolStripMenuItemSerialMarkLast_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialMarkLast, ToolStripMenuItemSerialMarkLast);
        }

        private void ToolStripMenuItemSerialMark_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialMark, ToolStripMenuItemSerialMark);
        }

        private void ToolStripMenuItemSerialSiteUpdated_Click(object sender, EventArgs e)
        {
            ShowHideColumns(columnHeaderSerialSiteUpdated, ToolStripMenuItemSerialSiteUpdated);
        }

        private void ShowHideColumns(ColumnHeader column, ToolStripMenuItem menuItem)
        {
            ShowHideColumns(column, menuItem, !menuItem.Checked);
        }

        private void ShowHideColumns(ColumnHeader column, bool show = true)
        {
            ShowHideColumns(column, null, show);
        }

        private void ShowHideColumns(ToolStripMenuItem menuItem, bool show = true)
        {
            ShowHideColumns(null, menuItem, show);
        }

        private void ShowHideColumns(ColumnHeader column, ToolStripMenuItem menuItem, bool show = true)
        {
            _ShowHideColumns(column, menuItem, show);
            if (null != column)
                resizeColumnsListViewSerials();
        }

        private void _ShowHideColumns(ColumnHeader column, ToolStripMenuItem menuItem, bool show = true)
        {
            if (null != menuItem)
                menuItem.Checked = show;

            if (null != column)
            {
                column.Width = show ? 0 < column.Width ? column.Width : 90 : 0;
                column.DisplayIndex = show ? listViewSerials.Columns.Count - 1 : 0;
            }
        }

        private void listViewSerials_ClientSizeChanged(object sender, EventArgs e)
        {
            resizeColumnsListViewSerials();
        }

        private void listViewDownload_ClientSizeChanged(object sender, EventArgs e)
        {
            resizeColumnsListViewDownload();
        }

        private void listViewSerials_ColumnClick(object sender, ColumnClickEventArgs e)
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

            listViewSerials.ListViewItemSorter = new ListViewItemComparer(currentOrderColumn, currentOrderInverted);
            listViewSerials.Sort();
        }

        private void listViewSerials_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var a = sender as ListView;
                if (a.SelectedItems.Count > 0)
                {
                    var item = a.SelectedItems[0].Tag as SerialSeason;
#if DEBUG
                    Console.WriteLine("MouseClick Right {0:S}", item.URL);
#endif
                    // UNDONE Фильтровать пункты контекстного меню
                }
            }
        }

        private void listViewSerials_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var a = sender as ListView;
            if (a.SelectedItems.Count > 0)
            {
                var item = a.SelectedItems[0].Tag as SerialSeason;
#if DEBUG
                Console.WriteLine("MouseDoubleClick {0:S}", item.URL);
#endif
                // UNDONE Открывать окно сериала
            }
        }

        private void listViewSerials_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (0 < listViewSerials.SelectedItems.Count)
            {
                var item = listViewSerials.SelectedItems[0].Tag as SerialSeason;
                if (item.URL != pictureBoxSeasonImage.Tag)
                {
#if DEBUG
                    Console.WriteLine("SelectedIndexChanged {0:S}", item.URL);
#endif
                    pictureBoxSeasonImage.LoadAsync(item.Image);
                    pictureBoxSeasonImage.Tag = item.URL;
                    linkLabelTitleFULL.Text =
                        string.Format(
                            (item.TitleRU != item.TitleOriginal ? "{0:S}\n{1:S}" : "{0:S}") +
                            (0 < item.Season ? "\nСезон: {2}" : ""), item.TitleRU, item.TitleOriginal, item.Season);
#if DEBUG
                    Console.WriteLine(linkLabelTitleFULL.Height);
#endif
                    // UNDONE Обновлять информацию сбоку
                }
            }
        }

        private void pictureBoxSeasonImage_Click(object sender, EventArgs e)
        {
            Process.Start(pictureBoxSeasonImage.Tag.ToString());
        }

        private void statusStripMain_ClientSizeChanged(object sender, EventArgs e)
        {
            decimal width = 0;
            for (var i = 0; i < statusStripMain.Items.Count; i++) width += statusStripMain.Items[i].Width;
            decimal widthParent = statusStripMain.Width - 20;
            for (var i = 0; i < statusStripMain.Items.Count; i++)
                statusStripMain.Items[i].Width =
                    decimal.ToInt32(Math.Floor(widthParent * statusStripMain.Items[i].Width / width));
        }

        private void моиСериалыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectedTab = tabPageMain;
        }

        private void поискToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectedTab = tabPageSearch;
        }

        private void новинкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectedTab = tabPageUpdates;
        }

        private void всеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SyncAsync(true, true, true, true);
        }

        private void linkLabelTitleFULL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBoxSeasonImage_Click(null, null);
        }

        private void splitContainerSubMain_Panel2_ClientSizeChanged(object sender, EventArgs e)
        {
            if (pictureBoxSeasonImage.Width * 2.5 < splitContainerSubMain.Panel2.Width)
            {
                panelTexts.Location = new Point(130, 0);
                panelTexts.Width = splitContainerSubMain.Panel2.Width * 2 / 3;
            }
            else
            {
                panelTexts.Location = new Point(0, 190);
                panelTexts.Width = splitContainerSubMain.Panel2.Width;
            }
        }
    }
}