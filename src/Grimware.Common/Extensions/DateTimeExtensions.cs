using System;
using System.Globalization;

namespace Grimware.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly Calendar _DefaultCalendar = new GregorianCalendar(GregorianCalendarTypes.Localized);

        public static DateTime? AddTime(this DateTime? dateTime, TimeSpan? time)
        {
            return dateTime != null ? dateTime.Value + (time ?? TimeSpan.Zero) : (DateTime?)null;
        }

        public static int GetDayOfWeekOccurence(this DateTime time)
        {
            return GetDayOfWeekOccurence(time, _DefaultCalendar);
        }

        public static int GetDayOfWeekOccurence(this DateTime time, Calendar calendar)
        {
            if (calendar == null)
                throw new ArgumentNullException(nameof(calendar));

            return ((time.Day - 1) / 7) + 1;
        }

        public static int GetWeekOfMonth(this DateTime time)
        {
            return GetWeekOfMonth(time, _DefaultCalendar);
        }

        public static int GetWeekOfMonth(this DateTime time, Calendar calendar)
        {
            if (calendar == null)
                throw new ArgumentNullException(nameof(calendar));

            return calendar.GetWeekOfYear(time) - calendar.GetWeekOfYear(new DateTime(time.Year, time.Month, 1));
        }

        public static int GetWeekOfYear(
            this DateTime time,
            CalendarWeekRule rule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
        {
            return GetWeekOfYear(time, _DefaultCalendar, rule, firstDayOfWeek);
        }

        public static int GetWeekOfYear(
            this DateTime time,
            Calendar calendar,
            CalendarWeekRule rule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
        {
            if (calendar == null)
                throw new ArgumentNullException(nameof(calendar));

            return calendar.GetWeekOfYear(time, rule, firstDayOfWeek);
        }
    }
}