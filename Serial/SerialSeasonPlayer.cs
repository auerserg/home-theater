using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HomeTheater.Helper;

namespace HomeTheater.Serial
{
    internal class SerialSeasonPlayer : APIParent
    {
        private DateTime __cache_date;
        private bool __forsed_update;
        private string __secure;

        public Dictionary<int, SerialSeasonPlayerPlaylist>
            Playlists = new Dictionary<int, SerialSeasonPlayerPlaylist>();

        public int SeasonID, SerialID, timeout;

        public SerialSeasonPlayer(int SeasonID, int SerialID, string Secure = "", int timeout = 60 * 60 * 24 * 10)
        {
            this.SeasonID = SeasonID;
            this.SerialID = SerialID;
            this.Secure = Secure;
            this.timeout = timeout;
            Init();
        }

        public SerialSeasonPlayer(int SeasonID, int SerialID, int timeout = 60 * 60 * 24 * 10)
        {
            this.SeasonID = SeasonID;
            this.SerialID = SerialID;
            this.timeout = timeout;
            Init();
        }

        public string Secure
        {
            get => string.IsNullOrWhiteSpace(__secure) ? "" : __secure;
            set
            {
                if (__secure != value && !string.IsNullOrWhiteSpace(value))
                    __secure = value;
            }
        }

        public void Load()
        {
            var cache = DB.Instance.CacheGet(APIServer.Instance.downloadPlayerCacheURL(SeasonID, SerialID), timeout);
            __cache_date = cache.date;
            var data = DB.Instance.PlaylistGets(SeasonID);
            for (var i = 0; i < data.Count; i++)
            {
                var id = int.Parse(data[i]["translate_key"]);
                if (!Playlists.ContainsKey(id))
                    Playlists.Add(id, new SerialSeasonPlayerPlaylist(id, SeasonID, SerialID, data[i], timeout));
            }

            __forsed_update = 0 == Playlists.Count;
        }

        private void Init()
        {
            Load();
        }

        public bool sync(bool forsed = false)
        {
            if (timeout < DateTime.UtcNow.Subtract(__cache_date).TotalSeconds || forsed || __forsed_update)
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
            var _playlists = new List<int>(Playlists.Keys);
            var result = true;
            for (var i = 0; i < _playlists.Count; i++)
            {
                var _result = Playlists[_playlists[i]].sync(forsed);
                result = result && _result;
            }

            return result;
        }

        private void parse(string html)
        {
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            var js = _getOnlyJS(html);
            _parseScripts(js);
            _parseTranslate(Match(html, "<ul class=\"pgs-trans\">(.*?)</ul>", REGEX_ICS, 1));
            _parseSeries(Match(js, "var arEpisodes = ({.*?});", REGEX_ICS, 1));
#if DEBUG
            Console.WriteLine("\tparsePlayer  {0}: {1} ", SeasonID, DateTime.UtcNow.Subtract(start).TotalSeconds);
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
                foreach (var item in _data)
                {
                    var id = IntVal(item.Key);
                    if (Playlists.ContainsKey(id)) Playlists[id].tempOrderVideo(item.Value);
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
                var url = match.Groups[2].ToString();
                if (!Playlists.ContainsKey(id))
                    Playlists.Add(id, new SerialSeasonPlayerPlaylist(id, SerialID, SERVER_URL + url, timeout));
            }
        }

        private Dictionary<int, SerialSeasonPlayerPlaylist> _parseStartPlayList(string js)
        {
            var _data = SimpleJson.SimpleJson.DeserializeObject<Dictionary<string, string>>(
                Match(js, "var pl = ({[^{}]+});", REGEX_IC, 1).Replace("'0'", "\"0\""));
            var data = new Dictionary<int, SerialSeasonPlayerPlaylist>();
            foreach (var item in _data)
            {
                var key = IntVal(item.Key);
                if (!data.ContainsKey(key))
                    data.Add(key, new SerialSeasonPlayerPlaylist(key, SerialID, SERVER_URL + item.Value, timeout));
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
                    Playlists.Add(id, new SerialSeasonPlayerPlaylist(id, SeasonID, SerialID, Secure, timeout));
                Playlists[id].TranslateName = name;
                Playlists[id].TranslatePercent = percent;
            }
        }

        private string download(bool forsed)
        {
            var cacheItem = APIServer.Instance.downloadPlayer(SeasonID, SerialID, Secure, forsed, timeout);
            if (cacheItem.isNew && cacheItem.data.ContainsKey("secure"))
            {
                __cache_date = cacheItem.date;
                Secure = cacheItem.data["secure"];
            }

            return cacheItem.ToString();
        }
    }
}