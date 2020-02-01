using System;
using System.Collections.Generic;
using HomeTheater.Helper;
using HomeTheater.Serial.data;

namespace HomeTheater.Serial
{
    internal class SerialSeasonPlayerPlaylistVideo : APIParent
    {
        private readonly List<string> __needSave = new List<string>();
        private DateTime __created_date, __updated_date;
        private int __serial_id, __season_id;
        private string __subtitle, __url, __video_id, __video_next_id, __translate_name;

        public int ID;
        private SerialSeasonPlayerPlaylistTranslate Translate;

        public SerialSeasonPlayerPlaylistVideo(int id, int seasonId, int serialId, video data,
            SerialSeasonPlayerPlaylistTranslate translate = null)
        {
            ID = id;
            Init();
            SeasonID = seasonId;
            SerialID = serialId;
            if (null != translate && null == Translate)
                Translate = translate;
            parseVideo(data);
        }

        public SerialSeasonPlayerPlaylistVideo(int id, int seasonId, int serialId, video data, int translateKey = -1)
        {
            ID = id;
            Init();
            SeasonID = seasonId;
            SerialID = serialId;
            if (0 <= translateKey && null != Translate)
                Translate = new SerialSeasonPlayerPlaylistTranslate(translateKey);
            parseVideo(data);
        }

        public SerialSeasonPlayerPlaylistVideo(int id, int seasonId, int serialId, video data,
            string translateName = "")
        {
            ID = id;
            Init();
            SeasonID = seasonId;
            SerialID = serialId;
            if (!string.IsNullOrWhiteSpace(translateName) && null != Translate)
                Translate = new SerialSeasonPlayerPlaylistTranslate(translateName);
            parseVideo(data);
        }

        public SerialSeasonPlayerPlaylistVideo(int id, Dictionary<string, string> data)
        {
            ID = id;
            if (0 < data.Count)
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
                    case "season_id":
                        result = __season_id.ToString();
                        break;
                    case "serial_id":
                        result = __serial_id.ToString();
                        break;
                    case "url":
                        result = __url;
                        break;
                    case "video_id":
                        result = __video_id;
                        break;
                    case "video_next_id":
                        result = __video_next_id;
                        break;
                    case "subtitle":
                        result = __subtitle;
                        break;
                    case "translate_id":
                        result = TranslateID.ToString();
                        break;
                    case "translate_name":
                        result = TranslateName;
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
                        case "translate_name":
                            TranslateName = value;
                            if (__needSave.Contains("translate_name"))
                                __needSave.Remove("translate_name");
                            if (__needSave.Contains("translate_id"))
                                __needSave.Remove("translate_id");
                            break;
                        case "season_id":
                            __season_id = IntVal(value);
                            break;
                        case "serial_id":
                            __serial_id = IntVal(value);
                            break;
                        case "video_id":
                            __video_id = value;
                            break;
                        case "video_next_id":
                            __video_next_id = value;
                            break;
                        case "url":
                            __url = value;
                            break;
                        case "file_size":
                            FileSize = IntVal(value);
                            break;
                        case "subtitle":
                            __subtitle = value;
                            break;
                        case "created_date":
                            __created_date = DateVal(value, DB.TIME_FORMAT);
                            break;
                        case "updated_date":
                            __updated_date = DateVal(value, DB.TIME_FORMAT);
                            break;
                    }
                }
            }
        }

        public int SerialID
        {
            get => __serial_id;
            set
            {
                if (__serial_id < value && value > 0)
                {
                    __needSave.Add("serial_id");
                    __serial_id = value;
                }
            }
        }

        public int SeasonID
        {
            get => __season_id;
            set
            {
                if (__season_id < value && value > 0)
                {
                    __needSave.Add("season_id");
                    __season_id = value;
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

        public int FileSize { get; set; }

        public string SubTitle
        {
            get => string.IsNullOrWhiteSpace(__subtitle) ? "" : __subtitle;
            set
            {
                if (__subtitle != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("subtitle");
                    __subtitle = value;
                }
            }
        }

        public string VideoID
        {
            get => string.IsNullOrWhiteSpace(__video_id) ? "" : __video_id;
            set
            {
                if (__video_id != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("video_id");
                    __video_id = value;
                }
            }
        }

        public string VideoNextID
        {
            get => string.IsNullOrWhiteSpace(__video_next_id) ? "" : __video_next_id;
            set
            {
                if (__video_next_id != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("video_next_id");
                    __video_next_id = value;
                }
            }
        }

        public int TranslateID => null != Translate ? Translate.ID : 0;

        public int TranslateKey => null != Translate ? Translate.Key : -1;

        public string TranslateName
        {
            get => !string.IsNullOrWhiteSpace(__translate_name) ? __translate_name :
                null != Translate ? Translate.Name : "";
            set
            {
                if (TranslateName != value && !string.IsNullOrWhiteSpace(value))
                {
                    __translate_name = value;
                    __needSave.Add("translate_name");
                    __needSave.Add("translate_id");
                    var _translate = new SerialSeasonPlayerPlaylistTranslate(value);
                    if (0 == _translate.ID)
                        _translate.Save();
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

        public void Load()
        {
            if (0 < ID)
            {
                var data = DB.Instance.VideoGet(ID);
                if (0 < data.Count)
                    foreach (var item in data)
                        this[item.Key] = item.Value;
            }
        }

        public async void SaveAsync()
        {
            Save();
        }

        public void Save()
        {
            if (0 == __needSave.Count || 0 == ID)
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
                if (__needSave.Contains("url"))
                {
                    var size = GetFileSize(URL);
                    if (FileSize != size && 0 < size)
                    {
                        FileSize = size;
                        data.Add("file_size", FileSize.ToString());
                    }
                }

                if (!data.ContainsKey("translate_id"))
                    data.Add("translate_id", TranslateID.ToString());

                if (!data.ContainsKey("translate_name"))
                    data.Add("translate_name", TranslateName);

                __needSave.Clear();
                DB.Instance.VideoSet(ID, data);
            }
#if DEBUG
            Console.WriteLine("\tSave Video {0}({1}): {2}", ID, SeasonID,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
        }

        public void parseVideo(video data)
        {
            URL = data.fileReal;
            VideoID = data.id;
            var _translateName = Match(data.title, "<br[^<>]*>(.*?)$", REGEX_IC, 1).Trim();

            if (!string.IsNullOrEmpty(_translateName)) ;
            TranslateName = _translateName;
        }

        private int GetFileSize(string url)
        {
            return APIServer.Instance.GetFileSize(url);
        }
    }
}