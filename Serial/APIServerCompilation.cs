using HomeTheater.Helper;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace HomeTheater.Serial
{
    class APIServerCompilation
    {
        static APIServerCompilation _i;
        private Dictionary<int, string> compilation = new Dictionary<int, string>();
        private Dictionary<int, int> relation = new Dictionary<int, int>();
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
            _CompilationLoad();
            _RelationLoad();
            if (0 == compilation.Count || 0 == relation.Count)
                UpdateAsync(true);
        }
        public void CompilationUpdate(Dictionary<int, string> data)
        {
            bool needSave = false;
            if (0 < data.Count)
                foreach (var item in data)
                    if (!compilation.ContainsKey(item.Key))
                    {
                        needSave = true;
                        compilation.Add(item.Key, item.Value);
                    }
            if (needSave)
                _CompilationSave();
        }
        private async void _CompilationSave()
        {
            DB.Instance.CompilationSet(compilation);
        }
        private void _CompilationLoad()
        {
            compilation = DB.Instance.CompilationGet();
        }
        public void RelationUpdate(int serialId, int compilationId)
        {
            if (!relation.ContainsKey(serialId))
            {
                relation.Add(serialId, compilationId);
                DB.Instance.CompilationRelationSet(serialId, compilationId);
            }
        }
        private async void _RelationSave()
        {
            DB.Instance.CompilationRelationSet(relation);
        }
        private void _RelationLoad()
        {
            relation = DB.Instance.CompilationRelationGet();
        }

        public string Set(int serialId, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                int compilationId = GetId(name);
                if (0 < serialId && 0 < compilationId)
                    return Set(serialId, compilationId);
                if (0 < serialId && !relation.ContainsKey(serialId))
                    if (_do(new NameValueCollection { { "serial_id", serialId.ToString() }, { "compilationAdd", name } }))
                    {
                        Update(true, true, false);
                        compilationId = GetId(name);
                        relation.Add(serialId, compilationId);
                        DB.Instance.CompilationRelationSet(serialId, compilationId);
                    }
            }
            return Get(serialId);
        }
        public string Set(int serialId, int compilationId)
        {
            if (compilationId > 0 && serialId > 0 && !relation.ContainsKey(serialId))
                if (_do(new NameValueCollection { { "serial_id", serialId.ToString() }, { "compilationSet", compilationId.ToString() } }))
                {
                    relation.Add(serialId, compilationId);
                    DB.Instance.CompilationRelationSet(serialId, compilationId);
                }
            return Get(serialId);
        }

        public bool Remove(int serialId, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                int compilationId = GetId(name);
                if (0 < compilationId)
                    return Remove(serialId, compilationId);
            }
            return false;
        }
        public bool Remove(int serialId)
        {
            if (relation.ContainsKey(serialId))
                return Remove(serialId, relation[serialId]);
            return false;
        }
        public bool Remove(int serialId, int compilationId)
        {
            if (compilationId > 0 && serialId > 0 && relation.ContainsKey(compilationId))
                if (_do(new NameValueCollection { { "serial_id", serialId.ToString() }, { "compilationRemove", compilationId.ToString() } }))
                {
                    relation.Remove(serialId);
                    DB.Instance.CompilationRelationRemove(serialId);
                    return true;
                }
            return false;
        }
        private bool _Remove(int serialId, int compilationId)
        {
            if (compilationId > 0 && serialId > 0)
                return _do(new NameValueCollection { { "serial_id", serialId.ToString() }, { "compilationRemove", compilationId.ToString() } });
            return false;
        }

        public string Get(int serialId)
        {
            if (relation.ContainsKey(serialId))
                return GetName(relation[serialId]);
            return "";
        }
        public int GetId(string name)
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
        public string GetName(int id)
        {
            if (compilation.ContainsKey(id))
                return compilation[id];
            return "";
        }

        private bool _do(NameValueCollection postData = null)
        {
            return APIServer.Instance.doCompilation(postData);
        }
        public async void UpdateAsync(bool forsed = true, bool compilation = true, bool relation = true)
        {
            Update(forsed, compilation, relation);
        }
        public void Update(bool forsed = true, bool compilation = true, bool relation = true)
        {
            if (compilation)
            {
                _parseProfileCompilation(forsed);
                _CompilationSave();
            }
            if (relation)
            {
                _parseCompilation(0, forsed);
                _RelationSave();
            }
        }
        private string _downloadProfile(bool forsed = false)
        {
            return APIServer.Instance.downloadProfile(forsed);
        }
        private string _downloadCompilation(int compilationList = 0, int page = 1, bool forsed = false)
        {
            return APIServer.Instance.downloadCompilation(compilationList, page, forsed);
        }
        private void _parseProfileCompilation(bool forsed = false)
        {
            string content = _downloadProfile(forsed);
            if (!string.IsNullOrWhiteSpace(content))
            {
                compilation = new Dictionary<int, string>();
                foreach (Match __ in Regex.Matches(content, "<a[^<>]*data-compid=\"([0-9]+)\"[^<>]*>(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    int compilationId = int.Parse(__.Groups[1].ToString());
                    if (!compilation.ContainsKey(compilationId))
                    {
                        compilation.Add(compilationId, __.Groups[2].ToString().Trim());
                    }
                }
                if (compilation.ContainsKey(0))
                    compilation.Remove(0);
            }
        }
        private void _parseCompilation(int compilationList = 0, bool forsed = false)
        {
            string content = _downloadCompilation(compilationList, 1, forsed);
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
                        content += _downloadCompilation(compilationList, i, forsed);
                    }
                }

                relation = new Dictionary<int, int>();
                foreach (Match __ in Regex.Matches(content, "<span[^<>]*data-serialid=\"(\\d+)\"[^<>]*data-compid=\"(\\d+)\"[^<>]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    int compilationId = int.Parse(__.Groups[2].ToString());
                    int serialId = int.Parse(__.Groups[1].ToString());

                    if (relation.ContainsKey(serialId))
                    {
                        Remove(serialId, compilationId);
                    }
                    else
                    {
                        relation.Add(serialId, compilationId);
                    }
                }
            }
        }
    }
}
