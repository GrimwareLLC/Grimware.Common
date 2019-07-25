using System;
using System.Globalization;

namespace Grimware.Extensions
{
    public static class CalendarExtensions
    {
        public static int GetWeekOfYear(
            this Calendar calendar,
            DateTime time,
            CalendarWeekRule rule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
        {
            if (calendar == null)
                throw new ArgumentNullException(nameof(calendar));

            return calendar.GetWeekOfYear(time, rule, firstDayOfWeek);
        }
    }
}