using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            await Task.Run(() =>
            {
                try
                {
                    Save();
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            });
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
            Console.WriteLine("\tSave Video\t\t{0}\t{1}({2})\t{3}:\t{4}", SeasonID, TranslateID,
                TranslateName, ID, DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
        }

        public void parseVideo(Response.Video data)
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

        public Video(int id, int seasonId, Response.Video data,
            Translate translate = null)
        {
            ID = id;
            Load();
            SeasonID = seasonId;
            if (null != translate && null == Translate)
                Translate = translate;
            parseVideo(data);
        }

        public Video(int id, int seasonId, Response.Video data, int translateKey = -1)
        {
            ID = id;
            Load();
            SeasonID = seasonId;
            if (0 <= translateKey && null != Translate)
                Translate = new Translate(translateKey);
            parseVideo(data);
        }

        public Video(int id, int seasonId, Response.Video data,
            string translateName = "")
        {
            ID = id;
            Load();
            SeasonID = seasonId;
            if (!string.IsNullOrWhiteSpace(translateName) && null != Translate)
                Translate = new Translate(translateName);
            parseVideo(data);
        }

        public Video(int id, Dictionary<string, string> data)
        {
            ID = id;
            LoadValues(() => { return data; });
            if (__data_old.ContainsKey("translate_id") && null == Translate)
                Translate = new Translate(IntVal(__data_old["translate_id"]));
        }

        #endregion

        #region Атрибуты

        public int SeasonID
        {
            get => GetValueInt("season_id");
            set => SetValue("season_id", value);
        }

        public string URL
        {
            get => Server.Instance.prepareSecureUrl(GetValue("url"));
            set => SetValue("url", value);
        }

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

        protected string SubTitle
        {
            get => GetValue("subtitle");
            private set => SetValue("subtitle", value);
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
            get => GetValue("video_id");
            set => SetValue("video_id", value);
        }

        public string VideoNextID
        {
            get => GetValue("video_next_id");
            set => SetValue("video_next_id", value);
        }

        public int TranslateID => null != Translate ? Translate.ID : 0;

        public string TranslateName
        {
            get => GetValue("translate_name", TranslateBaseName);
            set
            {
                if (TranslateBaseName != value)
                    SetValue("translate_name", value);
                else
                    SetValueEmpty("translate_name");
            }
        }

        public string TranslateBaseName => null != Translate ? Translate.Name : "";

        public string TranslateSlug => null != Translate ? Translate.Slug : "";

        public DateTime CreatedDate => GetValueDate("created_date");

        public DateTime UpdatedDate => GetValueDate("updated_date");
        public DateTime RemovedDate => GetValueDate("removed_date");

        #endregion
    }
}