using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace HomeTheater.Serial
{
    internal class APIParent
    {
        protected const RegexOptions REGEX_C = RegexOptions.Compiled;
        protected const RegexOptions REGEX_IC = REGEX_C | RegexOptions.IgnoreCase;
        protected const RegexOptions REGEX_ICS = REGEX_IC | RegexOptions.Singleline;

        protected string SERVER_CDN_URL =
            Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL2Nkbi5zZWFzb252YXIucnUvb2Jsb2prYS8="));

        protected string SERVER_URL = Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL3NlYXNvbnZhci5ydQ=="));

        public static string Match(string input, string pattern, RegexOptions options = REGEX_C, int index = 0)
        {
            var result = "";

            var resultmatch = Matches(input, pattern, options);
            if (index < resultmatch.Count) result = resultmatch[index];

            return result;
        }

        public static List<string> Matches(string input, string pattern, RegexOptions options = REGEX_C)
        {
            var result = new List<string>();
            var resultmatch = Regex.Match(input, pattern, options);
            if (resultmatch.Success && 0 < resultmatch.Groups.Count)
                for (var i = 0; i < resultmatch.Groups.Count; i++)
                    result.Add(resultmatch.Groups[i].ToString().Trim());

            return result;
        }

        public static int IntVal(string input)
        {
            var result = 0;
            input = Regex.Replace(input, "[^0-9-]*", "");
            if (!string.IsNullOrEmpty(input))
                int.TryParse(input, out result);

            return result;
        }

        public static float floatVal(string input)
        {
            float result = 0;
            input = Regex.Replace(input, "[^0-9.,-]*", "");
            input = input.Replace(".", ",");
            if (!string.IsNullOrEmpty(input))
                float.TryParse(input, out result);

            return result;
        }

        public static DateTime DateVal(string input, string format)
        {
            var result = new DateTime();
            if (!string.IsNullOrWhiteSpace(input))
                DateTime.TryParseExact(input, format, new CultureInfo("de-DE"), DateTimeStyles.None, out result);


            return result;
        }
    }
}