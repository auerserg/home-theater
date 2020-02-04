using System;
using System.Web;
using HomeTheater.Helper;

namespace HomeTheater.Serial
{
    internal class SerialSeasonPlayerPlaylistTranslate : SerialParent
    {
        public SerialSeasonPlayerPlaylistTranslate(int Key)
        {
            this.Key = Key;
            Load();
        }

        public SerialSeasonPlayerPlaylistTranslate(string Name)
        {
            this.Name = Name;
            Load();
        }

        public int ID
        {
            get => getValueInt("id");
            set => setValue("id", value);
        }

        public int Key
        {
            get => getValueInt("key", -1);
            set => setValue0("key", value, -1);
        }

        public string Name
        {
            get => getValue("name");
            set => setValue("name", value);
        }

        public string Slug
        {
            get => getValue("slug");
            set
            {
                setValue("slug", value);
                if (string.IsNullOrEmpty(Name))
                    setValue("name", HttpUtility.UrlDecode(value));
            }
        }

        protected void setValue0(string name, int value, int _default = 0)
        {
            if (0 > value)
                return;
            var value_old = getValueInt(name, _default);
            if (value_old == value)
                return;
            if (__data_int.ContainsKey(name))
                __data_int[name] = value;
            else
                __data_int.Add(name, value);
            setValue(name, value.ToString());
        }

        protected override void callbackValue(string name, string value)
        {
            switch (name)
            {
                case "slug":
                case "name":
                    SaveAsync();
                    break;
            }
        }

        public void Load()
        {
            LoadValues(() => { return DB.Instance.TranslateGet(0 < __data_old.Count ? __data_old : __data_new); });
        }

        public void Save()
        {
            if (0 == __data_new.Count)
                return;
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            SaveValues(data => { return DB.Instance.TranslateSet(data, __data_old); });
            if (0 == ID)
                Load();
#if DEBUG
            Console.WriteLine("\tSave Translate\t{0}({1}):\t{2}", ID, Name,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
        }

        public async void SaveAsync()
        {
            Save();
        }
    }
}