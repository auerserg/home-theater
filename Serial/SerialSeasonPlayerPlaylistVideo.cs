using System;
using System.Collections.Generic;
using HomeTheater.Helper;
using HomeTheater.Serial.data;

namespace HomeTheater.Serial
{
    internal class SerialSeasonPlayerPlaylistVideo : SerialParent
    {
        private readonly SerialSeasonPlayerPlaylistTranslate Translate;
        public int ID;

        public SerialSeasonPlayerPlaylistVideo(int id, int seasonId, int serialId, video data,
            SerialSeasonPlayerPlaylistTranslate translate = null)
        {
            ID = id;
            Load();
            SeasonID = seasonId;
            SerialID = serialId;
            if (null != translate && null == Translate)
                Translate = translate;
            parseVideo(data);
        }

        public SerialSeasonPlayerPlaylistVideo(int id, int seasonId, int serialId, video data, int translateKey = -1)
        {
            ID = id;
            Load();
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
            Load();
            SeasonID = seasonId;
            SerialID = serialId;
            if (!string.IsNullOrWhiteSpace(translateName) && null != Translate)
                Translate = new SerialSeasonPlayerPlaylistTranslate(translateName);
            parseVideo(data);
        }

        public SerialSeasonPlayerPlaylistVideo(int id, Dictionary<string, string> data)
        {
            ID = id;
            LoadValues(() => { return data; });
        }

        public int SerialID
        {
            get => getValueInt("serial_id");
            set => setValue("serial_id", value);
        }

        public int SeasonID
        {
            get => getValueInt("season_id");
            set => setValue("season_id", value);
        }

        public string URL
        {
            get => APIServer.Instance.prepareSecureUrl(getValue("url"));
            set => setValue("url", value);
        }

        public string Secure
        {
            get => getValue("secure");
            set => setValue("secure", value);
        }

        public string FileName
        {
            get => getValue("file_name");
            set => setValue("file_name", value);
        }

        public int FileSize
        {
            get => getValueInt("file_size");
            set => setValue("file_size", value);
        }

        public string SubTitle
        {
            get => getValue("subtitle");
            set => setValue("subtitle", value);
        }

        public string VideoID
        {
            get => getValue("video_id");
            set => setValue("video_id", value);
        }

        public string VideoNextID
        {
            get => getValue("video_next_id");
            set => setValue("video_next_id", value);
        }

        public int TranslateID => null != Translate ? Translate.ID : 0;

        public int TranslateKey => null != Translate ? Translate.Key : -1;

        public string TranslateName
        {
            get => getValue("translate_name", null != Translate ? Translate.Name : "");
            set => setValue("translate_name", value);
        }

        public string TranslateSlug => null != Translate ? Translate.Slug : "";

        public DateTime CreatedDate => getValueDate("created_date");

        public DateTime UpdatedDate => getValueDate("updated_date");


        protected override void callbackValue(string name, string value)
        {
        }

        public void Load()
        {
            if (0 == ID)
                return;
            LoadValues(() => { return DB.Instance.VideoGet(ID); });
        }

        public async void SaveAsync()
        {
            Save();
        }

        public void Save()
        {
            if (0 == __data_new.Count || 0 == ID)
                return;
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            if (!__data_new.ContainsKey("translate_id"))
                __data_new.Add("translate_id", TranslateID.ToString());
            if (__data_new.ContainsKey("url") || 0 == FileSize)
            {
                FileSize = GetFileSize(URL);
                Secure = APIServer.Instance.Secure;
            }

            SaveValues(data =>
            {
                if (0 == ID)
                    return false;
                return DB.Instance.VideoSet(ID, data);
            });
#if DEBUG
            Console.WriteLine("\tSave Video\t\t{0}\t{1}\t{2}({3})\t{4}:\t{5}", SerialID, SeasonID, TranslateID,
                TranslateName, ID, DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
        }

        public void parseVideo(video data)
        {
            SubTitle = data.subtitle;
            var url = data.fileReal;
            var secure = Match(url, "fi2lm/([^/]+)", REGEX_IC, 1);
            FileName = Match(url, "/([^/]+)$", REGEX_IC, 1);
            URL = url.Replace(secure, "{SECURE}");
            VideoID = data.id;
            var _translateName = Match(data.title, "<br[^<>]*>(.*?)$", REGEX_IC, 1).Trim();
            if (!string.IsNullOrEmpty(_translateName))
                TranslateName = _translateName;
        }

        private int GetFileSize(string url)
        {
            return APIServer.Instance.GetFileSize(url);
        }
    }
}