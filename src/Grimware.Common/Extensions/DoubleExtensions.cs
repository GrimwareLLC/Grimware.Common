using System;
using System.Globalization;

namespace Grimware.Extensions
{
    public static class DoubleExtensions
    {
        public static decimal? ToDecimalInvariant(this double? source) =>
            ToDecimal(source, CultureInfo.InvariantCulture);

        public static decimal? ToDecimal(this double? source, IFormatProvider provider) =>
            source != null ? Convert.ToDecimal(source, provider) : (decimal?)null;
    }
}