using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static class CalendarExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetWeekOfYear(
            this Calendar calendar,
            DateTime time,
            CalendarWeekRule rule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Sunday) =>
            calendar?.GetWeekOfYear(time, rule, firstDayOfWeek) ?? throw new ArgumentNullException(nameof(calendar));
    }
}