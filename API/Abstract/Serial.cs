using System;
using System.Collections.Generic;
using System.Linq;
using HomeTheater.Helper;

namespace HomeTheater.API.Abstract
{
    #region Delegate

    public delegate Dictionary<string, string> SerialLoadAction();

    public delegate bool SerialSaveAction(Dictionary<string, string> data);

    #endregion

    internal abstract class Serial : Base
    {
        #region Словари

        protected Dictionary<string, DateTime> __data_date = new Dictionary<string, DateTime>();
        protected Dictionary<string, float> __data_float = new Dictionary<string, float>();
        protected Dictionary<string, int> __data_int = new Dictionary<string, int>();
        protected Dictionary<string, string> __data_new = new Dictionary<string, string>();
        protected Dictionary<string, string> __data_old = new Dictionary<string, string>();

        #endregion

        #region Общие обработчики переменных

        protected void LoadValues(SerialLoadAction callback)
        {
            __data_old = callback();
            __data_date = new Dictionary<string, DateTime>();
            __data_float = new Dictionary<string, float>();
            __data_int = new Dictionary<string, int>();
            __data_new = __data_new.Where(entry =>
                    !__data_old.ContainsKey(entry.Key) || __data_old[entry.Key] != entry.Value)
                .ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        protected void SaveValues(SerialSaveAction callback)
        {
            if (0 == __data_new.Count)
                return;
            var data = __data_new.Where(entry =>
                    !__data_old.ContainsKey(entry.Key) || __data_old[entry.Key] != entry.Value)
                .ToDictionary(entry => entry.Key, entry => entry.Value);
            var result = false;
            string[] exclude =
            {
                "created_date",
                "updated_date",
                "cached_date"
            };
            foreach (var item in exclude)
                if (data.ContainsKey(item))
                    data.Remove(item);
            if (0 < data.Count)
                result = callback(data);
            if (!result)
                return;
            foreach (var item in data)
                if (__data_old.ContainsKey(item.Key))
                    __data_old[item.Key] = item.Value;
                else
                    __data_old.Add(item.Key, item.Value);
            __data_date = new Dictionary<string, DateTime>();
            __data_float = new Dictionary<string, float>();
            __data_int = new Dictionary<string, int>();
            __data_new = new Dictionary<string, string>();
        }

        protected virtual void CallValue(string name, string value = null, string value_old = null)
        {
        }

        #endregion

        #region Getter

        protected string GetValue(string name, string _default = "")
        {
            if (__data_new.ContainsKey(name) && !string.IsNullOrEmpty(__data_new[name]))
                return __data_new[name];
            if (__data_old.ContainsKey(name) && !string.IsNullOrEmpty(__data_old[name]))
                return __data_old[name];
            return _default;
        }

        protected int GetValueInt(string name, int _default = 0)
        {
            if (__data_int.ContainsKey(name) && __data_int[name] != 0)
                return __data_int[name];
            var value = IntVal(GetValue(name, _default.ToString()));
            if (__data_int.ContainsKey(name))
                __data_int[name] = value;
            else
                __data_int.Add(name, value);
            return __data_int[name];
        }

        protected float GetValueFloat(string name, float _default = 0)
        {
            if (__data_float.ContainsKey(name) && __data_float[name] != 0)
                return __data_float[name];
            var value = floatVal(GetValue(name, _default.ToString()));
            if (__data_float.ContainsKey(name))
                __data_float[name] = value;
            else
                __data_float.Add(name, value);
            return __data_float[name];
        }

        protected DateTime GetValueDate(string name)
        {
            return GetValueDate(name, new DateTime());
        }

        protected DateTime GetValueDate(string name, string format = DB.TIME_FORMAT)
        {
            return GetValueDate(name, new DateTime(), format);
        }

        protected DateTime GetValueDate(string name, DateTime _default = new DateTime(), string format = DB.TIME_FORMAT)
        {
            if (__data_date.ContainsKey(name) && __data_date[name] != new DateTime())
                return __data_date[name];
            var value = DateVal(GetValue(name, _default.ToString(format)), format);
            if (__data_date.ContainsKey(name))
                __data_date[name] = value;
            else
                __data_date.Add(name, value);
            return __data_date[name];
        }

        #endregion

        #region Setter

        protected void SetValue(string name, string value, string _default = "")
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            var value_old = GetValue(name, _default);
            if (value_old == value)
                return;
            if (__data_new.ContainsKey(name))
                __data_new[name] = value;
            else
                __data_new.Add(name, value);
            CallValue(name, value, value_old);
        }

        protected void SetValueEmpty(string name, string value = "")
        {
            var value_old = GetValue(name);
            if (value_old == value)
                return;
            if (__data_new.ContainsKey(name))
                __data_new[name] = value;
            else
                __data_new.Add(name, value);
            CallValue(name, value, value_old);
        }

        protected void SetValue(string name, int value, int _default = 0)
        {
            if (0 == value)
                return;
            var value_old = GetValueInt(name, _default);
            if (value_old == value)
                return;
            if (__data_int.ContainsKey(name))
                __data_int[name] = value;
            else
                __data_int.Add(name, value);
            SetValue(name, value.ToString());
        }

        protected void SetValue(string name, float value, float _default = 0)
        {
            if (0 == value)
                return;
            var value_old = GetValueFloat(name, _default);
            if (value_old == value)
                return;
            if (__data_float.ContainsKey(name))
                __data_float[name] = value;
            else
                __data_float.Add(name, value);
            SetValue(name, value.ToString());
        }

        protected void SetValue(string name, DateTime value, string format = DB.TIME_FORMAT)
        {
            if (new DateTime() == value)
                return;
            var value_old = GetValueDate(name, format);
            if (value_old == value)
                return;
            if (__data_date.ContainsKey(name))
                __data_date[name] = value;
            else
                __data_date.Add(name, value);
            SetValue(name, value.ToString(format));
        }

        #endregion
    }
}