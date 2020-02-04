using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace HomeTheater.Helper
{
    public class DB
    {
        public const string TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DATE_FORMAT = "yyyy-MM-dd";
        private static DB _i;
        private readonly string baseName = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ".db3");

        private readonly Dictionary<string, string> cachedOptions = new Dictionary<string, string>();
        public SQLiteConnection connection;

        public DB()
        {
            if (!File.Exists(baseName))
            {
                SQLiteConnection.CreateFile(baseName);
                _createTables();
                _defaultValues();
            }
            else
            {
                _createTables();
            }
        }

        public static DB Instance
        {
            get
            {
                if (_i == null) _load();
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
    [type] text,
    [site_updated] text,
    [secure] text,
    [created_date] text,
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
CREATE TABLE IF NOT EXISTS [translate] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [key] integer NOT NULL DEFAULT -1,
    [slug] text,
    [name] text
);
CREATE TABLE IF NOT EXISTS [playlist] (
	[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[serial_id] INTEGER NOT NULL,
	[season_id] INTEGER NOT NULL,
	[translate_id] INTEGER NOT NULL,
	[translate_key] INTEGER NOT NULL DEFAULT -1,
	[translate_slug] text,
	[url] TEXT,
	[percent] TEXT,
	[secure] TEXT,
	[created_date] TEXT,
	[updated_date] TEXT
);
CREATE TABLE IF NOT EXISTS [video] (
	[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[season_id] INTEGER NOT NULL,
	[serial_id] INTEGER NOT NULL,
	[translate_id] INTEGER NOT NULL,
	[translate_name] TEXT,
	[video_id] TEXT,
	[video_next_id] TEXT,
    [url] TEXT,
    [file_name] TEXT,
	[file_size] INTEGER,
	[subtitle] TEXT,
	[secure] TEXT,
	[created_date] TEXT,
	[updated_date] TEXT
);
");
            if (!ExistIndex("http_cache", "idx_http_cache_url"))
                _ExecuteNonQuery(@"CREATE INDEX idx_http_cache_url ON http_cache(url);");
        }

        private bool ExistIndex(string table, string name)
        {
            var result = _ExecuteReader(@"PRAGMA index_list('" + table + "');");
            if (0 < result.Count)
                for (var i = 0; i < result.Count; i++)
                    if (result[i].ContainsKey("name") && result[i]["name"] == name)
                        return true;
            return false;
        }

        private void _defaultValues()
        {
            OptionSet("cacheTimeSerial_new", (1 * 24 * 60 * 60).ToString());
            OptionSet("cacheTimeSerial_nonew", (3 * 24 * 60 * 60).ToString());
            OptionSet("cacheTimeSerial_want", (3 * 24 * 60 * 60).ToString());
            OptionSet("cacheTimeSerial_watched", (7 * 24 * 60 * 60).ToString());
            OptionSet("cacheTimeSerial_none", (20 * 24 * 60 * 60).ToString());
            OptionSet("SimultaneousDownloads", 3.ToString());
            OptionSet("NameFiles",
                "{Collection}\\{SerialName} {Season}\\{SerialName} S{Season}E{Episode} {Translate} {OriginalName}");
            OptionSet("listViewSerialsDisplayIndex", "[3,17,2,1,0,4,5,6,18,19,7,8,9,10,11,12,13,14,15,21,20,16]");
            OptionSet("listViewSerialsWidth", "[0,367,0,0,0,0,0,0,25,150,0,0,0,0,0,0,0,0,0,84,56,0]");
            OptionSet("listViewSerialsAutoSize", "[0,0,0,0,0,0,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1]");
            OptionSet("listViewDownloadWidth", "[556,100,100,200]");
            OptionSet("listViewSerialsView", "");
        }

        private void _checkConnectDataBase()
        {
            if (null == connection || null != connection &&
                (connection.State.Equals(ConnectionState.Closed) || connection.State.Equals(ConnectionState.Broken)))
            {
                var factory = (SQLiteFactory) DbProviderFactories.GetFactory("System.Data.SQLite");
                var _connection = (SQLiteConnection) factory.CreateConnection();
                _connection.ConnectionString = "Data Source = " + baseName;
                _connection.Open();
                if (_connection.State.Equals(ConnectionState.Open)) connection = _connection;
            }
        }

        private int _ExecuteNonQuery(string sql)
        {
            return _ExecuteNonQuery(sql, new Dictionary<string, string>());
        }

        private int _ExecuteNonQuery(string sql, Dictionary<string, string> data)
        {
            var result = 0;
            if (string.IsNullOrEmpty(sql))
                return result;
            try
            {
                _checkConnectDataBase();
                var command = new SQLiteCommand(sql, connection);
                command.CommandType = CommandType.Text;
                foreach (var item in data)
                    command.Parameters.AddWithValue("@" + item.Key, item.Value);
                result = command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Logger.Instance.Error(ex);
            }

            return result;
        }

        private List<Dictionary<string, string>> _ExecuteReader(string sql)
        {
            return _ExecuteReader(sql, new Dictionary<string, string>());
        }

        private List<Dictionary<string, string>> _ExecuteReader(string sql, Dictionary<string, string> data)
        {
            var result = new List<Dictionary<string, string>>();
            try
            {
                _checkConnectDataBase();
                var command = new SQLiteCommand(sql, connection);
                command.CommandType = CommandType.Text;
                foreach (var item in data)
                    command.Parameters.AddWithValue("@" + item.Key, item.Value);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var _result = new Dictionary<string, string>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var field = reader.GetName(i);
                        var value = reader[field].ToString();
                        _result.Add(field, value);
                    }

                    result.Add(_result);
                }
            }
            catch (SQLiteException ex)
            {
                Logger.Instance.Error(ex);
            }

            return result;
        }

        public bool VideoSet(int ID, Dictionary<string, string> data)
        {
            if (0 == data.Count)
                return false;
            var dataOld = VideoGet(ID);
            if (0 < dataOld.Count)
            {
                var dataDiff = data.Where(entry => !dataOld.ContainsKey(entry.Key) || dataOld[entry.Key] != entry.Value)
                    .ToDictionary(entry => entry.Key, entry => entry.Value);
                var fields = new List<string>();
                foreach (var item in dataDiff)
                    fields.Add(item.Key + " = @" + item.Key);
                if (0 < fields.Count)
                {
#if DEBUG
                    Console.WriteLine("\tUpdate Video\t{0}:\t{1}", ID, SimpleJson.SimpleJson.SerializeObject(dataDiff));
#endif
                    dataDiff.Add("id", ID.ToString());
                    dataDiff.Add("updated_date", DateTime.UtcNow.ToString(TIME_FORMAT));
                    return 0 < _ExecuteNonQuery(
                               @"UPDATE video SET " + string.Join(", ", fields.ToArray()) +
                               ", updated_date = @updated_date WHERE id = @id", dataDiff);
                }
            }
            else
            {
                data.Add("id", ID.ToString());
                var date = DateTime.UtcNow.ToString(TIME_FORMAT);
                data.Add("created_date", date);
                data.Add("updated_date", date);
                var fields = new List<string>(data.Keys);
#if DEBUG
                Console.WriteLine("\tInsert Video\t{0}:\t{1}", ID, SimpleJson.SimpleJson.SerializeObject(data));
#endif
                return 0 < _ExecuteNonQuery(
                           @"INSERT INTO video (" + string.Join(", ", fields.ToArray()) + ") VALUES (@" +
                           string.Join(", @", fields.ToArray()) + ")", data);
            }

            return false;
        }

        public Dictionary<string, string> VideoGet(int ID)
        {
            var data = new Dictionary<string, string>();
            var result = _ExecuteReader(@"SELECT * FROM video WHERE id=@id LIMIT 1;",
                new Dictionary<string, string> {{"id", ID.ToString()}});
            if (0 < result.Count)
                data = result[0];

            return data;
        }

        public void OptionSet(string name, string value = null)
        {
            if (string.IsNullOrEmpty(value))
                value = "";

            var data = new Dictionary<string, string> {{"name", name}, {"value", value}};
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
            if (cachedOptions.ContainsKey(name)) return cachedOptions[name];
            var result = _ExecuteReader(@"SELECT value FROM options WHERE name = @name LIMIT 1",
                new Dictionary<string, string> {{"name", name}});
            if (0 < result.Count)
            {
                cachedOptions.Add(name, result[0]["value"]);
                return cachedOptions[name];
            }

            return "";
        }

        public int CacheSet(string url, string content = null)
        {
            var data = new Dictionary<string, string>
                {{"url", url}, {"content", content}, {"create_date", DateTime.UtcNow.ToString(TIME_FORMAT)}};
            if (string.IsNullOrWhiteSpace(content))
                return 0;
            var item = CacheGet(url);
            if (item.isEmpty)
                return _ExecuteNonQuery(
                    @"INSERT INTO http_cache (url,content,create_date) VALUES (@url,@content,@create_date)", data);
            return _ExecuteNonQuery(
                @"UPDATE http_cache SET content = @content, create_date = @create_date WHERE url = @url", data);
        }

        public string CacheGetContent(string url, int period = 24 * 60 * 60)
        {
            return CacheGet(url, period).ToString();
        }

        public DBCache CacheGet(string url, int period = 24 * 60 * 60)
        {
            var result = _ExecuteReader(@"SELECT url, content, create_date FROM http_cache WHERE url = @url LIMIT 1",
                new Dictionary<string, string> {{"url", url}});
            var item = new DBCache(url, period);
            if (0 < result.Count)
                item.setURL(result[0]["url"]).setContent(result[0]["content"])
                    .setDate(result[0]["create_date"], TIME_FORMAT);

            return item;
        }

        public bool SeasonSet(int id, Dictionary<string, string> data)
        {
            if (0 == data.Count)
                return false;
            var dataOld = SeasonGet(id);
            if (0 < dataOld.Count)
            {
                var dataDiff = data.Where(entry => !dataOld.ContainsKey(entry.Key) || dataOld[entry.Key] != entry.Value)
                    .ToDictionary(entry => entry.Key, entry => entry.Value);
                var fields = new List<string>();
                foreach (var item in dataDiff)
                    fields.Add(item.Key + " = @" + item.Key);
                if (0 < fields.Count)
                {
#if DEBUG
                    Console.WriteLine("\tUpdate Season\t{0}:\t{1}", id,
                        SimpleJson.SimpleJson.SerializeObject(dataDiff));
#endif
                    dataDiff.Add("id", id.ToString());
                    dataDiff.Add("updated_date", DateTime.UtcNow.ToString(TIME_FORMAT));
                    return 0 < _ExecuteNonQuery(
                               @"UPDATE season SET " + string.Join(", ", fields.ToArray()) +
                               ", updated_date = @updated_date WHERE id = @id", dataDiff);
                }
            }
            else
            {
                data.Add("id", id.ToString());
                var date = DateTime.UtcNow.ToString(TIME_FORMAT);
                data.Add("created_date", date);
                data.Add("updated_date", date);
                var fields = new List<string>(data.Keys);
#if DEBUG
                Console.WriteLine("\tInsert Season\t{0}:\t{1}", id, SimpleJson.SimpleJson.SerializeObject(data));
#endif
                return 0 < _ExecuteNonQuery(
                           @"INSERT INTO season (" + string.Join(", ", fields.ToArray()) + ") VALUES (@" +
                           string.Join(", @", fields.ToArray()) + ")", data);
            }

            return false;
        }

        public bool SeasonRemove(int id)
        {
            return 0 < _ExecuteNonQuery(@"DELETE FROM season WHERE id = @id",
                       new Dictionary<string, string> {{"id", id.ToString()}});
        }

        public Dictionary<string, string> SeasonGet(int id)
        {
            var data = new Dictionary<string, string>();
            var result =
                _ExecuteReader(
                    @"SELECT season.*, http_cache.create_date AS cached_date  FROM season LEFT JOIN http_cache ON season.url = http_cache.url WHERE season.id = @id LIMIT 1",
                    new Dictionary<string, string> {{"id", id.ToString()}});
            if (0 < result.Count)
                data = result[0];
            return data;
        }

        public Dictionary<int, int> SeasonsGet(int serial_id, int id = 0)
        {
            var data = new Dictionary<int, int>();
            var result =
                _ExecuteReader(
                    @"SELECT id,season FROM season WHERE serial_id = @serial_id AND id <> @id ORDER BY season",
                    new Dictionary<string, string> {{"serial_id", serial_id.ToString()}, {"id", id.ToString()}});

            for (var i = 0; i < result.Count; i++)
            {
                var season = !string.IsNullOrEmpty(result[i]["season"]) ? int.Parse(result[i]["season"]) : 0;
                if (!data.ContainsKey(season))
                    data.Add(season, int.Parse(result[i]["id"]));
            }

            return data;
        }

        public List<int> RelatedGet(int seasonId)
        {
            var data = new List<int>();
            var result =
                _ExecuteReader(
                    @"SELECT DISTINCT related_id FROM season_related WHERE season_id = @season_id ORDER BY related_id",
                    new Dictionary<string, string> {{"season_id", seasonId.ToString()}});
            for (var i = 0; i < result.Count; i++)
            {
                var season = int.Parse(result[i]["related_id"]);
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

            var data = new Dictionary<string, string> {{"season_id", seasonId.ToString()}};
            var fields = new List<string>();
            for (var i = 0; i < related.Count; i++)
                if (!keysOld.Contains(related[i]))
                {
                    fields.Add("(@season_id, @related_id" + i + ")");
                    data.Add("related_id" + i, related[i].ToString());
                }
                else
                {
                    keysOld.Remove(related[i]);
                }

            var sql = "";
            if (0 < keysOld.Count)
                sql += @"DELETE FROM season_related WHERE season_id = @season_id AND related_id IN(" +
                       string.Join(", ", keysOld.ToArray()) + ");";
            if (0 < fields.Count)
                sql += @"INSERT INTO season_related (season_id, related_id) VALUES " +
                       string.Join(", ", fields.ToArray()) + ";";

            return 0 < _ExecuteNonQuery(sql, data);
        }

        public bool RelatedRemove(int seasonId)
        {
            return 0 < _ExecuteNonQuery(@"DELETE FROM season_related WHERE  season_id = @season_id;",
                       new Dictionary<string, string> {{"season_id", seasonId.ToString()}});
        }

        public Dictionary<int, string> CompilationGet()
        {
            var data = new Dictionary<int, string>();
            var result = _ExecuteReader(@"SELECT id,name FROM compilation");
            for (var i = 0; i < result.Count; i++)
            {
                var id = !string.IsNullOrEmpty(result[i]["id"]) ? int.Parse(result[i]["id"]) : 0;
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
            var i = 0;
            foreach (var item in compilation)
                if (!keysOld.Contains(item.Key))
                {
                    fields.Add("(@id" + i + ", @name" + i + ")");
                    data.Add("id" + i, item.Key.ToString());
                    data.Add("name" + i, item.Value);
                    i++;
                }
                else
                {
                    keysOld.Remove(item.Key);
                }

            var sql = "";
            if (0 < keysOld.Count)
                sql += @"DELETE FROM compilation WHERE id IN(" + string.Join(", ", keysOld.ToArray()) + ");";
            if (0 < fields.Count)
                sql += @"INSERT INTO compilation (id, name) VALUES " + string.Join(", ", fields.ToArray()) + ";";
            return 0 < _ExecuteNonQuery(sql, data);
        }

        public Dictionary<int, int> CompilationRelationGet()
        {
            var data = new Dictionary<int, int>();
            var result =
                _ExecuteReader(
                    @"SELECT DISTINCT serial_id,compilation_id FROM season_compilation ORDER BY serial_id ASC;");
            for (var i = 0; i < result.Count; i++)
            {
                var serial_id = int.Parse(result[i]["serial_id"]);
                var compilation_id = int.Parse(result[i]["compilation_id"]);
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
            var i = 0;
            foreach (var item in compilation)
                if (!keysOld.Contains(item.Key))
                {
                    fields.Add("(@serial_id" + i + ", @compilation_id" + i + ")");
                    data.Add("serial_id" + i, item.Key.ToString());
                    data.Add("compilation_id" + i, item.Value.ToString());
                    i++;
                }
                else
                {
                    keysOld.Remove(item.Key);
                }

            var sql = "";
            if (0 < keysOld.Count)
                sql += @"DELETE FROM season_compilation WHERE serial_id IN(" + string.Join(", ", keysOld.ToArray()) +
                       ");";
            if (0 < fields.Count)
                sql += @"INSERT INTO season_compilation (serial_id, compilation_id) VALUES " +
                       string.Join(", ", fields.ToArray()) + ";";
            return 0 < _ExecuteNonQuery(sql, data);
        }

        public bool CompilationRelationSet(int serialId, int compilationId)
        {
            return 0 < _ExecuteNonQuery(
                       @"INSERT INTO season_compilation (serial_id, compilation_id) VALUES (@serial_id, @compilation_id);",
                       new Dictionary<string, string>
                           {{"serial_id", serialId.ToString()}, {"compilation_id", compilationId.ToString()}});
        }

        public bool CompilationRelationRemove(int serialId)
        {
            return 0 < _ExecuteNonQuery(@"DELETE FROM season_compilation WHERE serial_id = @serial_id;",
                       new Dictionary<string, string> {{"serial_id", serialId.ToString()}});
        }

        public bool TranslateSet(Dictionary<string, string> data, Dictionary<string, string> where)
        {
            var dataOld = TranslateGet(where);
            if (0 < dataOld.Count)
            {
                var dataDiff = data.Where(entry => !dataOld.ContainsKey(entry.Key) || dataOld[entry.Key] != entry.Value)
                    .ToDictionary(entry => entry.Key, entry => entry.Value);
                var fieldsUpdate = new List<string>();
                foreach (var item in dataDiff)
                    fieldsUpdate.Add(item.Key + " = @" + item.Key);

                var fieldsWhere = new List<string>();
                foreach (var item in where)
                {
                    fieldsWhere.Add(string.Format("{0} = @ww{0}", item.Key));
                    dataDiff.Add("ww" + item.Key, item.Value);
                }

                if (0 < fieldsUpdate.Count && 0 < fieldsWhere.Count)
                {
#if DEBUG
                    Console.WriteLine("\tUpdate Translate\t{0}:\t{1}", SimpleJson.SimpleJson.SerializeObject(where),
                        SimpleJson.SimpleJson.SerializeObject(dataDiff));
#endif
                    return 0 < _ExecuteNonQuery(
                               @"UPDATE translate SET " + string.Join(", ", fieldsUpdate.ToArray()) + " WHERE " +
                               string.Join(" AND ", fieldsWhere.ToArray()), dataDiff);
                }


                return false;
            }

            var fieldsNew = new List<string>(data.Keys);
#if DEBUG
            Console.WriteLine("\tInsert Translate\t{0}:\t{1}", SimpleJson.SimpleJson.SerializeObject(where),
                SimpleJson.SimpleJson.SerializeObject(data));
#endif
            return 0 < _ExecuteNonQuery(
                       @"INSERT INTO translate (" + string.Join(", ", fieldsNew.ToArray()) + ") VALUES (@" +
                       string.Join(", @", fieldsNew.ToArray()) + ")", data);
        }

        public Dictionary<string, string> TranslateGet(Dictionary<string, string> where)
        {
            var data = new Dictionary<string, string>();
            if (0 < where.Count)
            {
                if (where.ContainsKey("id"))
                {
                    var resultFirst = TranslateGet(where["id"]);
                    if (0 < resultFirst.Count)
                        return resultFirst;
                }

                var fields = new List<string>();
                var keys = new List<string>(where.Keys);
                for (var i = 0; i < keys.Count; i++) fields.Add(string.Format("{0} = @{0}", keys[i]));
                var result =
                    _ExecuteReader(
                        @"SELECT id, key, slug, name FROM translate WHERE " + string.Join(" OR ", fields.ToArray()) +
                        " ORDER BY key ASC LIMIT 1;", where);
                if (0 < result.Count)
                    data = result[0];
            }


            return data;
        }

        public Dictionary<string, string> TranslateGet(string id)
        {
            var data = new Dictionary<string, string>();
            var result =
                _ExecuteReader(@"SELECT id, key, slug, name FROM translate WHERE id=@id ORDER BY key ASC LIMIT 1;",
                    new Dictionary<string, string> {{"id", id}});
            if (0 < result.Count)
                data = result[0];

            return data;
        }

        public List<Dictionary<string, string>> TranslateGetAll()
        {
            return _ExecuteReader(@"SELECT id, key, slug, name FROM translate ORDER BY key ASC;");
        }

        public bool PlaylistSet(int seasonID, int translateID, Dictionary<string, string> data)
        {
            if (0 == data.Count)
                return false;
            var dataOld = PlaylistGet(seasonID, translateID);
            if (0 < dataOld.Count)
            {
                var dataDiff = data.Where(entry => !dataOld.ContainsKey(entry.Key) || dataOld[entry.Key] != entry.Value)
                    .ToDictionary(entry => entry.Key, entry => entry.Value);
                var fieldsUpdate = new List<string>();
                foreach (var item in dataDiff)
                    fieldsUpdate.Add(item.Key + " = @" + item.Key);
                if (0 < fieldsUpdate.Count)
                {
#if DEBUG
                    Console.WriteLine("\tUpdate Playlist\t{0}\t{1}:\t{2}", seasonID, translateID,
                        SimpleJson.SimpleJson.SerializeObject(dataDiff));
#endif
                    dataDiff.Add("season_id", seasonID.ToString());
                    dataDiff.Add("translate_id", translateID.ToString());
                    dataDiff.Add("updated_date", DateTime.UtcNow.ToString(TIME_FORMAT));
                    return 0 < _ExecuteNonQuery(
                               @"UPDATE playlist SET " + string.Join(", ", fieldsUpdate.ToArray()) +
                               ", updated_date = @updated_date WHERE season_id=@season_id AND translate_id=@translate_id",
                               dataDiff);
                }

                return false;
            }

            if (!data.ContainsKey("season_id"))
                data.Add("season_id", seasonID.ToString());
            if (!data.ContainsKey("translate_id"))
                data.Add("translate_id", translateID.ToString());
            var date = DateTime.UtcNow.ToString(TIME_FORMAT);
            data.Add("created_date", date);
            data.Add("updated_date", date);
            var fieldsNew = new List<string>(data.Keys);
#if DEBUG
            Console.WriteLine("\tInsert Playlist\t{0}\t{1}:\t{2}", seasonID, translateID,
                SimpleJson.SimpleJson.SerializeObject(data));
#endif
            return 0 < _ExecuteNonQuery(
                       @"INSERT INTO playlist (" + string.Join(", ", fieldsNew.ToArray()) +
                       ") VALUES (@" + string.Join(", @", fieldsNew.ToArray()) +
                       ")", data);
        }

        public Dictionary<string, string> PlaylistGet(int seasonID, int translateID)
        {
            var data = new Dictionary<string, string>();
            var result = _ExecuteReader(
                @"SELECT playlist.*, http_cache.create_date AS cached_date FROM playlist LEFT JOIN http_cache ON playlist.url = http_cache.url WHERE season_id=@season_id AND translate_id=@translate_id LIMIT 1;",
                new Dictionary<string, string>
                    {{"season_id", seasonID.ToString()}, {"translate_id", translateID.ToString()}});
            if (0 < result.Count)
                data = result[0];

            return data;
        }

        public List<Dictionary<string, string>> PlaylistGets(int seasonID)
        {
            return _ExecuteReader(
                @"SELECT playlist.*, http_cache.create_date AS cached_date FROM playlist LEFT JOIN http_cache ON playlist.url = http_cache.url WHERE season_id=@season_id;",
                new Dictionary<string, string> {{"season_id", seasonID.ToString()}});
        }

        public List<int> PlayerGetTranslate(int seasonID)
        {
            var data = new List<int>();
            var result =
                _ExecuteReader(
                    @"SELECT translate_key FROM playlist WHERE season_id = @season_id ORDER BY translate_key ASC;",
                    new Dictionary<string, string> {{"season_id", seasonID.ToString()}});
            for (var i = 0; i < result.Count; i++)
            {
                var translate_key = int.Parse(result[i]["translate_key"]);
                if (!data.Contains(translate_key))
                    data.Add(translate_key);
            }

            return data;
        }
    }
}