using System;
using System.Text.RegularExpressions;

namespace HomeTheater.Serial
{
    class Serial : APIServerParent
    {
        public string serialUrl, Title, TitleRU, TitleEN, Description;
        public string marksCurrent = "-1";
        public string marksLast = "";
        public string Type = "";
        public int SeasonID, SerialID, Season;
        public string serialUrlPath, secureMark;

        public Serial(string url)
        {
            Match matchurl = Regex.Match(url, "^(.*?)#rewind=([0-9-]+)_seriya$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (matchurl.Success)
            {
                this.serialUrl = matchurl.Groups[1].ToString();
                this.marksCurrent = matchurl.Groups[2].ToString();
            }
            else
            {
                this.serialUrl = url;
            }
            matchurl = Regex.Match(this.serialUrl, "serial-([0-9]+)-", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (matchurl.Success)
                this.SeasonID = int.Parse(matchurl.Groups[1].ToString());
            matchurl = Regex.Match(this.serialUrl, "([^/]*?)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (matchurl.Success)
                this.serialUrlPath = matchurl.ToString();
            Init();
        }
        private void Init()
        {
            Console.WriteLine("\tInit: {0:S}", this.serialUrl);
            PreInit();
        }
        private void PreInit()
        {

        }
    }
}
