namespace HomeTheater
{
    partial class FormOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.groupDownloadSerial = new System.Windows.Forms.GroupBox();
            this.labelTagOriginalName = new System.Windows.Forms.Label();
            this.labelTagTranslate = new System.Windows.Forms.Label();
            this.labelTagEpisode = new System.Windows.Forms.Label();
            this.labelTagSeason = new System.Windows.Forms.Label();
            this.labelTagSerialName = new System.Windows.Forms.Label();
            this.labelTagCollection = new System.Windows.Forms.Label();
            this.labelTagType = new System.Windows.Forms.Label();
            this.numericSimultaneousDownloads = new System.Windows.Forms.NumericUpDown();
            this.labelSimultaneousDownloads = new System.Windows.Forms.Label();
            this.buttonDownloadDir = new System.Windows.Forms.Button();
            this.textNameFiles = new System.Windows.Forms.TextBox();
            this.textDownloadDir = new System.Windows.Forms.TextBox();
            this.labelDownloadDir = new System.Windows.Forms.Label();
            this.labelNameFiles = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.groupProxy = new System.Windows.Forms.GroupBox();
            this.numericProxyPort = new System.Windows.Forms.NumericUpDown();
            this.labelProxyPort = new System.Windows.Forms.Label();
            this.labelProxyAddress = new System.Windows.Forms.Label();
            this.textProxyAddress = new System.Windows.Forms.TextBox();
            this.checkUseProxy = new System.Windows.Forms.CheckBox();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.labelTagFormat = new System.Windows.Forms.Label();
            this.groupDownloadSerial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSimultaneousDownloads)).BeginInit();
            this.panelMain.SuspendLayout();
            this.groupProxy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericProxyPort)).BeginInit();
            this.SuspendLayout();
            // 
            // groupDownloadSerial
            // 
            this.groupDownloadSerial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupDownloadSerial.Controls.Add(this.labelTagFormat);
            this.groupDownloadSerial.Controls.Add(this.labelTagOriginalName);
            this.groupDownloadSerial.Controls.Add(this.labelTagTranslate);
            this.groupDownloadSerial.Controls.Add(this.labelTagEpisode);
            this.groupDownloadSerial.Controls.Add(this.labelTagSeason);
            this.groupDownloadSerial.Controls.Add(this.labelTagSerialName);
            this.groupDownloadSerial.Controls.Add(this.labelTagCollection);
            this.groupDownloadSerial.Controls.Add(this.labelTagType);
            this.groupDownloadSerial.Controls.Add(this.numericSimultaneousDownloads);
            this.groupDownloadSerial.Controls.Add(this.labelSimultaneousDownloads);
            this.groupDownloadSerial.Controls.Add(this.buttonDownloadDir);
            this.groupDownloadSerial.Controls.Add(this.textNameFiles);
            this.groupDownloadSerial.Controls.Add(this.textDownloadDir);
            this.groupDownloadSerial.Controls.Add(this.labelDownloadDir);
            this.groupDownloadSerial.Controls.Add(this.labelNameFiles);
            this.groupDownloadSerial.Location = new System.Drawing.Point(3, 3);
            this.groupDownloadSerial.MinimumSize = new System.Drawing.Size(535, 150);
            this.groupDownloadSerial.Name = "groupDownloadSerial";
            this.groupDownloadSerial.Size = new System.Drawing.Size(541, 199);
            this.groupDownloadSerial.TabIndex = 0;
            this.groupDownloadSerial.TabStop = false;
            this.groupDownloadSerial.Text = "Загрузка Серий";
            // 
            // labelTagOriginalName
            // 
            this.labelTagOriginalName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTagOriginalName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelTagOriginalName.Location = new System.Drawing.Point(285, 161);
            this.labelTagOriginalName.Name = "labelTagOriginalName";
            this.labelTagOriginalName.Size = new System.Drawing.Size(250, 13);
            this.labelTagOriginalName.TabIndex = 12;
            this.labelTagOriginalName.Text = "{OriginalName} - Оригинальное название файла";
            this.labelTagOriginalName.Click += new System.EventHandler(this.labelTagOriginalName_Click);
            // 
            // labelTagTranslate
            // 
            this.labelTagTranslate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTagTranslate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelTagTranslate.Location = new System.Drawing.Point(285, 146);
            this.labelTagTranslate.Name = "labelTagTranslate";
            this.labelTagTranslate.Size = new System.Drawing.Size(250, 13);
            this.labelTagTranslate.TabIndex = 11;
            this.labelTagTranslate.Text = "{Translate} - Наименование Озвучки";
            this.labelTagTranslate.Click += new System.EventHandler(this.labelTagTranslate_Click);
            // 
            // labelTagEpisode
            // 
            this.labelTagEpisode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTagEpisode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelTagEpisode.Location = new System.Drawing.Point(285, 131);
            this.labelTagEpisode.Name = "labelTagEpisode";
            this.labelTagEpisode.Size = new System.Drawing.Size(250, 13);
            this.labelTagEpisode.TabIndex = 10;
            this.labelTagEpisode.Text = "{Episode} - Номер Эпизода";
            this.labelTagEpisode.Click += new System.EventHandler(this.labelTagEpisode_Click);
            // 
            // labelTagSeason
            // 
            this.labelTagSeason.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTagSeason.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelTagSeason.Location = new System.Drawing.Point(285, 116);
            this.labelTagSeason.Name = "labelTagSeason";
            this.labelTagSeason.Size = new System.Drawing.Size(250, 13);
            this.labelTagSeason.TabIndex = 9;
            this.labelTagSeason.Text = "{Season} - Номер Сезона";
            this.labelTagSeason.Click += new System.EventHandler(this.labelTagSeason_Click);
            // 
            // labelTagSerialName
            // 
            this.labelTagSerialName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTagSerialName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelTagSerialName.Location = new System.Drawing.Point(285, 101);
            this.labelTagSerialName.Name = "labelTagSerialName";
            this.labelTagSerialName.Size = new System.Drawing.Size(250, 13);
            this.labelTagSerialName.TabIndex = 8;
            this.labelTagSerialName.Text = "{SerialName} - Название Сериала";
            this.labelTagSerialName.Click += new System.EventHandler(this.labelTagSerialName_Click);
            // 
            // labelTagCollection
            // 
            this.labelTagCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTagCollection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelTagCollection.Location = new System.Drawing.Point(285, 86);
            this.labelTagCollection.Name = "labelTagCollection";
            this.labelTagCollection.Size = new System.Drawing.Size(250, 13);
            this.labelTagCollection.TabIndex = 7;
            this.labelTagCollection.Text = "{Collection} - Наименование подборки сериала";
            this.labelTagCollection.Click += new System.EventHandler(this.labelTagCollection_Click);
            // 
            // labelTagType
            // 
            this.labelTagType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTagType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelTagType.Location = new System.Drawing.Point(285, 71);
            this.labelTagType.Name = "labelTagType";
            this.labelTagType.Size = new System.Drawing.Size(250, 13);
            this.labelTagType.TabIndex = 6;
            this.labelTagType.Text = "{Type} - Наименование списка сериала";
            this.labelTagType.Click += new System.EventHandler(this.labelTagType_Click);
            // 
            // numericSimultaneousDownloads
            // 
            this.numericSimultaneousDownloads.Location = new System.Drawing.Point(151, 71);
            this.numericSimultaneousDownloads.Name = "numericSimultaneousDownloads";
            this.numericSimultaneousDownloads.Size = new System.Drawing.Size(66, 20);
            this.numericSimultaneousDownloads.TabIndex = 4;
            this.numericSimultaneousDownloads.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelSimultaneousDownloads
            // 
            this.labelSimultaneousDownloads.AutoSize = true;
            this.labelSimultaneousDownloads.Location = new System.Drawing.Point(6, 73);
            this.labelSimultaneousDownloads.Name = "labelSimultaneousDownloads";
            this.labelSimultaneousDownloads.Size = new System.Drawing.Size(139, 13);
            this.labelSimultaneousDownloads.TabIndex = 5;
            this.labelSimultaneousDownloads.Text = "Одновременных загрузок";
            // 
            // buttonDownloadDir
            // 
            this.buttonDownloadDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownloadDir.Location = new System.Drawing.Point(460, 18);
            this.buttonDownloadDir.Name = "buttonDownloadDir";
            this.buttonDownloadDir.Size = new System.Drawing.Size(75, 22);
            this.buttonDownloadDir.TabIndex = 2;
            this.buttonDownloadDir.Text = "Обзор";
            this.buttonDownloadDir.UseVisualStyleBackColor = true;
            this.buttonDownloadDir.Click += new System.EventHandler(this.buttonDownloadDir_Click);
            // 
            // textNameFiles
            // 
            this.textNameFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textNameFiles.Location = new System.Drawing.Point(151, 45);
            this.textNameFiles.Name = "textNameFiles";
            this.textNameFiles.Size = new System.Drawing.Size(384, 20);
            this.textNameFiles.TabIndex = 3;
            // 
            // textDownloadDir
            // 
            this.textDownloadDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDownloadDir.Location = new System.Drawing.Point(151, 19);
            this.textDownloadDir.Name = "textDownloadDir";
            this.textDownloadDir.Size = new System.Drawing.Size(303, 20);
            this.textDownloadDir.TabIndex = 1;
            // 
            // labelDownloadDir
            // 
            this.labelDownloadDir.AutoSize = true;
            this.labelDownloadDir.Location = new System.Drawing.Point(6, 23);
            this.labelDownloadDir.Name = "labelDownloadDir";
            this.labelDownloadDir.Size = new System.Drawing.Size(124, 13);
            this.labelDownloadDir.TabIndex = 1;
            this.labelDownloadDir.Text = "Наименование файлов";
            // 
            // labelNameFiles
            // 
            this.labelNameFiles.AutoSize = true;
            this.labelNameFiles.Location = new System.Drawing.Point(6, 48);
            this.labelNameFiles.Name = "labelNameFiles";
            this.labelNameFiles.Size = new System.Drawing.Size(109, 13);
            this.labelNameFiles.TabIndex = 0;
            this.labelNameFiles.Text = "Папка для загрузки";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.groupProxy);
            this.panelMain.Controls.Add(this.groupDownloadSerial);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(6, 6);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(547, 436);
            this.panelMain.TabIndex = 2;
            // 
            // groupProxy
            // 
            this.groupProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupProxy.Controls.Add(this.numericProxyPort);
            this.groupProxy.Controls.Add(this.labelProxyPort);
            this.groupProxy.Controls.Add(this.labelProxyAddress);
            this.groupProxy.Controls.Add(this.textProxyAddress);
            this.groupProxy.Controls.Add(this.checkUseProxy);
            this.groupProxy.Location = new System.Drawing.Point(3, 208);
            this.groupProxy.Name = "groupProxy";
            this.groupProxy.Size = new System.Drawing.Size(541, 78);
            this.groupProxy.TabIndex = 1;
            this.groupProxy.TabStop = false;
            this.groupProxy.Text = "Настройка прокси";
            // 
            // numericProxyPort
            // 
            this.numericProxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericProxyPort.Location = new System.Drawing.Point(415, 50);
            this.numericProxyPort.Name = "numericProxyPort";
            this.numericProxyPort.Size = new System.Drawing.Size(120, 20);
            this.numericProxyPort.TabIndex = 2;
            this.numericProxyPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericProxyPort.ThousandsSeparator = true;
            // 
            // labelProxyPort
            // 
            this.labelProxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProxyPort.AutoSize = true;
            this.labelProxyPort.Location = new System.Drawing.Point(374, 52);
            this.labelProxyPort.Name = "labelProxyPort";
            this.labelProxyPort.Size = new System.Drawing.Size(35, 13);
            this.labelProxyPort.TabIndex = 3;
            this.labelProxyPort.Text = "Порт:";
            // 
            // labelProxyAddress
            // 
            this.labelProxyAddress.AutoSize = true;
            this.labelProxyAddress.Location = new System.Drawing.Point(6, 52);
            this.labelProxyAddress.Name = "labelProxyAddress";
            this.labelProxyAddress.Size = new System.Drawing.Size(41, 13);
            this.labelProxyAddress.TabIndex = 2;
            this.labelProxyAddress.Text = "Адрес:";
            // 
            // textProxyAddress
            // 
            this.textProxyAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textProxyAddress.Location = new System.Drawing.Point(53, 49);
            this.textProxyAddress.Name = "textProxyAddress";
            this.textProxyAddress.Size = new System.Drawing.Size(315, 20);
            this.textProxyAddress.TabIndex = 1;
            // 
            // checkUseProxy
            // 
            this.checkUseProxy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkUseProxy.Location = new System.Drawing.Point(9, 19);
            this.checkUseProxy.Name = "checkUseProxy";
            this.checkUseProxy.Size = new System.Drawing.Size(520, 24);
            this.checkUseProxy.TabIndex = 0;
            this.checkUseProxy.Text = "Использовать прокси";
            this.checkUseProxy.UseVisualStyleBackColor = true;
            this.checkUseProxy.CheckedChanged += new System.EventHandler(this.checkUseProxy_CheckedChanged);
            // 
            // menuMain
            // 
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(559, 24);
            this.menuMain.TabIndex = 7;
            this.menuMain.Text = "menuMain";
            this.menuMain.Visible = false;
            // 
            // labelTagFormat
            // 
            this.labelTagFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTagFormat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelTagFormat.Location = new System.Drawing.Point(285, 176);
            this.labelTagFormat.Name = "labelTagFormat";
            this.labelTagFormat.Size = new System.Drawing.Size(250, 13);
            this.labelTagFormat.TabIndex = 13;
            this.labelTagFormat.Text = "{Format} - Формат файла";
            this.labelTagFormat.Click += new System.EventHandler(this.labelTagFormat_Click);
            // 
            // FormOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 448);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.menuMain);
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(575, 39);
            this.Name = "FormOptions";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "Настройки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOptions_FormClosing);
            this.Shown += new System.EventHandler(this.FormOptions_Shown);
            this.groupDownloadSerial.ResumeLayout(false);
            this.groupDownloadSerial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSimultaneousDownloads)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.groupProxy.ResumeLayout(false);
            this.groupProxy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericProxyPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupDownloadSerial;
        private System.Windows.Forms.Label labelDownloadDir;
        private System.Windows.Forms.Label labelNameFiles;
        private System.Windows.Forms.Button buttonDownloadDir;
        private System.Windows.Forms.TextBox textNameFiles;
        private System.Windows.Forms.TextBox textDownloadDir;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.NumericUpDown numericSimultaneousDownloads;
        private System.Windows.Forms.Label labelSimultaneousDownloads;
        private System.Windows.Forms.GroupBox groupProxy;
        private System.Windows.Forms.NumericUpDown numericProxyPort;
        private System.Windows.Forms.Label labelProxyPort;
        private System.Windows.Forms.Label labelProxyAddress;
        private System.Windows.Forms.TextBox textProxyAddress;
        private System.Windows.Forms.CheckBox checkUseProxy;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.Label labelTagOriginalName;
        private System.Windows.Forms.Label labelTagTranslate;
        private System.Windows.Forms.Label labelTagEpisode;
        private System.Windows.Forms.Label labelTagSeason;
        private System.Windows.Forms.Label labelTagSerialName;
        private System.Windows.Forms.Label labelTagCollection;
        private System.Windows.Forms.Label labelTagType;
        private System.Windows.Forms.Label labelTagFormat;
    }
}