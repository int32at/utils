using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace int32.Utils.Core.Extensions
{
    public static class StringExtensions
    {
        public static T FromJSON<T>(this string s)
        {
            return s.FromJSON<T>(Constants.JsonSerializerDefaultSettings);
        }
        public static T FromJSON<T>(this string s, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(s, settings);
        }

        public static bool Matches(this string s, string regex)
        {
            s.ThrowIfNull("s");
            regex.ThrowIfNull("regex");
            return Regex.IsMatch(s, regex);
        }

        public static void Matches(this string s, string regex, Action action)
        {
            action.ThrowIfNull("action");
            if (s.Matches(regex))
                action();
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static string With(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        public static T ParseEnum<T>(this string s)
        {
            return (T)Enum.Parse(typeof(T), s, true);
        }
    }
}
