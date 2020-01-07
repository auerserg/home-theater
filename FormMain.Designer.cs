namespace HomeTheater
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Обновления", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Нет новых серий", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Хочу посмотреть", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Посмотрел", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.авторизацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateListsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateSerialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdatePlaylistsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateVideosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.panelMain = new System.Windows.Forms.Panel();
            this.splitContainerMian = new System.Windows.Forms.SplitContainer();
            this.splitContainerSubMain = new System.Windows.Forms.SplitContainer();
            this.listViewSerials = new System.Windows.Forms.ListView();
            this.listViewDownload = new System.Windows.Forms.ListView();
            this.columnHeaderDownloadTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDownloadSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDownloadStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDownloadProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelDownloadHeader = new System.Windows.Forms.Panel();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.columnHeaderTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSeason = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSeries = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStripMain.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMian)).BeginInit();
            this.splitContainerMian.Panel1.SuspendLayout();
            this.splitContainerMian.Panel2.SuspendLayout();
            this.splitContainerMian.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSubMain)).BeginInit();
            this.splitContainerSubMain.Panel1.SuspendLayout();
            this.splitContainerSubMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.UpdateToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(884, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.авторизацияToolStripMenuItem,
            this.настройкиToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // авторизацияToolStripMenuItem
            // 
            this.авторизацияToolStripMenuItem.Image = global::HomeTheater.Properties.Resources.user;
            this.авторизацияToolStripMenuItem.Name = "авторизацияToolStripMenuItem";
            this.авторизацияToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.авторизацияToolStripMenuItem.Text = "Авторизация";
            this.авторизацияToolStripMenuItem.Click += new System.EventHandler(this.АвторизацияToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Image = global::HomeTheater.Properties.Resources.gear;
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.НастройкиToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            // 
            // UpdateToolStripMenuItem
            // 
            this.UpdateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UpdateListsToolStripMenuItem,
            this.UpdateSerialsToolStripMenuItem,
            this.UpdatePlaylistsToolStripMenuItem,
            this.UpdateVideosToolStripMenuItem});
            this.UpdateToolStripMenuItem.Name = "UpdateToolStripMenuItem";
            this.UpdateToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.UpdateToolStripMenuItem.Text = "Обновить";
            // 
            // UpdateListsToolStripMenuItem
            // 
            this.UpdateListsToolStripMenuItem.Name = "UpdateListsToolStripMenuItem";
            this.UpdateListsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.UpdateListsToolStripMenuItem.Text = "Cписки сериалов";
            this.UpdateListsToolStripMenuItem.Click += new System.EventHandler(this.UpdateListsToolStripMenuItem_Click);
            // 
            // UpdateSerialsToolStripMenuItem
            // 
            this.UpdateSerialsToolStripMenuItem.Name = "UpdateSerialsToolStripMenuItem";
            this.UpdateSerialsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.UpdateSerialsToolStripMenuItem.Text = "Cтраницы сериалов";
            this.UpdateSerialsToolStripMenuItem.Click += new System.EventHandler(this.UpdateSerialsToolStripMenuItem_Click);
            // 
            // UpdatePlaylistsToolStripMenuItem
            // 
            this.UpdatePlaylistsToolStripMenuItem.Name = "UpdatePlaylistsToolStripMenuItem";
            this.UpdatePlaylistsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.UpdatePlaylistsToolStripMenuItem.Text = "Cписки плейлиство";
            this.UpdatePlaylistsToolStripMenuItem.Click += new System.EventHandler(this.UpdatePlaylistsToolStripMenuItem_Click);
            // 
            // UpdateVideosToolStripMenuItem
            // 
            this.UpdateVideosToolStripMenuItem.Name = "UpdateVideosToolStripMenuItem";
            this.UpdateVideosToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.UpdateVideosToolStripMenuItem.Text = "Списки видео";
            this.UpdateVideosToolStripMenuItem.Click += new System.EventHandler(this.UpdateVideosToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStripMain.Location = new System.Drawing.Point(0, 539);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(884, 22);
            this.statusStripMain.TabIndex = 1;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(660, 17);
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(210, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.splitContainerMian);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 24);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(884, 515);
            this.panelMain.TabIndex = 3;
            // 
            // splitContainerMian
            // 
            this.splitContainerMian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMian.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMian.Name = "splitContainerMian";
            this.splitContainerMian.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMian.Panel1
            // 
            this.splitContainerMian.Panel1.Controls.Add(this.splitContainerSubMain);
            // 
            // splitContainerMian.Panel2
            // 
            this.splitContainerMian.Panel2.Controls.Add(this.listViewDownload);
            this.splitContainerMian.Panel2.Controls.Add(this.panelDownloadHeader);
            this.splitContainerMian.Size = new System.Drawing.Size(884, 515);
            this.splitContainerMian.SplitterDistance = 381;
            this.splitContainerMian.TabIndex = 20;
            // 
            // splitContainerSubMain
            // 
            this.splitContainerSubMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSubMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSubMain.Name = "splitContainerSubMain";
            // 
            // splitContainerSubMain.Panel1
            // 
            this.splitContainerSubMain.Panel1.Controls.Add(this.listViewSerials);
            this.splitContainerSubMain.Size = new System.Drawing.Size(884, 381);
            this.splitContainerSubMain.SplitterDistance = 638;
            this.splitContainerSubMain.TabIndex = 10;
            // 
            // listViewSerials
            // 
            this.listViewSerials.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTitle,
            this.columnHeaderSeason,
            this.columnHeaderSeries});
            this.listViewSerials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewSerials.FullRowSelect = true;
            listViewGroup1.Header = "Обновления";
            listViewGroup1.Name = "listViewGroupnew";
            listViewGroup2.Header = "Нет новых серий";
            listViewGroup2.Name = "listViewGroupnonew";
            listViewGroup3.Header = "Хочу посмотреть";
            listViewGroup3.Name = "listViewGroupwant";
            listViewGroup4.Header = "Посмотрел";
            listViewGroup4.Name = "listViewGroupwatched";
            this.listViewSerials.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4});
            this.listViewSerials.Location = new System.Drawing.Point(0, 0);
            this.listViewSerials.MultiSelect = false;
            this.listViewSerials.Name = "listViewSerials";
            this.listViewSerials.Size = new System.Drawing.Size(638, 381);
            this.listViewSerials.TabIndex = 1;
            this.listViewSerials.UseCompatibleStateImageBehavior = false;
            this.listViewSerials.View = System.Windows.Forms.View.Details;
            // 
            // listViewDownload
            // 
            this.listViewDownload.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDownloadTitle,
            this.columnHeaderDownloadSpeed,
            this.columnHeaderDownloadStatus,
            this.columnHeaderDownloadProgress});
            this.listViewDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDownload.FullRowSelect = true;
            this.listViewDownload.HideSelection = false;
            this.listViewDownload.Location = new System.Drawing.Point(0, 34);
            this.listViewDownload.Name = "listViewDownload";
            this.listViewDownload.Size = new System.Drawing.Size(884, 96);
            this.listViewDownload.TabIndex = 21;
            this.listViewDownload.UseCompatibleStateImageBehavior = false;
            this.listViewDownload.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderDownloadTitle
            // 
            this.columnHeaderDownloadTitle.Text = "Заголовок";
            this.columnHeaderDownloadTitle.Width = 400;
            // 
            // columnHeaderDownloadSpeed
            // 
            this.columnHeaderDownloadSpeed.Text = "Скорость";
            this.columnHeaderDownloadSpeed.Width = 110;
            // 
            // columnHeaderDownloadStatus
            // 
            this.columnHeaderDownloadStatus.Text = "Статус";
            this.columnHeaderDownloadStatus.Width = 100;
            // 
            // columnHeaderDownloadProgress
            // 
            this.columnHeaderDownloadProgress.Text = "Прогресс";
            this.columnHeaderDownloadProgress.Width = 360;
            // 
            // panelDownloadHeader
            // 
            this.panelDownloadHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDownloadHeader.Location = new System.Drawing.Point(0, 0);
            this.panelDownloadHeader.Name = "panelDownloadHeader";
            this.panelDownloadHeader.Size = new System.Drawing.Size(884, 34);
            this.panelDownloadHeader.TabIndex = 0;
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.BalloonTipText = "gfdg";
            this.notifyIconMain.BalloonTipTitle = "dfgdfg";
            this.notifyIconMain.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconMain.Icon")));
            this.notifyIconMain.Text = "notifyIcon1";
            this.notifyIconMain.Visible = true;
            // 
            // columnHeaderTitle
            // 
            this.columnHeaderTitle.Text = "Заголовок";
            this.columnHeaderTitle.Width = 400;
            // 
            // columnHeaderSeason
            // 
            this.columnHeaderSeason.Text = "Сезон";
            this.columnHeaderSeason.Width = 50;
            // 
            // columnHeaderSeries
            // 
            this.columnHeaderSeries.Text = "Серии";
            this.columnHeaderSeries.Width = 90;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Домашний Театр";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.ResizeEnd += new System.EventHandler(this.FormMain_ResizeEnd);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.splitContainerMian.Panel1.ResumeLayout(false);
            this.splitContainerMian.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMian)).EndInit();
            this.splitContainerMian.ResumeLayout(false);
            this.splitContainerSubMain.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSubMain)).EndInit();
            this.splitContainerSubMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem авторизацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UpdateListsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.SplitContainer splitContainerMian;
        private System.Windows.Forms.ListView listViewDownload;
        private System.Windows.Forms.ColumnHeader columnHeaderDownloadTitle;
        private System.Windows.Forms.ColumnHeader columnHeaderDownloadSpeed;
        private System.Windows.Forms.ColumnHeader columnHeaderDownloadStatus;
        private System.Windows.Forms.ColumnHeader columnHeaderDownloadProgress;
        private System.Windows.Forms.Panel panelDownloadHeader;
        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private System.Windows.Forms.ToolStripMenuItem UpdateSerialsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UpdatePlaylistsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UpdateVideosToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerSubMain;
        private System.Windows.Forms.ListView listViewSerials;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ColumnHeader columnHeaderTitle;
        private System.Windows.Forms.ColumnHeader columnHeaderSeason;
        private System.Windows.Forms.ColumnHeader columnHeaderSeries;
    }
}

