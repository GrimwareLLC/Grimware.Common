using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static class DateTimeExtensions
    {
        private static Calendar DefaultCalendar { get; } = new GregorianCalendar(GregorianCalendarTypes.Localized);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime Add(this DateTime dateTime, TimeSpan? time)
        {
            return Add((DateTime?) dateTime, time) ?? dateTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime? Add(this DateTime? dateTime, TimeSpan? time)
        {
            return dateTime?.Add(time ?? TimeSpan.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDayOfWeekOccurence(this DateTime time)
        {
            return GetDayOfWeekOccurence(time, DefaultCalendar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDayOfWeekOccurence(this DateTime time, Calendar calendar)
        {
            return calendar != null ? (time.Day - 1) / 7 + 1 : throw new ArgumentNullException(nameof(calendar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetWeekOfMonth(this DateTime time)
        {
            return GetWeekOfMonth(time, DefaultCalendar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetWeekOfMonth(this DateTime time, Calendar calendar)
        {
            return calendar != null
                ? calendar.GetWeekOfYear(time) - calendar.GetWeekOfYear(new DateTime(time.Year, time.Month, 1))
                : throw new ArgumentNullException(nameof(calendar));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetWeekOfYear(
            this DateTime time,
            CalendarWeekRule rule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
        {
            return GetWeekOfYear(time, DefaultCalendar, rule, firstDayOfWeek);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetWeekOfYear(
            this DateTime time,
            Calendar calendar,
            CalendarWeekRule rule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
        {
            return calendar?.GetWeekOfYear(time, rule, firstDayOfWeek) ?? throw new ArgumentNullException(nameof(calendar));
        }
    }
}