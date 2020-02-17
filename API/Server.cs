using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;
using HomeTheater.API.Response;
using HomeTheater.API.Serial;
using HomeTheater.Helper;

namespace HomeTheater.API
{
    internal class Server : Abstract.Server
    {
        private static Server _i;
        private readonly string password;

        private string __secure;
        public string login;
        public int ProfileID;

        public Server(string _login = null, string _password = null)
        {
            login = _login;
            password = _password;
            if (string.IsNullOrWhiteSpace(login))
                login = DB.Instance.OptionGet("Login");
            if (string.IsNullOrWhiteSpace(password))
                password = DB.Instance.OptionGet("Password");
        }

        public string Secure
        {
            get
            {
                if (string.IsNullOrEmpty(__secure))
                    __secure = DB.Instance.OptionGet("secure");
                return __secure;
            }
            set
            {
                if (__secure != value)
                {
                    DB.Instance.OptionSetAsync("secure_old", Secure);
                    DB.Instance.OptionSetAsync("secure", value);
                    __secure = null;
                }
            }
        }

        public static Server Instance
        {
            get
            {
                if (_i == null) _i = new Server();
                return _i;
            }
        }

        public string prepareSecureUrl(string url)
        {
            return url.Replace("{SECURE}", Secure);
        }

        #region Download Page

        public DBCache downloadPage(string url, bool forsed = false, int timeout = 30 * 60)
        {
            var cacheItem = DB.Instance.CacheGet(url, timeout);
            if (!cacheItem.isActual || forsed)
                cacheItem.updateContent(Download(prepareSecureUrl(url)));
            return cacheItem;
        }

        #endregion

        #region Mark

        public markresponse doMarks(NameValueCollection postData = null)
        {
            var result = DownloadXHR(getURLMark, postData);
            var resultjson = SimpleJson.SimpleJson.DeserializeObject<markresponse>(result);
            return resultjson;
        }

        #endregion

        #region Playlist

        public DBCache downloadPlaylist(string url, bool forsed = false,
            int timeout = 60 * 60)
        {
            var cacheItem = downloadPage(url, forsed, timeout);
            if (cacheItem.url == url)
                cacheItem.data.Add("secure", Secure);

            return cacheItem;
        }

        #endregion

        #region Video

        public header downloadHeader(string url, WebHeaderCollection header = null)
        {
            return DownloadHeader(prepareSecureUrl(url), header);
        }

        #endregion

        #region URL

        private const string AJAX_PATH = "/ajax.php";
        private const string FORGOT_PATH = "/?mod=recover";
        private const string LOGIN_PATH = "/?mod=login";
        private const string MARK_PATH = "/jsonMark.php";
        private const string PAUSE_PATH = "/?mod=pause";
        private const string PLAYER_PATH = "/player.php";
        private const string PLAYLIST_PATH = "/playls2/{0:S}/trans{1:S}/{2}/plist.txt";
        private const string REGISTER_PATH = "/?mod=reg";
        protected string PROFILE_PATH;

        public string getURLAjax => SERVER_URL + AJAX_PATH;
        public string getURLForgon => SERVER_URL + FORGOT_PATH;
        public string getURLLogin => SERVER_URL + LOGIN_PATH;
        public string getURLMark => SERVER_URL + MARK_PATH;
        public string getURLPause => SERVER_URL + PAUSE_PATH;
        public string getURLPlayer => SERVER_URL + PLAYER_PATH;
        public string getURLProfile => !string.IsNullOrEmpty(PROFILE_PATH) ? SERVER_URL + PROFILE_PATH : "";
        public string getURLRegister => SERVER_URL + REGISTER_PATH;

        #endregion

        #region Авторизация

        public bool LogedIn(string _login = null, string _password = null)
        {
            var __login = login;
            var __password = password;
            if (string.IsNullOrEmpty(_login)) _login = __login;
            if (string.IsNullOrEmpty(_password)) _password = __password;
            var result = false;

            try
            {
                var content = Download(getURLLogin,
                    new NameValueCollection {{"login", _login}, {"password", _password}});
                _ = DB.Instance.CacheSetAsync(getURLLogin, content);
                result = isLogedIn(content);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }

            return result;
        }

        public bool isLogedIn(string content = null)
        {
            if (string.IsNullOrWhiteSpace(content)) content = downloadPage(getURLLogin, false, 5 * 60).content;

            var result = !Regex.IsMatch(content, @"loginbox-login", REGEX_IC);

            if (result) setURLProfile(content);

            return result;
        }

        #endregion

        #region Profile

        private void setURLProfile(string content)
        {
            var url = Match(content, "<a[^<>]*href=\"(/profile/[0-9]+)\"[^<>]*>", REGEX_IC, 1);
            if (!string.IsNullOrEmpty(url))
            {
                PROFILE_PATH = url;
                ProfileID = int.Parse(Match(url, "/([0-9]+)$", REGEX_C, 1));
            }
        }

        public string downloadProfile(bool forsed = false, int timeout = 30 * 60)
        {
            return downloadPage(getURLProfile, forsed, timeout).ToString();
        }

        #endregion

        #region Pause

        public string downloadPause(bool forsed = false, int timeout = 30 * 60)
        {
            return downloadPage(getURLPause, forsed, timeout).content;
        }

