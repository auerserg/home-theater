using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeTheater.API.Response;
using HomeTheater.Helper;

namespace HomeTheater.API.Serial
{
    public class Season : Abstract.Serial
    {
        private DateTime __cached_date;
        private string __compilation;
        private bool __forsed_update_page;
        private bool __forsed_update_player;
        private bool __forsed_update_playlist;
        private bool __needSaveRelated;
        private Player __player;
        private List<int> __related = new List<int>();
        private int __timeout;

        public ListViewItem ListViewItem = new ListViewItem(new[]
            {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""});

        private Form parentForm;

        public Dictionary<int, int> Seasons = new Dictionary<int, int>();

        public Dictionary<int, string> Tags = new Dictionary<int, string>();

        public Season(string url)
        {
            ID = IntVal(Match(url, "/serial-([0-9]+)-", REGEX_IC, 1));
            Init();

            var matchurl = Regex.Match(url, "^(.*?)#rewind=(.*?)_seriya$", REGEX_IC);
            if (matchurl.Success)
            {
                URL = matchurl.Groups[1].ToString();
                MarkCurrent = matchurl.Groups[2].ToString();
            }
            else
            {
                URL = url;
            }
        }

        public Season(string url, string Type)
        {
            ID = IntVal(Match(url, "/serial-([0-9]+)-", REGEX_IC, 1));
            Init();

            var matchurl = Regex.Match(url, "^(.*?)#rewind=(.*?)_seriya$", REGEX_IC);
            if (matchurl.Success)
            {
                URL = matchurl.Groups[1].ToString();
                MarkCurrent = matchurl.Groups[2].ToString();
            }
            else
            {
                URL = url;
            }

            this.Type = Type;
        }

        public Season(int id)
        {
            ID = id;
            Init();
        }

        protected override void CallValue(string name, string value = null, string value_old = null)
        {
            switch (name)
            {
                case "type":
                    if ("nonew" == value_old && "new" == value) __forsed_update_player = true;
                    break;
                case "marks_last":
                    __forsed_update_player = true;
                    break;
                case "site_updated":
                    __forsed_update_player = __forsed_update_page = true;
                    break;
            }
        }

        private void Init()
        {
            Load();
            CompilationLoad();
        }

        public Season Load()
        {
            LoadValues(() =>
            {
                var data = DB.Instance.SeasonGet(ID);
                if (data.ContainsKey("type")) data.Add("type_old", data["type"]);

                return data;
            });
            Seasons = DB.Instance.SeasonsGet(SerialID, ID);
            __related = DB.Instance.RelatedGet(ID);
            ToListViewItem();

            return this;
        }

