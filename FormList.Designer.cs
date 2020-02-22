namespace HomeTheater
{
    partial class FormList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "tokenSource")]
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Обновления", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Хочу посмотреть", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Нет новых серий", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Посмотрел", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("В черном списке", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("", System.Windows.Forms.HorizontalAlignment.Left);
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.обновитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cпискиСериаловToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cтраницыСериаловToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cпискиПлейлиствоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.спискиВидеоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.всеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.остановитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заголовокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialTitleFull = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialTitleRU = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialTitleEN = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialTitleOriginal = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialSeasonID = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialSerialID = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialserialUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialGenre = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialCountry = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialRelease = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialIMDB = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialKinoPoisk = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialLimitation = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialUserComments = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialUserViewsLastDay = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialCompilation = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialMarkCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialMarkLast = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialMark = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSerialSiteUpdated = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSyncTimer = new System.Windows.Forms.ToolStripMenuItem();
            this.выполнитьСейчасToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerMainList = new System.Windows.Forms.SplitContainer();
            this.listSerials = new System.Windows.Forms.ListView();
            this.columnSerialTitleFull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialTitleRU = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialTitleEN = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialTitleOriginal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialserialUrl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialSeasonID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialSerialID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialSeason = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialGenre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialCountry = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialRelease = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialLimitation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialIMDB = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialKinoPoisk = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialUserComments = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialUserViewsLastDay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialMarkCurrent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialMarkLast = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialMark = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialCompilation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSerialSiteUpdated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripSerials = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.страницуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.плеерToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видеоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.отметитьНаПоследнейСерииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.хочуПосмотретьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ужеПосмотрелToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вЧерныйСписокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.textFilter = new System.Windows.Forms.TextBox();
            this.panelTexts = new System.Windows.Forms.Panel();
            this.labelSiteUpdated = new System.Windows.Forms.Label();
            this.labelCompilation = new System.Windows.Forms.Label();
            this.labelMark = new System.Windows.Forms.Label();
            this.labelIMDB = new System.Windows.Forms.Label();
            this.labelKinoPoisk = new System.Windows.Forms.Label();
            this.labelLSiteUpdated = new System.Windows.Forms.Label();
            this.labelLCompilation = new System.Windows.Forms.Label();
            this.labelLMark = new System.Windows.Forms.Label();
            this.labelLKinoPoisk = new System.Windows.Forms.Label();
            this.labelLIMDB = new System.Windows.Forms.Label();
            this.labelRelease = new System.Windows.Forms.Label();
            this.labelLRelease = new System.Windows.Forms.Label();
            this.labelCountry = new System.Windows.Forms.Label();
            this.labelLCountry = new System.Windows.Forms.Label();
            this.labelGenre = new System.Windows.Forms.Label();
            this.labelLGenre = new System.Windows.Forms.Label();
            this.linkLabelTitleFULL = new System.Windows.Forms.LinkLabel();
            this.labelDescription = new System.Windows.Forms.Label();
            this.pictureSeasonImage = new System.Windows.Forms.PictureBox();
            this.listDownload = new System.Windows.Forms.ListView();
            this.columnDownloadTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDownloadSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDownloadStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDownloadProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolDownload = new System.Windows.Forms.ToolStrip();
            this.toolStripResme = new System.Windows.Forms.ToolStripButton();
            this.toolStripPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripErase = new System.Windows.Forms.ToolStripButton();
            this.toolStripOpenFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStripResumeAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripPauseAll = new System.Windows.Forms.ToolStripButton();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainList)).BeginInit();
            this.splitContainerMainList.Panel1.SuspendLayout();
            this.splitContainerMainList.Panel2.SuspendLayout();
            this.splitContainerMainList.SuspendLayout();
            this.contextMenuStripSerials.SuspendLayout();
            this.panelFilter.SuspendLayout();
            this.panelTexts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSeasonImage)).BeginInit();
            this.toolDownload.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.обновитьToolStripMenuItem,
            this.видToolStripMenuItem,
            this.toolStripSyncTimer});
            this.menuMain.Location = new System.Drawing.Point(6, 6);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(982, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuMain";
            this.menuMain.Visible = false;
            // 
            // обновитьToolStripMenuItem
            // 
            this.обновитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cпискиСериаловToolStripMenuItem,
            this.cтраницыСериаловToolStripMenuItem,
            this.cпискиПлейлиствоToolStripMenuItem,
            this.спискиВидеоToolStripMenuItem,
            this.toolStripMenuItem1,
            this.всеToolStripMenuItem,
            this.остановитьToolStripMenuItem});
            this.обновитьToolStripMenuItem.Enabled = false;
            this.обновитьToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.обновитьToolStripMenuItem.MergeIndex = 30;
            this.обновитьToolStripMenuItem.Name = "обновитьToolStripMenuItem";
            this.обновитьToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.обновитьToolStripMenuItem.Text = "&Обновить";
            // 
            // cпискиСериаловToolStripMenuItem
            // 
            this.cпискиСериаловToolStripMenuItem.Name = "cпискиСериаловToolStripMenuItem";
            this.cпискиСериаловToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.cпискиСериаловToolStripMenuItem.Text = "Cписки сериалов";
            this.cпискиСериаловToolStripMenuItem.Click += new System.EventHandler(this.cпискиСериаловToolStripMenuItem_Click);
            // 
            // cтраницыСериаловToolStripMenuItem
            // 
            this.cтраницыСериаловToolStripMenuItem.Name = "cтраницыСериаловToolStripMenuItem";
            this.cтраницыСериаловToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.cтраницыСериаловToolStripMenuItem.Text = "Cтраницы сериалов";
            this.cтраницыСериаловToolStripMenuItem.Click += new System.EventHandler(this.cтраницыСериаловToolStripMenuItem_Click);
            // 
            // cпискиПлейлиствоToolStripMenuItem
            // 
            this.cпискиПлейлиствоToolStripMenuItem.Name = "cпискиПлейлиствоToolStripMenuItem";
            this.cпискиПлейлиствоToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.cпискиПлейлиствоToolStripMenuItem.Text = "Cписки плейлиство";
            this.cпискиПлейлиствоToolStripMenuItem.Click += new System.EventHandler(this.cпискиПлейлиствоToolStripMenuItem_Click);
            // 
            // спискиВидеоToolStripMenuItem
            // 
            this.спискиВидеоToolStripMenuItem.Name = "спискиВидеоToolStripMenuItem";
            this.спискиВидеоToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.спискиВидеоToolStripMenuItem.Text = "Списки видео";
            this.спискиВидеоToolStripMenuItem.Click += new System.EventHandler(this.спискиВидеоToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(182, 6);
            // 
            // всеToolStripMenuItem
            // 
            this.всеToolStripMenuItem.Name = "всеToolStripMenuItem";
            this.всеToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.всеToolStripMenuItem.Text = "Все";
            this.всеToolStripMenuItem.Click += new System.EventHandler(this.всеToolStripMenuItem_Click);
            // 
            // остановитьToolStripMenuItem
            // 
            this.остановитьToolStripMenuItem.Name = "остановитьToolStripMenuItem";
            this.остановитьToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.остановитьToolStripMenuItem.Text = "Остановить";
            this.остановитьToolStripMenuItem.Visible = false;
            this.остановитьToolStripMenuItem.Click += new System.EventHandler(this.остановитьToolStripMenuItem_Click);
            // 
            // видToolStripMenuItem
            // 
            this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.заголовокToolStripMenuItem,
            this.ToolStripMenuItemSerialSeasonID,
            this.ToolStripMenuItemSerialSerialID,
            this.ToolStripMenuItemSerialserialUrl,
            this.ToolStripMenuItemSerialGenre,
            this.ToolStripMenuItemSerialCountry,
            this.ToolStripMenuItemSerialRelease,
            this.ToolStripMenuItemSerialIMDB,
            this.ToolStripMenuItemSerialKinoPoisk,
            this.ToolStripMenuItemSerialLimitation,
            this.ToolStripMenuItemSerialUserComments,
            this.ToolStripMenuItemSerialUserViewsLastDay,
            this.ToolStripMenuItemSerialCompilation,
            this.ToolStripMenuItemSerialMarkCurrent,
            this.ToolStripMenuItemSerialMarkLast,
            this.ToolStripMenuItemSerialMark,
            this.ToolStripMenuItemSerialSiteUpdated});
            this.видToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.видToolStripMenuItem.MergeIndex = 40;
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.видToolStripMenuItem.Text = "&Вид";
            // 
            // заголовокToolStripMenuItem
            // 
            this.заголовокToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemSerialTitleFull,
            this.ToolStripMenuItemSerialTitle,
            this.ToolStripMenuItemSerialTitleRU,
            this.ToolStripMenuItemSerialTitleEN,
            this.ToolStripMenuItemSerialTitleOriginal});
            this.заголовокToolStripMenuItem.Name = "заголовокToolStripMenuItem";
            this.заголовокToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.заголовокToolStripMenuItem.Text = "Заголовок";
            // 
            // ToolStripMenuItemSerialTitleFull
            // 
            this.ToolStripMenuItemSerialTitleFull.Name = "ToolStripMenuItemSerialTitleFull";
            this.ToolStripMenuItemSerialTitleFull.Size = new System.Drawing.Size(158, 22);
            this.ToolStripMenuItemSerialTitleFull.Text = "Полный";
            this.ToolStripMenuItemSerialTitleFull.Click += new System.EventHandler(this.ToolStripMenuItemSerialTitleFull_Click);
            // 
            // ToolStripMenuItemSerialTitle
            // 
            this.ToolStripMenuItemSerialTitle.Name = "ToolStripMenuItemSerialTitle";
            this.ToolStripMenuItemSerialTitle.Size = new System.Drawing.Size(158, 22);
            this.ToolStripMenuItemSerialTitle.Text = "Простой";
            this.ToolStripMenuItemSerialTitle.Click += new System.EventHandler(this.ToolStripMenuItemSerialTitle_Click);
            // 
            // ToolStripMenuItemSerialTitleRU
            // 
            this.ToolStripMenuItemSerialTitleRU.Name = "ToolStripMenuItemSerialTitleRU";
            this.ToolStripMenuItemSerialTitleRU.Size = new System.Drawing.Size(158, 22);
            this.ToolStripMenuItemSerialTitleRU.Text = "Русский";
            this.ToolStripMenuItemSerialTitleRU.Click += new System.EventHandler(this.ToolStripMenuItemSerialTitleRU_Click);
            // 
            // ToolStripMenuItemSerialTitleEN
            // 
            this.ToolStripMenuItemSerialTitleEN.Name = "ToolStripMenuItemSerialTitleEN";
            this.ToolStripMenuItemSerialTitleEN.Size = new System.Drawing.Size(158, 22);
            this.ToolStripMenuItemSerialTitleEN.Text = "Английский";
            this.ToolStripMenuItemSerialTitleEN.Click += new System.EventHandler(this.ToolStripMenuItemSerialTitleEN_Click);
            // 
            // ToolStripMenuItemSerialTitleOriginal
            // 
            this.ToolStripMenuItemSerialTitleOriginal.Name = "ToolStripMenuItemSerialTitleOriginal";
            this.ToolStripMenuItemSerialTitleOriginal.Size = new System.Drawing.Size(158, 22);
            this.ToolStripMenuItemSerialTitleOriginal.Text = "Оригинальный";
            this.ToolStripMenuItemSerialTitleOriginal.Click += new System.EventHandler(this.ToolStripMenuItemSerialTitleOriginal_Click);
            // 
            // ToolStripMenuItemSerialSeasonID
            // 
            this.ToolStripMenuItemSerialSeasonID.Name = "ToolStripMenuItemSerialSeasonID";
            this.ToolStripMenuItemSerialSeasonID.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialSeasonID.Text = "ID";
            this.ToolStripMenuItemSerialSeasonID.Click += new System.EventHandler(this.ToolStripMenuItemSerialSeasonID_Click);
            // 
            // ToolStripMenuItemSerialSerialID
            // 
            this.ToolStripMenuItemSerialSerialID.Name = "ToolStripMenuItemSerialSerialID";
            this.ToolStripMenuItemSerialSerialID.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialSerialID.Text = "ID Сериала";
            this.ToolStripMenuItemSerialSerialID.Click += new System.EventHandler(this.ToolStripMenuItemSerialSerialID_Click);
            // 
            // ToolStripMenuItemSerialserialUrl
            // 
            this.ToolStripMenuItemSerialserialUrl.Name = "ToolStripMenuItemSerialserialUrl";
            this.ToolStripMenuItemSerialserialUrl.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialserialUrl.Text = "URL";
            this.ToolStripMenuItemSerialserialUrl.Click += new System.EventHandler(this.ToolStripMenuItemSerialserialUrl_Click);
            // 
            // ToolStripMenuItemSerialGenre
            // 
            this.ToolStripMenuItemSerialGenre.Name = "ToolStripMenuItemSerialGenre";
            this.ToolStripMenuItemSerialGenre.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialGenre.Text = "Жанр";
            this.ToolStripMenuItemSerialGenre.Click += new System.EventHandler(this.ToolStripMenuItemSerialGenre_Click);
            // 
            // ToolStripMenuItemSerialCountry
            // 
            this.ToolStripMenuItemSerialCountry.Name = "ToolStripMenuItemSerialCountry";
            this.ToolStripMenuItemSerialCountry.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialCountry.Text = "Страна";
            this.ToolStripMenuItemSerialCountry.Click += new System.EventHandler(this.ToolStripMenuItemSerialCountry_Click);
            // 
            // ToolStripMenuItemSerialRelease
            // 
            this.ToolStripMenuItemSerialRelease.Name = "ToolStripMenuItemSerialRelease";
            this.ToolStripMenuItemSerialRelease.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialRelease.Text = "Вышел";
            this.ToolStripMenuItemSerialRelease.Click += new System.EventHandler(this.ToolStripMenuItemSerialRelease_Click);
            // 
            // ToolStripMenuItemSerialIMDB
            // 
            this.ToolStripMenuItemSerialIMDB.Name = "ToolStripMenuItemSerialIMDB";
            this.ToolStripMenuItemSerialIMDB.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialIMDB.Text = "IMDB";
            this.ToolStripMenuItemSerialIMDB.Click += new System.EventHandler(this.ToolStripMenuItemSerialIMDB_Click);
            // 
            // ToolStripMenuItemSerialKinoPoisk
            // 
            this.ToolStripMenuItemSerialKinoPoisk.Name = "ToolStripMenuItemSerialKinoPoisk";
            this.ToolStripMenuItemSerialKinoPoisk.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialKinoPoisk.Text = "Кинопоиск";
            this.ToolStripMenuItemSerialKinoPoisk.Click += new System.EventHandler(this.ToolStripMenuItemSerialKinoPoisk_Click);
            // 
            // ToolStripMenuItemSerialLimitation
            // 
            this.ToolStripMenuItemSerialLimitation.Name = "ToolStripMenuItemSerialLimitation";
            this.ToolStripMenuItemSerialLimitation.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialLimitation.Text = "Ограничение";
            this.ToolStripMenuItemSerialLimitation.Click += new System.EventHandler(this.ToolStripMenuItemSerialLimitation_Click);
            // 
            // ToolStripMenuItemSerialUserComments
            // 
            this.ToolStripMenuItemSerialUserComments.Name = "ToolStripMenuItemSerialUserComments";
            this.ToolStripMenuItemSerialUserComments.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialUserComments.Text = "Коментарии";
            this.ToolStripMenuItemSerialUserComments.Click += new System.EventHandler(this.ToolStripMenuItemSerialUserComments_Click);
            // 
            // ToolStripMenuItemSerialUserViewsLastDay
            // 
            this.ToolStripMenuItemSerialUserViewsLastDay.Name = "ToolStripMenuItemSerialUserViewsLastDay";
            this.ToolStripMenuItemSerialUserViewsLastDay.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialUserViewsLastDay.Text = "Просмотры";
            this.ToolStripMenuItemSerialUserViewsLastDay.Click += new System.EventHandler(this.ToolStripMenuItemSerialUserViewsLastDay_Click);
            // 
            // ToolStripMenuItemSerialCompilation
            // 
            this.ToolStripMenuItemSerialCompilation.Name = "ToolStripMenuItemSerialCompilation";
            this.ToolStripMenuItemSerialCompilation.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialCompilation.Text = "Колекция";
            this.ToolStripMenuItemSerialCompilation.Click += new System.EventHandler(this.ToolStripMenuItemSerialCompilation_Click);
            // 
            // ToolStripMenuItemSerialMarkCurrent
            // 
            this.ToolStripMenuItemSerialMarkCurrent.Name = "ToolStripMenuItemSerialMarkCurrent";
            this.ToolStripMenuItemSerialMarkCurrent.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialMarkCurrent.Text = "Текущая серия";
            this.ToolStripMenuItemSerialMarkCurrent.Click += new System.EventHandler(this.ToolStripMenuItemSerialMarkCurrent_Click);
            // 
            // ToolStripMenuItemSerialMarkLast
            // 
            this.ToolStripMenuItemSerialMarkLast.Name = "ToolStripMenuItemSerialMarkLast";
            this.ToolStripMenuItemSerialMarkLast.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialMarkLast.Text = "Последняя серия";
            this.ToolStripMenuItemSerialMarkLast.Click += new System.EventHandler(this.ToolStripMenuItemSerialMarkLast_Click);
            // 
            // ToolStripMenuItemSerialMark
            // 
            this.ToolStripMenuItemSerialMark.Name = "ToolStripMenuItemSerialMark";
            this.ToolStripMenuItemSerialMark.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialMark.Text = "Отметка";
            this.ToolStripMenuItemSerialMark.Click += new System.EventHandler(this.ToolStripMenuItemSerialMark_Click);
            // 
            // ToolStripMenuItemSerialSiteUpdated
            // 
            this.ToolStripMenuItemSerialSiteUpdated.Name = "ToolStripMenuItemSerialSiteUpdated";
            this.ToolStripMenuItemSerialSiteUpdated.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItemSerialSiteUpdated.Text = "Последнее обновление";
            this.ToolStripMenuItemSerialSiteUpdated.Click += new System.EventHandler(this.ToolStripMenuItemSerialSiteUpdated_Click);
            // 
            // toolStripSyncTimer
            // 
            this.toolStripSyncTimer.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSyncTimer.AutoToolTip = true;
            this.toolStripSyncTimer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.выполнитьСейчасToolStripMenuItem});
            this.toolStripSyncTimer.Name = "toolStripSyncTimer";
            this.toolStripSyncTimer.Size = new System.Drawing.Size(42, 20);
            this.toolStripSyncTimer.Text = "--:--";
            this.toolStripSyncTimer.Visible = false;
            // 
            // выполнитьСейчасToolStripMenuItem
            // 
            this.выполнитьСейчасToolStripMenuItem.AutoToolTip = true;
            this.выполнитьСейчасToolStripMenuItem.Image = global::HomeTheater.Properties.Resources.control;
            this.выполнитьСейчасToolStripMenuItem.Name = "выполнитьСейчасToolStripMenuItem";
            this.выполнитьСейчасToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.выполнитьСейчасToolStripMenuItem.Text = "Выполнить сейчас";
            this.выполнитьСейчасToolStripMenuItem.Click += new System.EventHandler(this.выполнитьСейчасToolStripMenuItem_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Enabled = false;
            this.splitContainerMain.Location = new System.Drawing.Point(6, 6);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerMainList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.listDownload);
            this.splitContainerMain.Panel2.Controls.Add(this.toolDownload);
            this.splitContainerMain.Size = new System.Drawing.Size(982, 656);
            this.splitContainerMain.SplitterDistance = 423;
            this.splitContainerMain.TabIndex = 0;
            // 
            // splitContainerMainList
            // 
            this.splitContainerMainList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMainList.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMainList.Name = "splitContainerMainList";
            // 
            // splitContainerMainList.Panel1
            // 
            this.splitContainerMainList.Panel1.Controls.Add(this.listSerials);
            this.splitContainerMainList.Panel1.Controls.Add(this.panelFilter);
            // 
            // splitContainerMainList.Panel2
            // 
            this.splitContainerMainList.Panel2.Controls.Add(this.panelTexts);
            this.splitContainerMainList.Panel2.Controls.Add(this.labelDescription);
            this.splitContainerMainList.Panel2.Controls.Add(this.pictureSeasonImage);
            this.splitContainerMainList.Panel2.ClientSizeChanged += new System.EventHandler(this.splitContainerMainList_Panel2_ClientSizeChanged);
            this.splitContainerMainList.Panel2MinSize = 211;
            this.splitContainerMainList.Size = new System.Drawing.Size(982, 423);
            this.splitContainerMainList.SplitterDistance = 760;
            this.splitContainerMainList.TabIndex = 0;
            // 
            // listSerials
            // 
            this.listSerials.AllowColumnReorder = true;
            this.listSerials.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnSerialTitleFull,
            this.columnSerialTitle,
            this.columnSerialTitleRU,
            this.columnSerialTitleEN,
            this.columnSerialTitleOriginal,
            this.columnSerialserialUrl,
            this.columnSerialSeasonID,
            this.columnSerialSerialID,
            this.columnSerialSeason,
            this.columnSerialGenre,
            this.columnSerialCountry,
            this.columnSerialRelease,
            this.columnSerialLimitation,
            this.columnSerialIMDB,
            this.columnSerialKinoPoisk,
            this.columnSerialUserComments,
            this.columnSerialUserViewsLastDay,
            this.columnSerialMarkCurrent,
            this.columnSerialMarkLast,
            this.columnSerialMark,
            this.columnSerialCompilation,
            this.columnSerialSiteUpdated});
            this.listSerials.ContextMenuStrip = this.contextMenuStripSerials;
            this.listSerials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSerials.Enabled = false;
            this.listSerials.FullRowSelect = true;
            this.listSerials.GridLines = true;
            listViewGroup1.Header = "Обновления";
            listViewGroup1.Name = "listViewGroupnew";
            listViewGroup2.Header = "Хочу посмотреть";
            listViewGroup2.Name = "listViewGroupwant";
            listViewGroup3.Header = "Нет новых серий";
            listViewGroup3.Name = "listViewGroupnonew";
            listViewGroup4.Header = "Посмотрел";
            listViewGroup4.Name = "listViewGroupwatched";
            listViewGroup5.Header = "В черном списке";
            listViewGroup5.Name = "listViewGroupnotwatch";
            listViewGroup6.Header = "";
            listViewGroup6.Name = "listViewGroupnone";
            this.listSerials.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6});
            this.listSerials.HideSelection = false;
            this.listSerials.Location = new System.Drawing.Point(0, 26);
            this.listSerials.Name = "listSerials";
            this.listSerials.Size = new System.Drawing.Size(760, 397);
            this.listSerials.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listSerials.TabIndex = 4;
            this.listSerials.UseCompatibleStateImageBehavior = false;
            this.listSerials.View = System.Windows.Forms.View.Details;
            this.listSerials.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listSerials_ColumnClick);
            this.listSerials.SelectedIndexChanged += new System.EventHandler(this.listSerials_SelectedIndexChanged);
            this.listSerials.ClientSizeChanged += new System.EventHandler(this.listSerials_ClientSizeChanged);
            this.listSerials.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listSerials_MouseClick);
            this.listSerials.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listSerials_MouseDoubleClick);
            // 
            // columnSerialTitleFull
            // 
            this.columnSerialTitleFull.Text = "Заголовок";
            this.columnSerialTitleFull.Width = 0;
            // 
            // columnSerialTitle
            // 
            this.columnSerialTitle.DisplayIndex = 17;
            this.columnSerialTitle.Text = "Заголовок";
            this.columnSerialTitle.Width = 250;
            // 
            // columnSerialTitleRU
            // 
            this.columnSerialTitleRU.DisplayIndex = 1;
            this.columnSerialTitleRU.Text = "Заголовок";
            this.columnSerialTitleRU.Width = 0;
            // 
            // columnSerialTitleEN
            // 
            this.columnSerialTitleEN.DisplayIndex = 2;
            this.columnSerialTitleEN.Text = "Заголовок";
            this.columnSerialTitleEN.Width = 0;
            // 
            // columnSerialTitleOriginal
            // 
            this.columnSerialTitleOriginal.DisplayIndex = 3;
            this.columnSerialTitleOriginal.Text = "Заголовок";
            this.columnSerialTitleOriginal.Width = 0;
            // 
            // columnSerialserialUrl
            // 
            this.columnSerialserialUrl.DisplayIndex = 4;
            this.columnSerialserialUrl.Text = "URL";
            this.columnSerialserialUrl.Width = 0;
            // 
            // columnSerialSeasonID
            // 
            this.columnSerialSeasonID.DisplayIndex = 5;
            this.columnSerialSeasonID.Text = "ID";
            this.columnSerialSeasonID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialSeasonID.Width = 0;
            // 
            // columnSerialSerialID
            // 
            this.columnSerialSerialID.DisplayIndex = 6;
            this.columnSerialSerialID.Text = "ID Сериала";
            this.columnSerialSerialID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialSerialID.Width = 0;
            // 
            // columnSerialSeason
            // 
            this.columnSerialSeason.DisplayIndex = 18;
            this.columnSerialSeason.Text = "Сезон";
            this.columnSerialSeason.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialSeason.Width = 25;
            // 
            // columnSerialGenre
            // 
            this.columnSerialGenre.DisplayIndex = 19;
            this.columnSerialGenre.Text = "Жанр";
            this.columnSerialGenre.Width = 150;
            // 
            // columnSerialCountry
            // 
            this.columnSerialCountry.DisplayIndex = 7;
            this.columnSerialCountry.Text = "Страна";
            this.columnSerialCountry.Width = 0;
            // 
            // columnSerialRelease
            // 
            this.columnSerialRelease.DisplayIndex = 8;
            this.columnSerialRelease.Text = "Вышел";
            this.columnSerialRelease.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialRelease.Width = 0;
            // 
            // columnSerialLimitation
            // 
            this.columnSerialLimitation.DisplayIndex = 9;
            this.columnSerialLimitation.Text = "Ограничение";
            this.columnSerialLimitation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialLimitation.Width = 0;
            // 
            // columnSerialIMDB
            // 
            this.columnSerialIMDB.DisplayIndex = 10;
            this.columnSerialIMDB.Text = "IMDB";
            this.columnSerialIMDB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialIMDB.Width = 0;
            // 
            // columnSerialKinoPoisk
            // 
            this.columnSerialKinoPoisk.DisplayIndex = 11;
            this.columnSerialKinoPoisk.Text = "Кинопоиск";
            this.columnSerialKinoPoisk.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialKinoPoisk.Width = 0;
            // 
            // columnSerialUserComments
            // 
            this.columnSerialUserComments.DisplayIndex = 12;
            this.columnSerialUserComments.Text = "Коментарии";
            this.columnSerialUserComments.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialUserComments.Width = 0;
            // 
            // columnSerialUserViewsLastDay
            // 
            this.columnSerialUserViewsLastDay.DisplayIndex = 13;
            this.columnSerialUserViewsLastDay.Text = "Просмотры";
            this.columnSerialUserViewsLastDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialUserViewsLastDay.Width = 0;
            // 
            // columnSerialMarkCurrent
            // 
            this.columnSerialMarkCurrent.DisplayIndex = 14;
            this.columnSerialMarkCurrent.Text = "Текущая серия";
            this.columnSerialMarkCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialMarkCurrent.Width = 0;
            // 
            // columnSerialMarkLast
            // 
            this.columnSerialMarkLast.DisplayIndex = 15;
            this.columnSerialMarkLast.Text = "Последняя серия";
            this.columnSerialMarkLast.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialMarkLast.Width = 0;
            // 
            // columnSerialMark
            // 
            this.columnSerialMark.DisplayIndex = 21;
            this.columnSerialMark.Text = "Отметка";
            this.columnSerialMark.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialMark.Width = 100;
            // 
            // columnSerialCompilation
            // 
            this.columnSerialCompilation.Text = "Колекция";
            this.columnSerialCompilation.Width = 100;
            // 
            // columnSerialSiteUpdated
            // 
            this.columnSerialSiteUpdated.DisplayIndex = 16;
            this.columnSerialSiteUpdated.Text = "Последнее обновление";
            this.columnSerialSiteUpdated.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSerialSiteUpdated.Width = 0;
            // 
            // contextMenuStripSerials
            // 
            this.contextMenuStripSerials.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem,
            this.toolStripUpdate,
            this.toolStripSeparator1,
            this.отметитьНаПоследнейСерииToolStripMenuItem,
            this.хочуПосмотретьToolStripMenuItem,
            this.ужеПосмотрелToolStripMenuItem,
            this.вЧерныйСписокToolStripMenuItem,
            this.toolStripMenuItem3,
            this.удалитьToolStripMenuItem});
            this.contextMenuStripSerials.Name = "contextMenuStrip1";
            this.contextMenuStripSerials.Size = new System.Drawing.Size(241, 170);
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // toolStripUpdate
            // 
            this.toolStripUpdate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.страницуToolStripMenuItem,
            this.плеерToolStripMenuItem,
            this.видеоToolStripMenuItem});
            this.toolStripUpdate.Enabled = false;
            this.toolStripUpdate.Name = "toolStripUpdate";
            this.toolStripUpdate.Size = new System.Drawing.Size(240, 22);
            this.toolStripUpdate.Text = "Обновить";
            this.toolStripUpdate.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // страницуToolStripMenuItem
            // 
            this.страницуToolStripMenuItem.Name = "страницуToolStripMenuItem";
            this.страницуToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.страницуToolStripMenuItem.Text = "Страницу";
            this.страницуToolStripMenuItem.Click += new System.EventHandler(this.страницуToolStripMenuItem_Click);
            // 
            // плеерToolStripMenuItem
            // 
            this.плеерToolStripMenuItem.Name = "плеерToolStripMenuItem";
            this.плеерToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.плеерToolStripMenuItem.Text = "Плеер";
            this.плеерToolStripMenuItem.Click += new System.EventHandler(this.плеерToolStripMenuItem_Click);
            // 
            // видеоToolStripMenuItem
            // 
            this.видеоToolStripMenuItem.Name = "видеоToolStripMenuItem";
            this.видеоToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.видеоToolStripMenuItem.Text = "Видео";
            this.видеоToolStripMenuItem.Click += new System.EventHandler(this.видеоToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(237, 6);
            // 
            // отметитьНаПоследнейСерииToolStripMenuItem
            // 
            this.отметитьНаПоследнейСерииToolStripMenuItem.Name = "отметитьНаПоследнейСерииToolStripMenuItem";
            this.отметитьНаПоследнейСерииToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.отметитьНаПоследнейСерииToolStripMenuItem.Text = "Отметить на последней серии";
            this.отметитьНаПоследнейСерииToolStripMenuItem.Click += new System.EventHandler(this.отметитьНаПоследнейСерииToolStripMenuItem_Click);
            // 
            // хочуПосмотретьToolStripMenuItem
            // 
            this.хочуПосмотретьToolStripMenuItem.Name = "хочуПосмотретьToolStripMenuItem";
            this.хочуПосмотретьToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.хочуПосмотретьToolStripMenuItem.Text = "Хочу посмотреть";
            this.хочуПосмотретьToolStripMenuItem.Click += new System.EventHandler(this.хочуПосмотретьToolStripMenuItem_Click);
            // 
            // ужеПосмотрелToolStripMenuItem
            // 
            this.ужеПосмотрелToolStripMenuItem.Name = "ужеПосмотрелToolStripMenuItem";
            this.ужеПосмотрелToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.ужеПосмотрелToolStripMenuItem.Text = "Уже посмотрел";
            this.ужеПосмотрелToolStripMenuItem.Click += new System.EventHandler(this.ужеПосмотрелToolStripMenuItem_Click);
            // 
            // вЧерныйСписокToolStripMenuItem
            // 
            this.вЧерныйСписокToolStripMenuItem.Name = "вЧерныйСписокToolStripMenuItem";
            this.вЧерныйСписокToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.вЧерныйСписокToolStripMenuItem.Text = "В черный список";
            this.вЧерныйСписокToolStripMenuItem.Click += new System.EventHandler(this.вЧерныйСписокToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(237, 6);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
            // 
            // panelFilter
            // 
            this.panelFilter.Controls.Add(this.textFilter);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(0, 0);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(760, 26);
            this.panelFilter.TabIndex = 3;
            // 
            // textFilter
            // 
            this.textFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textFilter.Location = new System.Drawing.Point(0, 0);
            this.textFilter.Name = "textFilter";
            this.textFilter.Size = new System.Drawing.Size(760, 20);
            this.textFilter.TabIndex = 0;
            this.textFilter.TextChanged += new System.EventHandler(this.textFilter_TextChanged);
            // 
            // panelTexts
            // 
            this.panelTexts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTexts.Controls.Add(this.labelSiteUpdated);
            this.panelTexts.Controls.Add(this.labelCompilation);
            this.panelTexts.Controls.Add(this.labelMark);
            this.panelTexts.Controls.Add(this.labelIMDB);
            this.panelTexts.Controls.Add(this.labelKinoPoisk);
            this.panelTexts.Controls.Add(this.labelLSiteUpdated);
            this.panelTexts.Controls.Add(this.labelLCompilation);
            this.panelTexts.Controls.Add(this.labelLMark);
            this.panelTexts.Controls.Add(this.labelLKinoPoisk);
            this.panelTexts.Controls.Add(this.labelLIMDB);
            this.panelTexts.Controls.Add(this.labelRelease);
            this.panelTexts.Controls.Add(this.labelLRelease);
            this.panelTexts.Controls.Add(this.labelCountry);
            this.panelTexts.Controls.Add(this.labelLCountry);
            this.panelTexts.Controls.Add(this.labelGenre);
            this.panelTexts.Controls.Add(this.labelLGenre);
            this.panelTexts.Controls.Add(this.linkLabelTitleFULL);
            this.panelTexts.Location = new System.Drawing.Point(0, 190);
            this.panelTexts.Name = "panelTexts";
            this.panelTexts.Padding = new System.Windows.Forms.Padding(6);
            this.panelTexts.Size = new System.Drawing.Size(218, 190);
            this.panelTexts.TabIndex = 3;
            this.panelTexts.Visible = false;
            // 
            // labelSiteUpdated
            // 
            this.labelSiteUpdated.AutoSize = true;
            this.labelSiteUpdated.Location = new System.Drawing.Point(150, 175);
            this.labelSiteUpdated.Name = "labelSiteUpdated";
            this.labelSiteUpdated.Size = new System.Drawing.Size(61, 13);
            this.labelSiteUpdated.TabIndex = 16;
            this.labelSiteUpdated.Text = "00.00.0000";
            // 
            // labelCompilation
            // 
            this.labelCompilation.AutoSize = true;
            this.labelCompilation.Location = new System.Drawing.Point(68, 160);
            this.labelCompilation.Name = "labelCompilation";
            this.labelCompilation.Size = new System.Drawing.Size(0, 13);
            this.labelCompilation.TabIndex = 15;
            // 
            // labelMark
            // 
            this.labelMark.AutoSize = true;
            this.labelMark.Location = new System.Drawing.Point(62, 145);
            this.labelMark.Name = "labelMark";
            this.labelMark.Size = new System.Drawing.Size(0, 13);
            this.labelMark.TabIndex = 14;
            // 
            // labelIMDB
            // 
            this.labelIMDB.AutoSize = true;
            this.labelIMDB.Location = new System.Drawing.Point(134, 115);
            this.labelIMDB.Name = "labelIMDB";
            this.labelIMDB.Size = new System.Drawing.Size(0, 13);
            this.labelIMDB.TabIndex = 13;
            // 
            // labelKinoPoisk
            // 
            this.labelKinoPoisk.AutoSize = true;
            this.labelKinoPoisk.Location = new System.Drawing.Point(134, 130);
            this.labelKinoPoisk.Name = "labelKinoPoisk";
            this.labelKinoPoisk.Size = new System.Drawing.Size(0, 13);
            this.labelKinoPoisk.TabIndex = 12;
            // 
            // labelLSiteUpdated
            // 
            this.labelLSiteUpdated.AutoSize = true;
            this.labelLSiteUpdated.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLSiteUpdated.Location = new System.Drawing.Point(3, 175);
            this.labelLSiteUpdated.Name = "labelLSiteUpdated";
            this.labelLSiteUpdated.Size = new System.Drawing.Size(150, 13);
            this.labelLSiteUpdated.TabIndex = 11;
            this.labelLSiteUpdated.Text = "Последнее обновление:";
            // 
            // labelLCompilation
            // 
            this.labelLCompilation.AutoSize = true;
            this.labelLCompilation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLCompilation.Location = new System.Drawing.Point(3, 160);
            this.labelLCompilation.Name = "labelLCompilation";
            this.labelLCompilation.Size = new System.Drawing.Size(68, 13);
            this.labelLCompilation.TabIndex = 10;
            this.labelLCompilation.Text = "Колекция:";
            // 
            // labelLMark
            // 
            this.labelLMark.AutoSize = true;
            this.labelLMark.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLMark.Location = new System.Drawing.Point(3, 145);
            this.labelLMark.Name = "labelLMark";
            this.labelLMark.Size = new System.Drawing.Size(62, 13);
            this.labelLMark.TabIndex = 9;
            this.labelLMark.Text = "Отметка:";
            // 
            // labelLKinoPoisk
            // 
            this.labelLKinoPoisk.AutoSize = true;
            this.labelLKinoPoisk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLKinoPoisk.Location = new System.Drawing.Point(3, 130);
            this.labelLKinoPoisk.Name = "labelLKinoPoisk";
            this.labelLKinoPoisk.Size = new System.Drawing.Size(129, 13);
            this.labelLKinoPoisk.TabIndex = 8;
            this.labelLKinoPoisk.Text = "Рейтинг КиноПоиск:";
            // 
            // labelLIMDB
            // 
            this.labelLIMDB.AutoSize = true;
            this.labelLIMDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLIMDB.Location = new System.Drawing.Point(3, 115);
            this.labelLIMDB.Name = "labelLIMDB";
            this.labelLIMDB.Size = new System.Drawing.Size(94, 13);
            this.labelLIMDB.TabIndex = 7;
            this.labelLIMDB.Text = "Рейтинг IMDB:";
            // 
            // labelRelease
            // 
            this.labelRelease.AutoSize = true;
            this.labelRelease.Location = new System.Drawing.Point(62, 100);
            this.labelRelease.Name = "labelRelease";
            this.labelRelease.Size = new System.Drawing.Size(0, 13);
            this.labelRelease.TabIndex = 6;
            // 
            // labelLRelease
            // 
            this.labelLRelease.AutoSize = true;
            this.labelLRelease.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLRelease.Location = new System.Drawing.Point(3, 100);
            this.labelLRelease.Name = "labelLRelease";
            this.labelLRelease.Size = new System.Drawing.Size(51, 13);
            this.labelLRelease.TabIndex = 5;
            this.labelLRelease.Text = "Вышел:";
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(62, 85);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(0, 13);
            this.labelCountry.TabIndex = 4;
            // 
            // labelLCountry
            // 
            this.labelLCountry.AutoSize = true;
            this.labelLCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLCountry.Location = new System.Drawing.Point(3, 85);
            this.labelLCountry.Name = "labelLCountry";
            this.labelLCountry.Size = new System.Drawing.Size(53, 13);
            this.labelLCountry.TabIndex = 3;
            this.labelLCountry.Text = "Страна:";
            // 
            // labelGenre
            // 
            this.labelGenre.AutoSize = true;
            this.labelGenre.Location = new System.Drawing.Point(62, 70);
            this.labelGenre.Name = "labelGenre";
            this.labelGenre.Size = new System.Drawing.Size(0, 13);
            this.labelGenre.TabIndex = 2;
            // 
            // labelLGenre
            // 
            this.labelLGenre.AutoSize = true;
            this.labelLGenre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLGenre.Location = new System.Drawing.Point(3, 70);
            this.labelLGenre.Name = "labelLGenre";
            this.labelLGenre.Size = new System.Drawing.Size(44, 13);
            this.labelLGenre.TabIndex = 1;
            this.labelLGenre.Text = "Жанр:";
            // 
            // linkLabelTitleFULL
            // 
            this.linkLabelTitleFULL.AutoSize = true;
            this.linkLabelTitleFULL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabelTitleFULL.Location = new System.Drawing.Point(3, 9);
            this.linkLabelTitleFULL.Name = "linkLabelTitleFULL";
            this.linkLabelTitleFULL.Size = new System.Drawing.Size(0, 20);
            this.linkLabelTitleFULL.TabIndex = 0;
            this.linkLabelTitleFULL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelTitleFULL_LinkClicked);
            // 
            // labelDescription
            // 
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelDescription.Location = new System.Drawing.Point(0, 378);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(218, 45);
            this.labelDescription.TabIndex = 2;
            // 
            // pictureSeasonImage
            // 
            this.pictureSeasonImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureSeasonImage.ErrorImage = global::HomeTheater.Properties.Resources.error_image_generic;
            this.pictureSeasonImage.Image = global::HomeTheater.Properties.Resources.logo;
            this.pictureSeasonImage.InitialImage = global::HomeTheater.Properties.Resources.logo;
            this.pictureSeasonImage.Location = new System.Drawing.Point(0, 0);
            this.pictureSeasonImage.Name = "pictureSeasonImage";
            this.pictureSeasonImage.Size = new System.Drawing.Size(130, 190);
            this.pictureSeasonImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureSeasonImage.TabIndex = 0;
            this.pictureSeasonImage.TabStop = false;
            this.pictureSeasonImage.Click += new System.EventHandler(this.pictureSeasonImage_Click);
            // 
            // listDownload
            // 
            this.listDownload.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnDownloadTitle,
            this.columnDownloadSpeed,
            this.columnDownloadStatus,
            this.columnDownloadProgress});
            this.listDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listDownload.FullRowSelect = true;
            this.listDownload.HideSelection = false;
            this.listDownload.Location = new System.Drawing.Point(0, 25);
            this.listDownload.Name = "listDownload";
            this.listDownload.Size = new System.Drawing.Size(982, 204);
            this.listDownload.TabIndex = 100;
            this.listDownload.UseCompatibleStateImageBehavior = false;
            this.listDownload.View = System.Windows.Forms.View.Details;
            this.listDownload.ClientSizeChanged += new System.EventHandler(this.listDownload_ClientSizeChanged);
            // 
            // columnDownloadTitle
            // 
            this.columnDownloadTitle.Text = "Заголовок";
            this.columnDownloadTitle.Width = 464;
            // 
            // columnDownloadSpeed
            // 
            this.columnDownloadSpeed.Text = "Скорость";
            this.columnDownloadSpeed.Width = 100;
            // 
            // columnDownloadStatus
            // 
            this.columnDownloadStatus.Text = "Статус";
            this.columnDownloadStatus.Width = 100;
            // 
            // columnDownloadProgress
            // 
            this.columnDownloadProgress.Text = "Прогресс";
            this.columnDownloadProgress.Width = 200;
            // 
            // toolDownload
            // 
            this.toolDownload.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolDownload.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripResme,
            this.toolStripPause,
            this.toolStripRemove,
            this.toolStripErase,
            this.toolStripOpenFolder,
            this.toolStripResumeAll,
            this.toolStripPauseAll});
            this.toolDownload.Location = new System.Drawing.Point(0, 0);
            this.toolDownload.Name = "toolDownload";
            this.toolDownload.Size = new System.Drawing.Size(982, 25);
            this.toolDownload.TabIndex = 95;
            // 
            // toolStripResme
            // 
            this.toolStripResme.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripResme.Image = global::HomeTheater.Properties.Resources.control;
            this.toolStripResme.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripResme.Name = "toolStripResme";
            this.toolStripResme.Size = new System.Drawing.Size(23, 22);
            this.toolStripResme.Text = "Возобновить";
            // 
            // toolStripPause
            // 
            this.toolStripPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripPause.Image = global::HomeTheater.Properties.Resources.control_pause;
            this.toolStripPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPause.Name = "toolStripPause";
            this.toolStripPause.Size = new System.Drawing.Size(23, 22);
            this.toolStripPause.Text = "Приостановить";
            // 
            // toolStripRemove
            // 
            this.toolStripRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripRemove.Image = global::HomeTheater.Properties.Resources.cross;
            this.toolStripRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripRemove.Name = "toolStripRemove";
            this.toolStripRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripRemove.Text = "Удалить";
            // 
            // toolStripErase
            // 
            this.toolStripErase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripErase.Image = global::HomeTheater.Properties.Resources.eraser;
            this.toolStripErase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripErase.Name = "toolStripErase";
            this.toolStripErase.Size = new System.Drawing.Size(23, 22);
            this.toolStripErase.Text = "Очистить";
            // 
            // toolStripOpenFolder
            // 
            this.toolStripOpenFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOpenFolder.Image = global::HomeTheater.Properties.Resources.folder_horizontal_open;
            this.toolStripOpenFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOpenFolder.Name = "toolStripOpenFolder";
            this.toolStripOpenFolder.Size = new System.Drawing.Size(23, 22);
            this.toolStripOpenFolder.Text = "Открыть папку";
            // 
            // toolStripResumeAll
            // 
            this.toolStripResumeAll.Image = global::HomeTheater.Properties.Resources.control;
            this.toolStripResumeAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripResumeAll.Name = "toolStripResumeAll";
            this.toolStripResumeAll.Size = new System.Drawing.Size(120, 22);
            this.toolStripResumeAll.Text = "Возобновить Все";
            // 
            // toolStripPauseAll
            // 
            this.toolStripPauseAll.Image = global::HomeTheater.Properties.Resources.control_pause;
            this.toolStripPauseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPauseAll.Name = "toolStripPauseAll";
            this.toolStripPauseAll.Size = new System.Drawing.Size(134, 22);
            this.toolStripPauseAll.Text = "Приостановить Все";
            // 
            // FormList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 668);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.menuMain);
            this.MainMenuStrip = this.menuMain;
            this.Name = "FormList";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Список Сериалов";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormList_FormClosing);
            this.Load += new System.EventHandler(this.FormList_Load);
            this.Shown += new System.EventHandler(this.FormList_Shown);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerMainList.Panel1.ResumeLayout(false);
            this.splitContainerMainList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainList)).EndInit();
            this.splitContainerMainList.ResumeLayout(false);
            this.contextMenuStripSerials.ResumeLayout(false);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.panelTexts.ResumeLayout(false);
            this.panelTexts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSeasonImage)).EndInit();
            this.toolDownload.ResumeLayout(false);
            this.toolDownload.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem обновитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cпискиСериаловToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cтраницыСериаловToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cпискиПлейлиствоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem спискиВидеоToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem всеToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ToolStrip toolDownload;
        private System.Windows.Forms.ToolStripButton toolStripResme;
        private System.Windows.Forms.ToolStripButton toolStripPause;
        private System.Windows.Forms.ToolStripButton toolStripRemove;
        private System.Windows.Forms.ToolStripButton toolStripErase;
        private System.Windows.Forms.ToolStripButton toolStripOpenFolder;
        private System.Windows.Forms.ToolStripButton toolStripResumeAll;
        private System.Windows.Forms.ToolStripButton toolStripPauseAll;
        private System.Windows.Forms.ListView listDownload;
        private System.Windows.Forms.ColumnHeader columnDownloadTitle;
        private System.Windows.Forms.ColumnHeader columnDownloadSpeed;
        private System.Windows.Forms.ColumnHeader columnDownloadStatus;
        private System.Windows.Forms.ColumnHeader columnDownloadProgress;
        private System.Windows.Forms.SplitContainer splitContainerMainList;
        private System.Windows.Forms.PictureBox pictureSeasonImage;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.TextBox textFilter;
        private System.Windows.Forms.Panel panelTexts;
        private System.Windows.Forms.Label labelSiteUpdated;
        private System.Windows.Forms.Label labelCompilation;
        private System.Windows.Forms.Label labelMark;
        private System.Windows.Forms.Label labelIMDB;
        private System.Windows.Forms.Label labelKinoPoisk;
        private System.Windows.Forms.Label labelLSiteUpdated;
        private System.Windows.Forms.Label labelLCompilation;
        private System.Windows.Forms.Label labelLMark;
        private System.Windows.Forms.Label labelLKinoPoisk;
        private System.Windows.Forms.Label labelLIMDB;
        private System.Windows.Forms.Label labelRelease;
        private System.Windows.Forms.Label labelLRelease;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.Label labelLCountry;
        private System.Windows.Forms.Label labelGenre;
        private System.Windows.Forms.Label labelLGenre;
        private System.Windows.Forms.LinkLabel linkLabelTitleFULL;
        private System.Windows.Forms.ListView listSerials;
        private System.Windows.Forms.ColumnHeader columnSerialTitleFull;
        private System.Windows.Forms.ColumnHeader columnSerialTitle;
        private System.Windows.Forms.ColumnHeader columnSerialTitleRU;
        private System.Windows.Forms.ColumnHeader columnSerialTitleEN;
        private System.Windows.Forms.ColumnHeader columnSerialTitleOriginal;
        private System.Windows.Forms.ColumnHeader columnSerialserialUrl;
        private System.Windows.Forms.ColumnHeader columnSerialSeasonID;
        private System.Windows.Forms.ColumnHeader columnSerialSerialID;
        private System.Windows.Forms.ColumnHeader columnSerialSeason;
        private System.Windows.Forms.ColumnHeader columnSerialGenre;
        private System.Windows.Forms.ColumnHeader columnSerialCountry;
        private System.Windows.Forms.ColumnHeader columnSerialRelease;
        private System.Windows.Forms.ColumnHeader columnSerialLimitation;
        private System.Windows.Forms.ColumnHeader columnSerialIMDB;
        private System.Windows.Forms.ColumnHeader columnSerialKinoPoisk;
        private System.Windows.Forms.ColumnHeader columnSerialUserComments;
        private System.Windows.Forms.ColumnHeader columnSerialUserViewsLastDay;
        private System.Windows.Forms.ColumnHeader columnSerialMarkCurrent;
        private System.Windows.Forms.ColumnHeader columnSerialMarkLast;
        private System.Windows.Forms.ColumnHeader columnSerialMark;
        private System.Windows.Forms.ColumnHeader columnSerialCompilation;
        private System.Windows.Forms.ColumnHeader columnSerialSiteUpdated;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заголовокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialTitleFull;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialTitle;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialTitleRU;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialTitleEN;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialTitleOriginal;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialSeasonID;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialSerialID;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialserialUrl;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialGenre;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialCountry;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialRelease;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialIMDB;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialKinoPoisk;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialLimitation;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialUserComments;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialUserViewsLastDay;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialCompilation;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialMarkCurrent;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialMarkLast;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialMark;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSerialSiteUpdated;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSerials;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripUpdate;
        private System.Windows.Forms.ToolStripMenuItem страницуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem плеерToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem видеоToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem отметитьНаПоследнейСерииToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem хочуПосмотретьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ужеПосмотрелToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem вЧерныйСписокToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem остановитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripSyncTimer;
        private System.Windows.Forms.ToolStripMenuItem выполнитьСейчасToolStripMenuItem;
    }
}