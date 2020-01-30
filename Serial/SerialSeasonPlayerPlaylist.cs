using System;
using System.Collections.Generic;
using HomeTheater.Helper;
using HomeTheater.Serial.data;

namespace HomeTheater.Serial
{
    internal class SerialSeasonPlayerPlaylist : APIParent
    {
        private readonly List<string> __needSave = new List<string>();
        private readonly SerialSeasonPlayerPlaylistTranslate Translate;
        private DateTime __created_date, __updated_date, __cached_date;
        private float __percent;
        private string __url, __secure;
        public int SeasonID, SerialID, timeout;

        private Dictionary<string, Dictionary<string, string>> videoOrder =
            new Dictionary<string, Dictionary<string, string>>();

        public Dictionary<int, SerialSeasonPlayerPlaylistVideo> Videos =
            new Dictionary<int, SerialSeasonPlayerPlaylistVideo>();

        public SerialSeasonPlayerPlaylist(int TranslateKey, int SeasonID, int SerialID, string Secure = "",
            int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new SerialSeasonPlayerPlaylistTranslate(TranslateKey);
            this.SeasonID = SeasonID;
            this.SerialID = SerialID;
            this.timeout = timeout;
            __secure = Secure;
            Init();
        }

        public SerialSeasonPlayerPlaylist(int TranslateKey, int SerialID, string url, int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new SerialSeasonPlayerPlaylistTranslate(TranslateKey);
            this.SerialID = SerialID;
            this.timeout = timeout;

            var data = Matches(url, "^(.*?)/playls2/([^/]+)/trans([^/]*)/([0-9]+)/plist.txt", REGEX_IC);
            SeasonID = IntVal(data[4]);

            TranslateSlug = data[3];
            __secure = data[2];
            Init();
        }

        public SerialSeasonPlayerPlaylist(int TranslateKey, int SeasonID, int SerialID, Dictionary<string, string> data,
            int timeout = 60 * 60 * 24 * 10)
        {
            Translate = new SerialSeasonPlayerPlaylistTranslate(TranslateKey);
            this.SeasonID = SeasonID;
            this.SerialID = SerialID;
            this.timeout = timeout;
            foreach (var item in data)
                this[item.Key] = item.Value;
        }

        private string this[string index]
        {
            get
            {
                var result = "";

                switch (index)
                {
                    case "url":
                        result = __url;
                        break;
                    case "secure":
                        result = __secure;
                        break;
                    case "percent":
                        var ___percent = __percent * 100;
                        result = ___percent.ToString();
                        break;
                }

                return result;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    value = value.Trim();
                    switch (index)
                    {
                        case "url":
                            __url = value;
                            break;
                        case "secure":
                            __secure = value;
                            break;
                        case "percent":
                            __percent = floatVal(value) / 100;
                            break;
                        case "created_date":
                            __created_date = DateVal(value, DB.TIME_FORMAT);
                            break;
                        case "updated_date":
                            __updated_date = DateVal(value, DB.TIME_FORMAT);
                            break;
                        case "cached_date":
                            __cached_date = DateVal(value, DB.TIME_FORMAT);
                            break;
                    }
                }
            }
        }

        public string URL
        {
            get => string.IsNullOrWhiteSpace(__url) ? "" : __url;
            set
            {
                if (__url != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("url");
                    __url = value;
                }
            }
        }

        public string Secure
        {
            get => string.IsNullOrWhiteSpace(__secure) ? "" : __secure;
            set
            {
                if (__secure != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("secure");
                    __secure = value;
                }
            }
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
            get => 0 < __percent ? __percent : 0;
            set
            {
                if (__percent != value && 0 < value)
                {
                    __needSave.Add("percent");
                    __percent = value;
                }
            }
        }

        public string TranslateSlug
        {
            get => Translate.Slug;
            set => Translate.Slug = value;
        }

        private void Init()
        {
            Load();
        }

        private void Load()
        {
            if (0 == SeasonID || 0 == TranslateID)
                return;
            var data = DB.Instance.PlaylistGet(SeasonID, TranslateID);
            if (0 < data.Count)
                foreach (var item in data)
                    this[item.Key] = item.Value;
        }

        public async void SaveAsync()
        {
            Translate.Save();
            Save();
        }

        private void Save()
        {
            if (0 == __needSave.Count || 0 == SeasonID || 0 == TranslateID)
                return;
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            var data = new Dictionary<string, string>();
            for (var i = 0; i < __needSave.Count; i++)
            {
                var field = __needSave[i];
                var value = this[field];
                if (!string.IsNullOrWhiteSpace(value) && !data.ContainsKey(field))
                    data.Add(field, value);
            }

            if (0 < data.Count)
            {
                data.Add("serial_id", SerialID.ToString());
                data.Add("translate_key", TranslateKey.ToString());
                __needSave.Clear();
                DB.Instance.PlaylistSet(SeasonID, TranslateID, data);
            }

#if DEBUG
            Console.WriteLine("\tSave Playlist {0}({1}): {2}", SeasonID, TranslateKey,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
        }

        public void tempOrderVideo(Dictionary<string, Dictionary<string, string>> value)
        {
            videoOrder = value;
        }

        public bool sync(bool forsed = false)
        {
            var content = download(forsed);
            parse(content);
            SaveAsync();
            return string.IsNullOrEmpty(content);
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
            for (var i = 0; i < data.Count; i++)
            {
                var item = data[i];
                if (null != item.folder)
                {
                    parseVideo(item.folder);
                }
                else
                {
                    var id = IntVal(item.vars);
                    if (!Videos.ContainsKey(id))
                    {
                        Videos.Add(id, new SerialSeasonPlayerPlaylistVideo(id, SeasonID, SerialID, item, Translate));
                        Videos[id].SaveAsync();
                    }
                    else
                    {
                        // UNDONE Обновление данных
                    }
                        
                }
            }
        }

        private string download(bool forsed)
        {
            var cacheItem = APIServer.Instance.downloadPlaylist(SeasonID, TranslateSlug, Secure, forsed, timeout);
            if ((cacheItem.isNew || string.IsNullOrEmpty(URL)) && cacheItem.data.ContainsKey("secure"))
            {
                Secure = cacheItem.data["secure"];
                URL = cacheItem.url;
            }

            return cacheItem.ToString();
        }
    }
}