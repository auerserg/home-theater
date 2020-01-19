using HomeTheater.Helper;
using HomeTheater.Serial.data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HomeTheater.Serial
{
    class SerialSeason : APIServerParent
    {
        private bool __forsed_update_page = false;
        private bool __forsed_update_player = false;
        private bool __forsed_update_playlist = false;
        private bool __needSaveRelated = false;
        private DateTime __create_date, __site_updated, __updated_date;
        private int __id, __season, __serial_id, __timeout = 0, __user_comments, __user_views_last_day;
        private string __compilation, __country, __description, __genre, __imdb, __kinopoisk, __limitation, __marks_current, __marks_last, __release, __secure_mark, __title, __title_en, __title_full, __title_original, __title_ru, __type, __type_old, __url;
        private List<int> __related = new List<int>();
        private Dictionary<int, int> __seasons = new Dictionary<int, int>();
        private List<string> __needSave = new List<string>();
        public Dictionary<int, string> Tags = new Dictionary<int, string>();
        public ListViewItem ListViewItem = new ListViewItem(new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });

        private string this[string index]
        {
            get
            {
                string result = "";

                switch (index)
                {
                    case "country": result = __country.ToString(); break;
                    case "description": result = __description.ToString(); break;
                    case "genre": result = __genre.ToString(); break;
                    case "imdb": result = __imdb.ToString(); break;
                    case "kinopoisk": result = __kinopoisk.ToString(); break;
                    case "limitation": result = __limitation.ToString(); break;
                    case "marks_current": result = __marks_current.ToString(); break;
                    case "marks_last": result = __marks_last.ToString(); break;
                    case "release": result = __release.ToString(); break;
                    case "season": result = __season.ToString(); break;
                    case "secure_mark": result = __secure_mark.ToString(); break;
                    case "serial_id": result = __serial_id.ToString(); break;
                    case "site_updated": result = __site_updated.ToString(DB.DATE_FORMAT); break;
                    case "title": result = __title.ToString(); break;
                    case "title_en": result = __title_en.ToString(); break;
                    case "title_full": result = __title_full.ToString(); break;
                    case "title_original": result = __title_original.ToString(); break;
                    case "title_ru": result = __title_ru.ToString(); break;
                    case "type": result = __type.ToString(); break;
                    case "url": result = __url.ToString(); break;
                    case "user_comments": result = __user_comments.ToString(); break;
                    case "user_views_last_day": result = __user_views_last_day.ToString(); break;
                }

                return result;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    value = value.ToString().Trim(); switch (index)
                    {
                        case "country": __country = value; break;
                        case "create_date": __create_date = DateVal(value, DB.TIME_FORMAT); break;
                        case "description": __description = value; break;
                        case "genre": __genre = value; break;
                        case "imdb": __imdb = value; break;
                        case "kinopoisk": __kinopoisk = value; break;
                        case "limitation": __limitation = value; break;
                        case "marks_current": __marks_current = value; break;
                        case "marks_last": __marks_last = value; break;
                        case "release": __release = value; break;
                        case "season": __season = IntVal(value); break;
                        case "secure_mark": __secure_mark = value; break;
                        case "serial_id": __serial_id = IntVal(value); break;
                        case "site_updated": __site_updated = DateVal(value, DB.DATE_FORMAT); break;
                        case "title": __title = value; break;
                        case "title_en": __title_en = value; break;
                        case "title_full": __title_full = value; break;
                        case "title_original": __title_original = value; break;
                        case "title_ru": __title_ru = value; break;
                        case "type": __type_old = value; break;
                        case "updated_date": __updated_date = DateVal(value, DB.TIME_FORMAT); break;
                        case "url": __url = value; break;
                        case "user_comments": __user_comments = IntVal(value); break;
                        case "user_views_last_day": __user_views_last_day = IntVal(value); break;
                    }
                }

            }
        }
        public int SeasonID
        {
            get => __id;
            set => __id = value;

        }
        public int SerialID
        {
            get => __serial_id;
            set
            {
                if (__serial_id != value && value > 0)
                {
                    __needSave.Add("serial_id");
                    __serial_id = value;
                }
            }
        }
        public int Season
        {
            get => __season;
            set
            {
                if (value > 0)
                {
                    if (__season != value)
                    {
                        __needSave.Add("season");
                        __season = value;
                    }
                }
                else
                {
                    __season = 0;
                }
            }
        }
        public int UserComments
        {
            get => __user_comments;
            set
            {
                if (value > 0)
                {
                    if (__user_comments != value)
                    {
                        __needSave.Add("user_comments");
                        __user_comments = value;
                    }
                }
                else
                {
                    __user_comments = 0;
                }
            }
        }
        public int UserViewsLastDay
        {
            get => __user_views_last_day;
            set
            {
                if (value > 0)
                {
                    if (__user_views_last_day != value)
                    {
                        __needSave.Add("user_views_last_day");
                        __user_views_last_day = value;
                    }
                }
                else
                {
                    __user_views_last_day = 0;
                }
            }
        }
        private int timeout
        {
            get
            {
                if (0 <= __timeout)
                {
                    string timeout = DB.Instance.OptionGet(String.Concat("cacheTimeSerial_" + this.Type));
                    if (string.IsNullOrEmpty(timeout))
                    {
                        timeout = DB.Instance.OptionGet(String.Concat("cacheTimeSerial_none"));
                    }
                    if (string.IsNullOrEmpty(timeout))
                    {
                        timeout = (24 * 60 * 60).ToString();
                    }
                    __timeout = IntVal(timeout);
                }

                return __timeout;
            }
        }
        public string Compilation
        {
            get => string.IsNullOrWhiteSpace(__compilation) ? string.Empty : __compilation;
            set
            {
                if (__compilation != value && !string.IsNullOrWhiteSpace(value))
                {
                    __compilation = value;
                }
            }
        }
        public string Genre
        {
            get => string.IsNullOrWhiteSpace(__genre) ? string.Empty : __genre;
            set
            {
                if (__genre != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("genre");
                    __genre = value;
                }
            }
        }
        public string Country
        {
            get => string.IsNullOrWhiteSpace(__country) ? string.Empty : __country;
            set
            {
                if (__country != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("country");
                    __country = value;
                }
            }
        }
        public string Release
        {
            get => string.IsNullOrWhiteSpace(__release) ? string.Empty : __release;
            set
            {
                if (__release != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("release");
                    __release = value;
                }
            }
        }
        public string IMDB
        {
            get => string.IsNullOrWhiteSpace(__imdb) ? string.Empty : __imdb;
            set
            {
                if (__imdb != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("imdb");
                    __imdb = value;
                }
            }
        }
        public string KinoPoisk
        {
            get => string.IsNullOrWhiteSpace(__kinopoisk) ? string.Empty : __kinopoisk;
            set
            {
                if (__kinopoisk != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("kinopoisk");
                    __kinopoisk = value;
                }
            }
        }
        public string Limitation
        {
            get => string.IsNullOrWhiteSpace(__limitation) ? string.Empty : __limitation;
            set
            {
                if (__limitation != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("limitation");
                    __limitation = value;
                }
            }
        }
        public string SerialUrl
        {
            get => string.IsNullOrWhiteSpace(__url) ? string.Empty : __url;
            set
            {
                if (__url != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("url");
                    __url = value;
                }
            }
        }
        public string Title
        {
            get => string.IsNullOrWhiteSpace(__title) ? this.TitleRU : __title;
            set
            {
                if (__title != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("title");
                    __title = value;
                }
            }
        }
        public string TitleRU
        {
            get => string.IsNullOrWhiteSpace(__title_ru) ? this.SerialUrl : __title_ru;
            set
            {
                if (__title_ru != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("title_ru");
                    __title_ru = value;
                }
            }
        }
        public string TitleFull
        {
            get => string.IsNullOrWhiteSpace(__title_full) ? this.Title : __title_full;
            set
            {
                if (__title_full != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("title_full");
                    __title_full = value;
                }
            }
        }
        public string TitleEN
        {
            get => string.IsNullOrWhiteSpace(__title_en) ? string.Empty : __title_en;
            set
            {
                if (__title_en != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("title_en");
                    __title_en = value;
                }
            }
        }
        public string TitleOriginal
        {
            get => string.IsNullOrWhiteSpace(__title_original) ? string.Empty : __title_original;
            set
            {
                if (__title_original != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("title_original");
                    __title_original = value;
                }
            }
        }
        public string Description
        {
            get => string.IsNullOrWhiteSpace(__description) ? string.Empty : __description;
            set
            {
                if (__description != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("description");
                    __description = value;
                }
            }
        }
        public string MarkCurrent
        {
            get => string.IsNullOrWhiteSpace(__marks_current) ? "-1" : __marks_current;
            set
            {
                if (__marks_current != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("marks_current");
                    __marks_current = value;
                }
            }
        }
        public string TypeOLD
        {
            get => string.IsNullOrWhiteSpace(__type_old) ? string.Empty : __type_old;
        }
        public string Type
        {
            get => string.IsNullOrWhiteSpace(__type) ? "none" : __type;
            set
            {
                if (__type != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("type");
                    __type = value;
                }
            }
        }
        public string MarkLast
        {
            get => string.IsNullOrWhiteSpace(__marks_last) ? string.Empty : __marks_last;
            set
            {
                if (__marks_last != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("marks_last");
                    __marks_last = value;
                }
            }
        }
        public string Mark
        {
            get
            {
                if ("-1" == MarkCurrent)
                {
                    return MarkLast;
                }
                if ("-2" == MarkCurrent || "-3" == MarkCurrent)
                {
                    return "";
                }
                return string.Format("{0} из {1}", MarkCurrent, MarkLast);
            }
            set
            {
            }
        }
        public string SecureMark
        {
            get => string.IsNullOrWhiteSpace(__secure_mark) ? string.Empty : __secure_mark;
            set
            {
                if (__secure_mark != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("secure_mark");
                    __secure_mark = value;
                }
            }
        }
        public string ImageSmall
        {
            get
            {
                return ImageURLGet("small");
            }
        }
        public string Image
        {
            get
            {
                return ImageURLGet();
            }
        }
        public string ImageLarge
        {
            get
            {
                return ImageURLGet("large");
            }
        }
        public DateTime SiteUpdated
        {
            get => __site_updated;
            set
            {
                if (__site_updated != value && new DateTime() != value)
                {
                    if (new DateTime() != __site_updated)
                        __needSave.Add("site_updated_forsed");
                    __needSave.Add("site_updated");
                    __site_updated = value;
                }
            }
        }
        public DateTime CachedDate
        {
            get => __create_date;
            set => __create_date = value;
        }
        public Dictionary<int, SerialSeason> Seasons
        {
            get
            {
                Dictionary<int, SerialSeason> items = new Dictionary<int, SerialSeason>();
                if (0 < __seasons.Count)
                {
#if DEBUG
                    DateTime start = DateTime.UtcNow;
#endif
                    foreach (KeyValuePair<int, int> item in __seasons)
                        if (this.SeasonID != item.Key)
                            if (items.ContainsKey(item.Key))
                            {
                                items[item.Key] = new SerialSeason(item.Value);
                            }
                            else
                            {
                                items.Add(item.Key, new SerialSeason(item.Value));
                            }
#if DEBUG
                    Console.WriteLine("\tLoadSeasons {0:S}: {1:S} ", this.SeasonID.ToString(), DateTime.UtcNow.Subtract(start).TotalSeconds.ToString());
#endif
                }

                return items;
            }
            set
            {
            }
        }
        public List<SerialSeason> Related
        {
            get
            {
                List<SerialSeason> items = new List<SerialSeason>();
                if (0 < __related.Count)
                {
#if DEBUG
                    DateTime start = DateTime.UtcNow;
#endif
                    foreach (int id in __related)
                    {
                        items.Add(new SerialSeason(id));
                    }
#if DEBUG
                    Console.WriteLine("\tLoadRelated {0:S}: {1:S} ", this.SeasonID.ToString(), DateTime.UtcNow.Subtract(start).TotalSeconds.ToString());
#endif
                }

                return items;
            }
            set
            {
            }
        }
        public SerialSeason(string url)
        {
            this.SeasonID = IntVal(Match(url, "/serial-([0-9]+)-", REGEX_IC, 1));
            Init();

            Match matchurl = Regex.Match(url, "^(.*?)#rewind=(.*?)_seriya$", REGEX_IC);
            if (matchurl.Success)
            {
                this.SerialUrl = matchurl.Groups[1].ToString();
                this.MarkCurrent = matchurl.Groups[2].ToString();
            }
            else
            {
                this.SerialUrl = url;
            }
        }
        public SerialSeason(int id)
        {
            this.SeasonID = id;
            Init();
        }
        private void Init()
        {
            Load();
            CompilationLoad();
        }
        public string CompilationGet()
        {
            string _type = !string.IsNullOrEmpty(this.__type) ? this.__type : this.__type_old;
            bool __type = "new" == _type || "nonew" == _type || "want" == _type;
            if (string.IsNullOrEmpty(this.Compilation) && __type)
            {
                this.Compilation = APIServerCompilation.Instance.Get(this.SerialID);
            }
            return this.Compilation;
        }
        public void CompilationSet(string name)
        {
            bool _is = true;
            if (!string.IsNullOrEmpty(Compilation))
                _is = CompilationRemove();
            if (_is)
                Compilation = APIServerCompilation.Instance.Set(SerialID, name);
        }
        public bool CompilationRemove()
        {
            return APIServerCompilation.Instance.Remove(SerialID);
        }
        public bool CompilationRemove(int ID)
        {
            bool result = APIServerCompilation.Instance.Remove(SerialID, ID);
            return result;
        }
        public void CompilationRemove(string name)
        {
            if (APIServerCompilation.Instance.Remove(this.SerialID, name) && this.Compilation == name)
            {
                this.Compilation = "";
                SaveAsync();
            }
        }
        private void CompilationLoad()
        {
            if (0 < this.SerialID)
            {
                this.Compilation = APIServerCompilation.Instance.Get(this.SerialID);
                if (string.IsNullOrEmpty(this.Compilation))
                {
                    string _type = !string.IsNullOrEmpty(this.__type) ? this.__type : this.__type_old;
                    bool __type = "new" == _type || "nonew" == _type || "want" == _type;
                    if (__type && !string.IsNullOrEmpty(Match(this.Genre, "анимационные", REGEX_IC)))
                    {
                        this.Compilation = APIServerCompilation.Instance.Set(this.SerialID, "Animated");
                        return;
                    }
                    if (__type && !string.IsNullOrEmpty(Match(this.Genre, "аниме", REGEX_IC)))
                    {
                        this.Compilation = APIServerCompilation.Instance.Set(this.SerialID, "Anime");
                        return;
                    }

                    switch (_type)
                    {
                        case "new":
                        case "nonew":
                            this.Compilation = APIServerCompilation.Instance.Set(this.SerialID, "Active");
                            return;
                        case "want":
                            this.Compilation = APIServerCompilation.Instance.Set(this.SerialID, "Want");
                            return;
                    }
                }
            }
        }
        public SerialSeason Load()
        {
#if DEBUG
            DateTime start = DateTime.UtcNow;
#endif
            Dictionary<string, string> data = DB.Instance.SeasonGet(this.__id);
            if (0 < data.Count)
                foreach (KeyValuePair<string, string> item in data)
                    this[item.Key] = item.Value;
            __seasons = DB.Instance.SeasonsGet(this.SerialID, this.SeasonID);
            __related = DB.Instance.RelatedGet(this.SeasonID);
            ToListViewItem();
#if DEBUG
            Console.WriteLine("\tLoad {0:S}: {1:S} ", this.SeasonID.ToString(), DateTime.UtcNow.Subtract(start).TotalSeconds.ToString());
#endif
            return this;
        }
        public void _saveRelated()
        {
            if (__needSaveRelated)
            {
#if DEBUG
                DateTime start = DateTime.UtcNow;
#endif
                DB.Instance.RelatedSet(this.SerialID, this.__related);
                __needSaveRelated = false;
#if DEBUG
                Console.WriteLine("\tSaveRelated {0:S}: {1:S} ", this.SeasonID.ToString(), DateTime.UtcNow.Subtract(start).TotalSeconds.ToString());
#endif
            }
        }
        public void Save()
        {
            if (0 >= __id || 0 >= __needSave.Count)
                return;
#if DEBUG
            DateTime start = DateTime.UtcNow;
#endif
            Dictionary<string, string> data = new Dictionary<string, string>();
            for (var i = 0; i < __needSave.Count; i++)
            {
                string field = __needSave[i];
                string value = this[field];
                if (!string.IsNullOrWhiteSpace(value) && !data.ContainsKey(field))
                    data.Add(field, value);
            }
            this.__forsed_update_playlist = this.__forsed_update_player = this.__forsed_update_page = __needSave.Contains("site_updated_forsed");
            __needSave.Clear();
            DB.Instance.SeasonSet(this.__id, data);
            ToListViewItem();
#if DEBUG
            Console.WriteLine("\tSave {0:S}: {1:S} ", this.SeasonID.ToString(), DateTime.UtcNow.Subtract(start).TotalSeconds.ToString());
#endif
        }
        public async void SaveAsync()
        {
            Save();
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
                    MarkLast = Regex.Replace(matchseason[2], " (\\([^\\(\\)]+\\)|серия)", "").Replace("Только трейлер", "");
                }
            }

            TitleRU = Match(html, "<div class=\"rside-t\">(.*?)</div>", REGEX_ICS, 1);
            TitleEN = Match(html, "<div class=\"rside-t_en\">(.*?)</div>", REGEX_ICS, 1);
            SaveAsync();
        }
        public void parsePause(string html)
        {
            Season = IntVal(Match(html, "<div class=\"pgs-marks-seas\">(.*?)</div>", REGEX_ICS, 1));
            TitleRU = Match(html, "<div class=\"pgs-marks-name\">(.*?)</div>", REGEX_ICS, 1);
            MarkLast = Match(html, "<div class=\"pgs-marks-current\">[^<>]*<strong>(.*?)</strong>[^<>]*</div>", REGEX_ICS, 1).Replace("Только трейлер", "");
            SiteUpdated = DateVal(Match(html, "<div class=\"pgs-marks-mess\">(.*?)</div>", REGEX_ICS, 1), "dd.MM.yyyy");
            SaveAsync();
        }
        private void parsePage(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
#if DEBUG
                Console.WriteLine("\t\t[!] Остановка парсинга!!! Страница небыла полученна!");
#endif
                return;
            }
