using System;
using System.Globalization;

namespace Grimware.Extensions
{
    public static class DateTimeExtensions
    {
        private static Calendar DefaultCalendar { get; } = new GregorianCalendar(GregorianCalendarTypes.Localized);

        public static DateTime Add(this DateTime dateTime, TimeSpan? time)
        {
            return Add((DateTime?)dateTime, time) ?? dateTime;
        }

        public static DateTime? Add(this DateTime? dateTime, TimeSpan? time)
        {
            return dateTime?.Add(time ?? TimeSpan.Zero);
        }

        public static int GetDayOfWeekOccurence(this DateTime time)
        {
            return GetDayOfWeekOccurence(time, DefaultCalendar);
        }

        public static int GetDayOfWeekOccurence(this DateTime time, Calendar calendar)
        {
            const int DaysInWeek = 7;

            return calendar != null ? (time.Day - 1) / DaysInWeek + 1 : throw new ArgumentNullException(nameof(calendar));
        }

        public static int GetWeekOfMonth(this DateTime time)
        {
            return GetWeekOfMonth(time, DefaultCalendar);
        }

        public static int GetWeekOfMonth(this DateTime time, Calendar calendar)
        {
            return calendar != null
                ? calendar.GetWeekOfYear(time) - calendar.GetWeekOfYear(new DateTime(time.Year, time.Month, 1))
                : throw new ArgumentNullException(nameof(calendar));
        }

        public static int GetWeekOfYear(this DateTime time)
        {
            return GetWeekOfYear(time, DayOfWeek.Sunday);
        }

        public static int GetWeekOfYear(this DateTime time, DayOfWeek firstDayOfWeek)
        {
            return GetWeekOfYear(time, CalendarWeekRule.FirstDay, firstDayOfWeek);
        }

        public static int GetWeekOfYear(this DateTime time, CalendarWeekRule rule)
        {
            return GetWeekOfYear(time, rule, DayOfWeek.Sunday);
        }

        public static int GetWeekOfYear(this DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
        {
            return GetWeekOfYear(time, DefaultCalendar, rule, firstDayOfWeek);
        }

        public static int GetWeekOfYear(this DateTime time, Calendar calendar)
        {
            return GetWeekOfYear(time, calendar, CalendarWeekRule.FirstDay);
        }

        public static int GetWeekOfYear(this DateTime time, Calendar calendar, CalendarWeekRule rule)
        {
            return GetWeekOfYear(time, calendar, rule, DayOfWeek.Sunday);
        }

        public static int GetWeekOfYear(this DateTime time, Calendar calendar, DayOfWeek firstDayOfWeek)
        {
            return GetWeekOfYear(time, calendar, CalendarWeekRule.FirstDay, firstDayOfWeek);
        }

        public static int GetWeekOfYear(this DateTime time, Calendar calendar, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
        {
            return calendar?.GetWeekOfYear(time, rule, firstDayOfWeek) ?? throw new ArgumentNullException(nameof(calendar));
        }
    }
}
