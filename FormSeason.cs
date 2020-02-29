using System;
using System.Diagnostics;
using System.Windows.Forms;
using HomeTheater.API.Serial;

namespace HomeTheater
{
    public partial class FormSeason : Form
    {
        private const string PREFIX_TITLE = "Сезон: ";

        public Season Season;

        public FormSeason(int ID)
        {
            InitializeComponent();
            Season = new Season(ID);
            UpdatePage();
        }

        public FormSeason(Season Season)
        {
            InitializeComponent();
            this.Season = Season;
            UpdatePage();
        }

        private void UpdatePage()
        {
            pictureSeasonImage.LoadAsync(Season.Image);
            Text = PREFIX_TITLE + Season.TitleFull;
            linkTitle.Text = Season.TitleFull;
            linkTitle.Tag = Season.URL;
            labelDescription.Text = "    " + Season.Description.Replace("\r\n", "\r\n    ");
            labelOriginal.Text = Season.TitleOriginal;
            labelLimitation.Text = Season.Limitation;
            labelGenre.Text = Season.Genre;
            labelCountry.Text = Season.Country;
            labelRelease.Text = Season.Release;
            labelIMDB.Text = Season.IMDB.ToString();
            labelKinoPoisk.Text = Season.KinoPoisk.ToString();
            labelSiteUpdated.Text =
                new DateTime() != Season.SiteUpdated ? Season.SiteUpdated.ToString("dd.MM.yyyy") : "";
        }

        internal void UpdateSeason()
        {
            Season.Load();
            UpdatePage();
        }

        internal void UpdateSeason(Season season)
        {
            Season = season;
            UpdatePage();
        }

        private void pictureSeasonImage_Click(object sender, EventArgs e)
        {
            Process.Start(Season.ImageLarge);
        }

        private void linkTitle_Click(object sender, EventArgs e)
        {
            Process.Start(Season.URL);
        }
    }
}