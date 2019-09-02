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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptions));
            this.panelMain = new System.Windows.Forms.Panel();
            this.groupBoxProxy = new System.Windows.Forms.GroupBox();
            this.textBoxProxyPort = new System.Windows.Forms.TextBox();
            this.checkBoxUseProxy = new System.Windows.Forms.CheckBox();
            this.labelProxyPort = new System.Windows.Forms.Label();
            this.textBoxProxyAddress = new System.Windows.Forms.TextBox();
            this.labelProxyAddress = new System.Windows.Forms.Label();
            this.groupBoxDownloads = new System.Windows.Forms.GroupBox();
            this.labelFileNameDescription = new System.Windows.Forms.Label();
            this.numericUpDownSimultaneousDownloads = new System.Windows.Forms.NumericUpDown();
            this.labelSimultaneousDownloads = new System.Windows.Forms.Label();
            this.textBoxDownloadDir = new System.Windows.Forms.TextBox();
            this.labelFileName = new System.Windows.Forms.Label();
            this.buttonDownloadDir = new System.Windows.Forms.Button();
            this.textBoxNameFiles = new System.Windows.Forms.TextBox();
            this.labelDownloadFolder = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.groupBoxProxy.SuspendLayout();
            this.groupBoxDownloads.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSimultaneousDownloads)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.groupBoxProxy);
            this.panelMain.Controls.Add(this.groupBoxDownloads);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(584, 302);
            this.panelMain.TabIndex = 12;
            // 
            // groupBoxProxy
            // 
            this.groupBoxProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProxy.Controls.Add(this.textBoxProxyPort);
            this.groupBoxProxy.Controls.Add(this.checkBoxUseProxy);
            this.groupBoxProxy.Controls.Add(this.labelProxyPort);
            this.groupBoxProxy.Controls.Add(this.textBoxProxyAddress);
            this.groupBoxProxy.Controls.Add(this.labelProxyAddress);
            this.groupBoxProxy.Location = new System.Drawing.Point(12, 215);
            this.groupBoxProxy.Name = "groupBoxProxy";
            this.groupBoxProxy.Size = new System.Drawing.Size(560, 75);
            this.groupBoxProxy.TabIndex = 12;
            this.groupBoxProxy.TabStop = false;
            this.groupBoxProxy.Text = "Настройка прокси";
            // 
            // textBoxProxyPort
            // 
            this.textBoxProxyPort.Enabled = false;
            this.textBoxProxyPort.Location = new System.Drawing.Point(290, 45);
            this.textBoxProxyPort.Name = "textBoxProxyPort";
            this.textBoxProxyPort.Size = new System.Drawing.Size(91, 20);
            this.textBoxProxyPort.TabIndex = 8;
            // 
            // checkBoxUseProxy
            // 
            this.checkBoxUseProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxUseProxy.Location = new System.Drawing.Point(6, 19);
            this.checkBoxUseProxy.Name = "checkBoxUseProxy";
            this.checkBoxUseProxy.Size = new System.Drawing.Size(548, 20);
            this.checkBoxUseProxy.TabIndex = 6;
            this.checkBoxUseProxy.Text = "Использовать прокси";
            this.checkBoxUseProxy.UseVisualStyleBackColor = true;
            this.checkBoxUseProxy.CheckedChanged += new System.EventHandler(this.CheckBoxUseProxy_CheckedChanged_1);
            // 
            // labelProxyPort
            // 
            this.labelProxyPort.AutoSize = true;
            this.labelProxyPort.Location = new System.Drawing.Point(249, 48);
            this.labelProxyPort.Name = "labelProxyPort";
            this.labelProxyPort.Size = new System.Drawing.Size(35, 13);
            this.labelProxyPort.TabIndex = 4;
            this.labelProxyPort.Text = "Порт:";
            this.labelProxyPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxProxyAddress
            // 
            this.textBoxProxyAddress.Enabled = false;
            this.textBoxProxyAddress.Location = new System.Drawing.Point(53, 45);
            this.textBoxProxyAddress.Name = "textBoxProxyAddress";
            this.textBoxProxyAddress.Size = new System.Drawing.Size(170, 20);
            this.textBoxProxyAddress.TabIndex = 7;
            // 
            // labelProxyAddress
            // 
            this.labelProxyAddress.AutoSize = true;
            this.labelProxyAddress.Location = new System.Drawing.Point(6, 48);
            this.labelProxyAddress.Name = "labelProxyAddress";
            this.labelProxyAddress.Size = new System.Drawing.Size(41, 13);
            this.labelProxyAddress.TabIndex = 3;
            this.labelProxyAddress.Text = "Адрес:";
            this.labelProxyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxDownloads
            // 
            this.groupBoxDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDownloads.Controls.Add(this.labelFileNameDescription);
            this.groupBoxDownloads.Controls.Add(this.numericUpDownSimultaneousDownloads);
            this.groupBoxDownloads.Controls.Add(this.labelSimultaneousDownloads);
            this.groupBoxDownloads.Controls.Add(this.textBoxDownloadDir);
            this.groupBoxDownloads.Controls.Add(this.labelFileName);
            this.groupBoxDownloads.Controls.Add(this.buttonDownloadDir);
            this.groupBoxDownloads.Controls.Add(this.textBoxNameFiles);
            this.groupBoxDownloads.Controls.Add(this.labelDownloadFolder);
            this.groupBoxDownloads.Location = new System.Drawing.Point(12, 12);
            this.groupBoxDownloads.Name = "groupBoxDownloads";
            this.groupBoxDownloads.Size = new System.Drawing.Size(560, 197);
            this.groupBoxDownloads.TabIndex = 11;
            this.groupBoxDownloads.TabStop = false;
            this.groupBoxDownloads.Text = "Загрузки";
            // 
            // labelFileNameDescription
            // 
            this.labelFileNameDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFileNameDescription.Location = new System.Drawing.Point(136, 68);
            this.labelFileNameDescription.Name = "labelFileNameDescription";
            this.labelFileNameDescription.Size = new System.Drawing.Size(418, 96);
            this.labelFileNameDescription.TabIndex = 7;
            this.labelFileNameDescription.Text = resources.GetString("labelFileNameDescription.Text");
            // 
            // numericUpDownSimultaneousDownloads
            // 
            this.numericUpDownSimultaneousDownloads.Location = new System.Drawing.Point(152, 167);
            this.numericUpDownSimultaneousDownloads.Name = "numericUpDownSimultaneousDownloads";
            this.numericUpDownSimultaneousDownloads.Size = new System.Drawing.Size(71, 20);
            this.numericUpDownSimultaneousDownloads.TabIndex = 4;
            this.numericUpDownSimultaneousDownloads.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelSimultaneousDownloads
            // 
            this.labelSimultaneousDownloads.AutoSize = true;
            this.labelSimultaneousDownloads.Location = new System.Drawing.Point(6, 169);
            this.labelSimultaneousDownloads.Name = "labelSimultaneousDownloads";
            this.labelSimultaneousDownloads.Size = new System.Drawing.Size(139, 13);
            this.labelSimultaneousDownloads.TabIndex = 6;
            this.labelSimultaneousDownloads.Text = "Одновременных загрузок";
            // 
            // textBoxDownloadDir
            // 
            this.textBoxDownloadDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDownloadDir.Location = new System.Drawing.Point(136, 19);
            this.textBoxDownloadDir.Name = "textBoxDownloadDir";
            this.textBoxDownloadDir.Size = new System.Drawing.Size(337, 20);
            this.textBoxDownloadDir.TabIndex = 1;
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(6, 48);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(124, 13);
            this.labelFileName.TabIndex = 4;
            this.labelFileName.Text = "Наименование файлов";
            // 
            // buttonDownloadDir
            // 
            this.buttonDownloadDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownloadDir.Location = new System.Drawing.Point(479, 17);
            this.buttonDownloadDir.Name = "buttonDownloadDir";
            this.buttonDownloadDir.Size = new System.Drawing.Size(75, 23);
            this.buttonDownloadDir.TabIndex = 2;
            this.buttonDownloadDir.Text = "Обзор";
            this.buttonDownloadDir.UseVisualStyleBackColor = true;
            this.buttonDownloadDir.Click += new System.EventHandler(this.ButtonDownloadFolder_Click);
            // 
            // textBoxNameFiles
            // 
            this.textBoxNameFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNameFiles.Location = new System.Drawing.Point(136, 45);
            this.textBoxNameFiles.Name = "textBoxNameFiles";
            this.textBoxNameFiles.Size = new System.Drawing.Size(418, 20);
            this.textBoxNameFiles.TabIndex = 3;
            // 
            // labelDownloadFolder
            // 
            this.labelDownloadFolder.AutoSize = true;
            this.labelDownloadFolder.Location = new System.Drawing.Point(6, 22);
            this.labelDownloadFolder.Name = "labelDownloadFolder";
            this.labelDownloadFolder.Size = new System.Drawing.Size(109, 13);
            this.labelDownloadFolder.TabIndex = 0;
            this.labelDownloadFolder.Text = "Папка для загрузки";
            // 
            // FormOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 302);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOptions_FormClosing);
            this.Load += new System.EventHandler(this.FormOptions_Load);
            this.panelMain.ResumeLayout(false);
            this.groupBoxProxy.ResumeLayout(false);
            this.groupBoxProxy.PerformLayout();
            this.groupBoxDownloads.ResumeLayout(false);
            this.groupBoxDownloads.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSimultaneousDownloads)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.GroupBox groupBoxProxy;
        private System.Windows.Forms.TextBox textBoxProxyPort;
        private System.Windows.Forms.CheckBox checkBoxUseProxy;
        private System.Windows.Forms.Label labelProxyPort;
        private System.Windows.Forms.TextBox textBoxProxyAddress;
        private System.Windows.Forms.Label labelProxyAddress;
        private System.Windows.Forms.GroupBox groupBoxDownloads;
        private System.Windows.Forms.Label labelFileNameDescription;
        private System.Windows.Forms.NumericUpDown numericUpDownSimultaneousDownloads;
        private System.Windows.Forms.Label labelSimultaneousDownloads;
        private System.Windows.Forms.TextBox textBoxDownloadDir;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Button buttonDownloadDir;
        private System.Windows.Forms.TextBox textBoxNameFiles;
        private System.Windows.Forms.Label labelDownloadFolder;
    }
}