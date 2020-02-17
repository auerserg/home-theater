using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using HomeTheater.API.Response;
using HomeTheater.Helper;

namespace HomeTheater.API.Abstract
{
    internal abstract class Server : Base
    {
        #region Cookies

        protected CookieContainer CookieContainer;
        protected string COOKIES_PATH = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ".cookies");

        #endregion

        #region Download

        protected const string USER_AGENT =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";

        public string Download(string url, NameValueCollection postData = null, WebHeaderCollection header = null)
        {
            using (var wc = new WebClientEx(GetCookie()))
            {
#if DEBUG
                var start = DateTime.UtcNow;
#endif
                var uri = new Uri(url);
                var _header = new WebHeaderCollection();
                _header.Add(HttpRequestHeader.Referer, SERVER_URL);
                _header.Add(HttpRequestHeader.UserAgent, USER_AGENT);
                if (null != header && 0 < header.Count)
                    for (var i = 0; i < header.Count; i++)
                        _header.Set(header.GetKey(i), header.Get(i));
                wc.Headers = _header;
                if ("1" == DB.Instance.OptionGet("proxy.Use"))
                    wc.Proxy = new WebProxy(DB.Instance.OptionGet("proxy.Host"),
                        int.Parse(DB.Instance.OptionGet("proxy.Port")));
                wc.Encoding = Encoding.UTF8;
                string content = null;
                if (null == postData || 0 == postData.Count)
                {
                    content = wc.DownloadString(uri);
                }
                else
                {
                    var responsebytes = wc.UploadValues(uri, "POST", postData);
                    content = Encoding.UTF8.GetString(responsebytes);
                }
#if DEBUG
                Console.WriteLine("Live Request: {0} - {1}", url,
                    DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
                if (0 < wc.CookieContainer.Count) SetCookies(wc.CookieContainer);

                return content;
            }
        }

        public Stream DownloadStream(string url, WebHeaderCollection header = null)
        {
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            var wc = (HttpWebRequest) WebRequest.Create(url);
            var _header = new WebHeaderCollection();
            if (null != header && 0 < header.Count)
                for (var i = 0; i < header.Count; i++)
                    _header.Set(header.GetKey(i), header.Get(i));
            wc.Referer = SERVER_URL;
            wc.UserAgent = USER_AGENT;
            wc.Headers = _header;
            if ("1" == DB.Instance.OptionGet("proxy.Use"))
                wc.Proxy = new WebProxy(DB.Instance.OptionGet("proxy.Host"),
                    int.Parse(DB.Instance.OptionGet("proxy.Port")));
            var resp = wc.GetResponse();
            var respStream = resp.GetResponseStream();
#if DEBUG
            Console.WriteLine("Live Request: {0} - {1}", url,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
            return respStream;
        }

        public header DownloadHeader(string url, WebHeaderCollection header = null)
        {
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            var headerResponse = new header();
            var wc = (HttpWebRequest) WebRequest.Create(url);
            var _header = new WebHeaderCollection();
            if (null != header && 0 < header.Count)
                for (var i = 0; i < header.Count; i++)
                    _header.Set(header.GetKey(i), header.Get(i));
            wc.Referer = SERVER_URL;
            wc.UserAgent = USER_AGENT;
            wc.Headers = _header;
            if ("1" == DB.Instance.OptionGet("proxy.Use"))
                wc.Proxy = new WebProxy(DB.Instance.OptionGet("proxy.Host"),
                    int.Parse(DB.Instance.OptionGet("proxy.Port")));
            wc.Method = "HEAD";
            using (var webResponse = wc.GetResponse())
            {
                headerResponse.ContentLength = webResponse.Headers.Get("Content-Length");
                headerResponse.ContentType = webResponse.Headers.Get("Content-Type");
                headerResponse.ETag = webResponse.Headers.Get("ETag");
                headerResponse.LastModified = webResponse.Headers.Get("Last-Modified");
            }

#if DEBUG
            Console.WriteLine("Live Request: {0} - {1}", url,
                DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
            return headerResponse;
        }

        public string DownloadXHR(string url, NameValueCollection postData = null, WebHeaderCollection header = null)
        {
            var _header = new WebHeaderCollection();
            _header.Add("X-Requested-With", "XMLHttpRequest");
            if (null != header && 0 < header.Count)
                for (var i = 0; i < header.Count; i++)
                    _header.Set(header.GetKey(i), header.Get(i));
            return Download(url, postData, _header);
        }

        #endregion

        #region Cookies

        public void SetCookies(CookieContainer cookies)
        {
            var stream = File.Create(COOKIES_PATH);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, cookies);
            stream.Close();
            CookieContainer = cookies;
        }

        public CookieContainer GetCookie()
        {
            if (null == CookieContainer)
            {
                CookieContainer = new CookieContainer();
                if (File.Exists(COOKIES_PATH))
                {
                    var stream = File.OpenRead(COOKIES_PATH);
                    var formatter = new BinaryFormatter();
                    CookieContainer = (CookieContainer) formatter.Deserialize(stream);
                    stream.Close();
                }
            }

            return CookieContainer;
        }

        #endregion
    }
}