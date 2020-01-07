using System;

namespace HomeTheater.Helper
{
    public class DBCache
    {
        public string url, content;
        public int period = 0;
        public DateTime date;
        public bool isOld
        {
            get
            {
                return 0 < period && period < DateTime.UtcNow.Subtract(date).TotalSeconds;
            }
        }
        public bool isEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(content);
            }
        }
        public bool isActual
        {
            get
            {
                return !isEmpty && !isOld;
            }
        }
        public DBCache(string url, string content = "", int period = 0)
        {
            this.url = url;
            this.content = content;
            this.period = period;
        }
    }
}
