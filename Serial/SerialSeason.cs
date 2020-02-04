﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeTheater.Helper;
using HomeTheater.Serial.data;

namespace HomeTheater.Serial
{
    internal class SerialSeason : SerialParent
    {
        private DateTime __cached_date;
        private string __compilation;
        private bool __forsed_update_page;
        private bool __forsed_update_player;
        private bool __forsed_update_playlist;
        private bool __needSaveRelated;
        private SerialSeasonPlayer __player;
        private List<int> __related = new List<int>();
        private Dictionary<int, int> __seasons = new Dictionary<int, int>();
        private string __secure_mark = "";
        private int __timeout;

        public ListViewItem ListViewItem = new ListViewItem(new[]
            {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""});

        public Dictionary<int, string> Tags = new Dictionary<int, string>();

        public SerialSeason(string url)
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

        public SerialSeason(int id)
        {
            ID = id;
            Init();
        }

        public int ID { get; set; }

        public SerialSeasonPlayer Player
        {
            get => LoadPlayer();
            set => __player = value;
        }

        public bool ExistPlayer => null != LoadPlayer();

        public int SerialID
        {
            get => getValueInt("serial_id");
            set => setValue("serial_id", value);
        }

        public int Season
        {
            get => getValueInt("season");
            set => setValue("season", value);
        }

        public int UserComments
        {
            get => getValueInt("user_comments");
            set => setValue("user_comments", value);
        }

        public int UserViewsLastDay
        {
            get => getValueInt("user_views_last_day");
            set => setValue("user_views_last_day", value);
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
            get => getValue("genre");
            set => setValue("genre", value);
        }

        public string Country
        {
            get => getValue("country");
            set => setValue("country", value);
        }

        public string Release
        {
            get => getValue("release");
            set => setValue("release", value);
        }

        public float IMDB
        {
            get => getValueFloat("imdb");
            set => setValue("imdb", value);
        }

        public float KinoPoisk
        {
            get => getValueFloat("kinopoisk");
            set => setValue("kinopoisk", value);
        }

        public string Limitation
        {
            get => getValue("limitation");
            set => setValue("limitation", value);
        }

        public string URL
        {
            get => getValue("url");
            set => setValue("url", value);
        }

        public string Title
        {
            get => getValue("title");
            set => setValue("title", value);
        }

        public string TitleRU
        {
            get => getValue("title_ru");
            set => setValue("title_ru", value);
        }

        public string TitleEN
        {
            get => getValue("title_en");
            set => setValue("title_en", value);
        }

        public string TitleFull
        {
            get => getValue("title_full");
            set => setValue("title_full", value);
        }

        public string TitleOriginal
        {
            get => getValue("title_original");
            set => setValue("title_original", value);
        }

        public string Description
        {
            get => getValue("description");
            set => setValue("description", value);
        }

        public string MarkCurrent
        {
            get => getValue("marks_current");
            set => setValue("marks_current", value);
        }

        public string TypeOLD => getValue("type_old");

        public string Type
        {
            get
            {
                var value = getValue("type");
                if (!string.IsNullOrEmpty(value))
                    return value;
                return "none";
            }
            set
            {
                if (TypeOLD != value && "none" != value)
                    setValue("type", value);
            }
        }

        public string MarkLast
        {
            get => getValue("marks_last");
            set => setValue("marks_last", value);
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
            get => getValue("secure");
            set => setValue("secure", value);
        }

        public string ImageSmall => ImageURLGet("small");

        public string Image => ImageURLGet();

        public string ImageLarge => ImageURLGet("large");

        public DateTime SiteUpdated
        {
            get => getValueDate("site_updated", DB.DATE_FORMAT);
            set => setValue("site_updated", value, DB.DATE_FORMAT);
        }

        public DateTime CreatedDate => getValueDate("created_date");

        public DateTime UpdatedDate => getValueDate("updated_date");

        public DateTime CachedDate
        {
            get => __cached_date != new DateTime() ? __cached_date : getValueDate("cached_date");
            set => __cached_date = value;
        }

        public List<SerialSeason> Related
        {
            get
            {
                var items = new List<SerialSeason>();
                if (0 < __related.Count)
                    foreach (var id in __related)
                        items.Add(new SerialSeason(id));

                return items;
            }
            set { }
        }

        protected override void callbackValue(string name, string value)
        {
            switch (name)
            {
                case "marks_last":
                    __forsed_update_playlist = __forsed_update_player = true;
                    break;
                case "site_updated":
                    __forsed_update_playlist = __forsed_update_player = __forsed_update_page = true;
                    break;
            }
        }

        private void Init()
        {
            Load();
            CompilationLoad();
        }

        private SerialSeasonPlayer LoadPlayer()
        {
            if (null == __player && 0 < ID && 0 < SerialID)
                __player = new SerialSeasonPlayer(ID, SerialID, Secure, timeout);
            return __player;
        }

