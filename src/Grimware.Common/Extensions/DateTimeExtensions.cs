using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static class DateTimeExtensions
    {
        public static Calendar DefaultCalendar { get; set; } = new GregorianCalendar(GregorianCalendarTypes.Localized);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime? AddTime(this DateTime? dateTime, TimeSpan? time) =>
            dateTime != null ? dateTime.Value + (time ?? TimeSpan.Zero) : (DateTime?)null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDayOfWeekOccurence(this DateTime time) => GetDayOfWeekOccurence(time, DefaultCalendar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDayOfWeekOccurence(this DateTime time, Calendar calendar) =>
            calendar != null ? (time.Day - 1) / 7 + 1 : throw new ArgumentNullException(nameof(calendar));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetWeekOfMonth(this DateTime time) => GetWeekOfMonth(time, DefaultCalendar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetWeekOfMonth(this DateTime time, Calendar calendar) =>
            calendar != null
                ? calendar.GetWeekOfYear(time) - calendar.GetWeekOfYear(new DateTime(time.Year, time.Month, 1))
                : throw new ArgumentNullException(nameof(calendar));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetWeekOfYear(
            this DateTime time,
            CalendarWeekRule rule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Sunday) =>
            GetWeekOfYear(time, DefaultCalendar, rule, firstDayOfWeek);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetWeekOfYear(
            this DateTime time,
            Calendar calendar,
            CalendarWeekRule rule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Sunday) =>
            calendar?.GetWeekOfYear(time, rule, firstDayOfWeek) ?? throw new ArgumentNullException(nameof(calendar));
    }
}