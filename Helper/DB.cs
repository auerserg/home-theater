using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;

namespace HomeTheater.Helper
{
    public class DB
    {
        static DB _i;
        private string baseName = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ".db3");
        public SQLiteConnection connection;
        public const string TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DATE_FORMAT = "yyyy-MM-dd";

        private Dictionary<string, string> cachedOptions = new Dictionary<string, string>();
        public DB()
        {
            if (!File.Exists(this.baseName))
            {
                SQLiteConnection.CreateFile(this.baseName);
                _createTables();
                _defaultValues();
            }
            else _createTables();
        }

        public static DB Instance
        {
            get
            {
                if (_i == null)
                {
                    _load();
                }
                return _i;
            }
        }

        public static void _load()
        {
            _i = new DB();
        }

        private void _createTables()
        {
            _ExecuteNonQuery(@"
CREATE TABLE IF NOT EXISTS [options] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [name] text NOT NULL,
    [value] text NOT NULL
);
CREATE TABLE IF NOT EXISTS [http_cache] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [url] text,
    [content] text,
    [create_date] text
);
CREATE TABLE IF NOT EXISTS [season] (
    [id] integer PRIMARY KEY NOT NULL,
    [serial_id] integer,
    [url] text,
    [title] text,
    [title_ru] text,
    [title_en] text,
    [title_original] text,
    [title_full] text,
    [season] integer,
    [genre] text,
    [country] text,
    [release] text,
    [limitation] text,
    [imdb] text,
    [kinopoisk] text,
    [user_comments] integer,
    [user_views_last_day] integer,
    [description] text,
    [marks_current] text,
    [marks_last] text,
    [secure_mark] text,
    [type] text,
    [site_updated] text,
    [updated_date] text
);
CREATE TABLE IF NOT EXISTS [season_related] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [season_id] integer NOT NULL,
    [related_id] integer NOT NULL
);
CREATE TABLE IF NOT EXISTS [compilation] (
    [id] integer PRIMARY KEY NOT NULL,
    [name] text NOT NULL
);
CREATE TABLE IF NOT EXISTS [season_compilation] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [serial_id] integer NOT NULL,
    [compilation_id] integer NOT NULL
);
");
        }
        private void _defaultValues()
        {
            OptionSet("cacheTimeSerial_new", (1 * 24 * 60 * 60).ToString());
            OptionSet("cacheTimeSerial_nonew", (3 * 24 * 60 * 60).ToString());
            OptionSet("cacheTimeSerial_want", (3 * 24 * 60 * 60).ToString());
            OptionSet("cacheTimeSerial_watched", (7 * 24 * 60 * 60).ToString());
            OptionSet("cacheTimeSerial_none", (20 * 24 * 60 * 60).ToString());
            OptionSet("SimultaneousDownloads", (3).ToString());
            OptionSet("NameFiles", "{Collection}\\{SerialName} {Season}\\{SerialName} S{Season}E{Episode} {Translate} {OriginalName}");
            OptionSet("listViewSerialsDisplayIndex", "[3,17,2,1,0,4,5,6,18,19,7,8,9,10,11,12,13,14,15,21,20,16]");
            OptionSet("listViewSerialsWidth", "[0,367,0,0,0,0,0,0,25,150,0,0,0,0,0,0,0,0,0,84,56,0]");
            OptionSet("listViewSerialsAutoSize", "[0,0,0,0,0,0,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1]");
            OptionSet("listViewDownloadWidth", "[556,100,100,200]");
            OptionSet("listViewSerialsView", "");
        }
        private void _checkConnectDataBase()
        {
            if (null == connection || (null != connection && (connection.State.Equals(ConnectionState.Closed) || connection.State.Equals(ConnectionState.Broken))))
            {
                SQLiteFactory factory = (SQLiteFactory)System.Data.Common.DbProviderFactories.GetFactory("System.Data.SQLite");
                SQLiteConnection _connection = (SQLiteConnection)factory.CreateConnection();
                _connection.ConnectionString = "Data Source = " + this.baseName;
                _connection.Open();
                if (_connection.State.Equals(ConnectionState.Open))
                {
                    connection = _connection;
                }
            }
        }
        private int _ExecuteNonQuery(string sql)
        {
            return _ExecuteNonQuery(sql, new Dictionary<string, string> { });
        }
        private int _ExecuteNonQuery(string sql, Dictionary<string, string> data)
        {
            int result = 0;
            if (string.IsNullOrEmpty(sql))
                return result;
            try
            {
                _checkConnectDataBase();
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.CommandType = CommandType.Text;
                foreach (var item in data)
                    command.Parameters.AddWithValue("@" + item.Key, item.Value);
                result = command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
            }
            return result;
        }
        private List<Dictionary<string, string>> _ExecuteReader(string sql)
        {
            return _ExecuteReader(sql, new Dictionary<string, string> { });
        }
        private List<Dictionary<string, string>> _ExecuteReader(string sql, Dictionary<string, string> data)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            try
            {
                _checkConnectDataBase();
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.CommandType = CommandType.Text;
                foreach (var item in data)
                    command.Parameters.AddWithValue("@" + item.Key, item.Value);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Dictionary<string, string> _result = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string field = reader.GetName(i);
                        string value = reader[field].ToString();
                        _result.Add(field, value);
                    }
                    result.Add(_result);
                }
            }
            catch (SQLiteException ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
            }
            return result;
        }

        public void OptionSet(string name, string value = null)
        {
            if (string.IsNullOrEmpty(value))
                value = "";

            var data = new Dictionary<string, string> { { "name", name }, { "value", value } };
            if ("" == OptionGet(name))
            {
                if ("" != value)
                    if (0 < _ExecuteNonQuery(@"INSERT INTO options (name,value) VALUES (@name,@value)", data))
                        cachedOptions.Add(name, value);
            }
            else if ("" != value)
            {
                if (0 < _ExecuteNonQuery(@"UPDATE options SET value = @value WHERE name = @name", data))
                    cachedOptions[name] = value;
            }
            else
            {
                if (0 < _ExecuteNonQuery(@"DELETE FROM options WHERE name = @name", data))
                    cachedOptions.Remove(name);
            }
        }
        public string OptionGet(string name)
        {
            if (cachedOptions.ContainsKey(name))
            {
                return cachedOptions[name];
            }
            var result = _ExecuteReader(@"SELECT value FROM options WHERE name = @name LIMIT 1", new Dictionary<string, string> { { "name", name } });
            if (0 < result.Count)
            {
                cachedOptions.Add(name, result[0]["value"]);
                return cachedOptions[name];
            }

            return "";
        }

        public void CacheSet(string url, string content = null)
        {
            DBCache item = CacheGet(url);
            var data = new Dictionary<string, string> { { "url", url }, { "content", content }, { "create_date", DateTime.UtcNow.ToString(TIME_FORMAT) } };
            if (null == item)
            {
                if (!string.IsNullOrWhiteSpace(content))
                    _ExecuteNonQuery(@"INSERT INTO http_cache (url,content,create_date) VALUES (@url,@content,@create_date)", data);
            }
            else if (!string.IsNullOrWhiteSpace(content))
            {
                _ExecuteNonQuery(@"UPDATE http_cache SET content = @content, create_date = @create_date WHERE url = @url", data);
            }
            else
            {
                _ExecuteNonQuery(@"DELETE FROM http_cache WHERE url = @url", data);
            }
        }
        public string CacheGetContent(string url, int period = 24 * 60 * 60)
        {
            DBCache item = CacheGet(url, period);
            if (null == item)
                return null;

            return item.isActual ? item.content : null;
        }
        public DBCache CacheGet(string url, int period = 24 * 60 * 60)
        {
            var result = _ExecuteReader(@"SELECT url, content, create_date FROM http_cache WHERE url = @url LIMIT 1", new Dictionary<string, string> { { "url", url } });
            if (0 < result.Count)
            {
                DBCache item = new DBCache(result[0]["url"].ToString(), result[0]["content"].ToString(), period);
                DateTime.TryParseExact(result[0]["create_date"].ToString(), TIME_FORMAT, new CultureInfo("en-US"), DateTimeStyles.None, out item.date);
                return item;
            }

            return null;
        }

        public bool SeasonSet(int id, Dictionary<string, string> data)
        {
            if (0 == data.Count)
                return false;
            var dataOld = SeasonGet(id);
            if (0 < dataOld.Count)
            {
                var fields = new List<string>();
                foreach (KeyValuePair<string, string> item in data)
                    if (!dataOld.ContainsKey(item.Key) || dataOld[item.Key] != item.Value)
                    {
                        fields.Add(item.Key + " = @" + item.Key);
                    }
                data.Add("id", id.ToString());
                data.Add("updated_date", DateTime.UtcNow.ToString(TIME_FORMAT));
                if (0 < fields.Count)
                    return 0 < _ExecuteNonQuery(@"UPDATE season SET " + String.Join(", ", fields.ToArray()) + ", updated_date = @updated_date WHERE id = @id", data);
            }
            else
            {
                data.Add("id", id.ToString());
                data.Add("updated_date", DateTime.UtcNow.ToString(TIME_FORMAT));
                var fields = new List<string>(data.Keys);
                return 0 < _ExecuteNonQuery(@"INSERT INTO season (" + String.Join(", ", fields.ToArray()) + ") VALUES (@" + String.Join(", @", fields.ToArray()) + ")", data);
            }

            return false;
        }
        public bool SeasonRemove(int id)
        {
            return 0 < _ExecuteNonQuery(@"DELETE FROM season WHERE id = @id", new Dictionary<string, string> { { "id", id.ToString() } });
        }
        public Dictionary<string, string> SeasonGet(int id)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            var result = _ExecuteReader(@"SELECT season.*, http_cache.create_date  FROM season LEFT JOIN http_cache ON season.url = http_cache.url WHERE season.id = @id LIMIT 1", new Dictionary<string, string> { { "id", id.ToString() } });
            if (0 < result.Count)
                data = result[0];
            return data;
        }

        public Dictionary<int, int> SeasonsGet(int serial_id, int id = 0)
        {
            var data = new Dictionary<int, int>();
            var result = _ExecuteReader(@"SELECT id,season FROM season WHERE serial_id = @serial_id AND id <> @id ORDER BY season", new Dictionary<string, string> { { "serial_id", serial_id.ToString() }, { "id", id.ToString() } });

            for (int i = 0; i < result.Count; i++)
            {
                int season = !string.IsNullOrEmpty(result[i]["season"].ToString()) ? int.Parse(result[i]["season"].ToString()) : 0;
                if (!data.ContainsKey(season))
                    data.Add(season, int.Parse(result[i]["id"].ToString()));
            }
            return data;
        }

        public List<int> RelatedGet(int seasonId)
        {
            List<int> data = new List<int>();
            var result = _ExecuteReader(@"SELECT DISTINCT related_id FROM season_related WHERE season_id = @season_id ORDER BY related_id", new Dictionary<string, string> { { "season_id", seasonId.ToString() } });
            for (int i = 0; i < result.Count; i++)
            {
                int season = int.Parse(result[i]["related_id"].ToString());
                if (0 < season && !data.Contains(season))
                    data.Add(season);
            }

            return data;
        }
        public bool RelatedSet(int seasonId, List<int> related)
        {
            if (0 >= related.Count)
                return false;
            var keysOld = RelatedGet(seasonId);

            var data = new Dictionary<string, string> { { "season_id", seasonId.ToString() } };
            var fields = new List<string>();
            for (int i = 0; i < related.Count; i++)
                if (!keysOld.Contains(related[i]))
                {
                    fields.Add("(@season_id, @related_id" + i + ")");
                    data.Add("related_id" + i, related[i].ToString());
                }
                else
                    keysOld.Remove(related[i]);
            string sql = "";
            if (0 < keysOld.Count)
                sql += @"DELETE FROM season_related WHERE season_id = @season_id AND related_id IN(" + String.Join(", ", keysOld.ToArray()) + ");";
            if (0 < fields.Count)
                sql += @"INSERT INTO season_related (season_id, related_id) VALUES " + String.Join(", ", fields.ToArray()) + ";";

            return 0 < _ExecuteNonQuery(sql, data);
        }
        public bool RelatedRemove(int seasonId)
        {
            return 0 < _ExecuteNonQuery(@"DELETE FROM season_related WHERE  season_id = @season_id;", new Dictionary<string, string> { { "season_id", seasonId.ToString() } });
        }

        public Dictionary<int, string> CompilationGet()
        {
            var data = new Dictionary<int, string>();
            var result = _ExecuteReader(@"SELECT id,name FROM compilation");
            for (int i = 0; i < result.Count; i++)
            {
                int id = !string.IsNullOrEmpty(result[i]["id"].ToString()) ? int.Parse(result[i]["id"].ToString()) : 0;
                if (!data.ContainsKey(id))
                    data.Add(id, result[i]["name"]);
            }
            return data;
        }
        public bool CompilationSet(Dictionary<int, string> compilation)
        {
            if (0 == compilation.Count)
                return false;
            var keysOld = new List<int>(CompilationGet().Keys);
            var fields = new List<string>();
            var data = new Dictionary<string, string>();
            int i = 0;
            foreach (var item in compilation)
                if (!keysOld.Contains(item.Key))
                {
                    fields.Add("(@id" + i + ", @name" + i + ")");
                    data.Add("id" + i, item.Key.ToString());
                    data.Add("name" + i, item.Value);
                    i++;
                }
                else
                    keysOld.Remove(item.Key);
            string sql = "";
            if (0 < keysOld.Count)
                sql += @"DELETE FROM compilation WHERE id IN(" + String.Join(", ", keysOld.ToArray()) + ");";
            if (0 < fields.Count)
                sql += @"INSERT INTO compilation (id, name) VALUES " + String.Join(", ", fields.ToArray()) + ";";
            return 0 < _ExecuteNonQuery(sql, data);
        }

        public Dictionary<int, int> CompilationRelationGet()
        {
            var data = new Dictionary<int, int>();
            var result = _ExecuteReader(@"SELECT DISTINCT serial_id,compilation_id FROM season_compilation ORDER BY serial_id ASC;");
            for (int i = 0; i < result.Count; i++)
            {
                int serial_id = int.Parse(result[i]["serial_id"].ToString());
                int compilation_id = int.Parse(result[i]["compilation_id"].ToString());
                if (!data.ContainsKey(serial_id))
                    data.Add(serial_id, compilation_id);
            }
            return data;
        }
        public bool CompilationRelationSet(Dictionary<int, int> compilation)
        {
            if (0 == compilation.Count)
                return false;
            var keysOld = new List<int>(CompilationRelationGet().Keys);
            var fields = new List<string>();
            var data = new Dictionary<string, string>();
            int i = 0;
            foreach (var item in compilation)
                if (!keysOld.Contains(item.Key))
                {
                    fields.Add("(@serial_id" + i + ", @compilation_id" + i + ")");
                    data.Add("serial_id" + i, item.Key.ToString());
                    data.Add("compilation_id" + i, item.Value.ToString());
                    i++;
                }
                else
                    keysOld.Remove(item.Key);
            string sql = "";
            if (0 < keysOld.Count)
                sql += @"DELETE FROM season_compilation WHERE serial_id IN(" + String.Join(", ", keysOld.ToArray()) + ");";
            if (0 < fields.Count)
                sql += @"INSERT INTO season_compilation (serial_id, compilation_id) VALUES " + String.Join(", ", fields.ToArray()) + ";";
            return 0 < _ExecuteNonQuery(sql, data);
        }
        public bool CompilationRelationSet(int serialId, int compilationId)
        {
            return 0 < _ExecuteNonQuery(@"INSERT INTO season_compilation (serial_id, compilation_id) VALUES (@serial_id, @compilation_id);", new Dictionary<string, string> { { "serial_id", serialId.ToString() }, { "compilation_id", compilationId.ToString() } });
        }
        public bool CompilationRelationRemove(int serialId)
        {
            return 0 < _ExecuteNonQuery(@"DELETE FROM season_compilation WHERE serial_id = @serial_id;", new Dictionary<string, string> { { "serial_id", serialId.ToString() } });
        }
    }
}