        public string CompilationGet()
        {
            var _type = Type;
            var __type = "new" == _type || "nonew" == _type || "want" == _type;
            if (string.IsNullOrEmpty(Compilation) && __type) Compilation = APIServerCompilation.Instance.Get(SerialID);
            return Compilation;
        }

        public void CompilationSet(string name)
        {
            var _is = true;
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
            var result = APIServerCompilation.Instance.Remove(SerialID, ID);
            return result;
        }

        public void CompilationRemove(string name)
        {
            if (APIServerCompilation.Instance.Remove(SerialID, name) && Compilation == name)
            {
                Compilation = "";
                SaveAsync();
            }
        }

        private void CompilationLoad()
        {
            if (0 < SerialID)
            {
                Compilation = APIServerCompilation.Instance.Get(SerialID);
                if (string.IsNullOrEmpty(Compilation))
                {
                    var _type = Type;
                    var __type = "new" == _type || "nonew" == _type || "want" == _type;
                    if (__type && !string.IsNullOrEmpty(Match(Genre, "анимационные", REGEX_IC)))
                    {
                        Compilation = APIServerCompilation.Instance.Set(SerialID, "Animated");
                        return;
                    }

                    if (__type && !string.IsNullOrEmpty(Match(Genre, "аниме", REGEX_IC)))
                    {
                        Compilation = APIServerCompilation.Instance.Set(SerialID, "Anime");
                        return;
                    }

                    switch (_type)
                    {
                        case "new":
                        case "nonew":
                            Compilation = APIServerCompilation.Instance.Set(SerialID, "Active");
                            return;
                        case "want":
                            Compilation = APIServerCompilation.Instance.Set(SerialID, "Want");
                            return;
                    }
                }
            }
        }

