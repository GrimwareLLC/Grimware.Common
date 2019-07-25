using System;

namespace Grimware.Extensions
{
    public static class DecimalExtensions
    {
        public static double? ToDouble(this decimal? source)
        {
            return source != null ? Convert.ToDouble(source.Value) : (double?)null;
        }

        public static short? ToInt16(this decimal? source)
        {
            return source != null ? Convert.ToInt16(source.Value) : (short?)null;
        }

        public static int? ToInt32(this decimal? source)
        {
            return source != null ? Convert.ToInt32(source.Value) : (int?)null;
        }

        public static long? ToInt64(this decimal? source)
        {
            return source != null ? Convert.ToInt64(source.Value) : (long?)null;
        }

        public static float? ToSingle(this decimal? source)
        {
            return source != null ? Convert.ToSingle(source.Value) : (float?)null;
        }
    }
}