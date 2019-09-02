using HomeTheater.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace HomeTheater.Serial
{
    class APIServer : APIServerParent
    {
        static APIServer _i;
        const string LOGIN_PATH = "/?mod=login";
        const string REGISTER_PATH = "/?mod=reg";
        const string FORGOT_PATH = "/?mod=recover";
        const string PAUSE_PATH = "/?mod=pause";
        const string MARK_PATH = "/jsonMark.php";
        const string AJAX_PATH = "/ajax.php";
        protected string PROFILE_PATH;
        public static string secureMark;
        public int ProfileID;
        public string login;
        private string password;
        public static APIServer Instance
        {
            get
            {
                if (_i == null)
                {
                    Load();
                }
                return _i;
            }
        }

        public static void Load()
        {
            _i = new APIServer();
        }

        public APIServer(string _login = null, string _password = null)
        {
            login = _login;
            password = _password;
            if (string.IsNullOrWhiteSpace(login))
                login = DB.Instance.getOption("Login");
            if (string.IsNullOrWhiteSpace(password))
                password = DB.Instance.getOption("Password");
        }

        public string getURLForgon()
        {
            return String.Concat(SERVER_URL, FORGOT_PATH);
        }

        public string getURLRegister()
        {
            return String.Concat(SERVER_URL, REGISTER_PATH);
        }

        public string getURLLogin()
        {
            return String.Concat(SERVER_URL, LOGIN_PATH);
        }

        public string getURLPause()
        {
            return String.Concat(SERVER_URL, PAUSE_PATH);
        }

        public string getURLMark()
        {
            return String.Concat(SERVER_URL, MARK_PATH);
        }

        public string getURLProfile()
        {
            if (String.IsNullOrEmpty(PROFILE_PATH))
            {
                return "";
            }
            return String.Concat(SERVER_URL, PROFILE_PATH);
        }

        public string getURLAjax()
        {
            return String.Concat(SERVER_URL, AJAX_PATH);
        }

        public bool LogedIn(string _login = null, string _password = null)
        {
            if (String.IsNullOrEmpty(_login))
            {
                _login = login;
            }
            if (String.IsNullOrEmpty(_password))
            {
                _password = password;
            }

            string content = Download(getURLLogin(), new NameValueCollection { { "login", _login }, { "password", _password } });

            return isLogedIn(content);
        }

        public bool isLogedIn(string content = null)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                content = Download(getURLLogin());
            }

            bool result = !Regex.IsMatch(content, @"loginbox-login", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (result)
            {
                setURLProfile(content);
            }

            return result;
        }

        private void setURLProfile(string content)
        {
            string url = Match(content, "<a[^<>]*href=\"(/profile/[0-9]+)\"[^<>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled, 1);
            if (!string.IsNullOrEmpty(url))
            {
                PROFILE_PATH = url;
                ProfileID = int.Parse(Match(url, "/([0-9]+)$", RegexOptions.Compiled, 1));
            }
        }

        public string downloadPause(bool forsed = false)
        {
            string url = getURLPause();
            string content = DB.Instance.getCache(url, 30 * 60);
            if (string.IsNullOrWhiteSpace(content) || forsed)
            {
                content = Download(url);
                if (!string.IsNullOrWhiteSpace(content))
                    DB.Instance.setCache(url, content);
            }

            return content;
        }
        public static string Match(string input, string pattern, RegexOptions options, int index = 0)
        {
            string result = null;
            Match resultmatch = Regex.Match(input, pattern, options);
            if (resultmatch.Success && index < resultmatch.Groups.Count)
            {
                result = resultmatch.Groups[index].ToString().Trim();
            }
            return result;
        }

        public List<Serial> getPause(bool forsed = false)
        {
            string content = this.downloadPause(forsed);
            var results = new List<Serial>();
            if (!string.IsNullOrWhiteSpace(content))
            {
                string tabs_result = Match(content, "<ul[^<>]*class=\"tabs-result\"[^<>]*>(.*?)</ul>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (!string.IsNullOrEmpty(tabs_result))
                {
                    foreach (Match tab in Regex.Matches(tabs_result, "<li[^<>]*data-tabr=\"([^\"]*)\"[^<>]*>(.*?)</li>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                    {
                        string type = Regex.Replace(tab.Groups[1].ToString(), "^marks-", "", RegexOptions.IgnoreCase);
                        foreach (Match serial in Regex.Matches(tab.Value, "<a[^<>]*href=\"([^\"]*)\"[^<>]*>(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                        {
                            var cserial = new Serial(serial.Groups[1].ToString());
                            string text_serial = Match(serial.Groups[2].ToString(), "<div class=\"pgs-marks-seas\">(.*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline, 1);
                            if (!string.IsNullOrEmpty(text_serial))
                                cserial.Season = int.Parse(Regex.Replace(text_serial, "[^0-9]*", ""));

                            text_serial = Match(serial.Groups[2].ToString(), "<div class=\"pgs-marks-name\">(.*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline, 1);
                            if (!string.IsNullOrEmpty(text_serial))
                                cserial.TitleRU = text_serial;

                            text_serial = Match(serial.Groups[2].ToString(), "<div class=\"pgs-marks-current\">[^<>]*<strong>(.*?)</strong>[^<>]*</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline, 1);
                            if (!string.IsNullOrEmpty(text_serial))
                                cserial.marksLast = text_serial;

                            if (type != cserial.Type)
                                cserial.Type = type;

                            results.Add(cserial);
                        }
                    }
                }
            }
            return results;
        }

        private string doMarks(NameValueCollection postData = null)
        {
            return DownloadXHR(getURLMark(), postData);
        }

        private bool setMarks(NameValueCollection postData = null)
        {
            string result = doMarks(postData);
            dynamic resultjson = SimpleJson.SimpleJson.DeserializeObject<dynamic>(result);
            string msg = null;
            bool auth = false;
            foreach (var id in resultjson)
            {
                switch (id.Key)
                {
                    case "msg":
                        msg = id.Value.ToString();
                        break;
                    case "auth":
                        auth = id.Value;
                        break;
                }
            }
            return auth && "success" == msg;
        }

        public bool setWantToSee(int ID)
        {
            if (0 >= ID)
                return false;
            return this.setMarks(new NameValueCollection { { "id", ID.ToString() }, { "seria", "-1" }, { "wanttosee", "true" }, { "minute", "0" }, { "second", "0" } });
        }

        public bool setDelSee(int ID)
        {
            if (0 >= ID)
                return false;
            return this.setMarks(new NameValueCollection { { "delId", ID.ToString() } });
        }

        public bool setWatched(int ID)
        {
            if (0 >= ID)
                return false;
            return this.setMarks(new NameValueCollection { { "id", ID.ToString() }, { "seria", "-2" }, { "watched", "true" }, { "minute", "0" }, { "second", "0" } });
        }

        public bool setPauseAdd(int ID, string Series = "")
        {
            if (0 >= ID || string.IsNullOrWhiteSpace(Series))
                return false;
            return this.setMarks(new NameValueCollection { { "id", ID.ToString() }, { "seria", Series }, { "pauseadd", "true" }, { "minute", "0" }, { "second", "0" }, { "tran", "0" } });
        }

        private string doAjax(string url = "", NameValueCollection postData = null)
        {
            return DownloadXHR(String.Concat(getURLAjax(), url), postData);
        }

        public string downloadSidebar(string mode = "new", bool forsed = false) {
            switch (mode) {
                case "pop":
                case "newest":
                case "new":
                    break;
                default:
                    mode = "new";
                    break;
            }
            string url = String.Concat(getURLAjax(), "?mode=", mode);
            string content = DB.Instance.getCache(url, 60 * 60);

            if (string.IsNullOrWhiteSpace(content) || forsed)
            {
                content = DownloadXHR(url, new NameValueCollection { { "ganre", "" }, { "country", "" }, { "block", "0" }, { "main", "1" } });
                if (!string.IsNullOrWhiteSpace(content))
                    DB.Instance.setCache(url, content);
            }

            return content;
        }

        public List<Serial> getSidebar(string mode = "new", bool forsed = false)
        {
            string content = this.downloadSidebar(mode, forsed);
            var results = new List<Serial>();
            if (!string.IsNullOrWhiteSpace(content))
            {
                foreach (Match serial in Regex.Matches(content, "<a[^<>]*href=\"([^\"]*)\"[^<>]*>(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    var cserial = new Serial(String.Concat(SERVER_URL,serial.Groups[1].ToString()));
                    string text_serial = Match(serial.Groups[2].ToString(), "<div class=\"rside-ss\">(.*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline, 1);
                    if (!string.IsNullOrEmpty(text_serial)) {
                        Match matchseason = Regex.Match(text_serial, "^(.*?)<span>([^<>]*)</span>$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        if (matchseason.Success)
                        {
                            text_serial = Regex.Replace(matchseason.Groups[1].ToString(), "[^0-9]*", "");
                            if (!string.IsNullOrEmpty(text_serial))
                            {
                                cserial.Season = int.Parse(text_serial);
                            }
                            cserial.marksLast = Regex.Replace(matchseason.Groups[2].ToString(), " (\\([^\\(\\)]+\\)|серия)", "");
                        }
                        
                    }
                        

                    text_serial = Match(serial.Groups[2].ToString(), "<div class=\"rside-t\">(.*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline, 1);
                    if (!string.IsNullOrEmpty(text_serial))
                        cserial.TitleRU = text_serial;

                    text_serial = Match(serial.Groups[2].ToString(), "<div class=\"rside-t_en\">(.*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline, 1);
                    if (!string.IsNullOrEmpty(text_serial))
                        cserial.TitleEN = text_serial;

                    results.Add(cserial);
                }
            }
            return results;
        }
    }
}