        public SerialSeason Load()
        {
            LoadValues(() =>
            {
                var data = DB.Instance.SeasonGet(ID);
                if (data.ContainsKey("type")) data.Add("type_old", data["type"]);

                return data;
            });
            __seasons = DB.Instance.SeasonsGet(SerialID, ID);
            __related = DB.Instance.RelatedGet(ID);
            ToListViewItem();

            return this;
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

        public async void SaveAsync()
        {
            await Task.Run(() => { Save(); });
        }

        public void parseSidebar(string html)
        {
            var text_serial = Match(html, "<div class=\"rside-ss\">(.*?)</div>", REGEX_ICS, 1);
            if (!string.IsNullOrEmpty(text_serial))
            {
                var matchseason = Matches(text_serial, "^(.*?)<span>([^<>]*)</span>$", REGEX_IC);
                if (3 <= matchseason.Count)
                {
                    Season = IntVal(matchseason[1]);
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
            Season = IntVal(Match(html, "<div class=\"pgs-marks-seas\">(.*?)</div>", REGEX_ICS, 1));
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
            SerialID = IntVal(Match(html, "data-id-serial=\"([0-9]+)\"", REGEX_ICS, 1));
            ID = IntVal(Match(html, "data-id-season=\"([0-9]+)\"", REGEX_ICS, 1));
            Description = Regex
                .Replace(WebUtility.HtmlDecode(Match(html, "<p itemprop=\"description\">(.*?)</p>", REGEX_ICS, 1)),
                    "<br[^<>]*>", "\r\n", REGEX_ICS).Replace("\r\n\r\n", "\r\n").Trim();
            Secure = _parseData4Play(Match(html, "data4play = ({.*?})", REGEX_ICS, 1));
            SiteUpdated =
                DateVal(Match(html, "<meta itemprop=\"dateModified\" content=\"([^\"+]+)([^\"]+)\">", REGEX_ICS, 1),
                    "yyyy-MM-ddTHH:mm:ss");
            UserComments = IntVal(Match(html, "<meta itemprop=\"interactionCount\" content=\"UserComments:([0-9]+)\"/>",
                REGEX_ICS, 1));
            UserViewsLastDay = IntVal(Match(html,
                "<meta itemprop=\"interactionCount\" content=\"UserViewsLastDay:([0-9]+)\"/>", REGEX_ICS, 1));


            _parseTitle(Match(html, "<h1 class=\"pgs-sinfo-title\"[^<>]*>(.*?)</h1>", REGEX_ICS, 1), this);
            //_parseTags(Match(html, "<div data-tag=\"wrap\">(.*?)</div>", REGEX_ICS, 1));
            _parseMetaInfo(html);
            _parseSeasons(Match(html, "<div class=\"pgs-seaslist\">(.*?)</div>", REGEX_ICS, 1));
            _parseCompilation(Match(html, "<div class=\"b-labellist\">(.*?)</div>", REGEX_ICS, 1));
            _parseRelatedAsync(html);

            CompilationLoad();
            SaveAsync();
#if DEBUG
            Console.WriteLine("\tParse Season\t{0}\t{1}:\t{2}", SerialID, ID,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
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

        private string _parseData4Play(string html)
        {
            html = Regex.Replace(html, "[\r\n\t ]+", "");
            if (string.IsNullOrEmpty(html))
            {
                Logger.Instance.Warn("Данные для плеера не найдены!!! Сериал заблокирован?");
                return "";
            }

            html = Regex.Replace(html, "'", "\"");
            var Data4Play = SimpleJson.SimpleJson.DeserializeObject<data4play>(html);
            return Data4Play.secureMark;
        }

        private void _parseTitle(string html, SerialSeason _o)
        {
            html = html.Trim();
            if (string.IsNullOrEmpty(html))
                return;

            html = Regex.Replace(WebUtility.HtmlDecode(html), "^>>> ", "");
            html = Regex.Replace(html, "<[^<>]+>[^<>]+</[^<>]+>", "").Trim();
            html = Regex.Replace(html, "(^сериал | онлайн$)", "", REGEX_IC);
            html = Regex.Replace(html, "[ ]{2,}", " ", REGEX_IC);
            _o.TitleFull = html.Trim();
            _o.Season = IntVal(Match(_o.TitleFull, "([0-9]+) сезон$", REGEX_ICS, 1));
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
                        }
                }
            }
        }

        private async void _parseRelatedAsync(string html)
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
                var season = new SerialSeason(url);
                season.TitleRU = Match(matchInfo.Groups[3].ToString(), "<div>(.+?)</div>", REGEX_ICS, 1);
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

        private void _parseTags(string html)
        {
            // TODO: Посмотреть насколько востребованы теги
            if (string.IsNullOrEmpty(html))
                return;
            foreach (Match matchTag in Regex.Matches(html, "<a[^<>]*>(.*?)</a>", REGEX_ICS))
            {
                var _html = matchTag.Groups[1].ToString();
                var __html = Match(html, "data-tagid=\"([0-9]+)\"", REGEX_ICS, 1);
                _html = Regex.Replace(_html, "<[^<>]+>[^<>]+</[^<>]+>", "").Trim();
                if (!string.IsNullOrEmpty(__html) && !string.IsNullOrEmpty(_html))
                {
                    var id = int.Parse(__html);
                    if (Tags.ContainsKey(id)) Tags.Add(id, _html);
                }
            }
        }

        private void _parseSeasons(string html)
        {
            if (string.IsNullOrEmpty(html))
                return;
            var i = 0;
            foreach (Match matchSeason in Regex.Matches(html, "<a[^<>]*href=\"([^<>\"]*)\"[^<>]*>(.*?)</a>", REGEX_ICS))
            {
                var url = SERVER_URL + matchSeason.Groups[1];
                if (URL == url) continue;
                var season = new SerialSeason(url);
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
                if (0 < season.Season)
                {
                    if (!__seasons.ContainsKey(season.Season))
                        __seasons.Add(season.Season, season.ID);
                }
                else
                {
                    i--;
                    if (!__seasons.ContainsKey(i))
                        __seasons.Add(i, season.ID);
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
                if (!data.ContainsKey(id))
                    data.Add(id, name);
                if (_checked)
                    APIServerCompilation.Instance.RelationUpdate(SerialID, id);
            }

            APIServerCompilation.Instance.CompilationUpdate(data);
        }

        public string downloadPage(bool forsed = false)
        {
            var cacheItem = APIServer.Instance.downloadPage(URL, forsed, timeout);
            CachedDate = cacheItem.date;

            return cacheItem.ToString();
        }

        public void syncPlayer(bool forsed = false)
        {
            forsed = forsed || __forsed_update_player;
            if (forsed)
                if (ExistPlayer && Player.sync(forsed))
                    __forsed_update_player = false;
        }

        public void syncPlaylists(bool forsed = false)
        {
            forsed = forsed || __forsed_update_playlist;
            if (ExistPlayer && Player.syncPlaylist(forsed))
                __forsed_update_playlist = false;
        }

        public ListViewItem ToListViewItem()
        {
            try
            {
                string[] newData =
                {
                    TitleFull,
                    Title,
                    TitleRU,
                    TitleEN,
                    TitleOriginal,
                    URL,
                    ID.ToString(),
                    SerialID.ToString(),
                    0 < Season ? Season.ToString() : "",
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
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine("\tЭто нормально:\t{0:S}", ex.Message);
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

            return string.Format("http://cdn.seasonvar.ru/oblojka/{0}{1}.jpg", size, ID);
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
            if (0 == ID)
                return false;
            NameValueCollection postData = null;
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

            return APIServer.Instance.doMarks(postData).Status();
        }
    }
}