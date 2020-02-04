using System;
using System.Collections.Generic;
using HomeTheater.Helper;
using HomeTheater.Serial.data;

namespace HomeTheater.Serial
{
    internal class SerialSeasonPlayerPlaylist : SerialParent
    {
        private DateTime __cached_date;
        public int SeasonID, timeout;
        public SerialSeasonPlayerPlaylistTranslate Translate;

        public Dictionary<int, SerialSeasonPlayerPlaylistVideo> Videos =
            new Dictionary<int, SerialSeasonPlayerPlaylistVideo>();

        public SerialSeasonPlayerPlaylist(int TranslateKey, int SerialID, int SeasonID, string Secure = "",
            int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new SerialSeasonPlayerPlaylistTranslate(TranslateKey);
            this.SeasonID = SeasonID;
            this.SerialID = SerialID;
            this.timeout = timeout;
            this.Secure = Secure;
            Load();
        }

        public SerialSeasonPlayerPlaylist(int TranslateKey, int SerialID, string url, int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new SerialSeasonPlayerPlaylistTranslate(TranslateKey);
            this.SerialID = SerialID;
            this.timeout = timeout;

            var data = Matches(url, "^(.*?)/playls2/([^/]+)/trans([^/]*)/([0-9]+)/plist.txt", REGEX_IC);
            SeasonID = IntVal(data[4]);

            TranslateSlug = data[3];
            Secure = data[2];
            Load();
        }

        public SerialSeasonPlayerPlaylist(int TranslateKey, int SerialID, int SeasonID, Dictionary<string, string> data,
            int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new SerialSeasonPlayerPlaylistTranslate(TranslateKey);
            if (0 < data.Count)
                LoadValues(() => { return data; });
            else
                Load();
            this.SeasonID = SeasonID;
            this.SerialID = SerialID;
            this.timeout = timeout;
        }

        public string URL
        {
            get => getValue("url");
            set => setValue("url", value);
        }

        public string newURL => APIServer.Instance.downloadPlaylistURL(SeasonID, TranslateSlug);

        public int SerialID
        {
            get => getValueInt("serial_id");
            set => setValue("serial_id", value);
        }

        public string Secure
        {
            get => getValue("secure");
            set => setValue("secure", value);
        }

        public int TranslateID
        {
            get => null != Translate ? Translate.ID : 0;
            set => Translate.ID = value;
        }

        public int TranslateKey
        {
            get => null != Translate ? Translate.Key : -1;
            set => Translate.Key = value;
        }

        public string TranslateName
        {
            get => null != Translate ? Translate.Name : "";
            set => Translate.Name = value;
        }

        public float TranslatePercent
        {
            get => getValueFloat("percent");
            set => setValue("percent", value);
        }

        public string TranslateSlug
        {
            get => null != Translate ? Translate.Slug : "";
            set => Translate.Slug = value;
        }

        public DateTime CreatedDate => getValueDate("created_date");

        public DateTime UpdatedDate => getValueDate("updated_date");

        public DateTime CachedDate
        {
            get => __cached_date != new DateTime() ? __cached_date : getValueDate("cached_date");
            set => __cached_date = value;
        }


        protected override void callbackValue(string name, string value)
        {
        }

        private void Load()
        {
            if (0 == SeasonID || 0 == TranslateID)
                return;
            LoadValues(() => { return DB.Instance.PlaylistGet(SeasonID, TranslateID); });
        }

        public async void SaveAsync()
        {
            Translate.Save();
            Save();
        }

        internal void tempOrderVideo(Dictionary<string, Dictionary<string, string>> value)
        {
            // throw new NotImplementedException();
        }

        private void Save()
        {
            if (0 == __data_new.Count || 0 == SeasonID || 0 == TranslateID)
                return;
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            if (!__data_new.ContainsKey("translate_id"))
                __data_new.Add("translate_id", TranslateID.ToString());
            if (!__data_new.ContainsKey("translate_key"))
                __data_new.Add("translate_key", TranslateKey.ToString());
            if (!__data_new.ContainsKey("translate_slug"))
                __data_new.Add("translate_slug", TranslateSlug);
            SaveValues(data =>
            {
                if (0 == SeasonID || 0 == TranslateID)
                    return false;
                return DB.Instance.PlaylistSet(SeasonID, TranslateID, data);
            });
#if DEBUG
            Console.WriteLine("\tSave Playlist\t{0}\t{1}\t{2}({3}):\t{4}", SerialID, SeasonID, TranslateID,
                TranslateName, DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
        }

        public bool sync(bool forsed = false)
        {
            if (timeout < DateTime.UtcNow.Subtract(CachedDate).TotalSeconds || forsed)
            {
                var content = download(forsed);
                parse(content);
                SaveAsync();
                return string.IsNullOrEmpty(content);
            }

            return true;
        }

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
                        Videos.Add(id, new SerialSeasonPlayerPlaylistVideo(id, SeasonID, SerialID, item, Translate));

                    Videos[id].SaveAsync();
                }
                else
                {
                    parseVideo(item.folder);
                }
        }

        private string download(bool forsed)
        {
            var cacheItem = APIServer.Instance.downloadPlaylist(SeasonID, TranslateSlug, forsed, timeout);
            if ((cacheItem.isNew || string.IsNullOrEmpty(URL)) && cacheItem.data.ContainsKey("secure"))
            {
                Secure = cacheItem.data["secure"];
                URL = cacheItem.url;
            }

            CachedDate = cacheItem.date;

            return cacheItem.ToString();
        }
    }
}