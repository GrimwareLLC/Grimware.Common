// ReSharper disable once RedundantUsingDirective
// Using directive required for net461 compilation, but not for netstandard2.0


using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Grimware.Extensions
{
    public static class StringExtensions
    {
        private const string _CasingSymbols = "{~_`:=@ \"&'(+,-./\t\r\n";
        private const RegexOptions _StandardRegexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant;

        private const string _UnusualGuidRegexExpression =
            @"\{0x[\da-f]{8},0x[\da-f]{4},0x[\da-f]{4},"
            + @"\{0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2}\}"
            + @"\}";

        private const string _ValidGuidRegexExpression =
            @"\A(?i)(?:"
            + @"[\da-f]{32}"
            + @"|[\da-f]{8}-[\da-f]{4}-[\da-f]{4}-[\da-f]{4}-[\da-f]{12}"
            + @"|\{[\da-f]{8}-[\da-f]{4}-[\da-f]{4}-[\da-f]{4}-[\da-f]{12}\}"
            + @"|\([\da-f]{8}-[\da-f]{4}-[\da-f]{4}-[\da-f]{4}-[\da-f]{12}\)"
            + @"|"
            + _UnusualGuidRegexExpression
            + @")\Z";

        private static readonly Regex _NonAlphaNumericRegex =
            new Regex("(?i)[^a-z0-9]", _StandardRegexOptions | RegexOptions.IgnoreCase);

        private static readonly Regex _NonAlphaNumericWhiteSpaceRegex =
            new Regex("(?i)[^a-z0-9 \r\n\t\v\f]", _StandardRegexOptions | RegexOptions.IgnoreCase);

        private static readonly Regex _NonAlphaRegex =
            new Regex("(?i)[^a-z]", _StandardRegexOptions | RegexOptions.IgnoreCase);

        private static readonly Regex _ToPhraseRegex =
            new Regex("([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", _StandardRegexOptions);

        private static readonly Regex _ValidGuidRegex = new Regex(_ValidGuidRegexExpression, _StandardRegexOptions | RegexOptions.IgnoreCase);

        /// <summary>
        ///     Converts the specified text to an object
        /// </summary>
        /// <typeparam name="T">the type to convert to</typeparam>
        /// <param name="source">specified text</param>
        /// <param name="defaultValue">default value if conversion fails</param>
        /// <returns>An Object that represents the converted text.</returns>
        public static T ConvertTo<T>(this string source, T defaultValue)
        {
            return TryConvertTo<T>(source, out var result) ? result : defaultValue;
        }

        public static bool In(this string source, params string[] values)
        {
            return source.In(StringComparison.Ordinal, values);
        }

        public static bool In(this string source, StringComparison comparisonType, params string[] values)
        {
            return source == null
                ? values.Any(s => s == null)
                : values.Any(s => source.Equals(s, comparisonType));
        }

        public static string NullIf(this string source, string value)
        {
            return NullIf(source, value, false);
        }

        public static string NullIf(this string source, string value, bool ignoreCase)
        {
            return source?.NullIfIn(ignoreCase, value);
        }

        public static string NullIf(this string source, string value, StringComparison comparisonType)
        {
            return source.NullIfIn(comparisonType, value);
        }

        public static string NullIfEmpty(this string source)
        {
            return source?.NullIf(String.IsNullOrEmpty);
        }

        public static string NullIfIn(this string source, params string[] values)
        {
            return NullIfIn(source, false, values);
        }

        public static string NullIfIn(this string source, bool ignoreCase, params string[] values)
        {
            return source?.NullIfIn(
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal,
                values);
        }

        public static string NullIfIn(this string source, StringComparison comparison, params string[] values)
        {
            return source?.NullIf(s => s.In(comparison, values));
        }

        public static string NullIfWhitespace(this string source)
        {
            return source.NullIf(String.IsNullOrWhiteSpace);
        }

        public static string StripNonAlphaCharacters(this string source)
        {
            return source == null ? null : _NonAlphaRegex.Replace(source, String.Empty);
        }

        public static string StripNonAlphanumericCharacters(this string source)
        {
            return source != null ? _NonAlphaNumericRegex.Replace(source, String.Empty) : null;
        }

        public static string StripNonAlphanumericOrWhiteSpaceCharacters(this string source)
        {
            return source != null ? _NonAlphaNumericWhiteSpaceRegex.Replace(source, String.Empty) : null;
        }

        public static string TitleCase(this string source)
        {
            return TitleCase(source, CultureInfo.InvariantCulture);
        }

        public static string TitleCase(this string source, CultureInfo culture)
        {
            if (source != null)
                return new string(
                    source
                        .ToUpper(culture)
                        .ToCharArray()
                        .Select((c, i) => i > 0 && _CasingSymbols.IndexOf(source[i - 1]) == -1 ? Char.ToLower(c, culture) : c)
                        .ToArray());

            return null;
        }

        public static DateTime? ToDateTime(this string source, IFormatProvider provider)
        {
            return ToDateTime(source, provider, DateTimeStyles.None);
        }

        public static DateTime? ToDateTime(this string source, IFormatProvider provider, DateTimeStyles styles)
        {
            if (source != null)
                return DateTime.TryParse(source, provider, styles, out var result)
                    ? (DateTime?)result
                    : null;

            return null;
        }

        public static DateTime? ToDateTime(this string source, string format, IFormatProvider provider)
        {
            return ToDateTime(source, format, provider, false);
        }

        public static DateTime? ToDateTime(this string source, string format, IFormatProvider provider, bool adjustCentury)
        {
            return ToDateTime(source, format, provider, DateTimeStyles.None, adjustCentury);
        }

        public static DateTime? ToDateTime(this string source, string format, IFormatProvider provider, DateTimeStyles style)
        {
            return ToDateTime(source, format, provider, style, false);
        }

        public static DateTime? ToDateTime(this string source, string format, IFormatProvider provider, DateTimeStyles style, bool adjustCentury)
        {
            return format != null
                ? ToDateTime(source, new[] {format}, provider, style, adjustCentury)
                : throw new ArgumentNullException(nameof(format));
        }

        public static DateTime? ToDateTime(this string source, string[] formats, IFormatProvider provider)
        {
            return ToDateTime(source, formats, provider, DateTimeStyles.None);
        }

        public static DateTime? ToDateTime(this string source, string[] formats, IFormatProvider provider, DateTimeStyles style)
        {
            return ToDateTime(source, formats, provider, style, false);
        }

        public static DateTime? ToDateTime(this string source, string[] formats, IFormatProvider provider, bool adjustCentury)
        {
            return ToDateTime(source, formats, provider, DateTimeStyles.None, adjustCentury);
        }

        public static DateTime? ToDateTime(this string source, string[] formats, IFormatProvider provider, DateTimeStyles style, bool adjustCentury)
        {
            const int AdjustWindowStart = 1900;
            const int AdjustWindowEnd = 1950;
            const int YearsInCentury = 100;

            if (formats == null)
                throw new ArgumentNullException(nameof(formats));

            if (source == null)
                return null;

            if (DateTime.TryParseExact(source, formats, provider, style, out var result))
            {
                if (adjustCentury && result.Year >= AdjustWindowStart && result.Year <= AdjustWindowEnd) result = result.AddYears(YearsInCentury);

                return result;
            }

            return null;
        }

        public static DateTime? ToDateTimeInvariant(this string source)
        {
            return ToDateTime(source, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToDateTimeInvariant(this string source, string format)
        {
            return ToDateTime(source, format, CultureInfo.InvariantCulture);
        }

        public static decimal? ToDecimal(this string source)
        {
            if (source != null)
                return Decimal.TryParse(source, out var result)
                    ? (decimal?)result
                    : null;

            return null;
        }

        public static decimal? ToDecimal(this string source, NumberStyles style, IFormatProvider provider)
        {
            if (source != null)
                return Decimal.TryParse(source, style, provider, out var result)
                    ? (decimal?)result
                    : null;

            return null;
        }

        public static double? ToDouble(this string source)
        {
            if (source != null)
                return Double.TryParse(source, out var result)
                    ? (double?)result
                    : null;

            return null;
        }

        public static double? ToDouble(this string source, NumberStyles style, IFormatProvider provider)
        {
            if (source != null)
                return Double.TryParse(source, style, provider, out var result)
                    ? (double?)result
                    : null;

            return null;
        }

        public static TEnum? ToEnum<TEnum>(this string value)
            where TEnum : struct, Enum
        {
            return ToEnum<TEnum>(value, false);
        }

        public static TEnum? ToEnum<TEnum>(this string value, bool ignoreCase)
            where TEnum : struct, Enum
        {
            return Enum.TryParse(value, ignoreCase, out TEnum result) ? result : (TEnum?)null;
        }

        public static Guid? ToGuid(this string source)
        {
            if (source != null)
                return _ValidGuidRegex.IsMatch(source)
                    ? (Guid?)new Guid(source)
                    : null;

            return null;
        }

        public static short? ToInt16(this string source)
        {
            if (source != null)
                return Int16.TryParse(source, out var result)
                    ? (short?)result
                    : null;

            return null;
        }

        public static short? ToInt16(this string source, NumberStyles style, IFormatProvider provider)
        {
            if (source != null)
                return Int16.TryParse(source, style, provider, out var result)
                    ? (short?)result
                    : null;

            return null;
        }

        public static int? ToInt32(this string source)
        {
            if (source != null)
                return Int32.TryParse(source, out var result)
                    ? (int?)result
                    : null;

            return null;
        }

        public static int? ToInt32(this string source, NumberStyles style, IFormatProvider provider)
        {
            if (source != null)
                return Int32.TryParse(source, style, provider, out var result)
                    ? (int?)result
                    : null;

            return null;
        }

        public static long? ToInt64(this string source)
        {
            if (source != null)
                return Int64.TryParse(source, out var result)
                    ? (long?)result
                    : null;

            return null;
        }

        public static long? ToInt64(this string source, NumberStyles style, IFormatProvider provider)
        {
            if (source != null)
                return Int64.TryParse(source, style, provider, out var result)
                    ? (long?)result
                    : null;

            return null;
        }

        public static string ToPhrase(this string source)
        {
            return source != null ? _ToPhraseRegex.Replace(source, "$1 ").Trim() : null;
        }

        public static unsafe SecureString ToSecuredString(this string source)
        {
            if (source == null)
                return null;

            fixed (char* pChars = source.ToCharArray())
            {
                var secured = new SecureString(pChars, source.Length);
                return secured;
            }
        }

        public static float? ToSingle(this string source)
        {
            if (source != null)
                return Single.TryParse(source, out var result)
                    ? (float?)result
                    : null;

            return null;
        }

        public static float? ToSingle(this string source, NumberStyles style, IFormatProvider provider)
        {
            if (source != null)
                return Single.TryParse(source, style, provider, out var result)
                    ? (float?)result
                    : null;

            return null;
        }

        public static TimeSpan? ToTimeSpan(this string source)
        {
            return ToTimeSpan(source, CultureInfo.InvariantCulture);
        }

        public static TimeSpan? ToTimeSpan(this string source, IFormatProvider formatProvider)
        {
            if (source != null)
                return TimeSpan.TryParse(source, formatProvider, out var result)
                    ? (TimeSpan?)result
                    : null;

            return null;
        }

        public static string Translate(this string source, string from, string to)
        {
            if (String.IsNullOrEmpty(source) || String.IsNullOrEmpty(from))
                return source;

            if (to == null)
                to = String.Empty;

            var sb = new StringBuilder(source);

            for (var i = 0; i < sb.Length; i++)
            {
                var j = from.IndexOf(sb[i]);
                if (j >= 0 && j < to.Length)
                    sb[i] = to[j];
                else if (j >= 0)
                    sb.Remove(i--, 1);
            }

            return sb.ToString();
        }

        public static bool TryConvertTo<T>(this string source, out T returnValue)
        {
            var type = typeof(T);

            var successful = true;
            object convertedValue = null;

            try
            {
                if (source == null)
                {
                    if (type.IsValueType)
                        successful = false;
                }
                else
                {
                    convertedValue = type.IsInstanceOfType(source) ? source : TryConvertFromStringInternal(type, source);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            {
                successful = false;
            }
#pragma warning restore CA1031 // Do not catch general exception types

            returnValue = successful ? (T)convertedValue : default;
            return successful;
        }

        private static object TryConvertFromStringInternal(Type type, string value)
        {
            if (type.IsEnum) return Enum.Parse(type, value, true);

            var typeConverter = TypeDescriptor.GetConverter(type);
            return typeConverter.CanConvertFrom(typeof(string))
                ? typeConverter.ConvertFromString(value)
                : null;
        }

#if NET46
        public static IEnumerable<string> Split(this string source, string separator)
        {
            return Split(source, separator, StringSplitOptions.None);
        }

        public static IEnumerable<string> Split(this string source, string separator, StringSplitOptions options)
        {
            var separatorList = separator == null ? null : new[] {separator};

            return source != null ? source.Split(separatorList, options) : Array.Empty<string>();
        }
#endif
    }
}
