using System;
using System.Globalization;

namespace Grimware.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly Calendar _DefaultCalendar = new GregorianCalendar(GregorianCalendarTypes.Localized);

        public static DateTime? AddTime(this DateTime? dateTime, TimeSpan? time) =>
            dateTime != null ? dateTime.Value + (time ?? TimeSpan.Zero) : (DateTime?)null;

        public static int GetDayOfWeekOccurence(this DateTime time) => GetDayOfWeekOccurence(time, _DefaultCalendar);

        public static int GetDayOfWeekOccurence(this DateTime time, Calendar calendar) =>
            calendar != null ? (time.Day - 1) / 7 + 1 : throw new ArgumentNullException(nameof(calendar));

        public static int GetWeekOfMonth(this DateTime time) => GetWeekOfMonth(time, _DefaultCalendar);

        public static int GetWeekOfMonth(this DateTime time, Calendar calendar) =>
            calendar != null
                ? calendar.GetWeekOfYear(time) - calendar.GetWeekOfYear(new DateTime(time.Year, time.Month, 1))
                : throw new ArgumentNullException(nameof(calendar));

        public static int GetWeekOfYear(
            this DateTime time,
            CalendarWeekRule rule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Sunday) =>
            GetWeekOfYear(time, _DefaultCalendar, rule, firstDayOfWeek);

        public static int GetWeekOfYear(
            this DateTime time,
            Calendar calendar,
            CalendarWeekRule rule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Sunday) =>
            calendar?.GetWeekOfYear(time, rule, firstDayOfWeek) ?? throw new ArgumentNullException(nameof(calendar));
    }
}