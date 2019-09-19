using System;
using System.Globalization;

namespace Grimware.Extensions
{
    public static class CalendarExtensions
    {
        public static int GetWeekOfYear(this Calendar calendar, DateTime time, DayOfWeek firstDayOfWeek)
        {
            return GetWeekOfYear(calendar, time, CalendarWeekRule.FirstDay, firstDayOfWeek);
        }

        public static int GetWeekOfYear(this Calendar calendar, DateTime time)
        {
            return GetWeekOfYear(calendar, time, CalendarWeekRule.FirstDay);
        }

        public static int GetWeekOfYear(this Calendar calendar, DateTime time, CalendarWeekRule rule)
        {
            return GetWeekOfYear(calendar, time, rule, DayOfWeek.Sunday);
        }

        public static int GetWeekOfYear(this Calendar calendar, DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
        {
            return calendar?.GetWeekOfYear(time, rule, firstDayOfWeek) ?? throw new ArgumentNullException(nameof(calendar));
        }
    }
}
