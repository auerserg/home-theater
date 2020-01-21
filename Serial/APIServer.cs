using HomeTheater.Helper;
using HomeTheater.Serial.data;
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
        const string PLAYER_PATH = "/player.php";
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
                login = DB.Instance.OptionGet("Login");
            if (string.IsNullOrWhiteSpace(password))
                password = DB.Instance.OptionGet("Password");
        }

        public string getURLForgon
        {
            get => SERVER_URL + FORGOT_PATH;
        }

        public string getURLRegister
        {
            get => SERVER_URL + REGISTER_PATH;
        }

        public string getURLLogin
        {
            get => SERVER_URL + LOGIN_PATH;
        }

        public string getURLPause
        {
            get => SERVER_URL + PAUSE_PATH;
        }

        public string getURLMark
        {
            get => SERVER_URL + MARK_PATH;
        }
        public string getURLProfile
        {
            get => !string.IsNullOrEmpty(PROFILE_PATH) ? SERVER_URL + PROFILE_PATH : "";
        }
        public string getURLAjax
        {
            get => SERVER_URL + AJAX_PATH;
        }
        public string getURLPlayer
        {
            get => SERVER_URL + PLAYER_PATH;
        }

        public bool LogedIn(string _login = null, string _password = null)
        {
            string __login = login;
            string __password = password;
            if (String.IsNullOrEmpty(_login))
            {
                _login = __login;
            }
            if (String.IsNullOrEmpty(_password))
            {
                _password = __password;
            }
            bool result = false;
            try
            {
                string content = Download(getURLLogin, new NameValueCollection { { "login", _login }, { "password", _password } });
                result = isLogedIn(content);
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif
            }
            return result;
        }

        public bool isLogedIn(string content = null)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                content = Download(getURLLogin);
            }

            bool result = !Regex.IsMatch(content, @"loginbox-login", REGEX_IC);

            if (result)
            {
                setURLProfile(content);
            }

            return result;
        }

        private void setURLProfile(string content)
        {
            string url = Match(content, "<a[^<>]*href=\"(/profile/[0-9]+)\"[^<>]*>", REGEX_IC, 1);
            if (!string.IsNullOrEmpty(url))
            {
                PROFILE_PATH = url;
                ProfileID = int.Parse(Match(url, "/([0-9]+)$", REGEX_C, 1));
            }
        }

        public string downloadPause(bool forsed = false)
        {
            string url = getURLPause;
            string content = DB.Instance.CacheGetContent(url, 30 * 60);
            if (string.IsNullOrWhiteSpace(content) || forsed)
            {
                content = Download(url);
                if (!string.IsNullOrWhiteSpace(content))
                    DB.Instance.CacheSet(url, content);
            }

            return content;
        }

        public List<SerialSeason> getPause(bool forsed = false)
        {
            string content = downloadPause(forsed);
            var results = new List<SerialSeason>();
            if (!string.IsNullOrWhiteSpace(content))
            {
                string tabs_result = Match(content, "<ul[^<>]*class=\"tabs-result\"[^<>]*>(.*?)</ul>", REGEX_ICS);
                if (!string.IsNullOrEmpty(tabs_result))
                {
                    foreach (Match tab in Regex.Matches(tabs_result, "<li[^<>]*data-tabr=\"([^\"]*)\"[^<>]*>(.*?)</li>", REGEX_ICS))
                    {
                        string type = Regex.Replace(tab.Groups[1].ToString(), "^marks-", "", REGEX_IC);
                        foreach (Match serial in Regex.Matches(tab.Value, "<a[^<>]*href=\"([^\"]*)\"[^<>]*>(.*?)</a>", REGEX_ICS))
                        {
                            var cserial = new SerialSeason(serial.Groups[1].ToString());
                            cserial.parsePause(serial.Groups[2].ToString());
                            if (type != cserial.Type)
                                cserial.Type = type;
                            results.Add(cserial);
                        }
                    }
                }
            }
            return results;
        }

        public markresponse doMarks(NameValueCollection postData = null)
        {
            string result = DownloadXHR(getURLMark, postData);
            markresponse resultjson = SimpleJson.SimpleJson.DeserializeObject<markresponse>(result);
            return resultjson;
        }

        public string downloadSidebar(string mode = "new", bool forsed = false)
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
            string url = String.Concat(getURLAjax, "?mode=", mode);
            string content = DB.Instance.CacheGetContent(url, 60 * 60);

            if (string.IsNullOrWhiteSpace(content) || forsed)
            {
                content = DownloadXHR(url, new NameValueCollection { { "ganre", "" }, { "country", "" }, { "block", "0" }, { "main", "1" } });
                if (!string.IsNullOrWhiteSpace(content))
                    DB.Instance.CacheSet(url, content);
            }

            return content;
        }

        public List<SerialSeason> getSidebar(string mode = "new", bool forsed = false)
        {
            string content = this.downloadSidebar(mode, forsed);
            var results = new List<SerialSeason>();
            if (!string.IsNullOrWhiteSpace(content))
            {
                foreach (Match serial in Regex.Matches(content, "<a[^<>]*href=\"([^\"]*)\"[^<>]*>(.*?)</a>", REGEX_ICS))
                {
                    var cserial = new SerialSeason(String.Concat(SERVER_URL, serial.Groups[1].ToString()));
                    cserial.parseSidebar(serial.Groups[2].ToString());

                    results.Add(cserial);
                }
            }
            return results;
        }
        public string downloadCompilation(int compilationList = 0, int page = 1, bool forsed = false)
        {
            string url = string.Concat(getURLAjax, "?compilationList=", compilationList.ToString(), "&page=", page.ToString(), "&user=", ProfileID.ToString());
            string content = DB.Instance.CacheGetContent(url, 30 * 60);
            if (string.IsNullOrWhiteSpace(content) || forsed)
            {
                content = DownloadXHR(getURLAjax, new NameValueCollection { { "compilationList", compilationList.ToString() }, { "page", page.ToString() }, { "user", ProfileID.ToString() } });
                if (!string.IsNullOrWhiteSpace(content))
                    DB.Instance.CacheSet(url, content);
            }

            return content;
        }
        public bool doCompilation(NameValueCollection postData = null)
        {
            string result = DownloadXHR(APIServer.Instance.getURLAjax, postData);
            dynamic resultjson = SimpleJson.SimpleJson.DeserializeObject<dynamic>(result);
            string id = "", status = "";
            foreach (var _id in resultjson)
                switch (_id.Key)
                {
                    case "status": status = _id.Value.ToString(); break;
                    case "id": id = _id.Value.ToString(); break;
                }

            return status == "ok" || (status == "error" && id == "Сериал уже назначен в данную подборку");
        }

        public string downloadProfile(bool forsed = false, int timeout = 30 * 60)
        {
            string url = getURLProfile;
            string content = DB.Instance.CacheGetContent(url, timeout);
            if (string.IsNullOrWhiteSpace(content) || forsed)
            {
                content = Download(url);
                if (!string.IsNullOrWhiteSpace(content))
                    DB.Instance.CacheSet(url, content);
            }

            return content;
        }
    }
}
