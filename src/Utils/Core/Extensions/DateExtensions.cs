using System;

namespace int32.Utils.Core.Extensions
{
    public static class DateExtensions
    {
        public static DateTime Tomorrow(this DateTime date)
        {
            date.ThrowIfNull("date");
            return date.AddDays(1);
        }

        public static DateTime Yesterday(this DateTime date)
        {
            date.ThrowIfNull("date");
            return date.AddDays(-1);
        }
    }
}
