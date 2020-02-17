using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeTheater.API.Response;
using HomeTheater.Helper;

namespace HomeTheater.API.Serial
{
    internal class Playlist : Abstract.Serial
    {
        private DateTime __cached_date;
        private bool __forsed_update_playlist;
        public int SeasonID, timeout;
        public Translate Translate;

        public Dictionary<int, Video> Videos =
            new Dictionary<int, Video>();

        public Dictionary<string, string> VideosOrder = new Dictionary<string, string>();

        public Playlist(int TranslateID, int SerialID, int SeasonID, string Secure = "",
            int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new Translate(TranslateID);
            this.SeasonID = SeasonID;
            this.SerialID = SerialID;
            this.timeout = timeout;
            this.Secure = Secure;
            Load();
        }

        public Playlist(int TranslateID, int SerialID, string url, int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new Translate(TranslateID);
            this.SerialID = SerialID;
            this.timeout = timeout;
            var data = Matches(url, "^(.*?)/playls2/([^/]+)/trans([^/]*)/([0-9]+)/plist.txt", REGEX_IC);
            SeasonID = IntVal(data[4]);

            TranslateSlug = data[3];
            Secure = data[2];
            URL = url.Replace(Secure, "{SECURE}");
            Load();
        }

        public Playlist(int TranslateID, int SerialID, int SeasonID, Dictionary<string, string> data,
            int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new Translate(TranslateID);
            if (0 < data.Count)
            {
                LoadValues(() => { return data; });
                getVideosOrder();
                var videoData = DB.Instance.VideoGetSeason(SeasonID);
                foreach (var video in videoData)
                {
                    var id = IntVal(video["id"]);
                    if (Videos.ContainsKey(id))
                        Videos[id] = new Video(id, video);
                    else
                        Videos.Add(id, new Video(id, video));
                }
            }
            else
            {
                Load();
            }

            this.SeasonID = SeasonID;
            this.SerialID = SerialID;
            this.timeout = timeout;
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
            getVideosOrder();
            var videoData = DB.Instance.VideoGetSeason(SeasonID);
            foreach (var video in videoData)
            {
                var id = IntVal(video["id"]);
                if (Videos.ContainsKey(id))
                    Videos[id] = new Video(id, video);
                else
                    Videos.Add(id, new Video(id, video));
            }
        }

        private void Save()
        {
            setVideosOrder();
            if (0 == __data_new.Count || 0 == SeasonID || 0 > TranslateID)
                return;
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            if (!__data_new.ContainsKey("translate_id"))
                __data_new.Add("translate_id", TranslateID.ToString());
            SaveValues(data =>
            {
                if (0 == SeasonID || 0 > TranslateID)
                    return false;
                return DB.Instance.PlaylistSet(SeasonID, TranslateID, data);
            });
#if DEBUG
            Console.WriteLine("\tSave Playlist\t{0}\t{1}\t{2}({3}):\t{4}", SerialID, SeasonID, TranslateID,
                TranslateName, DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
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
            }).ConfigureAwait(true);
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
                if (result.ContainsKey(n))
                    result[n] = next;
                else
                    result.Add(n, next);
            }

            var data = new Dictionary<string, string>();
            var key = "";
            while (result.ContainsKey(key))
            {
                if (data.ContainsKey(key))
                    data[key] = result[key];
                else
                    data.Add(key, result[key]);
                key = !string.IsNullOrEmpty(result[key]) && key != result[key] ? result[key] : "+_+";
            }

            VideosOrder = data;
            setVideosOrder();
        }

        public void setVideosOrder()
        {
            OrderVideos = SimpleJson.SimpleJson.SerializeObject(VideosOrder);
        }

        public void getVideosOrder()
        {
            if (!string.IsNullOrEmpty(OrderVideos))
                VideosOrder = SimpleJson.SimpleJson.DeserializeObject<Dictionary<string, string>>(OrderVideos);
        }


        #region Атрибуты

        public string URL
        {
            get => getValue("url");
            set => setValue("url", value);
        }

        public string newURL => Server.Instance.prepareSecureUrl(URL);

        public int SerialID
        {
            get => getValueInt("serial_id");
            set => setValue("serial_id", value);
        }

        public string Secure
        {
            get => getValue("secure", Server.Instance.Secure);
            set
            {
                if (Server.Instance.Secure != value)
                    setValue("secure", value);
                else
                    setValueEmpty("secure");
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
            get => getValueFloat("percent");
            set => setValue("percent", value);
        }

        protected string OrderVideos
        {
            get => getValue("order_videos");
            set => setValue("order_videos", value);
        }

        public DateTime CreatedDate => getValueDate("created_date");

        public DateTime UpdatedDate => getValueDate("updated_date");

        public DateTime CachedDate
        {
            get => __cached_date != new DateTime() ? __cached_date : getValueDate("cached_date");
            set => __cached_date = value;
        }

        #endregion

        #region Парсинг

        private void parse(string html)
        {
            var data = new List<video>();
            try
            {
                data = SimpleJson.SimpleJson.DeserializeObject<List<video>>(html);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }

            parseVideo(data);
        }

        private void parseVideo(List<video> data)
        {
            foreach (var item in data)
                if (null == item.folder)
                {
                    var id = IntVal(item.vars);
                    if (Videos.ContainsKey(id))
                        Videos[id].parseVideo(item);
                    else
                        Videos.Add(id, new Video(id, SeasonID, SerialID, item, Translate));
                    Videos[id].VideoNextID = getNextVideoID(Videos[id].VideoID);
                    Videos[id].SaveAsync();
                }
                else
                {
                    parseVideo(item.folder);
                }
        }

        private string getNextVideoID(string ID)
        {
            if (VideosOrder.ContainsKey(ID))
                return VideosOrder[ID];
            return "";
        }

        #endregion
    }
}