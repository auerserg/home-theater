using System;
using System.Threading.Tasks;
using System.Web;
using HomeTheater.Helper;

namespace HomeTheater.API.Serial
{
    internal class Translate : Abstract.Serial
    {
        public Translate(int ID)
        {
            this.ID = ID;
            Load();
        }

        public Translate(string Name)
        {
            this.Name = Name;
            Load();
        }

        protected void setValue0(string name, int value, int _default = -1)
        {
            if (0 > value)
                return;
            var value_old = GetValueInt(name, _default);
            if (value_old == value)
                return;
            if (__data_int.ContainsKey(name))
                __data_int[name] = value;
            else
                __data_int.Add(name, value);
            SetValue(name, value.ToString());
        }

        protected override void CallValue(string name, string value = null, string value_old = null)
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
            LoadValues(() => { return DB.Instance.TranslateGet(ID); });
        }

        public void Save()
        {
            if (0 == __data_new.Count)
                return;
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            SaveValues(data => { return DB.Instance.TranslateSet(IDOld, data); });
#if DEBUG
            Console.WriteLine("\tSave Translate\t{0}({1}):\t{2}", ID, Name,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
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
            }).ConfigureAwait(true);
        }

        #region Атрибуты

        public int ID
        {
            get => GetValueInt("id", -1);
            set => setValue0("id", value);
        }

        private int IDOld
        {
            get
            {
                if (__data_old.ContainsKey("id") && !string.IsNullOrEmpty(__data_old["id"]))
                    return IntVal(__data_old["id"]);
                if (__data_new.ContainsKey("id") && !string.IsNullOrEmpty(__data_new["id"]))
                    return IntVal(__data_new["id"]);
                return -1;
            }
        }

        public string Name
        {
            get => GetValue("name");
            set => SetValue("name", value);
        }

        public string Slug
        {
            get => GetValue("slug");
            set
            {
                SetValue("slug", value);
                if (string.IsNullOrEmpty(Name))
                    SetValue("name", HttpUtility.UrlDecode(value));
            }
        }

        #endregion
    }
}