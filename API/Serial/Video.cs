using System;
using System.Collections.Generic;
using HomeTheater.API.Response;
using HomeTheater.Helper;

namespace HomeTheater.API.Serial
{
    internal class Video : Abstract.Serial
    {
        private readonly Translate Translate;
        public int ID;

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
            Secure = Match(url, "fi2lm/([^/]+)", REGEX_IC, 1);
            URL = url.Replace(Secure, "{SECURE}");
            VideoID = data.id;
            var _translateName = Match(data.title, "<br[^<>]*>(.*?)$", REGEX_IC, 1).Trim();
            if (!string.IsNullOrEmpty(_translateName))
                TranslateName = _translateName;
        }

        #region Конструктор

        public Video(int id, int seasonId, int serialId, video data,
            Translate translate = null)
        {
            ID = id;
            Load();
            SeasonID = seasonId;
            SerialID = serialId;
            if (null != translate && null == Translate)
                Translate = translate;
            parseVideo(data);
        }

        public Video(int id, int seasonId, int serialId, video data, int translateKey = -1)
        {
            ID = id;
            Load();
            SeasonID = seasonId;
            SerialID = serialId;
            if (0 <= translateKey && null != Translate)
                Translate = new Translate(translateKey);
            parseVideo(data);
        }

        public Video(int id, int seasonId, int serialId, video data,
            string translateName = "")
        {
            ID = id;
            Load();
            SeasonID = seasonId;
            SerialID = serialId;
            if (!string.IsNullOrWhiteSpace(translateName) && null != Translate)
                Translate = new Translate(translateName);
            parseVideo(data);
        }

        public Video(int id, Dictionary<string, string> data)
        {
            ID = id;
            LoadValues(() => { return data; });
        }

        #endregion

        #region Атрибуты

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
            get => Server.Instance.prepareSecureUrl(getValue("url"));
            set => setValue("url", value);
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

        protected string SubTitle
        {
            get => getValue("subtitle");
            private set => setValue("subtitle", value);
        }

        public Dictionary<string, string> SubTitles
        {
            get
            {
                var subs = SubTitle.Split(',');
                var data = new Dictionary<string, string>();
                foreach (var sub in subs)
                {
                    var _sub = Matches(sub.Trim(), "^[[]([^[]]+)[[](.+?)$", REGEX_IC);
                    data.Add(_sub[1], _sub[2]);
                }

                return data;
            }
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

        public string TranslateName
        {
            get => getValue("translate_name", TranslateBaseName);
            set
            {
                if (TranslateBaseName != value)
                    setValue("translate_name", value);
                else
                    setValueEmpty("translate_name");
            }
        }

        public string TranslateBaseName => null != Translate ? Translate.Name : "";

        public string TranslateSlug => null != Translate ? Translate.Slug : "";

        public DateTime CreatedDate => getValueDate("created_date");

        public DateTime UpdatedDate => getValueDate("updated_date");

        #endregion
    }
}