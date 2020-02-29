using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomeTheater.Helper
{
    public class DB
    {
        public const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DATE_FORMAT = "yyyy-MM-dd";
        public const string TIME_FORMAT = "HH:mm:ss";
        private static DB _i;
        private readonly string baseName = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ".sqlite");

        private Dictionary<string, string> cachedOptions = new Dictionary<string, string>();
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

            OptionGetAll();
        }

        public static DB Instance
        {
            get
            {
                Load();
                return _i;
            }
        }

        public static void Load()
        {
            if (_i == null)
                _i = new DB();
        }

        private void _createTables()
        {
            _ExecuteNonQuery(@"
CREATE TABLE IF NOT EXISTS [options] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [name] text NOT NULL UNIQUE,
    [value] text NOT NULL,
    [created_date] text,
    [updated_date] text
);
CREATE TABLE IF NOT EXISTS [http_cache] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [url] text UNIQUE,
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
    [secure] text DEFAULT '',
    [error] text DEFAULT '',
    [blocked] integer DEFAULT 0,
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
    [slug] text DEFAULT '',
    [name] text
);
CREATE TABLE IF NOT EXISTS [playlist] (
	[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[season_id] INTEGER NOT NULL,
	[translate_id] INTEGER NOT NULL DEFAULT 0,
	[url] TEXT,
	[percent] TEXT DEFAULT '0',
	[secure] TEXT,
	[order_videos] TEXT DEFAULT '',
	[created_date] TEXT,
	[updated_date] TEXT,
	[removed_date] TEXT DEFAULT ''
);
CREATE TABLE IF NOT EXISTS [video] (
	[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[season_id] INTEGER NOT NULL,
	[translate_id] INTEGER NOT NULL DEFAULT -1,
	[translate_name] TEXT DEFAULT '',
	[video_id] TEXT DEFAULT '',
    [url] TEXT,
	[subtitle] TEXT,
	[secure] TEXT,
	[created_date] TEXT,
	[updated_date] TEXT,
	[removed_date] TEXT DEFAULT ''
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
            OptionSet("cacheTimeSerial_want", (1 * 24 * 60 * 60).ToString());
            OptionSet("cacheTimeSerial_nonew", (1 * 24 * 60 * 60).ToString());
            OptionSet("cacheTimeSerial_watched", (7 * 24 * 60 * 60).ToString());
            OptionSet("cacheTimeSerial_none", (20 * 24 * 60 * 60).ToString());
            OptionSet("OldesDaysSeason", (365 * 1).ToString());
            OptionSet("SimultaneousDownloads", 3.ToString());
            OptionSet("Timer", "01:00:00");
            OptionSet("NameFiles",
                "{Collection}\\{SerialName} {Season}\\{SerialName} S{Season}E{Episode} {Translate} {OriginalName}.{Format}");
            OptionSet("listSerialsDisplayIndex", "[0,16,1,2,3,4,5,6,17,18,7,8,9,10,11,12,13,14,15,20,19,21]");
            OptionSet("listSerialsWidth", "[0,377,0,0,0,0,0,0,25,100,0,0,0,0,0,0,0,0,0,84,56,66]");
            OptionSet("listSerialsAutoSize", "[0,0,0,0,0,0,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1]");
            OptionSet("listDownloadWidth", "[556,100,100,200]");
            OptionSet("checkUpdate", "1,1,1,1,1,1,1,1,1,1,1,0");
            OptionSet("checkSilentUpdateCache", "1,0,0,1,1,1,1,0,0,1,1,0");
            OptionSet("checkSilentUpdate", "0,1,1,0,0,0,0,1,1,0,0,0");
            OptionSet("listSerialsView", "");
            OptionSet("firstLaunch", "1");
            OptionSet("oldestAllow", "1");
            OptionSet("oldAllow", "1");
            OptionSet("newAllow", "1");
        }

        private void _checkConnectDataBase()
        {
            if (null == connection || null != connection &&
                (connection.State.Equals(ConnectionState.Closed) || connection.State.Equals(ConnectionState.Broken)))
            {
                var _connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", baseName));
                _connection.Open();
                if (_connection.State.Equals(ConnectionState.Open)) connection = _connection;
            }
        }

        #region Player

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

        #endregion

        #region ExecuteQuery

        #region ExecuteNonQuery

        private int _ExecuteNonQuery(string sql)
        {
            return _ExecuteNonQuery(sql, new Dictionary<string, string>());
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Проверка запросов SQL на уязвимости безопасности")]
        private int _ExecuteNonQuery(string sql, Dictionary<string, string> data)
        {
            var result = 0;
            if (string.IsNullOrEmpty(sql))
                return result;
            try
            {
                _checkConnectDataBase();

                var command = new SQLiteCommand(sql, connection)
                {
                    CommandType = CommandType.Text
                };

                foreach (var item in data)
                    command.Parameters.AddWithValue("@" + item.Key, item.Value);
                result = command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Logger.Instance.Error("{0}\n{1}", sql, SimpleJson.SimpleJson.SerializeObject(data));
                Logger.Instance.Error(ex);
            }

            return result;
        }

        #endregion

        #region ExecuteReader

        private List<Dictionary<string, string>> _ExecuteReader(string sql)
        {
            return _ExecuteReader(sql, new Dictionary<string, string>());
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Проверка запросов SQL на уязвимости безопасности")]
        private List<Dictionary<string, string>> _ExecuteReader(string sql, Dictionary<string, string> data)
        {
            var result = new List<Dictionary<string, string>>();
            try
            {
                _checkConnectDataBase();

#pragma warning disable CA2100 // Проверка запросов SQL на уязвимости безопасности
                var command = new SQLiteCommand(sql, connection)
                {
                    CommandType = CommandType.Text
                };
#pragma warning restore CA2100 // Проверка запросов SQL на уязвимости безопасности

                foreach (var item in data)
                    command.Parameters.AddWithValue("@" + item.Key, item.Value);
                using (var reader = command.ExecuteReader())
                {
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
            }
            catch (SQLiteException ex)
            {
                Logger.Instance.Error("{0}\n{1}", sql, SimpleJson.SimpleJson.SerializeObject(data));
                Logger.Instance.Error(ex);
            }

            return result;
        }

        #endregion

        #endregion

        #region Option

        public int OptionSet(string name, float value = 0)
        {
            return OptionSet(name, value.ToString());
        }

        public int OptionSet(string name, int value = 0)
        {
            return OptionSet(name, value.ToString());
        }

        public int OptionSet(string name, bool value)
        {
            return OptionSet(name, value ? 1 : 0);
        }

        public int OptionSet(string name, string value = null)
        {
            if (string.IsNullOrEmpty(value))
                value = "";

            var data = new Dictionary<string, string> {{"name", name}, {"value", value}};
            var date = DateTime.UtcNow.ToString(DATETIME_FORMAT);
            data.Add("created_date", date);
            data.Add("updated_date", date);
            cachedOptions[name] = value;
            return _ExecuteNonQuery(
                @"INSERT OR REPLACE INTO options (name,value,created_date,updated_date) VALUES (@name,@value,@created_date,@updated_date)",
                data);
        }

        public void OptionSetAsync(string name, float value = 0)
        {
            OptionSetAsync(name, value.ToString());
        }

        public void OptionSetAsync(string name, int value = 0)
        {
            OptionSetAsync(name, value.ToString());
        }

        public void OptionSetAsync(string name, bool value)
        {
            OptionSetAsync(name, value ? 1 : 0);
        }

        public async void OptionSetAsync(string name, string value = null)
        {
            await Task.Run(() => OptionSet(name, value));
            cachedOptions[name] = value;
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

        public void OptionGetAll()
        {
            var data = new Dictionary<string, string>();
            var result = _ExecuteReader(@"SELECT name,value FROM options");
            foreach (var item in result)
                if (!data.ContainsKey(item["name"]))
                    data.Add(item["name"], item["value"]);
            cachedOptions = data;
        }

        #endregion

        #region Cache

        public int CacheSet(string url, string content = null)
        {
            if (string.IsNullOrEmpty(content))
                return 0;
            return _ExecuteNonQuery(
                @"INSERT OR REPLACE INTO http_cache (url,content,create_date) VALUES (@url,@content,@create_date)",
                new Dictionary<string, string>
                    {{"url", url}, {"content", content}, {"create_date", DateTime.UtcNow.ToString(DATETIME_FORMAT)}});
        }

        public async Task<int> CacheSetAsync(string url, string content = null)
        {
            return await Task.Run(() => CacheSet(url, content));
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
                    .setDate(result[0]["create_date"], DATETIME_FORMAT);

            return item;
        }

        #endregion

        #region Season

        public bool SeasonSet(int id, Dictionary<string, string> data)
        {
            if (null == data || 0 == data.Count)
                return false;
#if DEBUG
            Console.WriteLine("\tDB Season\t{0}:\t{1}", id, SimpleJson.SimpleJson.SerializeObject(data));
#endif
            var fieldsUpdate = new List<string>();
            foreach (var item in data)
                fieldsUpdate.Add(item.Key + " = @" + item.Key);
            data.Add("id", id.ToString());
            var date = DateTime.UtcNow.ToString(DATETIME_FORMAT);
            data.Add("created_date", date);
            data.Add("updated_date", date);
            var fieldsInsert = new List<string>(data.Keys);
            return 0 < _ExecuteNonQuery(
                       @"INSERT OR IGNORE INTO season (" + string.Join(", ", fieldsInsert.ToArray()) + ") VALUES (@" +
                       string.Join(", @", fieldsInsert.ToArray()) + "); UPDATE season SET " +
                       string.Join(", ", fieldsUpdate.ToArray()) +
                       ", updated_date = @updated_date WHERE id = @id", data);
        }

        public bool SeasonRemove(int id)
        {
            return 0 < _ExecuteNonQuery(@"DELETE FROM season WHERE id = @id",
                       new Dictionary<string, string> {{"id", id.ToString()}});
        }

        public bool SeasonClearOld(List<int> IDs)
        {
            if (null == IDs || 0 == IDs.Count)
                return false;
            var strIDs = string.Join(", ", IDs.ToArray());
            _ExecuteNonQuery(@"UPDATE season SET type=NULL WHERE type NOT NULL AND type<>'notwatch' AND id NOT IN (" +
                             strIDs + ")");
            return true;
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

        #endregion

        #region Related

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
            if (null == related || 0 >= related.Count)
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

        #endregion

        #region Compilation

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
            if (null == compilation || 0 == compilation.Count)
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

        #endregion

        #region CompilationRelation

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
            if (null == compilation || 0 == compilation.Count)
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

        #endregion

        #region Playlist

        public bool PlaylistSet(int seasonID, int translateID, Dictionary<string, string> data)
        {
            if (null == data || 0 == data.Count)
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
                    dataDiff.Add("updated_date", DateTime.UtcNow.ToString(DATETIME_FORMAT));
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
            var date = DateTime.UtcNow.ToString(DATETIME_FORMAT);
            data.Add("created_date", date);
            data.Add("updated_date", date);
            data.Add("removed_date", "");
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
                @"SELECT playlist.*, http_cache.create_date AS cached_date FROM playlist LEFT JOIN http_cache ON playlist.url = http_cache.url WHERE season_id=@season_id AND removed_date = '';",
                new Dictionary<string, string> {{"season_id", seasonID.ToString()}});
        }

        public bool PlaylistSetOld(int seasonID, List<int> translateIDs)
        {
            // TODO Не работает
            var sql =
                @"UPDATE playlist SET removed_date=@removed_date WHERE season_id = @season_id";
            if (null != translateIDs && 0 < translateIDs.Count)
            {
                var ids = string.Join(", ", translateIDs.ToArray());
                sql +=
                    @" AND translate_id NOT IN (" + ids +
                    "); UPDATE playlist SET removed_date='' WHERE season_id = @season_id AND translate_id NOT IN (" +
                    ids + ");";
            }

            return 0 < _ExecuteNonQuery(sql,
                       new Dictionary<string, string>
                       {
                           {"removed_date", DateTime.UtcNow.ToString(DATETIME_FORMAT)},
                           {"season_id", seasonID.ToString()}
                       });
        }

        #endregion

        #region Translate

        public bool TranslateSet(int ID, Dictionary<string, string> data)
        {
            if (null == data || 0 == data.Count)
                return false;
#if DEBUG
            Console.WriteLine("\tDB Translate\t{0}:\t{1}", ID,
                SimpleJson.SimpleJson.SerializeObject(data));
#endif
            var fieldsUpdate = new List<string>();
            foreach (var item in data)
                fieldsUpdate.Add(item.Key + " = @" + item.Key);
            if (!data.ContainsKey("id"))
                data.Add("id", ID.ToString());
            var fieldsNew = new List<string>(data.Keys);
            data.Add("old_id", ID.ToString());
            return 0 < _ExecuteNonQuery(
                       @"INSERT OR IGNORE INTO translate (" + string.Join(", ", fieldsNew.ToArray()) + ") VALUES (@" +
                       string.Join(", @", fieldsNew.ToArray()) + ");UPDATE translate SET " +
                       string.Join(", ", fieldsUpdate.ToArray()) + " WHERE id = @old_id", data);
        }

        public Dictionary<string, string> TranslateGet(int id)
        {
            var data = new Dictionary<string, string>();
            var result =
                _ExecuteReader(@"SELECT * FROM translate WHERE id=@id ORDER BY id ASC LIMIT 1;",
                    new Dictionary<string, string> {{"id", id.ToString()}});
            if (0 < result.Count)
                data = result[0];

            return data;
        }

        public List<Dictionary<string, string>> TranslateGetAll()
        {
            return _ExecuteReader(@"SELECT * FROM translate ORDER BY id ASC;");
        }

        public List<string> TranslateGetRate()
        {
            var data = new List<string>();
            var result =
                _ExecuteReader(@"SELECT 
    name_translate,
    SUM(percent) AS percent,
    SUM(items) AS items
FROM (
    SELECT 
        CASE WHEN video.translate_name <> ''
            THEN video.translate_name
            ELSE translate.name
        END name_translate,
        COUNT(video.id)*AVG(playlist.percent)/100 AS percent,
        COUNT(video.id) AS items
    FROM video
    LEFT JOIN translate
    ON video.translate_id = translate.id
    LEFT JOIN playlist
    ON video.season_id = playlist.season_id AND video.translate_id = playlist.translate_id
    WHERE
        name_translate NOT IN ('Стандартный', 'Трейлеры')
    GROUP BY name_translate
    UNION
    SELECT
        name AS name_translate,
        null AS percent,
        NULL AS items
    FROM translate
    WHERE name_translate NOT IN ('Стандартный', 'Трейлеры')
)
GROUP BY name_translate
ORDER BY percent DESC, items DESC;");
            if (0 < result.Count)
                foreach (var item in result)
                    data.Add(item["name_translate"]);

            return data;
        }

        #endregion

        #region Video

        public bool VideoSet(int ID, Dictionary<string, string> data)
        {
            if (null == data || 0 == data.Count)
                return false;

            var fieldsUpdate = new List<string>();
            foreach (var item in data)
                fieldsUpdate.Add(item.Key + " = @" + item.Key);
#if DEBUG
            Console.WriteLine("\tDB Video\t{0}:\t{1}", ID,
                SimpleJson.SimpleJson.SerializeObject(data));
#endif
            data.Add("id", ID.ToString());
            var date = DateTime.UtcNow.ToString(DATETIME_FORMAT);
            data.Add("created_date", date);
            data.Add("updated_date", date);
            data.Add("removed_date", "");
            var fields = new List<string>(data.Keys);
            var _sql = "";
            if (data.ContainsKey("season_id") && data.ContainsKey("translate_id") && data.ContainsKey("video_id"))
                _sql =
                    @"UPDATE video SET removed_date=@updated_date WHERE season_id = @season_id AND translate_id = @translate_id AND video_id = @video_id AND id <> @id;";
            return 0 < _ExecuteNonQuery(_sql +
                                        @"INSERT OR IGNORE INTO video (" + string.Join(", ", fields.ToArray()) +
                                        ") VALUES (@" +
                                        string.Join(", @", fields.ToArray()) + ");UPDATE video SET " +
                                        string.Join(", ", fieldsUpdate.ToArray()) +
                                        ", updated_date = @updated_date WHERE id = @id", data);
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

        public List<int> VideoGetTemp()
        {
            var data = new List<int>();
            var result =
                _ExecuteReader(
                    @"SELECT DISTINCT season_id FROM video WHERE url LIKE '%temp-cdn%' AND removed_date='' ORDER BY season_id;");
            if (0 < result.Count)
                foreach (var item in result)
                    data.Add(Convert.ToInt32(item["season_id"]));

            return data;
        }

        public List<Dictionary<string, string>> VideoGets(int SeasonID)
        {
            return _ExecuteReader(@"SELECT * FROM video WHERE season_id=@id AND removed_date='' ORDER BY video_id ASC;",
                new Dictionary<string, string> {{"id", SeasonID.ToString()}});
        }

        //public bool VideoSetOld(int seasonID, int translateID, List<int> videoIDs)
        //{
        //    var sql =
        //        @"UPDATE video SET removed_date=@removed_date WHERE season_id = @season_id AND translate_id = @translate_id";
        //    if (null != videoIDs && 0 < videoIDs.Count)
        //    {
        //        var ids = string.Join(", ", videoIDs.ToArray());
        //        sql +=
        //            @" AND id NOT IN (" + ids +
        //            "); UPDATE video SET removed_date='' WHERE season_id = @season_id AND translate_id = @translate_id AND id IN (" +
        //            ids + ")";
        //    }

        //    return 0 < _ExecuteNonQuery(sql,
        //               new Dictionary<string, string>
        //               {
        //                   {"removed_date", DateTime.UtcNow.ToString(TIME_FORMAT)},
        //                   {"season_id", seasonID.ToString()},
        //                   {"translate_id", translateID.ToString()}
        //               });
        //}

        #endregion
    }
}