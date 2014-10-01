using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace int32.Utils.Core.Extensions
{
    public static class StringExtensions
    {
        public static T FromJSON<T>(this string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
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
    }
}