#if DEBUG
            DateTime start = DateTime.UtcNow;
#endif
            SerialID = IntVal(Match(html, "data-id-serial=\"([0-9]+)\"", REGEX_ICS, 1));
            SeasonID = IntVal(Match(html, "data-id-season=\"([0-9]+)\"", REGEX_ICS, 1));
            Description = WebUtility.HtmlDecode(Match(html, "<p itemprop=\"description\">(.*?)</p>", REGEX_ICS, 1));
            SecureMark = _parseData4Play(Match(html, "data4play = ({.*?})", REGEX_ICS, 1));
            SiteUpdated = DateVal(Match(html, "<meta itemprop=\"dateModified\" content=\"([^\"+]+)([^\"]+)\">", REGEX_ICS, 1), "yyyy-MM-ddTHH:mm:ss");
            UserComments = IntVal(Match(html, "<meta itemprop=\"interactionCount\" content=\"UserComments:([0-9]+)\"/>", REGEX_ICS, 1));
            UserViewsLastDay = IntVal(Match(html, "<meta itemprop=\"interactionCount\" content=\"UserViewsLastDay:([0-9]+)\"/>", REGEX_ICS, 1));


            _parseTitle(Match(html, "<h1 class=\"pgs-sinfo-title\"[^<>]*>(.*?)</h1>", REGEX_ICS, 1), this);
            //_parseTags(Match(html, "<div data-tag=\"wrap\">(.*?)</div>", REGEX_ICS, 1));
            _parseMetaInfo(html);
            _parseSeasons(Match(html, "<div class=\"pgs-seaslist\">(.*?)</div>", REGEX_ICS, 1));
            _parseCompilation(Match(html, "<div class=\"b-labellist\">(.*?)</div>", REGEX_ICS, 1));
            _parseRelatedAsync(html);

            CompilationLoad();
            SaveAsync();
