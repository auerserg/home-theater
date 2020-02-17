using System;
using System.Collections.Generic;
using System.Globalization;

namespace HomeTheater.Helper
{
    public class DBCache
    {
        public Dictionary<string, string> data = new Dictionary<string, string>();
        public DateTime date;
        public bool isNew;
        public int period;
        public string url, content;

        public DBCache(string url, string content = "", int period = 0)
        {
            setURL(url);
            setContent(url);
            this.period = period;
        }

        public DBCache(string url, int period = 0)
        {
            setURL(url);
            this.period = period;
        }

        public bool isOld => 0 < period && period < DateTime.UtcNow.Subtract(date).TotalSeconds;

        public bool isEmpty => string.IsNullOrWhiteSpace(content);

        public bool isActual => !isEmpty && !isOld;


        public DBCache setDate(string date, string format)
        {
            DateTime.TryParseExact(date, format, new CultureInfo("en-US"), DateTimeStyles.None, out this.date);
            return this;
        }

        public DBCache setURL(string url)
        {
            this.url = url.Trim();
            return this;
        }

        #region Content

        public override string ToString()
        {
            return isActual ? content : "";
        }

        public DBCache setContent(string content)
        {
            this.content = content.Trim();
            return this;
        }

        public DBCache updateContent(string content)
        {
            this.content = content.Trim();
            if (!string.IsNullOrEmpty(this.content))
            {
                isNew = true;
                date = DateTime.UtcNow;
                saveContent();
            }

            return this;
        }

        private void saveContent()
        {
            _ = DB.Instance.CacheSetAsync(url, content);
        }

        #endregion
    }
}