using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HomeTheater.Helper;

namespace HomeTheater.API.Serial
{
    internal class Player : Abstract.Serial
    {
        private DateTime __cache_date;
        private string __secure = "";

        public Dictionary<int, Playlist>
            Playlists = new Dictionary<int, Playlist>();

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
                var TranslateID = IntVal(item["translate_id"]);
                if (!Playlists.ContainsKey(TranslateID))
                    Playlists.Add(TranslateID, new Playlist(TranslateID, SerialID, SeasonID, item, timeout));
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
                if (0 < Playlists.Count)
                    foreach (var item in Playlists)
                        Playlists[item.Key].SaveAsync();
                return string.IsNullOrEmpty(content);
            }

            return false;
        }

        public bool syncPlaylist(bool forsed = false)
        {
            if (0 == Playlists.Count)
                sync();
            var result = true;
            foreach (var item in Playlists)
            {
                var _result = item.Value.sync(forsed);
                result = result && _result;
            }

            return result;
        }

        #endregion

        #region Парсинг

        private void parse(string html)
        {
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            var js = _getOnlyJS(html);
            _parseScripts(js);
            var _js = Match(html, "<ul class=\"pgs-trans\">(.*?)</ul>", REGEX_ICS, 1);
            _parseTranslate(_js);
            _js = Match(js, "var arEpisodes = ([[{].*?[]}]);", REGEX_ICS, 1);
            if (!string.IsNullOrEmpty(_js))
                _parseSeries(_js);
#if DEBUG
            else
                Console.WriteLine("Не найден список эпизодов\t{0}\t{1}", SerialID, SeasonID);
            Console.WriteLine("\tParse Player\t{0}\t{1}:\t{2}", SerialID, SeasonID,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
        }

        private void _parseSeries(string js)
        {
            if (string.IsNullOrWhiteSpace(js))
                return;
            try
            {
                var _data = SimpleJson.SimpleJson
                    .DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(js);
                if (null != _data)
                {
                    foreach (var item in _data)
                    {
                        var id = IntVal(item.Key);
                        if (Playlists.ContainsKey(id)) Playlists[id].tempOrderVideo(item.Value);
                    }
                }
                else
                {
                    var __data = SimpleJson.SimpleJson
                        .DeserializeObject<List<Dictionary<string, Dictionary<string, string>>>>(js);
                    var id = 0;
                    if (Playlists.ContainsKey(id)) Playlists[id].tempOrderVideo(__data[id]);
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

        private void _parseScripts(string js)
        {
            Playlists = _parseStartPlayList(js);
            foreach (Match match in Regex.Matches(js, "pl([0-9[]+)] = \"(.+?)\";", REGEX_ICS))
            {
                var id = IntVal(match.Groups[1].ToString());
                var url = SERVER_URL + Regex.Replace(match.Groups[2].ToString(), "[?]time=([0-9]+)$", "", REGEX_IC);
                if (Playlists.ContainsKey(id))
                {
                    Playlists[id].SerialID = SerialID;
                    Playlists[id].URL = url;
                }
                else
                {
                    Playlists.Add(id, new Playlist(id, SerialID, url, timeout));
                }
            }
        }

        private Dictionary<int, Playlist> _parseStartPlayList(string js)
        {
            var _data = SimpleJson.SimpleJson.DeserializeObject<Dictionary<string, string>>(
                Match(js, "var pl = ({[^{}]+});", REGEX_IC, 1).Replace("'0'", "\"0\""));
            var data = new Dictionary<int, Playlist>();
            foreach (var item in _data)
            {
                var key = IntVal(item.Key);
                var url = Regex.Replace(item.Value, "[?]time=([0-9]+)$", "", REGEX_IC);
                if (!data.ContainsKey(key))
                    data.Add(key, new Playlist(key, SerialID, SERVER_URL + url, timeout));
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
                    Logger.Instance.Error("Ошибка плеера! Не правильные поля атрибутов: {0:S} - {1:S}", name, args);
                    continue;
                }

                var id = IntVal(_id);
                var percent = floatVal(Match(args, "data-translate-percent=\"([0-9.]+)\"", REGEX_IC, 1));
                if (!Playlists.ContainsKey(id))
                    Playlists.Add(id, new Playlist(id, SerialID, SeasonID, Secure, timeout));
                Playlists[id].TranslateName = name;
                Playlists[id].TranslatePercent = percent;
            }
        }

        #endregion
    }
}