using HomeTheater.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HomeTheater.Serial
{
    class SerialSeason : APIServerParent
    {
        public List<SerialSeason> Seasons = new List<SerialSeason>();
        public List<SerialSeason> Related = new List<SerialSeason>();
        public Dictionary<int, string> Tags = new Dictionary<int, string>();
        public Dictionary<string, string> SInfo = new Dictionary<string, string>();

        private bool isChild = false;
        private List<string> needSave = new List<string>();

        private int id;
        public int SeasonID
        {
            get => id;
            set => id = value;

        }

        private int serial_id;
        public int SerialID
        {
            get => serial_id;
            set
            {
                if (serial_id != value && value > 0)
                {
                    needSave.Add("serial_id");
                    serial_id = value;
                }
            }
        }

        private int season;
        public int Season
        {
            get => season;
            set
            {
                if (value > 0)
                {
                    if (season != value)
                    {
                        needSave.Add("season");
                        season = value;
                    }
                }
                else
                {
                    season = 0;
                }
            }
        }
        private string url;
        public string serialUrl
        {
            get => url;
            set
            {
                if (url != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("url");
                    url = value;
                }
            }
        }
        private string title;
        public string Title
        {
            get => title;
            set
            {
                if (title != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("title");
                    title = value;
                }
            }
        }
        private string title_ru;
        public string TitleRU
        {
            get => title_ru;
            set
            {
                if (title_ru != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("title_ru");
                    title_ru = value;
                }
            }
        }
        private string title_en;
        public string TitleEN
        {
            get => title_en;
            set
            {
                if (title_en != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("title_en");
                    title_en = value;
                }
            }
        }
        private string description;
        public string Description
        {
            get => description;
            set
            {
                if (description != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("description");
                    description = value;
                }
            }
        }
        private string marks_current;
        public string marksCurrent
        {
            get
            {
                if (string.IsNullOrWhiteSpace(marks_current))
                    return "-1";
                return marks_current;
            }
            set
            {
                if (marks_current != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("marks_current");
                    marks_current = value;
                }
            }
        }
        private string type_old;
        public string TypeOLD
        {
            get => type_old;
        }
        private string type;
        public string Type
        {
            get
            {
                if (string.IsNullOrWhiteSpace(type))
                    return "None";
                return type;
            }
            set
            {
                if (type != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("type");
                    type = value;
                }
            }
        }
        private string marks_last;
        public string marksLast
        {
            get
            {
                if (string.IsNullOrWhiteSpace(marks_last))
                    return string.Empty;
                return marks_last;
            }
            set
            {
                if (marks_last != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("marks_last");
                    marks_last = value;
                }
            }
        }
        private string secure_mark;
        public string secureMark
        {
            get
            {
                if (string.IsNullOrWhiteSpace(secure_mark))
                    return string.Empty;
                return secure_mark;
            }
            set
            {
                if (secure_mark != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("secure_mark");
                    secure_mark = value;
                }
            }
        }
        private DateTime site_updated;
        private DateTime updated_date;
        public DateTime SiteUpdated
        {
            get => site_updated;
            set
            {
                if (site_updated != value && new DateTime() != value)
                {
                    needSave.Add("site_updated");
                    site_updated = value;
                }
            }
        }
        private DateTime create_date;
        public DateTime CachedDate
        {
            get => create_date;
            set => create_date = value;
        }

        private int _timeout = 0;
        private int timeout
        {
            get
            {
                if (0 <= _timeout)
                {
                    string timeout = DB.Instance.getOption(String.Concat("cacheTimeSerial_" + this.Type));
                    _timeout = !string.IsNullOrEmpty(timeout) ? int.Parse(timeout) : 24 * 60 * 60;
                }

                return _timeout;
            }
        }

        private string this[string index]
        {
            get
            {
                string result = "";

                switch (index)
                {
                    case "serial_id": result = serial_id.ToString(); break;
                    case "season": result = season.ToString(); break;
                    case "url": result = url.ToString(); break;
                    case "title": result = title.ToString(); break;
                    case "title_ru": result = title_ru.ToString(); break;
                    case "title_en": result = title_en.ToString(); break;
                    case "description": result = description.ToString(); break;
                    case "marks_current": result = marks_current.ToString(); break;
                    case "marks_last": result = marks_last.ToString(); break;
                    case "secure_mark": result = secure_mark.ToString(); break;
                    case "type": result = secure_mark.ToString(); break;
                    case "site_updated": result = site_updated.ToString(DB.DATE_FORMAT); break;
                }

                return result;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    value = value.ToString().Trim(); switch (index)
                    {
                        case "url": url = value; break;
                        case "title": title = value; break;
                        case "title_ru": title_ru = value; break;
                        case "title_en": title_en = value; break;
                        case "description": description = value; break;
                        case "marks_current": marks_current = value; break;
                        case "marks_last": marks_last = value; break;
                        case "secure_mark": secure_mark = value; break;
                        case "type": type_old = value; break;
                        case "serial_id": serial_id = IntVal(value); break;
                        case "season": season = IntVal(value); break;
                        case "site_updated": site_updated = DateVal(value, DB.DATE_FORMAT); break;
                        case "create_date": create_date = DateVal(value, DB.TIME_FORMAT); break;
                        case "updated_date": updated_date = DateVal(value, DB.TIME_FORMAT); break;
                    }
                }

            }
        }


        public string ImageSmall
        {
            get
            {
                return SERVER_CDN_URL + "small/" + SeasonID + ".jpg";
            }
        }
        public string Image
        {
            get
            {
                return SERVER_CDN_URL + SeasonID + ".jpg";
            }
        }
        public string ImageLarge
        {
            get
            {
                return SERVER_CDN_URL + "large/" + SeasonID + ".jpg";
            }
        }

        public SerialSeason(string url)
        {
            this.SeasonID = IntVal(Match(url, "serial-([0-9]+)-", REGEX_IC, 1));

            Init();

            Match matchurl = Regex.Match(url, "^(.*?)#rewind=(.*?)_seriya$", REGEX_IC);
            if (matchurl.Success)
            {
                this.serialUrl = matchurl.Groups[1].ToString();
                this.marksCurrent = matchurl.Groups[2].ToString();
            }
            else
            {
                this.serialUrl = url;
            }
        }
        public SerialSeason(int id)
        {
            this.SeasonID = id;

            Init();
        }
        public void setIsChild(bool child = true)
        {
            this.isChild = child;
        }
        private void Init()
        {
            Console.WriteLine("\tInit: {0:S}", !string.IsNullOrEmpty(this.serialUrl) ? this.serialUrl : this.SeasonID.ToString());
            Load();
        }
        public SerialSeason Load()
        {
            Dictionary<string, string> data = DB.Instance.getSeason(this.id);
            if (0 < data.Count)
            {
                foreach (KeyValuePair<string, string> item in data)
                    this[item.Key] = item.Value;
                Console.WriteLine("\t\tLoaded: {0:S}", !string.IsNullOrEmpty(this.serialUrl) ? this.serialUrl : this.SeasonID.ToString());
            }

            return this;
        }
        public SerialSeason Save()
        {
            if (0 >= id || 0 >= needSave.Count)
                return this;
            Dictionary<string, string> data = new Dictionary<string, string>();
            for (var i = 0; i < needSave.Count; i++)
            {
                string field = needSave[i];
                string value = this[field];
                if (!string.IsNullOrWhiteSpace(value) && !data.ContainsKey(field))
                    data.Add(field, value);
            }
            needSave.Clear();
            if (DB.Instance.setSeason(this.id, data))
                Console.WriteLine("\tSaved:  {0:S}", !string.IsNullOrEmpty(this.serialUrl) ? this.serialUrl : this.SeasonID.ToString());

            return this;
        }
        public void parseSidebar(string html)
        {
            string text_serial = Match(html, "<div class=\"rside-ss\">(.*?)</div>", REGEX_ICS, 1);
            if (!string.IsNullOrEmpty(text_serial))
            {
                List<string> matchseason = Matches(text_serial, "^(.*?)<span>([^<>]*)</span>$", REGEX_IC);
                if (3 <= matchseason.Count)
                {
                    Season = IntVal(matchseason[1]);
                    marksLast = Regex.Replace(matchseason[2], " (\\([^\\(\\)]+\\)|серия)", "").Replace("Только трейлер", "");
                }
            }

            TitleRU = Match(html, "<div class=\"rside-t\">(.*?)</div>", REGEX_ICS, 1);
            TitleEN = Match(html, "<div class=\"rside-t_en\">(.*?)</div>", REGEX_ICS, 1);
            Save();
        }

        public void parsePause(string html)
        {
            Season = IntVal(Match(html, "<div class=\"pgs-marks-seas\">(.*?)</div>", REGEX_ICS, 1));
            TitleRU = Match(html, "<div class=\"pgs-marks-name\">(.*?)</div>", REGEX_ICS, 1);
            marksLast = Match(html, "<div class=\"pgs-marks-current\">[^<>]*<strong>(.*?)</strong>[^<>]*</div>", REGEX_ICS, 1).Replace("Только трейлер", "");
            SiteUpdated = DateVal(Match(html, "<div class=\"pgs-marks-mess\">(.*?)</div>", REGEX_ICS, 1), "dd.MM.yyyy");
            Save();
        }
        private void parsePage(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                Console.WriteLine("\t\t[!] Остановка парсинга!!! Страница небыла полученна!");
                return;
            }
            SerialID = IntVal(Match(html, "data-id-serial=\"([0-9]+)\"", REGEX_ICS, 1));
            SeasonID = IntVal(Match(html, "data-id-season=\"([0-9]+)\"", REGEX_ICS, 1));
            Description = Regex.Replace(Regex.Replace(WebUtility.HtmlDecode(Match(html, "<p itemprop=\"description\">(.*?)</p>", REGEX_ICS, 1)), "<br[^<>]*>", "\r\n"), "(\r\n){2,}>", "\r\n");
            secureMark = _parseData4Play(Match(html, "data4play = ({.*?})", REGEX_ICS, 1));

            _parseTitle(Match(html, "<h1 class=\"pgs-sinfo-title\"[^<>]*>(.*?)</h1>", REGEX_ICS, 1), this);
            _parseTags(Match(html, "<div data-tag=\"wrap\">(.*?)</div>", REGEX_ICS, 1));
            _parseMetaInfo(html);

            if (!isChild)
            {
                _parseSeasons(Match(html, "<div class=\"pgs-seaslist\">(.*?)</div>", REGEX_ICS, 1));
                _parseRelated(html);
            }
            Save();
        }

        public void syncPage(bool forsed = false)
        {
            if ((timeout < DateTime.UtcNow.Subtract(create_date).TotalSeconds || timeout < DateTime.UtcNow.Subtract(updated_date).TotalSeconds) || forsed)
                parsePage(downloadPage(forsed));

        }
        private string _parseData4Play(string html)
        {
            html = Regex.Replace(html, "[\r\n\t ]+", string.Empty);
            if (string.IsNullOrEmpty(html))
            {
                Console.WriteLine("\t\t[?] Данные для плеера не найдены!!! Сериал заблокирован?");
                return "";
            }
            html = Regex.Replace(html, "'", "\"");
            data4play Data4Play = SimpleJson.SimpleJson.DeserializeObject<data4play>(html);
            return Data4Play.secureMark;
        }
        private void _parseTitle(string html, SerialSeason _o)
        {
            html = html.Trim();
            if (string.IsNullOrEmpty(html))
                return;

            html = Regex.Replace(WebUtility.HtmlDecode(html), "^>>> ", string.Empty);
            html = Regex.Replace(html, "<[^<>]+>[^<>]+</[^<>]+>", string.Empty).Trim();
            html = Regex.Replace(html, "(^сериал | онлайн$)", string.Empty, REGEX_IC);
            _o.Season = IntVal(Match(html, "([0-9]+) сезон$", REGEX_ICS, 1));
            html = Regex.Replace(html, " [0-9]+ сезон$", string.Empty, REGEX_IC).Trim();
            _o.Title = html;
            if (!string.IsNullOrEmpty(_o.TitleRU))
            {
                html = Regex.Replace(_o.Title.Replace(_o.TitleRU, string.Empty), "(^/|/$)", string.Empty);
                if (html != _o.TitleRU)
                {
                    _o.TitleEN = html;
                }
            }
            else
            {
                String[] sp_html = _o.Title.Split('/');
                if (2 == sp_html.Length)
                {
                    _o.TitleRU = sp_html[0].ToString().Trim();
                    _o.TitleEN = sp_html[1].ToString().Trim();
                }
                else if (2 < sp_html.Length)
                {
                    Console.WriteLine("\t\t[?]Не возможно разделить название сериала: {0:S}", _o.Title);
                }
            }
        }

        private void _parseMetaInfo(string html)
        {
            if (string.IsNullOrEmpty(html))
                return;
            foreach (Match matchInfo in Regex.Matches(html, "<div class=\"pgs-sinfo_list[^\"]*\">(.*?)</div>", REGEX_ICS))
            {
                string info = matchInfo.Groups[1].ToString().Trim();
                info = Regex.Replace(info, "</span>", "||", REGEX_ICS);
                info = Regex.Replace(info, "(<[^<>]*>|\n|\r|\t)", " ", REGEX_ICS);
                info = Regex.Replace(info, "[ ]{2,}", " ", REGEX_ICS);
                foreach (Match _matchInfo in Regex.Matches(info, "([^:\\|]+?):([^:\\|]+?)\\|\\|", REGEX_ICS))
                {
                    string name = _matchInfo.Groups[1].ToString().Trim();
                    string value = _matchInfo.Groups[2].ToString().Trim();
                    if (!this.SInfo.ContainsKey(name))
                        this.SInfo.Add(name, value);
                }
            }
        }
        private void _parseRelated(string html)
        {
            if (string.IsNullOrEmpty(html))
                return;
            foreach (Match matchInfo in Regex.Matches(html, "<a href=\"/([^\"]+)\" data-id=\"([0-9]+)\" class=\"pst\">(.*?)</a>", REGEX_ICS))
            {
                string url = this.SERVER_URL + "/" + matchInfo.Groups[1].ToString().Trim();
                if (this.serialUrl == url)
                {
                    continue;
                }
                SerialSeason season = new SerialSeason(url);
                season.setIsChild();
                season.TitleRU = Match(matchInfo.Groups[3].ToString(), "<div>(.+?)</div>", REGEX_ICS, 1);
                season.Save();
                this.Related.Add(season);
            }
        }

        private void _parseTags(string html)
        {
            if (string.IsNullOrEmpty(html))
                return;
            foreach (Match matchTag in Regex.Matches(html, "<a[^<>]*>(.*?)</a>", REGEX_ICS))
            {
                string _html = matchTag.Groups[1].ToString();
                string __html = Match(html, "data-tagid=\"([0-9]+)\"", REGEX_ICS, 1);
                _html = Regex.Replace(_html, "<[^<>]+>[^<>]+</[^<>]+>", string.Empty).Trim();
                if (!string.IsNullOrEmpty(__html) && !string.IsNullOrEmpty(_html))
                {
                    int id = int.Parse(__html);
                    if (this.Tags.ContainsKey(id))
                    {
                        this.Tags.Add(id, _html);
                    }
                }
            }
        }
        private void _parseSeasons(string html)
        {
            if (string.IsNullOrEmpty(html))
                return;
            foreach (Match matchSeason in Regex.Matches(html, "<a[^<>]*href=\"([^<>\"]*)\"[^<>]*>(.*?)</a>", REGEX_ICS))
            {
                string url = this.SERVER_URL + matchSeason.Groups[1].ToString();
                if (this.serialUrl == url)
                {
                    continue;
                }
                SerialSeason season = new SerialSeason(url);
                season.setIsChild();
                _parseTitle(matchSeason.Groups[2].ToString().Trim(), season);
                season.Title = this.Title;
                season.TitleRU = this.TitleRU;
                season.TitleEN = this.TitleEN;
                season.SerialID = this.SerialID;
                season.Save();
                this.Seasons.Add(season);
            }
        }

        public string downloadPage(bool forsed = false)
        {
            string content = "";
            bool isActual = false;
            DBCache cacheItem = DB.Instance.getCache(serialUrl, this.timeout);
            if (null != cacheItem)
            {
                content = cacheItem.content;
                isActual = cacheItem.isActual;
                this.CachedDate = cacheItem.date;
            }

            if (!isActual || forsed)
            {
                content = Download(serialUrl);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    DB.Instance.setCache(serialUrl, content);
                    this.CachedDate = DateTime.UtcNow;
                }
                else if (null != cacheItem)
                {
                    content = cacheItem.content;
                    this.CachedDate = cacheItem.date;
                }
            }

            return content;
        }

        public void syncPlayer(bool forsed = false)
        {
            // UNDONE: Скачивать и парсить страницу плеера
        }
        public void syncPlaylists(bool forsed = false)
        {
            // UNDONE: Скачивать и парсить страницы плейлистов
        }



        public ListViewItem ToListViewItem()
        {
            var a = new ListViewItem(this.SeasonID.ToString());
            // UNDONE: Сделать конвертацию в ListViewItem

            return a;
        }
    }
}
