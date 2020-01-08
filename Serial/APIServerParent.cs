using HomeTheater.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace HomeTheater.Serial
{
    class APIServerParent
    {
        public const RegexOptions REGEX_C = RegexOptions.Compiled;
        public const RegexOptions REGEX_IC = REGEX_C | RegexOptions.IgnoreCase;
        public const RegexOptions REGEX_ICS = REGEX_IC | RegexOptions.Singleline;

        protected CookieContainer CookieContainer;
        protected string COOKIES_PATH = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ".cookies");
        protected string SERVER_URL = Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL3NlYXNvbnZhci5ydQ=="));
        protected string SERVER_CDN_URL = Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL2Nkbi5zZWFzb252YXIucnUvb2Jsb2prYS8="));
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
        public static string Match(string input, string pattern, RegexOptions options = REGEX_C, int index = 0)
        {
            string result = "";

            List<string> resultmatch = Matches(input, pattern, options);
            if (index < resultmatch.Count)
            {
                result = resultmatch[index];
            }

            return result;
        }
        public static List<string> Matches(string input, string pattern, RegexOptions options = REGEX_C)
        {
            List<string> result = new List<string>();
            Match resultmatch = Regex.Match(input, pattern, options);
            if (resultmatch.Success && 0 < resultmatch.Groups.Count)
                for (int i = 0; i < resultmatch.Groups.Count; i++)
                    result.Add(resultmatch.Groups[i].ToString().Trim());

            return result;
        }
        public static int IntVal(string input)
        {
            int result = 0;
            input = Regex.Replace(input, "[^0-9]*", string.Empty);
            if (!string.IsNullOrEmpty(input))
                int.TryParse(input, out result);

            return result;
        }
        public static DateTime DateVal(string input, string format)
        {
            DateTime result = new DateTime();
            if (!string.IsNullOrWhiteSpace(input))
                DateTime.TryParseExact(input, format, new CultureInfo("de-DE"), DateTimeStyles.None, out result);


            return result;
        }

    }
}
