using System;

namespace RingDownConsole.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsBetween(this DateTime dateTime, int daysAgo)
        {
            var now = DateTime.UtcNow;
            var minDate = now.AddDays(daysAgo * -1);
            return dateTime >= minDate && dateTime <= now;
        }

        public static string PrettyLocalDate(this DateTime dateTime)
        {
            return $"{dateTime.ToLocalTime().ToShortDateString()} {dateTime.ToLocalTime().ToShortTimeString()}";
        }
    }
}
