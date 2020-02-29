using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HomeTheater.Helper;

namespace HomeTheater.API.Serial
{
    public class Playlist : Abstract.Serial
    {
        private DateTime __cached_date;
        private bool __forsed_update_playlist;

        private Dictionary<string, string> __oreder_videos;
        private Dictionary<int, Video> __videos;
        public int SeasonID, timeout;
        public Translate Translate;

        public Playlist(int TranslateID, int SeasonID, string Secure = "",
            int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new Translate(TranslateID);
            this.SeasonID = SeasonID;
            this.timeout = timeout;
            this.Secure = Secure;
            Load();
        }

        public Playlist(int TranslateID, string url, int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new Translate(TranslateID);
            this.timeout = timeout;
            var data = Matches(url, "^(.*?)/playls2/([^/]+)/trans([^/]*)/([0-9]+)/plist.txt", REGEX_IC);
            SeasonID = IntVal(data[4]);

            TranslateSlug = data[3];
            Secure = data[2];
            URL = url;
            Load();
        }

        public Playlist(int TranslateID, int SeasonID, Dictionary<string, string> data,
            int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new Translate(TranslateID);
            if (0 < data.Count)
                LoadValues(() => { return data; });
            else
                Load();

            this.SeasonID = SeasonID;
            this.timeout = timeout;
        }

        public Dictionary<int, Video> Videos
        {
            get
            {
                if (null == __videos)
                {
                    __videos = new Dictionary<int, Video>();
                    var videoData = DB.Instance.VideoGets(SeasonID);
                    foreach (var item in videoData)
                    {
                        var id = IntVal(item["id"]);
                        __videos[id] = new Video(id, item);
                    }
                }

                return __videos;
            }
            set
            {
                if (null != value)
                    __videos = value;
            }
        }

        protected override void CallValue(string name, string value = null, string value_old = null)
        {
            switch (name)
            {
                case "order_videos":
                    __forsed_update_playlist = true;
                    break;
            }
        }

        private void Load()
        {
            if (0 == SeasonID || 0 > TranslateID)
                return;
            LoadValues(() => { return DB.Instance.PlaylistGet(SeasonID, TranslateID); });
        }

        private void Save()
        {
            setOrderVideos();
            if (0 == __data_new.Count || 0 == SeasonID || 0 > TranslateID)
                return;
            if (!__data_new.ContainsKey("translate_id"))
                __data_new["translate_id"] = TranslateID.ToString();
            SaveValues(data =>
            {
                if (0 == SeasonID || 0 > TranslateID)
                    return false;
                return DB.Instance.PlaylistSet(SeasonID, TranslateID, data);
            });
        }

