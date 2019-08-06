﻿using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static class DoubleExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal ToDecimal(this double source, IFormatProvider provider) =>
            Convert.ToDecimal(source, provider);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal? ToDecimal(this double? source, IFormatProvider provider) =>
            source != null ? Convert.ToDecimal(source.Value, provider) : (decimal?)null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal ToDecimalInvariant(this double source) => ToDecimal(source, CultureInfo.InvariantCulture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal? ToDecimalInvariant(this double? source) => ToDecimal(source, CultureInfo.InvariantCulture);
    }
}
