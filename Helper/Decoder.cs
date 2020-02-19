using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.JScript;
using Convert = System.Convert;

namespace HomeTheater.Helper
{
    /**
     * * ORIGINAL JS
     * var v = {
     * file3_separator: '//',
     * bk0: "ololo",
     * };
     * var exist = function (x) {
     * return x != null && typeof(x) != 'undefined'
     * };
     * function fd2(x) {
     * var a;
     * a = x.substr(2);
     * for (var i = 4; i > -1; i--) {
     * if (exist(v["bk" + i])) {
     * if (v["bk" + i] != "") {
     * a = a.replace(v.file3_separator + b1(v["bk" + i]), "");
     * }
     * }
     * }
     * try {
     * a = b2(a);
     * } catch (e) {
     * a = ""
     * }
     * function b1(str) {
     * return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g, function toSolidBytes(match, p1) {
     * return String.fromCharCode("0x" + p1);
     * }));
     * }
     * function b2(str) {
     * return decodeURIComponent(atob(str).split("").map(function (c) {
     * return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
     * }).join(""));
     * }
     * return a
     * }
     * function fd3(x) {
     * var a;
     * return a
     * }
     * function fd0(s) {
     * if (s.indexOf('.') == -1) {
     * s = s.substr(1);
     * s2 = '';
     * for (i = 0; i
     * < s.length; i += 3) {
     *     s2 +='%u0' + s.slice( i, i + 3)
     *     }
     *     s= unescape( s2)
     *     }
     *     return s
     *     }
     *     var File= function ( x) {
     *     if ( x.indexOf("#2") == 0) {
     *     x= fd2( x)
     *     }
     *     if ( x.indexOf("#3") == 0 && x.indexOf( v.file3_separator)>
     *     0) {
     *     x = fd3(x)
     *     }
     *     if (x.indexOf("#0") == 0) {
     *     x = fd0(x)
     *     }
     *     return x;
     *     };
     */
    internal class Decoder
    {
        private static readonly Dictionary<string, string> v = new Dictionary<string, string>
            {{"file3_separator", "//"}, {"bk0", "ololo"}};

        public static string File(string x)
        {
            if (x.IndexOf("#2") == 0) x = fd2(x);
            if (x.IndexOf("#3") == 0 && x.IndexOf(v["file3_separator"]) > 0) x = fd3(x);
            if (x.IndexOf("#0") == 0) x = fd0(x);
            if (string.IsNullOrEmpty(x)) x = "";

            return x;
        }

        private static string fd0(string s)
        {
            if (s.IndexOf(".") == -1)
            {
                s = s.Substring(1);
                var s2 = "";
                for (var i = 0; i < s.Length; i += 3)
                    s2 += "%u0" + s.Substring(i, i + 3);
                s = Regex.Unescape(s2);
            }

            return s;
        }

#pragma warning disable IDE0060 // Удалите неиспользуемый параметр
        private static string fd3(string x)
#pragma warning restore IDE0060 // Удалите неиспользуемый параметр
        {
            string a = null;
            return a;
        }

        private static string fd2(string x)
        {
            var a = x.Substring(2);
            for (var i = 4; i > -1; i--)
                if (v.ContainsKey("bk" + i))
                    if (!string.IsNullOrEmpty(v["bk" + i]))
                        a = a.Replace(v["file3_separator"] + b1(v["bk" + i]), "");
            try
            {
                a = b2(a);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
                a = "";
            }

            return a;
        }

        private static string b2(string str)
        {
            return HttpUtility.UrlDecode(atob(str));
        }

        private static string b1(string str)
        {
            return btoa(Regex.Replace(HttpUtility.UrlEncode(str), "%([0-9A-F]{2})", toSolidBytes));
        }

        private static string toSolidBytes(Match match)
        {
            return StringConstructor.fromCharCode("0x" + match.Groups[1]);
        }

        private static string atob(string toEncode)
        {
            var bytes = Convert.FromBase64String(toEncode);
            var toReturn = Encoding.ASCII.GetString(bytes);
            return toReturn;
        }

        private static string btoa(string toEncode)
        {
            var bytes = Encoding.GetEncoding(28591).GetBytes(toEncode);
            var toReturn = Convert.ToBase64String(bytes);
            return toReturn;
        }
    }
}