        public Dictionary<int, Season> getPause(bool forsed = false, int timeout = 30 * 60)
        {
#if DEBUG
            var start = DateTime.UtcNow;
#endif
            forsed = forsed || "1" == DB.Instance.OptionGet("needListUpdate");
            var results = new Dictionary<int, Season>();
            try
            {
                var content = downloadPause(forsed, timeout);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var tabs_result = Match(content, "<ul[^<>]*class=\"tabs-result\"[^<>]*>(.*?)</ul>", REGEX_ICS);
                    if (!string.IsNullOrEmpty(tabs_result))
                        foreach (Match tab in Regex.Matches(tabs_result,
                            "<li[^<>]*data-tabr=\"([^\"]*)\"[^<>]*>(.*?)</li>", REGEX_ICS))
                        {
                            var type = Regex.Replace(tab.Groups[1].ToString(), "^marks-", "", REGEX_IC);
                            foreach (Match marksElA in Regex.Matches(tab.Value,
                                "<a[^<>]*href=\"([^\"]*)\"[^<>]*>(.*?)</a>", REGEX_ICS))
                            {
                                var item = new Season(marksElA.Groups[1].ToString(), type);
                                item.parsePause(marksElA.Groups[2].ToString());
                                results.Add(item.ID, item);
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }
#if DEBUG
            Console.WriteLine("\tgetPause: {0}", DateTime.UtcNow.Subtract(start).TotalSeconds);
#endif
            return results;
        }

        #endregion

        #region Sidebar

        public string downloadSidebar(string mode = "new", bool forsed = false, int timeout = 60 * 60)
        {
            switch (mode)
            {
                case "pop":
                case "newest":
                case "new":
                    break;
                default:
                    mode = "new";
                    break;
            }

            var url = string.Concat(getURLAjax, "?mode=", mode);
            var cacheItem = DB.Instance.CacheGet(url, timeout);
            if (!cacheItem.isActual || forsed)
                cacheItem.updateContent(DownloadXHR(url,
                    new NameValueCollection {{"ganre", ""}, {"country", ""}, {"block", "0"}, {"main", "1"}}));

            return cacheItem.ToString();
        }

        public Dictionary<int, Season> getSidebar(string mode = "new", bool forsed = false, int timeout = 60 * 60)
        {
            var results = new Dictionary<int, Season>();
            try
            {
                var content = downloadSidebar(mode, forsed, timeout);
                if (!string.IsNullOrWhiteSpace(content))
                    foreach (Match serial in Regex.Matches(content, "<a[^<>]*href=\"([^\"]*)\"[^<>]*>(.*?)</a>",
                        REGEX_ICS))
                    {
                        var cserial = new Season(string.Concat(SERVER_URL, serial.Groups[1].ToString()));
                        cserial.parseSidebar(serial.Groups[2].ToString());
                        if (!results.ContainsKey(cserial.ID))
                            results.Add(cserial.ID, cserial);
                    }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }

            return results;
        }

        #endregion

        #region Compilation

        public string downloadCompilation(int compilationList = 0, int page = 1, bool forsed = false,
            int timeout = 60 * 60)
        {
            var url = string.Concat(getURLAjax, "?compilationList=", compilationList.ToString(), "&page=",
                page.ToString(), "&user=", ProfileID.ToString());
            var cacheItem = DB.Instance.CacheGet(url, timeout);
            if (!cacheItem.isActual || forsed)
                cacheItem.updateContent(DownloadXHR(getURLAjax,
                    new NameValueCollection
                    {
                        {"compilationList", compilationList.ToString()}, {"page", page.ToString()},
                        {"user", ProfileID.ToString()}
                    }));

            return cacheItem.ToString();
        }

        public bool doCompilation(NameValueCollection postData = null)
        {
            var result = DownloadXHR(Instance.getURLAjax, postData);
            var resultjson = SimpleJson.SimpleJson.DeserializeObject<dynamic>(result);
            string id = "", status = "";
            foreach (var _id in resultjson)
                switch (_id.Key)
                {
                    case "status":
                        status = _id.Value.ToString();
                        break;
                    case "id":
                        id = _id.Value.ToString();
                        break;
                }

            return status == "ok" || status == "error" && id == "Сериал уже назначен в данную подборку";
        }

        #endregion

        #region Player

        public string downloadPlayerCacheURL(int id, int serial)
        {
            return string.Concat(getURLPlayer, "?id=", id.ToString(), "&serial=", serial.ToString());
        }

        public DBCache downloadPlayer(int id, int serial, string secure = "", bool forsed = false,
            int timeout = 60 * 60)
        {
            if (!string.IsNullOrEmpty(Secure))
                secure = Secure;
            var url = downloadPlayerCacheURL(id, serial);
            if (!string.IsNullOrEmpty(secure))
            {
                var cacheItem = DB.Instance.CacheGet(url, timeout);
                if (!cacheItem.isActual || forsed)
                    cacheItem.updateContent(DownloadXHR(getURLPlayer,
                        new NameValueCollection
                        {
                            {"id", id.ToString()}, {"serial", serial.ToString()}, {"type", "html5"}, {"secure", secure}
                        }));
                cacheItem.data.Add("secure", secure);

                return cacheItem;
            }

            return new DBCache(url, timeout);
        }

        #endregion
    }
}