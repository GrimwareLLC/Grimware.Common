using System;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static class DecimalExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToDouble(this decimal source) => Decimal.ToDouble(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double? ToDouble(this decimal? source) => source != null ? Decimal.ToDouble(source.Value) : (double?)null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ToInt16(this decimal source) => Decimal.ToInt16(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short? ToInt16(this decimal? source) => source != null ? Decimal.ToInt16(source.Value) : (short?)null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(this decimal source) => Decimal.ToInt32(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int? ToInt32(this decimal? source) => source != null ? Decimal.ToInt32(source.Value) : (int?)null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToInt64(this decimal source) => Decimal.ToInt64(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long? ToInt64(this decimal? source) => source != null ? Decimal.ToInt64(source.Value) : (long?)null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToSingle(this decimal source) => Decimal.ToSingle(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float? ToSingle(this decimal? source) => source != null ? Decimal.ToSingle(source.Value) : (float?)null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ToUInt16(this decimal source) => Decimal.ToUInt16(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort? ToUInt16(this decimal? source) => source != null ? Decimal.ToUInt16(source.Value) : (ushort?)null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(this decimal source) => Decimal.ToUInt32(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint? ToUInt32(this decimal? source) => source != null ? Decimal.ToUInt32(source.Value) : (uint?)null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToUInt64(this decimal source) => Decimal.ToUInt64(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong? ToUInt64(this decimal? source) => source != null ? Decimal.ToUInt64(source.Value) : (ulong?)null;
    }
}
