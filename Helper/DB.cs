﻿using System;
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
    [season] integer,
    [description] text,
    [marks_current] text,
    [marks_last] text,
    [secure_mark] text,
    [site_updated] text
);
CREATE TABLE IF NOT EXISTS [season_related] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [season_id] integer NOT NULL,
    [serial_id] integer NOT NULL,
    [season_related_id] integer NOT NULL,
    [serial_related_id] integer NOT NULL
);
CREATE TABLE IF NOT EXISTS [season_meta] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [season_id] integer NOT NULL,
    [name] text,
    [value] text
);
CREATE TABLE IF NOT EXISTS [tags] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [name] text
);
CREATE TABLE IF NOT EXISTS [season_tags] (
    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
    [season_id] integer NOT NULL,
    [tag_id] integer NOT NULL
);
CREATE TABLE IF NOT EXISTS [compilation] (
    [id] integer PRIMARY KEY NOT NULL,
    [text] text
);
CREATE TABLE IF NOT EXISTS [compilation_serial] (
    [id] integer PRIMARY KEY NOT NULL,
    [compilation_id] integer,
    [serial_id] integer
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string _option(string name, string action = null, string value = null)
        {
            if (("INSERT" == action || "UPDATE" == action) && null == value)
            {
                return _option(name, "DETETE");
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
                        return reader["value"].ToString();
                    }
                }
                else
                {
                    sql.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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
                string time_format = "yyyy-MM-dd hh:mm:ss";
                DateTime create_date = DateTime.UtcNow;


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
                sql.Parameters.AddWithValue("@create_date", create_date.ToString(time_format));
                if ("SELECT" == action || null == action)
                {
                    SQLiteDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        DBCache item = new DBCache(reader["url"].ToString(), reader["content"].ToString(), period);
                        DateTime.TryParseExact(reader["create_date"].ToString(), time_format, new CultureInfo("en-US"), DateTimeStyles.None, out item.date);
                        return item;
                    }
                }
                else
                {
                    sql.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
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

        public void setSeason(int id, string url = "")
        {
            // UNDONE: получать сезон по айди
        }

        public void getSeason(int id, int period = 24 * 60 * 60)
        {
            // UNDONE: получать сезон по айди
        }

    }
}
