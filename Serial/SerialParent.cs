using System;
using System.Collections.Generic;
using System.Linq;
using HomeTheater.Helper;

namespace HomeTheater.Serial
{
    public delegate Dictionary<string, string> SerialLoadAction();

    public delegate bool SerialSaveAction(Dictionary<string, string> data);

    internal abstract class SerialParent : APIParent
    {
        protected Dictionary<string, DateTime> __data_date = new Dictionary<string, DateTime>();
        protected Dictionary<string, float> __data_float = new Dictionary<string, float>();
        protected Dictionary<string, int> __data_int = new Dictionary<string, int>();
        protected Dictionary<string, string> __data_new = new Dictionary<string, string>();
        protected Dictionary<string, string> __data_old = new Dictionary<string, string>();

        protected string getValue(string name, string _default = "")
        {
            if (__data_new.ContainsKey(name))
                return __data_new[name];
            if (__data_old.ContainsKey(name))
                return __data_old[name];
            return _default;
        }

        protected int getValueInt(string name, int _default = 0)
        {
            if (__data_int.ContainsKey(name) && __data_int[name] != 0)
                return __data_int[name];
            var value = IntVal(getValue(name, _default.ToString()));
            if (__data_int.ContainsKey(name))
                __data_int[name] = value;
            else
                __data_int.Add(name, value);
            return __data_int[name];
        }

        protected float getValueFloat(string name, float _default = 0)
        {
            if (__data_float.ContainsKey(name) && __data_float[name] != 0)
                return __data_float[name];
            var value = floatVal(getValue(name, _default.ToString()));
            if (__data_float.ContainsKey(name))
                __data_float[name] = value;
            else
                __data_float.Add(name, value);
            return __data_float[name];
        }

        protected DateTime getValueDate(string name)
        {
            return getValueDate(name, new DateTime());
        }

        protected DateTime getValueDate(string name, string format = DB.TIME_FORMAT)
        {
            return getValueDate(name, new DateTime(), format);
        }

        protected DateTime getValueDate(string name, DateTime _default = new DateTime(), string format = DB.TIME_FORMAT)
        {
            if (__data_date.ContainsKey(name) && __data_date[name] != new DateTime())
                return __data_date[name];
            var value = DateVal(getValue(name, _default.ToString(format)), format);
            if (__data_date.ContainsKey(name))
                __data_date[name] = value;
            else
                __data_date.Add(name, value);
            return __data_date[name];
        }

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
            var exclude = new[]
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

        protected abstract void callbackValue(string name, string value);

        protected void setValue(string name, string value, string _default = "")
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            var value_old = getValue(name, _default);
            if (value_old == value)
                return;
            if (__data_new.ContainsKey(name))
                __data_new[name] = value;
            else
                __data_new.Add(name, value);
            callbackValue(name, value);
        }

        protected void setValue(string name, int value, int _default = 0)
        {
            if (0 == value)
                return;
            var value_old = getValueInt(name, _default);
            if (value_old == value)
                return;
            if (__data_int.ContainsKey(name))
                __data_int[name] = value;
            else
                __data_int.Add(name, value);
            setValue(name, value.ToString());
        }

        protected void setValue(string name, float value, float _default = 0)
        {
            if (0 == value)
                return;
            var value_old = getValueFloat(name, _default);
            if (value_old == value)
                return;
            if (__data_float.ContainsKey(name))
                __data_float[name] = value;
            else
                __data_float.Add(name, value);
            setValue(name, value.ToString());
        }

        protected void setValue(string name, DateTime value, string format = DB.TIME_FORMAT)
        {
            if (new DateTime() == value)
                return;
            var value_old = getValueDate(name, format);
            if (value_old == value)
                return;
            if (__data_date.ContainsKey(name))
                __data_date[name] = value;
            else
                __data_date.Add(name, value);
            setValue(name, value.ToString(format));
        }
    }
}