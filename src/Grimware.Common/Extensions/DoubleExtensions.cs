using System;
using System.Globalization;

namespace Grimware.Extensions
{
    public static class DoubleExtensions
    {
        public static decimal ToDecimal(this double source, IFormatProvider provider)
        {
            return Convert.ToDecimal(source, provider);
        }

        public static decimal? ToDecimal(this double? source, IFormatProvider provider)
        {
            return source != null ? Convert.ToDecimal(source.Value, provider) : (decimal?) null;
        }

        public static decimal ToDecimalInvariant(this double source)
        {
            return ToDecimal(source, CultureInfo.InvariantCulture);
        }

        public static decimal? ToDecimalInvariant(this double? source)
        {
            return ToDecimal(source, CultureInfo.InvariantCulture);
        }
    }
}