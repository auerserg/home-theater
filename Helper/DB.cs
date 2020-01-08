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
        public string baseName = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ".db3");
        public SQLiteConnection connection;
        public const string TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DATE_FORMAT = "yyyy-MM-dd";

        private Dictionary<string, string> cachedOptions = new Dictionary<string, string>();
        public DB()
        {
            CreateDataBase();
            CreateTables();
        }

        public static DB Instance
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
            _i = new DB();
        }

        private void CreateDataBase()
        {
            if (!File.Exists(this.baseName))
            {
                SQLiteConnection.CreateFile(this.baseName);

                CreateTables();

                DefaultValues();
            }
        }

        private void DefaultValues()
        {
            setOption("cacheTimeSerial_new", (1 * 24 * 60 * 60).ToString());
            setOption("cacheTimeSerial_nonew", (3 * 24 * 60 * 60).ToString());
            setOption("cacheTimeSerial_want", (3 * 24 * 60 * 60).ToString());
            setOption("cacheTimeSerial_watched", (7 * 24 * 60 * 60).ToString());
            setOption("cacheTimeSerial_none", (20 * 24 * 60 * 60).ToString());
            setOption("SimultaneousDownloads", (3).ToString());
            setOption("NameFiles", "{Collection}\\{SerialName} {Season}\\{SerialName} S{Season}E{Episode} {Translate} {OriginalName}");
        }

        private void CreateTables()
        {
            CheckConnectDataBase();
            using (SQLiteCommand sql = new SQLiteCommand(connection))
            {
                sql.CommandText = @"
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
    [season] integer,
    [genre] text,
    [country] text,
    [release] text,
    [limitation] text,
    [imdb] text,
    [kinopoisk] text,
    [description] text,
    [marks_current] text,
    [marks_last] text,
    [secure_mark] text,
    [type] text,
    [compilation] Text,
    [site_updated] text,
    [updated_date] text
);
CREATE TABLE IF NOT EXISTS [season_related] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [season_id] integer NOT NULL,
    [related_id] integer NOT NULL
);
";
                sql.CommandType = CommandType.Text;
                sql.ExecuteNonQuery();
            }
        }

        private void CheckConnectDataBase()
        {
            if (null == connection || (null != connection && (connection.State.Equals(ConnectionState.Closed) || connection.State.Equals(ConnectionState.Broken))))
            {
                ConnectDataBase();
            }
        }
        private void ConnectDataBase()
        {
            try
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
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string _option(string name, string action = null, string value = "")
        {
            if (("INSERT" == action || "UPDATE" == action) && string.IsNullOrEmpty(value))
            {
                return _option(name, "DETETE");
            }
            if (cachedOptions.ContainsKey(name))
            {
                if (("INSERT" == action || "UPDATE" == action || "DETETE" == action))
                {
                    cachedOptions.Remove(name);
                }
                else
                {
                    return cachedOptions[name];
                }
            }
            CheckConnectDataBase();
            try
            {
                SQLiteCommand sql = new SQLiteCommand(connection);
                switch (action)
                {
                    case "INSERT":
                        sql.CommandText = @"INSERT INTO options (name,value) VALUES (@name,@value)";
                        break;
                    case "UPDATE":
                        sql.CommandText = @"UPDATE options SET value = @value WHERE name = @name";
                        break;
                    case "DETETE":
                        sql.CommandText = @"DELETE FROM options WHERE name = @name";
                        break;
                    case "SELECT":
                    default:
                        sql.CommandText = @"SELECT value FROM options WHERE name = @name";
                        break;
                }
                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@name", name);
                sql.Parameters.AddWithValue("@value", value);
                if ("SELECT" == action || null == action)
                {
                    SQLiteDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        value = reader["value"].ToString();
                        if (cachedOptions.ContainsKey(name))
                            cachedOptions.Remove(name);
                        cachedOptions.Add(name, value);
                        return value;
                    }
                }
                else
                {
                    sql.ExecuteNonQuery();
                }

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (cachedOptions.ContainsKey(name))
                cachedOptions.Remove(name);
            cachedOptions.Add(name, "");
            return "";
        }

        public void setOption(string name, string value = null)
        {
            string _value = _option(name);
            if ("" == _value)
            {
                _option(name, "INSERT", value);
            }
            else
            {
                _option(name, "UPDATE", value);
            }
        }

        public string getOption(string name)
        {
            return _option(name);
        }

        private DBCache _cache(string url, string action = null, string content = null, int period = 0)
        {
            if (("INSERT" == action || "UPDATE" == action) && string.IsNullOrWhiteSpace(content))
            {
                return _cache(url, "DETETE");
            }
            CheckConnectDataBase();
            try
            {
                SQLiteCommand sql = new SQLiteCommand(connection);
                switch (action)
                {
                    case "INSERT":
                        sql.CommandText = @"INSERT INTO http_cache (url,content,create_date) VALUES (@url,@content,@create_date)";
                        break;
                    case "UPDATE":
                        sql.CommandText = @"UPDATE http_cache SET content = @content, create_date = @create_date WHERE url = @url";
                        break;
                    case "DETETE":
                        sql.CommandText = @"DELETE FROM http_cache WHERE url = @url";
                        break;
                    case "SELECT":
                    default:
                        sql.CommandText = @"SELECT url, content, create_date FROM http_cache WHERE url = @url";
                        break;
                }
                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@url", url);
                sql.Parameters.AddWithValue("@content", content);
                sql.Parameters.AddWithValue("@create_date", DateTime.UtcNow.ToString(TIME_FORMAT));
                if ("SELECT" == action || null == action)
                {
                    SQLiteDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        DBCache item = new DBCache(reader["url"].ToString(), reader["content"].ToString(), period);
                        DateTime.TryParseExact(reader["create_date"].ToString(), TIME_FORMAT, new CultureInfo("en-US"), DateTimeStyles.None, out item.date);
                        return item;
                    }
                }
                else
                {
                    sql.ExecuteNonQuery();
                }

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }


        public void setCache(string url, string content = null)
        {
            DBCache item = getCache(url);
            if (null == item || "" == item.content)
            {
                _cache(url, "INSERT", content);
            }
            else
            {
                _cache(url, "UPDATE", content);
            }
        }
        public string getCacheContent(string url, int period = 24 * 60 * 60)
        {
            DBCache item = getCache(url, period);
            if (null == item)
                return null;

            return item.isActual ? item.content : null;
        }

        public DBCache getCache(string url, int period = 24 * 60 * 60)
        {
            return _cache(url, null, null, period);
        }

        public bool setSeason(int id, Dictionary<string, string> data)
        {
            if (0 == data.Count)
                return false;
            CheckConnectDataBase();
            try
            {
                SQLiteCommand sql = new SQLiteCommand(connection);
                Dictionary<string, string> dataOld = getSeason(id);
                if (0 < dataOld.Count)
                {
                    string fields = "";
                    int counts = 0;
                    foreach (KeyValuePair<string, string> item in data)
                    {
                        if (dataOld.ContainsKey(item.Key) && dataOld[item.Key] == item.Value)
                            continue;
                        fields += item.Key + " = @" + item.Key + ", ";
                        sql.Parameters.AddWithValue("@" + item.Key, item.Value);
                        counts++;
                    }
                    if (0 >= counts)
                    {
                        return false;
                    }
                    sql.Parameters.AddWithValue("@updated_date", DateTime.UtcNow.ToString(TIME_FORMAT));
                    sql.Parameters.AddWithValue("@id", id.ToString());
                    sql.CommandText = @"UPDATE season SET " + fields + "updated_date = @updated_date WHERE id = @id";
                }
                else
                {
                    data.Add("id", id.ToString());
                    string fields = "";
                    string values = "";
                    foreach (KeyValuePair<string, string> item in data)
                    {
                        fields += item.Key + ",";
                        values += "@" + item.Key + ",";
                        sql.Parameters.AddWithValue("@" + item.Key, item.Value);
                    }
                    sql.Parameters.AddWithValue("@updated_date", DateTime.UtcNow.ToString(TIME_FORMAT));
                    sql.CommandText = @"INSERT INTO season (" + fields + "updated_date) VALUES (" + values + "@updated_date)";
                }
                int updatedRows = sql.ExecuteNonQuery();
                return 0 < updatedRows;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        public bool removeSeason(int id)
        {
            CheckConnectDataBase();
            try
            {
                SQLiteCommand sql = new SQLiteCommand(connection);
                sql.CommandText = @"DELETE FROM season WHERE id = @id";
                sql.Parameters.AddWithValue("@id", id.ToString());
                int updatedRows = sql.ExecuteNonQuery();
                return 0 < updatedRows;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        public Dictionary<string, string> getSeason(int id)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            CheckConnectDataBase();
            try
            {
                SQLiteCommand sql = new SQLiteCommand(connection);
                sql.CommandText = @"SELECT season.*,http_cache.create_date  FROM season LEFT JOIN http_cache ON season.url = http_cache.url WHERE season.id = @id";
                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@id", id.ToString());
                SQLiteDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string field = reader.GetName(i);
                        string value = reader[field].ToString();
                        if (!string.IsNullOrWhiteSpace(value))
                            data.Add(field, value);
                    }
                    return data;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public Dictionary<int, int> getSeasons(int serial_id, int id = 0)
        {
            Dictionary<int, int> data = new Dictionary<int, int>();
            CheckConnectDataBase();
            try
            {
                SQLiteCommand sql = new SQLiteCommand(connection);
                sql.CommandText = @"SELECT id,season FROM season WHERE serial_id = @serial_id AND id <> @id ORDER BY season";
                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@serial_id", serial_id.ToString());
                sql.Parameters.AddWithValue("@id", id.ToString());
                SQLiteDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    int season = !string.IsNullOrEmpty(reader["season"].ToString()) ? int.Parse(reader["season"].ToString()) : 0;
                    if (!data.ContainsKey(season))
                        data.Add(season, int.Parse(reader["id"].ToString()));
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
    }
}
