using System;

namespace Grimware.Extensions
{
    public static class DecimalExtensions
    {
        public static double ToDouble(this decimal source)
        {
            return Decimal.ToDouble(source);
        }

        public static double? ToDouble(this decimal? source)
        {
            return source != null ? Decimal.ToDouble(source.Value) : (double?)null;
        }

        public static short ToInt16(this decimal source)
        {
            return Decimal.ToInt16(source);
        }

        public static short? ToInt16(this decimal? source)
        {
            return source != null ? Decimal.ToInt16(source.Value) : (short?)null;
        }

        public static int ToInt32(this decimal source)
        {
            return Decimal.ToInt32(source);
        }

        public static int? ToInt32(this decimal? source)
        {
            return source != null ? Decimal.ToInt32(source.Value) : (int?)null;
        }

        public static long ToInt64(this decimal source)
        {
            return Decimal.ToInt64(source);
        }

        public static long? ToInt64(this decimal? source)
        {
            return source != null ? Decimal.ToInt64(source.Value) : (long?)null;
        }

        public static float ToSingle(this decimal source)
        {
            return Decimal.ToSingle(source);
        }

        public static float? ToSingle(this decimal? source)
        {
            return source != null ? Decimal.ToSingle(source.Value) : (float?)null;
        }
    }
}