        public async void SaveAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    Translate.Save();
                    Save();
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            });
        }


        public bool sync(bool forsed = false)
        {
            forsed = forsed || __forsed_update_playlist;
            if (timeout < DateTime.UtcNow.Subtract(CachedDate).TotalSeconds || forsed)
            {
                var content = download(forsed);
                parse(content);
                SaveAsync();
                return string.IsNullOrEmpty(content);
            }

            return true;
        }

        private string download(bool forsed)
        {
            var cacheItem = Server.Instance.downloadPlaylist(URL, forsed, timeout);
            if ((cacheItem.isNew || string.IsNullOrEmpty(URL)) && cacheItem.data.ContainsKey("secure"))
            {
                Secure = cacheItem.data["secure"];
                URL = cacheItem.url;
            }

            CachedDate = cacheItem.date;

            return cacheItem.ToString();
        }

        public void tempOrderVideo(Dictionary<string, Dictionary<string, string>> value)
        {
            var result = new Dictionary<string, string>();
            foreach (var item in value)
            {
                var n = item.Value.ContainsKey("n") ? item.Value["n"] : "";
                var next = item.Value.ContainsKey("next") ? item.Value["next"] : "";
                result[n] = next;
            }

            var data = new Dictionary<string, string>();
            var key = "";
            while (result.ContainsKey(key))
            {
                data[key] = result[key];
                key = !string.IsNullOrEmpty(result[key]) && key != result[key] ? result[key] : "+_+";
            }

            OrderVideos = data;
            setOrderVideos();
        }

        public void setOrderVideos()
        {
            if (null == __oreder_videos)
                __oreder_videos = new Dictionary<string, string>();
            var str = SimpleJson.SimpleJson.SerializeObject(__oreder_videos);
            SetValue("order_videos", str);
        }

        #region Атрибуты

        public string URL
        {
            get => GetValue("url");
            set
            {
                value = Regex.Replace(value, "[?]time=([0-9]+)$", "", REGEX_IC);
                value = value.Replace(Secure, "{SECURE}");
                SetValue("url", value);
            }
        }

        public string newURL => Server.Instance.prepareSecureUrl(URL);

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

        public int TranslateID
        {
            get => null != Translate ? Translate.ID : -1;
            set => Translate.ID = value;
        }

        public string TranslateName
        {
            get => null != Translate ? Translate.Name : "";
            set => Translate.Name = value;
        }

        public string TranslateSlug
        {
            get => null != Translate ? Translate.Slug : "";
            set => Translate.Slug = value;
        }

        public float TranslatePercent
        {
            get => GetValueFloat("percent");
            set => SetValue("percent", value);
        }

        public Dictionary<string, string> OrderVideos
        {
            get
            {
                if (null == __oreder_videos)
                {
                    var ___order_videos = GetValue("order_videos");
                    if (!string.IsNullOrEmpty(___order_videos))
                        __oreder_videos =
                            SimpleJson.SimpleJson.DeserializeObject<Dictionary<string, string>>(___order_videos);
                    else
                        __oreder_videos = new Dictionary<string, string>();
                }

                return __oreder_videos;
            }
            set
            {
                __oreder_videos = value;
                setOrderVideos();
            }
        }

        public Dictionary<string, Video> VideosOrdered
        {
            get
            {
                var _data = new Dictionary<string, int>();
                Parallel.ForEach(Videos, item =>
                {
                    if (_data.ContainsKey(item.Value.VideoID) && _data[item.Value.VideoID] < item.Key)
                        Logger.Instance.Notice("Присутствует устаревшее видео {0} в {1} для {2}", item.Key,
                            item.Value.VideoID, SeasonID);
                    else
                        _data[item.Value.VideoID] = item.Key;
                });
                var data = new Dictionary<string, Video>();
                var _order = new List<string>(OrderVideos.Values);
                if (_order.Contains(""))
                    _order.Remove("");

                foreach (var item in _order)
                    if (_data.ContainsKey(item) && Videos.ContainsKey(_data[item]))
                    {
                        data[item] = Videos[_data[item]];
                    }
                    else
                    {
                        data[item] = null;
                        if (_data.ContainsKey(item))
                            Logger.Instance.Warn("Не найдено видео {0} в {1} для {2}", _data[item], item, SeasonID);
                        else
                            Logger.Instance.Warn("Не найдено видео в {1} для {2}", item, SeasonID);
                    }

                return data;
            }
        }

        public DateTime CreatedDate => GetValueDate("created_date");

        public DateTime UpdatedDate => GetValueDate("updated_date");
        public DateTime RemovedDate => GetValueDate("removed_date");

        public DateTime CachedDate
        {
            get => __cached_date != new DateTime() ? __cached_date : GetValueDate("cached_date");
            set => __cached_date = value;
        }

        #endregion

        #region Парсинг

        private void parse(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return;
            var data = new List<Response.Video>();
            try
            {
                data = SimpleJson.SimpleJson.DeserializeObject<List<Response.Video>>(html);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }

            Videos = parseVideo(data);
            Parallel.ForEach(Videos, item => item.Value.SaveAsync());
        }

        private Dictionary<int, Video> parseVideo(List<Response.Video> data, Dictionary<int, Video> _videos = null)
        {
            if (null == _videos)
                _videos = new Dictionary<int, Video>();
            foreach (var item in data)
                if (null == item.folder)
                {
                    var id = IntVal(item.vars);
                    if (_videos.ContainsKey(id))
                    {
                        _videos[id].parseVideo(item);
                    }
                    else if (Videos.ContainsKey(id))
                    {
                        _videos[id] = Videos[id];
                        _videos[id].parseVideo(item);
                    }
                    else
                    {
                        _videos.Add(id, new Video(id, SeasonID, item, Translate));
                    }
                }
                else
                {
                    _videos = parseVideo(item.folder, _videos);
                }

            return _videos;
        }

        #endregion
    }
}