#if DEBUG
            Console.WriteLine("\tparsePage {0:S}: {1:S} ", this.SeasonID.ToString(), DateTime.UtcNow.Subtract(start).TotalSeconds.ToString());
#endif
        }
        public void syncPage(bool forsed = false)
        {
            forsed = forsed || __forsed_update_page;
            if ((timeout < DateTime.UtcNow.Subtract(__create_date).TotalSeconds || timeout < DateTime.UtcNow.Subtract(__updated_date).TotalSeconds) || forsed)
            {
                string content = downloadPage(forsed);
                parsePage(content);
                __forsed_update_page = false;
            }
        }
        private string _parseData4Play(string html)
        {
            html = Regex.Replace(html, "[\r\n\t ]+", string.Empty);
            if (string.IsNullOrEmpty(html))
            {
#if DEBUG
                Console.WriteLine("\t\t[?] Данные для плеера не найдены!!! Сериал заблокирован?");
#endif
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
            html = Regex.Replace(html, "[ ]{2,}", " ", REGEX_IC);
            _o.TitleFull = html.Trim();
            _o.Season = IntVal(Match(_o.TitleFull, "([0-9]+) сезон$", REGEX_ICS, 1));
            html = Regex.Replace(_o.TitleFull, " [0-9]+ сезон$", string.Empty, REGEX_IC).Trim();
            _o.Title = html.Trim();
            if (!string.IsNullOrEmpty(_o.TitleRU))
            {
                html = Regex.Replace(_o.Title.Replace(_o.TitleRU, string.Empty), "(^/|/$)", string.Empty);
                if (html != _o.TitleRU)
                {
                    _o.TitleEN = html.Trim();
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
#if DEBUG
                    Console.WriteLine("\t\t[?]Не возможно разделить название сериала: {0:S}", _o.Title);
#endif
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
        private async void _parseRelatedAsync(string html)
        {
            _parseRelated(html);
            _saveRelated();
        }
        private void _parseRelated(string html)
        {
            if (string.IsNullOrEmpty(html))
                return;
            List<int> _related = new List<int>();
            foreach (Match matchInfo in Regex.Matches(html, "<a href=\"/([^\"]+)\" data-id=\"([0-9]+)\" class=\"pst\">(.*?)</a>", REGEX_ICS))
            {
                string url = this.SERVER_URL + "/" + matchInfo.Groups[1].ToString().Trim();
                if (this.SerialUrl == url)
                {
                    continue;
                }
                SerialSeason season = new SerialSeason(url);
                season.TitleRU = Match(matchInfo.Groups[3].ToString(), "<div>(.+?)</div>", REGEX_ICS, 1);
                season.SaveAsync();
                if (!_related.Contains(season.SeasonID))
                {
                    _related.Add(season.SeasonID);
                }
            }
            List<int> _relatedOld = this.__related;
            List<int> _relatedNew = _related;
            this.__related = _related;
            if (0 < _related.Count)
            {
                if (0 < _relatedOld.Count)
                    for (int i = _relatedOld.Count - 1; i >= 0; i--)
                    {
                        if (_relatedNew.Contains(_relatedOld[i]))
                        {
                            _relatedNew.Remove(_relatedOld[i]);
                            _relatedOld.RemoveAt(i);
                        }
                    }
                __needSaveRelated = __needSaveRelated || 0 < _relatedNew.Count || 0 < _relatedOld.Count;
            }
        }
        private void _parseTags(string html)
        {
            // TODO: Посмотреть насколько востребованы теги
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
            int i = 0;
            foreach (Match matchSeason in Regex.Matches(html, "<a[^<>]*href=\"([^<>\"]*)\"[^<>]*>(.*?)</a>", REGEX_ICS))
            {
                string url = this.SERVER_URL + matchSeason.Groups[1].ToString();
                if (this.SerialUrl == url)
                {
                    continue;
                }
                SerialSeason season = new SerialSeason(url);
                _parseTitle(matchSeason.Groups[2].ToString().Trim(), season);
                season.Title = this.Title;
                season.TitleRU = this.TitleRU;
                season.TitleEN = this.TitleEN;
                season.SerialID = this.SerialID;
                season.SaveAsync();
                if (0 < season.Season)
                {
                    if (!__seasons.ContainsKey(season.Season))
                        __seasons.Add(season.Season, season.SeasonID);
                }
                else
                {
                    i--;
                    if (!__seasons.ContainsKey(i))
                        __seasons.Add(i, season.SeasonID);
                }

            }
        }
        private void _parseCompilation(string html)
        {
            if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(Compilation))
                return;
            Dictionary<int, string> data = new Dictionary<int, string>();
            foreach (Match matchCompilation in Regex.Matches(html, "<label>(.*?)</label>", REGEX_ICS))
            {
                string _html = matchCompilation.Groups[1].ToString().Trim();
                bool _checked = "checked" == Match(_html, "checked", REGEX_IC, 0);
                int id = IntVal(Match(_html, "value=\"([0-9]+)\"", REGEX_IC, 1));
                string name = Match(_html, "<span>(.*?)</span>", REGEX_IC, 1);
                if (!data.ContainsKey(id))
                    data.Add(id, name);
                if (_checked)
                    APIServerCompilation.Instance.RelationUpdate(SerialID, id);
            }
            APIServerCompilation.Instance.CompilationUpdate(data);
        }
        public string downloadPage(bool forsed = false)
        {
            string content = "";
            bool isActual = false;
            DBCache cacheItem = DB.Instance.CacheGet(SerialUrl, this.timeout);
            if (null != cacheItem)
            {
                content = cacheItem.content;
                isActual = cacheItem.isActual;
                this.CachedDate = cacheItem.date;
            }

            if (!isActual || forsed)
            {
                content = Download(SerialUrl);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    DB.Instance.CacheSet(SerialUrl, content);
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
            forsed = forsed || __forsed_update_player;
            // UNDONE: Скачивать и парсить страницу плеера
            __forsed_update_player = false;
        }
        public void syncPlaylists(bool forsed = false)
        {
            forsed = forsed || __forsed_update_playlist;
            // UNDONE: Скачивать и парсить страницы плейлистов
            __forsed_update_playlist = false;
        }
        public ListViewItem ToListViewItem()
        {
            try
            {
                ListViewItem.Name = SeasonID.ToString();
                ListViewItem.ImageKey = SeasonID.ToString();
                ListViewItem.Tag = this;
                ListViewItem.Text = TitleFull;
                ListViewItem.SubItems[0].Text = TitleFull;
                ListViewItem.SubItems[1].Text = Title;
                ListViewItem.SubItems[2].Text = TitleRU;
                ListViewItem.SubItems[3].Text = TitleEN;
                ListViewItem.SubItems[4].Text = TitleOriginal;
                ListViewItem.SubItems[5].Text = SerialUrl;
                ListViewItem.SubItems[6].Text = SeasonID.ToString();
                ListViewItem.SubItems[7].Text = SerialID.ToString();
                ListViewItem.SubItems[8].Text = 0 < Season ? Season.ToString() : "";
                ListViewItem.SubItems[9].Text = Genre;
                ListViewItem.SubItems[10].Text = Country;
                ListViewItem.SubItems[11].Text = Release;
                ListViewItem.SubItems[12].Text = Limitation;
                ListViewItem.SubItems[13].Text = IMDB;
                ListViewItem.SubItems[14].Text = KinoPoisk;
                ListViewItem.SubItems[15].Text = 0 < UserComments ? UserComments.ToString() : "";
                ListViewItem.SubItems[16].Text = 0 < UserViewsLastDay ? UserViewsLastDay.ToString() : "";
                ListViewItem.SubItems[17].Text = MarkCurrent;
                ListViewItem.SubItems[18].Text = MarkLast;
                ListViewItem.SubItems[19].Text = Mark;
                ListViewItem.SubItems[20].Text = Compilation;
                ListViewItem.SubItems[21].Text = SiteUpdated.ToString("dd.MM.yyyy");
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
            }
            return ListViewItem;
        }
        public string ImageURLGet(string size = "")
        {
            switch (size)
            {
                case "small":
                case "large":
                    size = size + "/";
                    break;
                default:
                    size = "";
                    break;
            }
            return string.Format("http://cdn.seasonvar.ru/oblojka/{0}{1}.jpg", size, SeasonID);
        }
        public bool MarkSetPause(string seria = "")
        {
            return MarkDo("pauseadd", seria);
        }
        public bool MarkSeWantToSee()
        {
            return MarkDo("wanttosee");
        }
        public bool MarkSetWatched()
        {
            return MarkDo("watched");
        }
        public bool MarkSetNotWatched()
        {
            return MarkDo("notWatched");
        }
        public bool MarkSetDelSee()
        {
            return MarkDo("delete");
        }
        private bool MarkDo(string type = "", string seria = "")
        {
            if (0 >= SeasonID)
                return false;
            NameValueCollection postData = null;
            switch (type)
            {
                case "pauseadd": postData = new NameValueCollection { { "id", SeasonID.ToString() }, { "seria", seria }, { "pauseadd", "true" }, { "minute", "0" }, { "second", "0" }, { "tran", "0" } }; break;
                case "wanttosee": postData = new NameValueCollection { { "id", SeasonID.ToString() }, { "seria", "-1" }, { "wanttosee", "true" }, { "minute", "0" }, { "second", "0" } }; break;
                case "watched": postData = new NameValueCollection { { "id", SeasonID.ToString() }, { "seria", "-2" }, { "watched", "true" }, { "minute", "0" }, { "second", "0" } }; break;
                case "notWatched": postData = new NameValueCollection { { "id", SeasonID.ToString() }, { "seria", "-3" }, { "notWatched", "true" }, { "minute", "0" }, { "second", "0" } }; break;

                case "delete":
                default:
                    postData = new NameValueCollection { { "delId", SeasonID.ToString() } }; break;
            }

            return APIServer.Instance.doMarks(postData).Status();
        }
    }
}
