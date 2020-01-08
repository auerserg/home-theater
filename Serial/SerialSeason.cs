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
        public Dictionary<int, SerialSeason> Seasons = new Dictionary<int, SerialSeason>();
        public List<SerialSeason> Related = new List<SerialSeason>();
        public Dictionary<int, string> Tags = new Dictionary<int, string>();
        public Dictionary<string, string> MetaInfo = new Dictionary<string, string>();

        private bool isChild = false;
        private bool forsed_update_page = false;
        private bool forsed_update_player = false;
        private bool forsed_update_playlist = false;
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

        private string title_original;
        public string TitleOriginal
        {
            get => title_original;
            set
            {
                if (title_original != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("title_original");
                    title_original = value;
                }
            }
        }
        private string compilation;
        public string Compilation
        {
            get => compilation;
            set
            {
                if (compilation != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("compilation");
                    compilation = value;
                }
            }
        }

        private string genre;
        public string Genre
        {
            get => genre;
            set
            {
                if (genre != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("genre");
                    genre = value;
                }
            }
        }

        private string country;
        public string Country
        {
            get => country;
            set
            {
                if (country != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("country");
                    country = value;
                }
            }
        }
        private string release;
        public string Release
        {
            get => release;
            set
            {
                if (release != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("release");
                    release = value;
                }
            }
        }
        private string imdb;
        public string IMDB
        {
            get => imdb;
            set
            {
                if (imdb != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("imdb");
                    imdb = value;
                }
            }
        }
        private string kinopoisk;
        public string KinoPoisk
        {
            get => kinopoisk;
            set
            {
                if (kinopoisk != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("kinopoisk");
                    kinopoisk = value;
                }
            }
        }
        private string limitation;
        public string Limitation
        {
            get => limitation;
            set
            {
                if (limitation != value && !string.IsNullOrWhiteSpace(value))
                {
                    needSave.Add("limitation");
                    limitation = value;
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
                    if (string.IsNullOrEmpty(timeout))
                    {
                        timeout = DB.Instance.getOption(String.Concat("cacheTimeSerial_none"));
                    }
                    if (string.IsNullOrEmpty(timeout))
                    {
                        timeout = (24 * 60 * 60).ToString();
                    }
                    _timeout = IntVal(timeout);
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
                    case "compilation": result = compilation.ToString(); break;


                    case "title_original": result = title_original.ToString(); break;
                    case "genre": result = genre.ToString(); break;
                    case "country": result = country.ToString(); break;
                    case "release": result = release.ToString(); break;
                    case "limitation": result = limitation.ToString(); break;
                    case "imdb": result = imdb.ToString(); break;
                    case "kinopoisk": result = kinopoisk.ToString(); break;


                    case "type": result = type.ToString(); break;
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
                        case "compilation": compilation = value; break;

                        case "title_original": title_original = value; break;
                        case "genre": genre = value; break;
                        case "country": country = value; break;
                        case "release": release = value; break;
                        case "limitation": limitation = value; break;
                        case "imdb": imdb = value; break;
                        case "kinopoisk": kinopoisk = value; break;

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

        public SerialSeason(string url, bool isChild = false)
        {
            this.SeasonID = IntVal(Match(url, "/serial-([0-9]+)-", REGEX_IC, 1));
            if (isChild)
                setIsChild();
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
        public SerialSeason(int id, bool isChild = false)
        {
            this.SeasonID = id;
            if (isChild)
                setIsChild();
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
            _LoadCompilation();
        }
        public string getCompilation()
        {
            string _type = !string.IsNullOrEmpty(this.type) ? this.type : this.type_old;
            bool __type = "new" == _type || "nonew" == _type || "want" == _type;
            if (string.IsNullOrEmpty(this.Compilation) && __type)
            {
                this.Compilation = APIServerCompilation.Instance.getCompilation(this.SerialID);
                Save();
            }
            return this.Compilation;
        }

        public void setCompilation(string name)
        {
            if (this.Compilation != name && string.IsNullOrWhiteSpace(name))
            {
                if (APIServerCompilation.Instance.removeCompilation(this.SerialID))
                {
                    this.Compilation = APIServerCompilation.Instance.setCompilation(this.SerialID, name);
                    Save();
                }
            }
        }
        public void removeCompilation()
        {
            if (APIServerCompilation.Instance.removeCompilation(this.SerialID))
            {
                this.Compilation = APIServerCompilation.Instance.getCompilation(this.SerialID);
                Save();
            }
        }

        private void _LoadCompilation()
        {
            if (0 < this.SerialID)
            {
                this.Compilation = APIServerCompilation.Instance.getCompilation(this.SerialID);
                if (string.IsNullOrEmpty(this.Compilation))
                {
                    string _type = !string.IsNullOrEmpty(this.type) ? this.type : this.type_old;
                    bool __type = "new" == _type || "nonew" == _type || "want" == _type;
                    if (__type && !string.IsNullOrEmpty(Match(this.Genre, "анимационные", REGEX_IC)))
                    {
                        this.Compilation = APIServerCompilation.Instance.setCompilation(this.SerialID, "Animated");
                        return;
                    }
                    if (__type && !string.IsNullOrEmpty(Match(this.Genre, "аниме", REGEX_IC)))
                    {
                        this.Compilation = APIServerCompilation.Instance.setCompilation(this.SerialID, "Anime");
                        return;
                    }

                    switch (_type)
                    {
                        case "new":
                        case "nonew":
                            this.Compilation = APIServerCompilation.Instance.setCompilation(this.SerialID, APIServerCompilation.ACTIVE_COMPILATION);
                            return;
                        case "want":
                            this.Compilation = APIServerCompilation.Instance.setCompilation(this.SerialID, APIServerCompilation.WANT_COMPILATION);
                            return;
                    }
                }
            }
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
            if (!this.isChild)
            {
                Dictionary<int, int> data2 = DB.Instance.getSeasons(this.SerialID, this.SeasonID);
                if (0 < data.Count)
                {
                    foreach (KeyValuePair<int, int> item in data2)
                        if (this.SeasonID != item.Key)
                            if (this.Seasons.ContainsKey(item.Key))
                            {
                                this.Seasons[item.Key] = new SerialSeason(item.Value, true);
                            }
                            else
                            {
                                this.Seasons.Add(item.Key, new SerialSeason(item.Value, true));
                            }

                    Console.WriteLine("\t\tLoaded Seasons for: {0:S}", !string.IsNullOrEmpty(this.serialUrl) ? this.serialUrl : this.SeasonID.ToString());
                }
            }

            // TODO: Загружать Релейтеды

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
            this.forsed_update_playlist = this.forsed_update_player = this.forsed_update_page = needSave.Contains("site_updated");
            needSave.Clear();
            if (DB.Instance.setSeason(this.id, data))
                Console.WriteLine("\t\tSaved:  {0:S}", !string.IsNullOrEmpty(this.serialUrl) ? this.serialUrl : this.SeasonID.ToString());
            // TODO: Сохронять релейтеды

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
            Description = Regex.Replace(Regex.Replace(WebUtility.HtmlDecode(Match(html, "<p itemprop=\"description\">(.*?)</p>", REGEX_ICS, 1)), "<br[^<>]*>", "\r\n"), "(\r\n){2,}", "\r\n");
            secureMark = _parseData4Play(Match(html, "data4play = ({.*?})", REGEX_ICS, 1));

            _parseTitle(Match(html, "<h1 class=\"pgs-sinfo-title\"[^<>]*>(.*?)</h1>", REGEX_ICS, 1), this);
            _parseTags(Match(html, "<div data-tag=\"wrap\">(.*?)</div>", REGEX_ICS, 1));
            _parseMetaInfo(html);

            if (!isChild)
            {
                _parseSeasons(Match(html, "<div class=\"pgs-seaslist\">(.*?)</div>", REGEX_ICS, 1));
                _parseRelated(html);
            }
            _LoadCompilation();
            Save();
        }

        public void syncPage(bool forsed = false)
        {
            forsed = forsed || forsed_update_page;
            if ((timeout < DateTime.UtcNow.Subtract(create_date).TotalSeconds || timeout < DateTime.UtcNow.Subtract(updated_date).TotalSeconds) || forsed)
            {
                parsePage(downloadPage(forsed));
                forsed_update_page = false;
            }
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
            Dictionary<string, string> _MetaInfo = new Dictionary<string, string>();
            foreach (Match matchInfo in Regex.Matches(html, "<div class=\"pgs-sinfo_list[^\"]*\">(.*?)</div>", REGEX_ICS))
            {
                string info = matchInfo.Groups[1].ToString().Trim();
                info = Regex.Replace(info, "</span>", "||", REGEX_ICS);
                info = Regex.Replace(info, "(<[^<>]*>|\n|\r|\t)", " ", REGEX_ICS);
                info = Regex.Replace(info, "[ ]{2,}", " ", REGEX_ICS);
                foreach (Match _matchInfo in Regex.Matches(info, "([^:\\|]+?):([^\\|]+?)\\|\\|", REGEX_ICS))
                {
                    string name = _matchInfo.Groups[1].ToString().Trim();
                    string value = _matchInfo.Groups[2].ToString().Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        switch (name)
                        {
                            case "Оригинал": this.TitleOriginal = value; break;
                            case "Жанр": this.Genre = value; break;
                            case "Страна": this.Country = value; break;
                            case "Ориентировочная дата выхода":
                            case "Вышел": this.Release = value; break;
                            case "Ограничение": this.Limitation = value; break;
                            case "IMDB": this.IMDB = value; break;
                            case "КиноПоиск": this.KinoPoisk = value; break;
                        }
                    }

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
                SerialSeason season = new SerialSeason(url, true);
                season.TitleRU = Match(matchInfo.Groups[3].ToString(), "<div>(.+?)</div>", REGEX_ICS, 1);
                season.Save();
                this.Related.Add(season);
            }
        }

        private void _parseTags(string html)
        {
            /*
             * TODO: Посмотреть насколько востребованы теги
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
            */
        }
        private void _parseSeasons(string html)
        {
            if (string.IsNullOrEmpty(html))
                return;
            int i = 0;
            foreach (Match matchSeason in Regex.Matches(html, "<a[^<>]*href=\"([^<>\"]*)\"[^<>]*>(.*?)</a>", REGEX_ICS))
            {
                string url = this.SERVER_URL + matchSeason.Groups[1].ToString();
                if (this.serialUrl == url)
                {
                    continue;
                }
                SerialSeason season = new SerialSeason(url, true);
                _parseTitle(matchSeason.Groups[2].ToString().Trim(), season);
                season.Title = this.Title;
                season.TitleRU = this.TitleRU;
                season.TitleEN = this.TitleEN;
                season.SerialID = this.SerialID;
                season.Save();
                if (0 < season.Season)
                {
                    if (!this.Seasons.ContainsKey(season.Season))
                    {
                        this.Seasons.Add(season.Season, season);
                    }
                    else this.Seasons[season.Season].Load();
                }
                else
                {
                    i--;
                    if (!this.Seasons.ContainsKey(i))
                        this.Seasons.Add(i, season);
                }

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
            forsed = forsed || forsed_update_player;
            // UNDONE: Скачивать и парсить страницу плеера
            forsed_update_player = false;
        }
        public void syncPlaylists(bool forsed = false)
        {
            forsed = forsed || forsed_update_playlist;
            // UNDONE: Скачивать и парсить страницы плейлистов
            forsed_update_playlist = false;
        }



        public ListViewItem ToListViewItem()
        {
            var a = new ListViewItem(this.SeasonID.ToString());
            // UNDONE: Сделать конвертацию в ListViewItem

            return a;
        }
    }
}
