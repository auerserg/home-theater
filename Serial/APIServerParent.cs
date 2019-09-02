using HomeTheater.Helper;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace HomeTheater.Serial
{
    class APIServerParent
    {

        protected CookieContainer CookieContainer;
        protected string COOKIES_PATH = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ".cookies");
        protected string SERVER_URL = Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL3NlYXNvbnZhci5ydQ=="));
        public string Download(string url, NameValueCollection postData = null, WebHeaderCollection header = null)
        {
            using (WebClientEx wc = new WebClientEx(this.GetCookie()))
            {
                Console.WriteLine("Live Request: {0:S}", url);
                Uri uri = new Uri(url);
                WebHeaderCollection _header = new WebHeaderCollection();
                _header.Add(HttpRequestHeader.Referer, SERVER_URL);
                _header.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");
                if (null != header && 0 < header.Count)
                {
                    for (int i = 0; i < header.Count; i++)
                        _header.Set(header.GetKey(i), header.Get(i));

                }
                wc.Headers = _header;
                if ("1" == DB.Instance.getOption("proxy.Use"))
                {
                    wc.Proxy = new WebProxy(DB.Instance.getOption("proxy.Host"), int.Parse(DB.Instance.getOption("proxy.Port")));
                }
                wc.Encoding = Encoding.UTF8;
                string content = null;
                if (null == postData || 0 == postData.Count)
                {
                    content = wc.DownloadString(uri);
                }
                else
                {
                    byte[] responsebytes = wc.UploadValues(uri, "POST", postData);
                    content = Encoding.UTF8.GetString(responsebytes);
                }
                if (0 < wc.CookieContainer.Count)
                {
                    this.saveCookies(wc.CookieContainer);
                }

                return content;
            }
        }
        public string DownloadXHR(string url, NameValueCollection postData = null, WebHeaderCollection header = null)
        {
            WebHeaderCollection _header = new WebHeaderCollection();
            _header.Add("X-Requested-With", "XMLHttpRequest");
            if (null != header && 0 < header.Count)
            {
                for (int i = 0; i < header.Count; i++)
                    _header.Set(header.GetKey(i), header.Get(i));

            }
            return Download(url, postData, _header);
        }
        public void saveCookies(CookieContainer cookies)
        {
            FileStream stream = File.Create(COOKIES_PATH);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, cookies);
            stream.Close();
            this.CookieContainer = cookies;
        }

        public CookieContainer GetCookie()
        {
            if (null == this.CookieContainer)
            {
                this.CookieContainer = new CookieContainer();
                if (File.Exists(COOKIES_PATH))
                {
                    var stream = File.OpenRead(COOKIES_PATH);
                    var formatter = new BinaryFormatter();
                    this.CookieContainer = (CookieContainer)formatter.Deserialize(stream);
                    stream.Close();
                }
            }

            return this.CookieContainer;
        }
    }
}
