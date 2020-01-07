using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace HomeTheater.Serial
{
    class APIServerCompilation
    {
        const string ACTIVE_COMPILATION = "Active";
        const string WANT_COMPILATION = "Want";
        static APIServerCompilation _i;
        private int ACTIVE_ID = 0;
        private int WANT_ID = 0;
        private Dictionary<int, string> compilation;
        private Dictionary<int, List<int>> serialRelation;
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
        private string getCompilationName(int compid)
        {
            var result = "";
            if (compilation.ContainsKey(compid))
            {
                result = compilation[compid];
            }
            return result;
        }
        private int getCompilationId(string name)
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
        private List<int> getSerialCompilationId(int serialid)
        {
            if (serialRelation.ContainsKey(serialid))
            {
                return serialRelation[serialid];
            }

            return new List<int>();
        }
        private int getActiveId()
        {
            if (ACTIVE_ID == 0)
            {
                ACTIVE_ID = getCompilationId(ACTIVE_COMPILATION);
            }
            return ACTIVE_ID;
        }
        private int getWantId()
        {
            if (WANT_ID == 0)
            {
                WANT_ID = getCompilationId(WANT_COMPILATION);
            }
            return WANT_ID;
        }
        private List<int> getBasicId()
        {
            List<int> result = new List<int>();
            result.Add(getWantId());
            result.Add(getActiveId());
            result.Remove(0);
            result.Remove(0);

            return result;
        }
        public string setCompilation(int serialid, string name)
        {
            if (compilationSet(serialid, name))
            {
                List<int> _compilation = getSerialCompilationId(serialid);
                bool update = false;
                for (var i = 0; i < _compilation.Count; i++)
                {
                    if (name != getCompilationName(_compilation[i]))
                    {
                        if (compilationRemovebyId(serialid, _compilation[i], false))
                        {
                            update = true;
                        }
                    }
                }
                if (update)
                {
                    Update();
                }

            }

            return getCompilation(serialid);
        }

        public string getCompilation(int serialid)
        {
            string result = "";
            int compId;
            List<int> _compilation = getSerialCompilationId(serialid);
            bool update = false;
            List<int> _compilationBasic = getBasicId();
            for (var i = 0; i < _compilationBasic.Count; i++)
            {
                compId = _compilationBasic[i];
                if (1 < _compilation.Count && _compilation.Contains(compId))
                {
                    if (compilationRemovebyId(serialid, compId, false))
                    {
                        update = true;
                    }
                }
            }
            if (update)
            {
                Update();
                _compilation = getSerialCompilationId(serialid);
            }
            update = false;
            for (var i = 0; i < _compilation.Count; i++)
            {
                compId = _compilation[i];
                /*
                if (_compilationBasic.Contains(compId)) {
                    continue;
                }
                */
                string name = getCompilationName(compId);
                if (!string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(result))
                {
                    result = name;
                }
                else
                {
                    if (compilationRemovebyId(serialid, compId, false))
                    {
                        update = true;
                    }
                }
            }
            if (update)
            {
                Update();
            }


            return result;
        }
        public bool compilationRemove(int serialid, string name, bool update = true)
        {
            int compid = getCompilationId(name);
            bool result = compilationRemovebyId(serialid, compid, update);
            return result;
        }
        private bool compilationRemovebyId(int serialid, int compid, bool update = true)
        {
            bool result = false;
            if (compid > 0 && serialRelation.ContainsKey(serialid) && serialRelation[serialid].Contains(compid))
            {
                result = doCompilation(new NameValueCollection { { "serial_id", serialid.ToString() }, { "compilationRemove", compid.ToString() } });
                if (result && update)
                    Update();
            }
            return result;
        }

        private bool compilationSet(int serialid, string name)
        {
            int compid = getCompilationId(name);
            bool result = false;
            if (compid > 0)
            {
                if (serialRelation.ContainsKey(serialid) && serialRelation[serialid].Contains(compid))
                {
                    return true;
                }
                result = doCompilation(new NameValueCollection { { "serial_id", serialid.ToString() }, { "compilationSet", compid.ToString() } });
            }
            else
            {
                result = doCompilation(new NameValueCollection { { "serial_id", serialid.ToString() }, { "compilationAdd", name } });

            }
            if (result)
                Update();

            return result;
        }
        private bool doCompilation(NameValueCollection postData = null)
        {
            string result = APIServer.Instance.DownloadXHR(APIServer.Instance.getURLAjax(), postData);
            dynamic resultjson = SimpleJson.SimpleJson.DeserializeObject<dynamic>(result);
            bool status = false;
            foreach (var _id in resultjson)
            {
                if (_id.Key == "status")
                {
                    status = _id.Value.ToString() == "ok";
                }
            }
            return status;
        }

        private string downloadProfile(bool forsed = false)
        {
            return APIServer.Instance.downloadProfile(forsed);
        }

        private string downloadCompilation(int compilationList = 0, bool forsed = false)
        {
            return APIServer.Instance.downloadCompilation(compilationList, forsed);
        }
        private void parseCompilation(int compilationList = 0, bool forsed = false)
        {
            List<int> value;
            string content = downloadProfile(forsed);
            if (!string.IsNullOrWhiteSpace(content))
            {
                compilation = new Dictionary<int, string>();
                serialRelation = new Dictionary<int, List<int>>();
                foreach (Match __ in Regex.Matches(content, "<a[^<>]*data-compid=\"(\\d+)\"[^<>]*>(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    int compid = int.Parse(__.Groups[1].ToString());
                    if (!compilation.ContainsKey(compid))
                    {
                        compilation.Add(compid, __.Groups[2].ToString().Trim());
                    }
                }
            }

            content = downloadCompilation(0, forsed);
            if (!string.IsNullOrWhiteSpace(content))
            {
                foreach (Match __ in Regex.Matches(content, "<span[^<>]*data-serialid=\"(\\d+)\"[^<>]*data-compid=\"(\\d+)\"[^<>]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    int compid = int.Parse(__.Groups[2].ToString());
                    int serialid = int.Parse(__.Groups[1].ToString());

                    if (serialRelation.ContainsKey(serialid))
                    {
                        value = serialRelation[serialid];
                        value.Add(compid);
                        serialRelation[serialid] = value;
                    }
                    else
                    {
                        value = new List<int>();
                        value.Add(compid);
                        serialRelation.Add(serialid, value);
                    }
                }
            }
        }
    }
}
