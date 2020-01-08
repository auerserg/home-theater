using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace HomeTheater.Serial
{
    class APIServerCompilation
    {
        public const string ACTIVE_COMPILATION = "Active";
        public const string WANT_COMPILATION = "Want";
        static APIServerCompilation _i;
        private Dictionary<int, string> compilation;
        private Dictionary<int, int> serialRelation;
        public static APIServerCompilation Instance
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
            _i = new APIServerCompilation();
        }
        public APIServerCompilation()
        {
            parseCompilation();
        }
        public APIServerCompilation Update()
        {
            parseCompilation(0, true);

            return this;
        }
        public string getCompilation(int serialid)
        {
            if (serialRelation.ContainsKey(serialid))
            {
                int compId = serialRelation[serialid];
                if (0 < compId)
                {
                    if (compilation.ContainsKey(compId))
                    {
                        return compilation[compId];
                    }
                    else
                    {
                        removeCompilation(serialid, compId);
                    }
                }

            }
            return "";
        }
        public string setCompilation(int serialid, string name)
        {
            int compid = _getCompilationId(name);
            if (0 < compid)
            {
                return setCompilation(serialid, compid);
            }
            else
            {
                _setCompilation(serialid, name);
            }
            return getCompilation(serialid);
        }
        public string setCompilation(int serialid, int compid)
        {
            if (0 < compid && !serialRelation.ContainsKey(serialid))
            {
                _setCompilation(serialid, compid);
            }

            return getCompilation(serialid);
        }

        public bool removeCompilation(int serialid, string name)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(name))
            {
                int compid = _getCompilationId(name);
                if (0 < compid)
                    result = removeCompilation(serialid, compid);
            }
            return result;
        }
        public bool removeCompilation(int serialid)
        {
            if (serialRelation.ContainsKey(serialid))
            {
                return removeCompilation(serialid, serialRelation[serialid]);
            }
            return false;
        }

        public bool removeCompilation(int serialid, int compid)
        {
            bool result = false;
            if (compid > 0 && serialid > 0)
            {
                result = doCompilation(new NameValueCollection { { "serial_id", serialid.ToString() }, { "compilationRemove", compid.ToString() } });
                if (result && serialRelation.ContainsKey(serialid) && serialRelation[serialid] == compid)
                {
                    serialRelation.Remove(serialid);
                }
            }
            return result;
        }

        private int _getCompilationId(string name)
        {
            foreach (KeyValuePair<int, string> kvp in compilation)
            {
                if (kvp.Value.ToLower() == name.ToLower())
                {
                    return kvp.Key;
                }
            }
            return 0;
        }
        private bool _setCompilation(int serialid, string name)
        {
            int compid = _getCompilationId(name);
            bool result = false;
            if (0 < compid)
            {
                result = _setCompilation(serialid, compid);
            }
            else
            {
                result = doCompilation(new NameValueCollection { { "serial_id", serialid.ToString() }, { "compilationAdd", name } });
                if (result)
                {
                    Update();
                }
            }
            return result;
        }
        private bool _setCompilation(int serialid, int compid)
        {
            bool result = false;
            if (compid > 0 && serialid > 0)
            {
                result = doCompilation(new NameValueCollection { { "serial_id", serialid.ToString() }, { "compilationSet", compid.ToString() } });
                if (result)
                {
                    if (serialRelation.ContainsKey(serialid))
                    {
                        serialRelation[serialid] = compid;

                    }
                    else
                    {
                        serialRelation.Add(serialid, compid);
                    }
                }

            }
            return result;
        }


        private bool doCompilation(NameValueCollection postData = null)
        {
            string result = APIServer.Instance.DownloadXHR(APIServer.Instance.getURLAjax(), postData);

            dynamic resultjson = SimpleJson.SimpleJson.DeserializeObject<dynamic>(result);
            string id = "", status = "";
            foreach (var _id in resultjson)
                switch (_id.Key)
                {
                    case "status": status = _id.Value.ToString(); break;
                    case "id": id = _id.Value.ToString(); break;
                }
            if (status == "error" && id == "Сериал уже назначен в данную подборку")
            {
                Update();
            }

            return status == "ok" || (status == "error" && id == "Сериал уже назначен в данную подборку");
        }

        private string downloadProfile(bool forsed = false)
        {
            return APIServer.Instance.downloadProfile(forsed);
        }

        private string downloadCompilation(int compilationList = 0, int page = 1, bool forsed = false)
        {
            return APIServer.Instance.downloadCompilation(compilationList, page, forsed);
        }
        private void parseCompilation(int compilationList = 0, bool forsed = false)
        {
            string content = downloadProfile(forsed);
            if (!string.IsNullOrWhiteSpace(content))
            {
                compilation = new Dictionary<int, string>();
                foreach (Match __ in Regex.Matches(content, "<a[^<>]*data-compid=\"(\\d+)\"[^<>]*>(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    int compid = int.Parse(__.Groups[1].ToString());
                    if (!compilation.ContainsKey(compid))
                    {
                        compilation.Add(compid, __.Groups[2].ToString().Trim());
                    }
                }
            }

            content = downloadCompilation(compilationList, 1, forsed);
            if (!string.IsNullOrWhiteSpace(content))
            {
                int page = 1;
                foreach (Match __ in Regex.Matches(content, " data-page=\"([0-9]+)\"", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    int _page = int.Parse(__.Groups[1].ToString());
                    if (page < _page)
                        page = _page;
                }
                if (1 < page)
                {
                    for (var i = 2; i <= page; i++)
                    {
                        content += downloadCompilation(compilationList, i, forsed);
                    }
                }

                serialRelation = new Dictionary<int, int>();
                foreach (Match __ in Regex.Matches(content, "<span[^<>]*data-serialid=\"(\\d+)\"[^<>]*data-compid=\"(\\d+)\"[^<>]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    int compid = int.Parse(__.Groups[2].ToString());
                    int serialid = int.Parse(__.Groups[1].ToString());

                    if (serialRelation.ContainsKey(serialid))
                    {
                        removeCompilation(serialid, compid);
                    }
                    else
                    {
                        serialRelation.Add(serialid, compid);
                    }
                }
            }
        }
    }
}