        public void Save()
        {
            if (0 == ID || 0 == __data_new.Count)
                return;
#if DEBUG
            var start = DateTime.UtcNow;
#endif

            SaveValues(data =>
            {
                if (0 == ID)
                    return false;
                DB.Instance.SeasonSet(ID, data);
                ToListViewItem();
                return true;
            });
#if DEBUG
            Console.WriteLine("\tSave Season\t\t{0}\t{1}:\t{2}", SerialID, ID,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
        }

        public void SaveAsync()
        {
            Task.Run(() =>
            {
                try
                {
                    Save();
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            });
        }

        public string GetOnlySecure()
        {
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            var html = downloadPage(true);
            var result = "";
            if (!string.IsNullOrWhiteSpace(html))
            {
                Secure = _parseData4Play(Match(html, "data4play = ({.*?})", REGEX_ICS, 1));
                result = Secure;
            }
#if DEBUG
            Console.WriteLine("\tGet Secure\t{0}\t{1}:\t{2}", SerialID, ID,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif

            return result = Secure;
        }

        public string downloadPage(bool forsed = false)
        {
            var cacheItem = Server.Instance.downloadPage(URL, forsed, timeout);
            CachedDate = cacheItem.date;

            return cacheItem.ToString();
        }

        public ListViewItem ToListViewItem(Form parentForm = null)
        {
            if (null != parentForm)
                this.parentForm = parentForm;
            if (1 != Thread.CurrentThread.ManagedThreadId)
            {
                if (null != parentForm)
                    this.parentForm.Invoke(new Action(() => ToListViewItem()));
                return ListViewItem;
            }

            try
            {
                string[] newData =
                {
                    !string.IsNullOrEmpty(TitleFull)
                        ? TitleFull
                        : string.Format(0 < SeasonNum ? "{0} {1} сезон" : "{0}", TitleRU, SeasonNum),
                    !string.IsNullOrEmpty(Title) ? Title : TitleRU,
                    TitleRU,
                    !string.IsNullOrEmpty(TitleEN) ? TitleEN : TitleRU,
                    !string.IsNullOrEmpty(TitleOriginal) ? TitleOriginal : TitleRU,
                    URL,
                    ID.ToString(),
                    SerialID.ToString(),
                    0 < SeasonNum ? SeasonNum.ToString() : "",
                    Genre,
                    Country,
                    Release,
                    Limitation,
                    IMDB.ToString(),
                    KinoPoisk.ToString(),
                    0 < UserComments ? UserComments.ToString() : "",
                    0 < UserViewsLastDay ? UserViewsLastDay.ToString() : "",
                    MarkCurrent,
                    MarkLast,
                    Mark,
                    Compilation,
                    SiteUpdated.ToString("dd.MM.yyyy")
                };
                for (var i = 0; i < newData.Length; i++)
                    if (ListViewItem.SubItems[i].Text != newData[i])
                        ListViewItem.SubItems[i].Text = newData[i];
                if (ListViewItem.Name != newData[6])
                {
                    ListViewItem.Name = newData[6];
                    ListViewItem.ImageKey = newData[6];
                }

                if (ListViewItem.Text != newData[0])
                    ListViewItem.Text = newData[0];
                ListViewItem.Tag = this;
                ListViewItem.BackColor = isBlocked ? Color.IndianRed : SystemColors.Window;
            }
#if DEBUG
            catch (Exception ex)
            {
                Console.WriteLine("\tЭто нормально:\t{0:S}", ex.Message);
            }
# else
            catch (Exception)
            {
            }
#endif


            return ListViewItem;
        }

        public string ImageURLGet(string size = "")
        {
            switch (size)
            {
                case "small":
                case "large":
                    size += "/";
                    break;
                default:
                    size = "";
                    break;
            }

            return string.Format("http://cdn.seasonvar.ru/oblojka/{0}{1}.jpg", size, ID);
        }

        #region Player

        private Player LoadPlayer()
        {
            if (null == __player && 0 < ID && 0 < SerialID)
                __player = new Player(ID, SerialID, Secure, timeout);
            return __player;
        }

        #endregion

        #region Атрибуты

        public int ID { get; set; }

        public Player Player
        {
            get => LoadPlayer();
            set => __player = value;
        }

        public Dictionary<int, Playlist> Playlists => Player.Playlists;

        public bool ExistPlayer => null != LoadPlayer();

        public int SerialID
        {
            get => GetValueInt("serial_id");
            set => SetValue("serial_id", value);
        }

        public int SeasonNum
        {
            get => GetValueInt("season");
            set => SetValue("season", value);
        }

        public int UserComments
        {
            get => GetValueInt("user_comments");
            set => SetValue("user_comments", value);
        }

        public int UserViewsLastDay
        {
            get => GetValueInt("user_views_last_day");
            set
            {
                if (UserViewsLastDay < value)
                    SetValue("user_views_last_day", value);
            }
        }

        private int timeout
        {
            get
            {
                if (0 <= __timeout)
                {
                    var timeout = DB.Instance.OptionGet(string.Concat("cacheTimeSerial_" + Type));
                    if (string.IsNullOrEmpty(timeout))
                        timeout = DB.Instance.OptionGet(string.Concat("cacheTimeSerial_none"));
                    if (string.IsNullOrEmpty(timeout)) timeout = (24 * 60 * 60).ToString();
                    __timeout = IntVal(timeout);
                }

                return __timeout;
            }
        }

        public string Compilation
        {
            get => string.IsNullOrWhiteSpace(__compilation) ? "" : __compilation;
            set
            {
                if (__compilation != value && !string.IsNullOrWhiteSpace(value)) __compilation = value;
            }
        }

        public string Genre
        {
            get => GetValue("genre");
            set => SetValue("genre", value);
        }

        public string Country
        {
            get => GetValue("country");
            set => SetValue("country", value);
        }

        public string Release
        {
            get => GetValue("release");
            set => SetValue("release", value);
        }

        public float IMDB
        {
            get => GetValueFloat("imdb");
            set => SetValue("imdb", value);
        }

        public float KinoPoisk
        {
            get => GetValueFloat("kinopoisk");
            set => SetValue("kinopoisk", value);
        }

        public string Limitation
        {
            get => GetValue("limitation");
            set => SetValue("limitation", value);
        }

        public string URL
        {
            get => GetValue("url");
            set => SetValue("url", value);
        }

        public string Title
        {
            get => GetValue("title");
            set => SetValue("title", value);
        }

        public string TitleRU
        {
            get => GetValue("title_ru");
            set => SetValue("title_ru", value);
        }

        public string TitleEN
        {
            get => GetValue("title_en");
            set => SetValue("title_en", value);
        }

        public string TitleFull
        {
            get => GetValue("title_full");
            set => SetValue("title_full", value);
        }

        public string TitleOriginal
        {
            get => GetValue("title_original");
            set => SetValue("title_original", value);
        }

        public string Description
        {
            get => GetValue("description");
            set => SetValue("description", value);
        }

        public string MarkCurrent
        {
            get => GetValue("marks_current");
            set => SetValue("marks_current", value);
        }

        public string TypeOLD => GetValue("type_old");

        public string Type
        {
            get
            {
                var value = GetValue("type");
                if (!string.IsNullOrEmpty(value))
                    return value;
                return "none";
            }
            set
            {
                if ("none" != value)
                    SetValue("type", value);
                else
                    SetValueEmpty("type");
            }
        }

        public string MarkLast
        {
            get => GetValue("marks_last");
            set => SetValue("marks_last", value);
        }

        public string Mark
        {
            get
            {
                if ("-1" == MarkCurrent || "-2" == MarkCurrent || "-3" == MarkCurrent) return MarkLast;
                return string.Format("{0} из {1}", MarkCurrent, MarkLast);
            }
            set { }
        }

        public string Secure
        {
            get => GetValue("secure", Server.Instance.Secure);
            set
            {
                if (Server.Instance.Secure != value)
                    SetValue("secure", value);
                else
                    SetValueEmpty("secure");
            }
        }

        public string ImageSmall => ImageURLGet("small");

        public string Image => ImageURLGet();

        public string ImageLarge => ImageURLGet("large");

        public DateTime SiteUpdated
        {
            get => GetValueDate("site_updated", DB.DATE_FORMAT);
            set => SetValue("site_updated", value, DB.DATE_FORMAT);
        }

        public DateTime CreatedDate => GetValueDate("created_date");

        public DateTime UpdatedDate => GetValueDate("updated_date");

        public DateTime CachedDate
        {
            get => __cached_date != new DateTime() ? __cached_date : GetValueDate("cached_date");
            set => __cached_date = value;
        }

        public List<Season> Related
        {
            get
            {
                var items = new List<Season>();
                if (0 < __related.Count)
                    foreach (var id in __related)
                        items.Add(new Season(id));

                return items;
            }
        }

        public string ErrorMessage
        {
            get => GetValue("error");
            private set => SetValue("error", value);
        }

        public bool isBlocked
        {
            get => 0 < GetValueInt("blocked");
            private set => SetValue("blocked", value ? 1 : 0);
        }

        public List<string> OrderVideos
        {
            get
            {
                var data = new List<string>();
                if (0 < Playlists.Count)
                    if (1 == Playlists.Count)
                    {
                        var id = new List<int>(Playlists.Keys)[0];
                        data = new List<string>(Playlists[id].OrderVideos.Values);
                        if (data.Contains(""))
                            data.Remove("");
                    }
                    else
                    {
                        var _orders = new List<Dictionary<string, string>>();
                        foreach (var itemPlaylist in Playlists)
                            _orders.Add(itemPlaylist.Value.OrderVideos);
                        var _data = new Dictionary<string, List<string>>();
                        var indexLists = new List<string> {""};
                        var nextIndexLists = new List<string>();
                        var _indexListAll = new List<string> {""};
                        do
                        {
                            nextIndexLists = new List<string>();
                            foreach (var index in indexLists)
                            {
                                if (!_data.ContainsKey(index))
                                    _data[index] = new List<string>();
                                foreach (var item in _orders)
                                    if (item.ContainsKey(index) &&
                                        !_indexListAll.Contains(item[index]))
                                    {
                                        _indexListAll.Add(item[index]);
                                        if (!nextIndexLists.Contains(item[index]))
                                            nextIndexLists.Add(item[index]);
                                        if (!_data[index].Contains(item[index]))
                                            _data[index].Add(item[index]);
                                    }

                                _data[index].Sort();
                            }

                            nextIndexLists.Sort();
                            indexLists = nextIndexLists;
                        } while (0 < nextIndexLists.Count);

                        foreach (var item in _data)
                        foreach (var _item in item.Value)
                            if (!data.Contains(_item))
                                data.Add(_item);
                    }

                return data;
            }
        }

        #endregion

        #region Compilation

        public string CompilationGet()
        {
            var _type = Type;
            var __type = "new" == _type || "nonew" == _type || "want" == _type;
            if (string.IsNullOrEmpty(Compilation) && __type) Compilation = ServerCompilation.Instance.Get(SerialID);
            return Compilation;
        }

        public void CompilationSet(string name)
        {
            var _is = true;
            if (!string.IsNullOrEmpty(Compilation))
                _is = CompilationRemove();
            if (_is)
                Compilation = ServerCompilation.Instance.Set(SerialID, name);
        }

        public bool CompilationRemove()
        {
            return ServerCompilation.Instance.Remove(SerialID);
        }

        public bool CompilationRemove(int ID)
        {
            var result = ServerCompilation.Instance.Remove(SerialID, ID);
            return result;
        }

        public void CompilationRemove(string name)
        {
            if (ServerCompilation.Instance.Remove(SerialID, name) && Compilation == name)
            {
                Compilation = "";
                SaveAsync();
            }
        }

        private void CompilationLoad()
        {
            if (0 < SerialID)
            {
                Compilation = ServerCompilation.Instance.Get(SerialID);
                if (string.IsNullOrEmpty(Compilation))
                {
                    var _type = Type;
                    var __type = "new" == _type || "nonew" == _type || "want" == _type;
                    if (__type && !string.IsNullOrEmpty(Match(Genre, "анимационные", REGEX_IC)))
                    {
                        Compilation = ServerCompilation.Instance.Set(SerialID, "Animated");
                        return;
                    }

                    if (__type && !string.IsNullOrEmpty(Match(Genre, "аниме", REGEX_IC)))
                    {
                        Compilation = ServerCompilation.Instance.Set(SerialID, "Anime");
                        return;
                    }

                    switch (_type)
                    {
                        case "new":
                        case "nonew":
                            Compilation = ServerCompilation.Instance.Set(SerialID, "Active");
                            return;
                        case "want":
                            Compilation = ServerCompilation.Instance.Set(SerialID, "Want");
                            return;
                    }
                }
            }
        }

        #endregion

        #region Парсинг

        public void parseSidebar(string html)
        {
            var text_serial = Match(html, "<div class=\"rside-ss\">(.*?)</div>", REGEX_ICS, 1);
            if (!string.IsNullOrEmpty(text_serial))
            {
                var matchseason = Matches(text_serial, "^(.*?)<span>([^<>]*)</span>$", REGEX_IC);
                if (3 <= matchseason.Count)
                {
                    SeasonNum = IntVal(matchseason[1]);
                    MarkLast = Regex.Replace(matchseason[2], " (\\([^\\(\\)]+\\)|серия)", "")
                        .Replace("Только трейлер", "");
                }
            }

            TitleRU = Match(html, "<div class=\"rside-t\">(.*?)</div>", REGEX_ICS, 1);
            TitleEN = Match(html, "<div class=\"rside-t_en\">(.*?)</div>", REGEX_ICS, 1);
            SaveAsync();
        }

        public void parsePause(string html)
        {
            SeasonNum = IntVal(Match(html, "<div class=\"pgs-marks-seas\">(.*?)</div>", REGEX_ICS, 1));
            TitleRU = Match(html, "<div class=\"pgs-marks-name\">(.*?)</div>", REGEX_ICS, 1);
            MarkLast = Match(html, "<div class=\"pgs-marks-current\">[^<>]*<strong>(.*?)</strong>[^<>]*</div>",
                REGEX_ICS, 1).Replace("Только трейлер", "");
            SiteUpdated = DateVal(Match(html, "<div class=\"pgs-marks-mess\">(.*?)</div>", REGEX_ICS, 1), "dd.MM.yyyy");
        }

        private void parsePage(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                Logger.Instance.Warn("Остановка парсинга!!! Страница небыла полученна!");
                return;
            }
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            var _html = Match(html, "data-id-serial=\"([0-9]+)\"", REGEX_ICS, 1);
            SerialID = IntVal(_html);
            _html = Match(html, "data-id-season=\"([0-9]+)\"", REGEX_ICS, 1);
            ID = IntVal(_html);
            _html = Match(html, "<p itemprop=\"description\">(.*?)</p>", REGEX_ICS, 1);
            Description = Regex.Replace(WebUtility.HtmlDecode(_html),
                "<br[^<>]*>", "\r\n", REGEX_ICS).Replace("\r\n\r\n", "\r\n").Trim();
            _html = Match(html, "data4play = ({.*?})", REGEX_ICS, 1);
            Secure = _parseData4Play(_html);
            _html = Match(html, "mark = ({.*?})", REGEX_ICS, 1);
            _parseMark(_html);
            _html = Match(html, "<div[^<>]*class=\"[^\">]*pgs-player-block[^\">]*\"[^<>]*>(.*?)</div>", REGEX_ICS, 1);
            _html = Regex.Replace(_html, "<[^<>]*>(.*?)<[^<>]*>", "", REGEX_ICS).Trim()
                .Replace("Ошибка при загрузке плеера, попробуйте перезагрузить страницу", "");
            ErrorMessage = _html;
            isBlocked = "" != Match(ErrorMessage, "заблокирован", REGEX_ICS);
            _html = Match(html, "<meta itemprop=\"dateModified\" content=\"([^\"+]+)([^\"]+)\">", REGEX_ICS, 1);
            SiteUpdated = DateVal(_html, "yyyy-MM-ddTHH:mm:ss");
            _html = Match(html, "<meta itemprop=\"interactionCount\" content=\"UserComments:([0-9]+)\"/>",
                REGEX_ICS, 1);
            UserComments = IntVal(_html);
            _html = Match(html,
                "<meta itemprop=\"interactionCount\" content=\"UserViewsLastDay:([0-9]+)\"/>", REGEX_ICS, 1);
            UserViewsLastDay = IntVal(_html);

            _html = Match(html, "<h1 class=\"pgs-sinfo-title\"[^<>]*>(.*?)</h1>", REGEX_ICS, 1);
            _parseTitle(_html, this);
            //_parseTags(Match(html, "<div data-tag=\"wrap\">(.*?)</div>", REGEX_ICS, 1));
            _parseMetaInfo(html);
            _html = Match(html, "<div class=\"pgs-seaslist\">(.*?)</div>", REGEX_ICS, 1);
            _parseSeasons(_html);
            _html = Match(html, "<div class=\"b-labellist\">(.*?)</div>", REGEX_ICS, 1);
            _parseCompilation(_html);
            _parseRelatedAsync(html);

            CompilationLoad();
            SaveAsync();
#if DEBUG
            Console.WriteLine("\tParse Season\t{0}\t{1}:\t{2}", SerialID, ID,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
        }

        private string _parseData4Play(string html)
        {
            html = Regex.Replace(html, "[\r\n\t ]+", "");
            if (string.IsNullOrEmpty(html))
            {
                Logger.Instance.Warn("Данные для плеера не найдены!!! Сериал заблокирован?");
                return "";
            }

            html = Regex.Replace(html, "'", "\"");
            var Data4Play = SimpleJson.SimpleJson.DeserializeObject<Data4play>(html);
            return Data4Play.secureMark;
        }

        private void _parseMark(string html)
        {
            html = Regex.Replace(html, "[\r\n\t ]+", "");
            if (!string.IsNullOrEmpty(html))
            {
                html = Regex.Replace(html, "'", "\"");
                var mark = SimpleJson.SimpleJson.DeserializeObject<Mark>(html);
                var matchurl = Regex.Match(mark.href, "^(.*?)#rewind=(.*?)_seriya$", REGEX_IC);
                if (matchurl.Success)
                {
                    URL = matchurl.Groups[1].ToString();
                    MarkCurrent = matchurl.Groups[2].ToString();
                    switch (MarkCurrent)
                    {
                        case "-1":
                            Type = "want";
                            break;
                        case "-2":
                            Type = "watched";
                            break;
                        case "-3":
                            Type = "notwatch";
                            break;
                    }
                }
                else
                {
                    Type = "none";
                    URL = mark.href;
                }
            }
        }

        private void _parseTitle(string html, Season _o)
        {
            html = html.Trim();
            if (string.IsNullOrEmpty(html))
                return;

            html = Regex.Replace(WebUtility.HtmlDecode(html), "^>>> ", "");
            html = Regex.Replace(html, "<[^<>]+>[^<>]+</[^<>]+>", "").Trim();
            html = Regex.Replace(html, "(^сериал | онлайн$)", "", REGEX_IC);
            html = Regex.Replace(html, "[ ]{2,}", " ", REGEX_IC);
            _o.TitleFull = html.Trim();
            _o.SeasonNum = IntVal(Match(_o.TitleFull, "([0-9]+) сезон$", REGEX_ICS, 1));
            html = Regex.Replace(_o.TitleFull, " [0-9]+ сезон$", "", REGEX_IC).Trim();
            _o.Title = html.Trim();
            if (!string.IsNullOrEmpty(_o.TitleRU))
            {
                html = Regex.Replace(_o.Title.Replace(_o.TitleRU, ""), "(^/|/$)", "");
                if (html != _o.TitleRU) _o.TitleEN = html.Trim();
            }
            else
            {
                var sp_html = _o.Title.Split('/');
                if (2 == sp_html.Length)
                {
                    _o.TitleRU = sp_html[0].Trim();
                    _o.TitleEN = sp_html[1].Trim();
                }
                else if (2 < sp_html.Length)
                {
                    Logger.Instance.Warn("Не возможно разделить название сериала: {0:S}", _o.Title);
                }
            }
        }

        private void _parseMetaInfo(string html)
        {
            if (string.IsNullOrEmpty(html))
                return;
            var _MetaInfo = new Dictionary<string, string>();
            foreach (Match matchInfo in Regex.Matches(html, "<div class=\"pgs-sinfo_list[^\"]*\">(.*?)</div>",
                REGEX_ICS))
            {
                var info = matchInfo.Groups[1].ToString().Trim();
                info = Regex.Replace(info, "</span>", "||", REGEX_ICS);
                info = Regex.Replace(info, "(<[^<>]*>|\n|\r|\t)", " ", REGEX_ICS);
                info = Regex.Replace(info, "[ ]{2,}", " ", REGEX_ICS);
                foreach (Match _matchInfo in Regex.Matches(info, "([^:\\|]+?):([^\\|]+?)\\|\\|", REGEX_ICS))
                {
                    var name = _matchInfo.Groups[1].ToString().Trim();
                    var value = _matchInfo.Groups[2].ToString().Trim();
                    if (!string.IsNullOrEmpty(value))
                        switch (name)
                        {
                            case "Оригинал":
                                TitleOriginal = value;
                                break;
                            case "Жанр":
                                Genre = value;
                                break;
                            case "Страна":
                                Country = value;
                                break;
                            case "Ориентировочная дата выхода":
                            case "Вышел":
                                Release = value;
                                break;
                            case "Ограничение":
                                Limitation = value;
                                break;
                            case "IMDB":
                                IMDB = floatVal(value);
                                break;
                            case "КиноПоиск":
                                KinoPoisk = floatVal(value);
                                break;
                            default:
                                _MetaInfo[name] = value;
                                break;
                        }
                }
            }
        }

        private void _parseRelated(string html)
        {
            if (string.IsNullOrEmpty(html))
                return;
            var _related = new List<int>();
            foreach (Match matchInfo in Regex.Matches(html,
                "<a href=\"/([^\"]+)\" data-id=\"([0-9]+)\" class=\"pst\">(.*?)</a>", REGEX_ICS))
            {
                var url = SERVER_URL + "/" + matchInfo.Groups[1].ToString().Trim();
                if (URL == url) continue;
                var season = new Season(url)
                {
                    TitleRU = Match(matchInfo.Groups[3].ToString(), "<div>(.+?)</div>", REGEX_ICS, 1)
                };
                season.SaveAsync();
                if (!_related.Contains(season.ID)) _related.Add(season.ID);
            }

            var _relatedOld = __related;
            var _relatedNew = _related;
            __related = _related;
            if (0 < _related.Count)
            {
                if (0 < _relatedOld.Count)
                    for (var i = _relatedOld.Count - 1; i >= 0; i--)
                        if (_relatedNew.Contains(_relatedOld[i]))
                        {
                            _relatedNew.Remove(_relatedOld[i]);
                            _relatedOld.RemoveAt(i);
                        }

                __needSaveRelated = __needSaveRelated || 0 < _relatedNew.Count || 0 < _relatedOld.Count;
            }
        }

        //private void _parseTags(string html)
        //{
        //    // TODO: Посмотреть насколько востребованы теги
        //    if (string.IsNullOrEmpty(html))
        //        return;
        //    foreach (Match matchTag in Regex.Matches(html, "<a[^<>]*>(.*?)</a>", REGEX_ICS))
        //    {
        //        var _html = matchTag.Groups[1].ToString();
        //        var __html = Match(html, "data-tagid=\"([0-9]+)\"", REGEX_ICS, 1);
        //        _html = Regex.Replace(_html, "<[^<>]+>[^<>]+</[^<>]+>", "").Trim();
        //        if (!string.IsNullOrEmpty(__html) && !string.IsNullOrEmpty(_html))
        //        {
        //            var id = int.Parse(__html);
        //            Tags[id] = _html;
        //        }
        //    }
        //}

        private void _parseSeasons(string html)
        {
            if (string.IsNullOrEmpty(html))
                return;
            var i = 0;
            foreach (Match matchSeason in Regex.Matches(html, "<a[^<>]*href=\"([^<>\"]*)\"[^<>]*>(.*?)</a>", REGEX_ICS))
            {
                var url = SERVER_URL + matchSeason.Groups[1];
                if (URL == url) continue;
                var season = new Season(url);
                if (string.IsNullOrEmpty(season.Title) ||
                    string.IsNullOrEmpty(season.TitleRU) ||
                    string.IsNullOrEmpty(season.TitleEN) ||
                    string.IsNullOrEmpty(season.TitleFull)
                )
                    _parseTitle(matchSeason.Groups[2].ToString().Trim(), season);
                if (string.IsNullOrEmpty(season.Title)) season.Title = Title;
                if (string.IsNullOrEmpty(season.TitleRU)) season.TitleRU = TitleRU;
                if (string.IsNullOrEmpty(season.TitleEN)) season.TitleEN = TitleEN;
                if (0 == season.SerialID) season.SerialID = SerialID;
                season.SaveAsync();
                if (0 < season.SeasonNum)
                {
                    Seasons[season.SeasonNum] = season.ID;
                }
                else
                {
                    i--;
                    Seasons[i] = season.ID;
                }
            }
        }

        private void _parseCompilation(string html)
        {
            if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(Compilation))
                return;
            var data = new Dictionary<int, string>();
            foreach (Match matchCompilation in Regex.Matches(html, "<label>(.*?)</label>", REGEX_ICS))
            {
                var _html = matchCompilation.Groups[1].ToString().Trim();
                var _checked = "checked" == Match(_html, "checked", REGEX_IC);
                var id = IntVal(Match(_html, "value=\"([0-9]+)\"", REGEX_IC, 1));
                var name = Match(_html, "<span>(.*?)</span>", REGEX_IC, 1);
                data[id] = name;
                if (_checked)
                    ServerCompilation.Instance.RelationUpdate(SerialID, id);
            }

            ServerCompilation.Instance.CompilationUpdate(data);
        }

        private void _parseRelatedAsync(string html)
        {
            Task.Run(() =>
            {
                try
                {
                    _parseRelated(html);
                    _saveRelated();
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            });
        }

        public void _saveRelated()
        {
            if (__needSaveRelated)
            {
#if DEBUG
                var start = DateTime.UtcNow;
#endif
                DB.Instance.RelatedSet(SerialID, __related);
                __needSaveRelated = false;
#if DEBUG
                Console.WriteLine("\tSave Related\t{0}\t{1}:\t{2}", SerialID, ID,
                    DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
            }
        }

        #endregion

        #region Синхронизация

        public void syncPage(bool forsed = false)
        {
            forsed = forsed || __forsed_update_page;
            if (timeout < DateTime.UtcNow.Subtract(CachedDate).TotalSeconds || forsed)
            {
                var content = downloadPage(forsed);
                parsePage(content);
                if (!string.IsNullOrWhiteSpace(content))
                    __forsed_update_page = false;
            }
        }

        public void syncPlayer(bool forsed = false)
        {
            forsed = forsed || __forsed_update_player;
            if (ExistPlayer && Player.sync(forsed))
                __forsed_update_player = false;
        }

        public void syncPlaylists(bool forsed = false)
        {
            forsed = forsed || __forsed_update_playlist;
            if (ExistPlayer && Player.syncPlaylist(forsed))
                __forsed_update_playlist = false;
        }

        #endregion

        #region Mark

        public void MarkSetPause(string seria = "")
        {
            if (MarkCurrent == seria)
                return;
            if (MarkDo("pauseadd", seria))
            {
                MarkCurrent = seria;
                Type = seria == MarkLast ? "nonew" : "new";
                SaveAsync();
                DB.Instance.OptionSetAsync("needListUpdate", "1");
            }
        }

        public void MarkSetLast()
        {
            if (Type == "nonew")
                return;
            MarkSetPause(MarkLast);
        }

        public void MarkSeWantToSee()
        {
            if (Type == "want")
                return;
            if (MarkDo("wanttosee"))
            {
                Type = "want";
                SaveAsync();
                DB.Instance.OptionSetAsync("needListUpdate", "1");
            }
        }

        public void MarkSetWatched()
        {
            if (Type == "watched")
                return;
            if (MarkDo("watched"))
            {
                Type = "watched";
                SaveAsync();
                DB.Instance.OptionSetAsync("needListUpdate", "1");
            }
        }

        public void MarkSetNotWatched()
        {
            if (Type == "notwatch")
                return;
            if (MarkDo("notWatched"))
            {
                Type = "notwatch";
                SaveAsync();
                CompilationRemove();
                DB.Instance.OptionSetAsync("needListUpdate", "1");
            }
        }

        public void MarkSetDelSee()
        {
            if (Type == "none")
                return;
            if (MarkDo("delete"))
            {
                Type = "none";
                SaveAsync();
                DB.Instance.OptionSetAsync("needListUpdate", "1");
            }
        }

        private bool MarkDo(string type = "", string seria = "")
        {
            if (0 == ID)
                return false;
            NameValueCollection postData;
            switch (type)
            {
                case "pauseadd":
                    postData = new NameValueCollection
                    {
                        {"id", ID.ToString()}, {"seria", seria}, {"pauseadd", "true"}, {"minute", "0"},
                        {"second", "0"}, {"tran", "0"}
                    };
                    break;
                case "wanttosee":
                    postData = new NameValueCollection
                    {
                        {"id", ID.ToString()}, {"seria", "-1"}, {"wanttosee", "true"}, {"minute", "0"},
                        {"second", "0"}
                    };
                    break;
                case "watched":
                    postData = new NameValueCollection
                    {
                        {"id", ID.ToString()}, {"seria", "-2"}, {"watched", "true"}, {"minute", "0"},
                        {"second", "0"}
                    };
                    break;
                case "notWatched":
                    postData = new NameValueCollection
                    {
                        {"id", ID.ToString()}, {"seria", "-3"}, {"notWatched", "true"}, {"minute", "0"},
                        {"second", "0"}
                    };
                    break;

                case "delete":
                default:
                    postData = new NameValueCollection {{"delId", ID.ToString()}};
                    break;
            }

            return Server.Instance.doMarks(postData).Status();
        }

        #endregion
    }
}