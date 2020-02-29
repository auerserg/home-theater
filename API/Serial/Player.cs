using HomeTheater.Helper;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HomeTheater.API.Serial
{
    public class Player : Abstract.Serial
    {
        public const int TRAILERS_ID = 68;
        private DateTime __cache_date;
        private string __secure = "";

        public Dictionary<int, Playlist>
            Playlists = new Dictionary<int, Playlist>();

        public Playlist Trailers;

        public int SeasonID, SerialID, timeout;

        public Player(int SeasonID, int SerialID, string Secure = "", int timeout = 60 * 60 * 24 * 10)
        {
            this.SeasonID = SeasonID;
            this.SerialID = SerialID;
            this.Secure = Secure;
            this.timeout = timeout;
            Load();
        }

        public Player(int SeasonID, int SerialID, int timeout = 60 * 60 * 24 * 10)
        {
            this.SeasonID = SeasonID;
            this.SerialID = SerialID;
            this.timeout = timeout;
            Load();
        }

        public string Secure
        {
            get => __secure;
            set
            {
                if (__secure != value && !string.IsNullOrWhiteSpace(value))
                    __secure = value;
            }
        }

        public void Load()
        {
            var cache = DB.Instance.CacheGet(Server.Instance.downloadPlayerCacheURL(SeasonID, SerialID), timeout);
            __cache_date = cache.date;
            var data = DB.Instance.PlaylistGets(SeasonID);
            foreach (var item in data)
            {
                var id = IntVal(item["translate_id"]);
                if (TRAILERS_ID == id)
                    Trailers = new Playlist(id, SeasonID, item, timeout);
                else if (!Playlists.ContainsKey(id))
                    Playlists.Add(id, new Playlist(id, SeasonID, item, timeout));
            }
        }

        private string download(bool forsed)
        {
            var cacheItem = Server.Instance.downloadPlayer(SeasonID, SerialID, Secure, forsed, timeout);
            if (cacheItem.isNew && cacheItem.data.ContainsKey("secure"))
            {
                __cache_date = cacheItem.date;
                Secure = cacheItem.data["secure"];
            }

            return cacheItem.ToString();
        }

        #region Синхронизация

        public bool sync(bool forsed = false)
        {
            if (timeout < DateTime.UtcNow.Subtract(__cache_date).TotalSeconds || 0 == Playlists.Count || forsed)
            {
                var content = download(forsed);
                parse(content);
                return string.IsNullOrEmpty(content);
            }

            return false;
        }

        public bool syncPlaylist(bool forsed = false)
        {
            if (0 == Playlists.Count)
                sync();
            var result = true;
            Parallel.ForEach(Playlists, item =>
            {
                try
                {
                    var _result = item.Value.sync(forsed);
                    result = result && _result;
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }

            });
            if (null != Trailers)
            {
                var _result = Trailers.sync(forsed);
                result = result && _result;
            }

            return result;
        }

        #endregion

        #region Парсинг

        private void parse(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return;
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            var js = _getOnlyJS(html);
            Playlists = _parseScripts(js);
            if (Playlists.ContainsKey(TRAILERS_ID))
            {
                Trailers = Playlists[TRAILERS_ID];
                Playlists.Remove(TRAILERS_ID);
            }
            js = Match(js, "var arEpisodes = ([[{].*?[]}]);", REGEX_ICS, 1);
            _parseSeries(js);

            html = Match(html, "<ul class=\"pgs-trans\">(.*?)</ul>", REGEX_ICS, 1);
            _parseTranslate(html);

            Parallel.ForEach(Playlists, item => item.Value.SaveAsync());
            DB.Instance.PlaylistSetOld(SeasonID, new List<int>(Playlists.Keys));
#if DEBUG
            Console.WriteLine("\tParse Player\t{0}\t{1}:\t{2}", SerialID, SeasonID,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
        }

        private void _parseSeries(string js)
        {
            if (string.IsNullOrWhiteSpace(js))
            {
#if DEBUG
                Console.WriteLine("Не найден список эпизодов\t{0}\t{1}", SerialID, SeasonID);
#endif
                return;
            }

            try
            {
                var _data = SimpleJson.SimpleJson
                    .DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(js);
                if (null != _data)
                {
                    Parallel.ForEach(_data, item =>
                    {
                        try
                        {
                            var id = IntVal(item.Key);
                            if (TRAILERS_ID == id)
                            {
                                if (null == Trailers)
                                    Trailers = new Playlist(id, SeasonID, Secure, timeout);
                                Trailers.tempOrderVideo(item.Value);
                            }
                            else
                            {
                                if (!Playlists.ContainsKey(id))
                                    Playlists.Add(id, new Playlist(id, SeasonID, Secure, timeout));
                                Playlists[id].tempOrderVideo(item.Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.Error(ex);
                        }

                    });
                }
                else
                {
                    var __data = SimpleJson.SimpleJson
                        .DeserializeObject<List<Dictionary<string, Dictionary<string, string>>>>(js);
                    var id = 0;
                    if (!Playlists.ContainsKey(id))
                        Playlists.Add(id, new Playlist(id, SeasonID, Secure, timeout));
                    Playlists[id].tempOrderVideo(__data[id]);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }
        }

        private string _getOnlyJS(string html)
        {
            var js = "";
            foreach (Match match in Regex.Matches(html, "<script[^<>]*>(.*?)</script>", REGEX_ICS))
            {
                var _js = match.Groups[1].ToString().Trim();
                if (!string.IsNullOrWhiteSpace(_js))
                    js += _js + "\n";
            }

            return js;
        }

        private Dictionary<int, Playlist> _parseScripts(string js)
        {
            var data = _parseStartPlayList(js);
            if (string.IsNullOrWhiteSpace(js))
                return data;
            foreach (Match match in Regex.Matches(js, "pl([0-9[]+)] = \"(.+?)\";", REGEX_ICS))
            {
                var id = IntVal(match.Groups[1].ToString());
                var url = SERVER_URL + match.Groups[2];
                if (data.ContainsKey(id))
                    data[id].URL = url;
                else if (Playlists.ContainsKey(id))
                {
                    data[id] = Playlists[id];
                    data[id].URL = url;
                }
                else
                    data.Add(id, new Playlist(id, url, timeout));
            }

            return data;
        }

        private Dictionary<int, Playlist> _parseStartPlayList(string js)
        {
            var data = new Dictionary<int, Playlist>();
            if (string.IsNullOrWhiteSpace(js))
                return data;
            js = Match(js, "var pl = ({[^{}]+});", REGEX_IC, 1).Replace("'0'", "\"0\"");
            if (string.IsNullOrWhiteSpace(js))
                return data;
            var _data = SimpleJson.SimpleJson.DeserializeObject<Dictionary<string, string>>(js);
            foreach (var item in _data)
            {
                var key = IntVal(item.Key);
                if (!data.ContainsKey(key))
                    data.Add(key, new Playlist(key, SERVER_URL + item.Value, timeout));
            }

            return data;
        }

        private void _parseTranslate(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return;
            foreach (Match match in Regex.Matches(html, "<li data-click=\"translate\"([^<>]*)>(.*?)</li>", REGEX_ICS))
            {
                var args = match.Groups[1].ToString();
                var _id = Match(args, "data-translate=\"([0-9]+)\"", REGEX_IC, 1);
                var name = match.Groups[2].ToString().Trim();
                if (string.IsNullOrEmpty(_id))
                {
                    Logger.Instance.Error("Ошибка плеера! Не правильные поля атрибутов {0}: {1} - {2}", SeasonID, name,
                        args);
                    continue;
                }

                var id = IntVal(_id);
                var percent = floatVal(Match(args, "data-translate-percent=\"([0-9.]+)\"", REGEX_IC, 1));
                if (TRAILERS_ID == id)
                {
                    if (null == Trailers)
                        Trailers = new Playlist(id, SeasonID, Secure, timeout);
                    Trailers.TranslateName = name;
                    Trailers.TranslatePercent = percent;
                }
                else
                {
                    if (!Playlists.ContainsKey(id))
                        Playlists.Add(id, new Playlist(id, SeasonID, Secure, timeout));
                    Playlists[id].TranslateName = name;
                    Playlists[id].TranslatePercent = percent;
                }
            }
        }

        #endregion
    }
}