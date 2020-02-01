using System;
using System.Collections.Generic;
using System.Web;
using HomeTheater.Helper;

namespace HomeTheater.Serial
{
    internal class SerialSeasonPlayerPlaylistTranslate : APIParent
    {
        private readonly List<string> __needSave = new List<string>();
        private int __key = -1;
        private string __name = "";
        private string __slug = "";

        public SerialSeasonPlayerPlaylistTranslate(int Key)
        {
            __key = Key;
            Init();
        }

        public SerialSeasonPlayerPlaylistTranslate(string Name)
        {
            __name = Name;
            Init();
        }

        private string this[string index]
        {
            get
            {
                var result = "";

                switch (index)
                {
                    case "id":
                        result = 0 < ID ? ID.ToString() : "";
                        break;
                    case "key":
                        result = 0 <= __key ? __key.ToString() : "";
                        break;
                    case "name":
                        result = __name;
                        break;
                    case "slug":
                        result = __slug;
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
                        case "id":
                            ID = IntVal(value);
                            break;
                        case "key":
                            __key = IntVal(value);
                            break;
                        case "name":
                            __name = value;
                            break;
                        case "slug":
                            __slug = value;
                            break;
                    }
                }
            }
        }

        public int ID { get; set; }

        public int Key
        {
            get => __key;
            set
            {
                if (__key != value && 0 <= value)
                {
                    __needSave.Add("key");
                    __key = value;
                    SaveAsync();
                }
            }
        }

        public string Name
        {
            get => string.IsNullOrWhiteSpace(__name) ? "" : __name;
            set
            {
                if (__name != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("name");
                    __name = value;
                    SaveAsync();
                }
            }
        }

        public string Slug
        {
            get => string.IsNullOrWhiteSpace(__slug) ? "" : __slug;
            set
            {
                if (__slug != value && !string.IsNullOrWhiteSpace(value))
                {
                    __needSave.Add("slug");
                    __slug = value;
                    if (string.IsNullOrWhiteSpace(Name))
                        Name = HttpUtility.UrlDecode(__slug);
                    SaveAsync();
                }
            }
        }

        public void Save()
        {
            if (0 >= __needSave.Count)
                return;
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            string[] fields = {"id", "key", "slug", "name"};
            var data = new Dictionary<string, string>();
            var where = new Dictionary<string, string>();
            for (var i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var value = this[field];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (!data.ContainsKey(field))
                        data.Add(field, value);
                    if (!__needSave.Contains(field) && !where.ContainsKey(field))
                        where.Add(field, value);
                }
            }

            __needSave.Clear();
            if (0 < data.Count)
            {
                if (DB.Instance.TranslateSet(data, where) && 0 == ID)
                    Load();
#if DEBUG
                Console.WriteLine("\tSave Translate\t{0}({1}):\t{2}", ID, Name,
                    DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
            }
        }

        public async void SaveAsync()
        {
            Save();
        }

        private void Init()
        {
            Load();
        }

        public void Load()
        {
            string[] fields = {"id", "key", "slug", "name"};
            var data = new Dictionary<string, string>();
            for (var i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var value = this[field];
                if (!string.IsNullOrWhiteSpace(value) && !data.ContainsKey(field))
                    data.Add(field, value);
            }

            var dataNew = DB.Instance.TranslateGet(data);
            if (0 < dataNew.Count)
                foreach (var item in dataNew)
                    this[item.Key] = item.Value;
        }
    }
}