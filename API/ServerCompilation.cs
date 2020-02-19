using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HomeTheater.Helper;

namespace HomeTheater.API
{
    public class ServerCompilation
    {
        private static ServerCompilation _i;
        private Dictionary<int, string> compilation = new Dictionary<int, string>();
        private Dictionary<int, int> relation = new Dictionary<int, int>();

        public ServerCompilation()
        {
            _CompilationLoad();
            _RelationLoad();
            if (0 == compilation.Count || 0 == relation.Count)
                UpdateAsync();
        }

        public static ServerCompilation Instance
        {
            get
            {
                if (_i == null) Load();
                return _i;
            }
        }

        public static void Load()
        {
            _i = new ServerCompilation();
        }

        public void CompilationUpdate(Dictionary<int, string> data)
        {
            var needSave = false;
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
            await Task.Run(() =>
            {
                try
                {
                    DB.Instance.CompilationSet(compilation);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            }).ConfigureAwait(true);
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

        private async Task _RelationSaveAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    DB.Instance.CompilationRelationSet(relation);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            }).ConfigureAwait(true);
        }

        private void _RelationLoad()
        {
            relation = DB.Instance.CompilationRelationGet();
        }

        public string Set(int serialId, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var compilationId = GetId(name);
                if (0 < serialId && 0 < compilationId)
                    return Set(serialId, compilationId);
                if (0 < serialId && !relation.ContainsKey(serialId))
                    if (_do(new NameValueCollection {{"serial_id", serialId.ToString()}, {"compilationAdd", name}}))
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
                if (_do(new NameValueCollection
                    {{"serial_id", serialId.ToString()}, {"compilationSet", compilationId.ToString()}}))
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
                var compilationId = GetId(name);
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
                if (_do(new NameValueCollection
                    {{"serial_id", serialId.ToString()}, {"compilationRemove", compilationId.ToString()}}))
                {
                    relation.Remove(serialId);
                    DB.Instance.CompilationRelationRemove(serialId);
                    return true;
                }

            return false;
        }

        //private bool _Remove(int serialId, int compilationId)
        //{
        //    if (compilationId > 0 && serialId > 0)
        //        return _do(new NameValueCollection
        //            {{"serial_id", serialId.ToString()}, {"compilationRemove", compilationId.ToString()}});
        //    return false;
        //}

        public string Get(int serialId)
        {
            if (relation.ContainsKey(serialId))
                return GetName(relation[serialId]);
            return "";
        }

        public int GetId(string name)
        {
            foreach (var kvp in compilation)
                if (kvp.Value.ToLower() == name.ToLower())
                    return kvp.Key;
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
            return Server.Instance.doCompilation(postData);
        }

        public async void UpdateAsync(bool forsed = true, bool compilation = true, bool relation = true)
        {
            await Task.Run(() =>
            {
                try
                {
                    Update(forsed, compilation, relation);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex);
                }
            }).ConfigureAwait(true);
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
                _ = _RelationSaveAsync();
            }
        }

        private string _downloadProfile(bool forsed = false)
        {
            return Server.Instance.downloadProfile(forsed);
        }

        private string _downloadCompilation(int compilationList = 0, int page = 1, bool forsed = false)
        {
            return Server.Instance.downloadCompilation(compilationList, page, forsed);
        }

        private void _parseProfileCompilation(bool forsed = false)
        {
            var content = _downloadProfile(forsed);
            if (!string.IsNullOrWhiteSpace(content))
            {
                compilation = new Dictionary<int, string>();
                foreach (Match __ in Regex.Matches(content, "<a[^<>]*data-compid=\"([0-9]+)\"[^<>]*>(.*?)</a>",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    var compilationId = int.Parse(__.Groups[1].ToString());
                    if (!compilation.ContainsKey(compilationId))
                        compilation.Add(compilationId, __.Groups[2].ToString().Trim());
                }

                if (compilation.ContainsKey(0))
                    compilation.Remove(0);
            }
        }

        private void _parseCompilation(int compilationList = 0, bool forsed = false)
        {
            var content = _downloadCompilation(compilationList, 1, forsed);
            if (!string.IsNullOrWhiteSpace(content))
            {
                var page = 1;
                foreach (Match __ in Regex.Matches(content, " data-page=\"([0-9]+)\"",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    var _page = int.Parse(__.Groups[1].ToString());
                    if (page < _page)
                        page = _page;
                }

                if (1 < page)
                    for (var i = 2; i <= page; i++)
                        content += _downloadCompilation(compilationList, i, forsed);

                relation = new Dictionary<int, int>();
                foreach (Match __ in Regex.Matches(content,
                    "<span[^<>]*data-serialid=\"(\\d+)\"[^<>]*data-compid=\"(\\d+)\"[^<>]*>",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    var compilationId = int.Parse(__.Groups[2].ToString());
                    var serialId = int.Parse(__.Groups[1].ToString());

                    if (relation.ContainsKey(serialId))
                        Remove(serialId, compilationId);
                    else
                        relation.Add(serialId, compilationId);
                }
            }
        }
    }
}