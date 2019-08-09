using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Grimware.Extensions
{
    public static class StringExtensions
    {
        private const RegexOptions _StandardRegexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant;

        private const string _ValidGuidExpression =
            @"\A(?i)(?:"
            + @"[\da-f]{32}"
            + @"|[\da-f]{8}-[\da-f]{4}-[\da-f]{4}-[\da-f]{4}-[\da-f]{12}"
            + @"|\{[\da-f]{8}-[\da-f]{4}-[\da-f]{4}-[\da-f]{4}-[\da-f]{12}\}"
            + @"|\([\da-f]{8}-[\da-f]{4}-[\da-f]{4}-[\da-f]{4}-[\da-f]{12}\)"
            + @"|\{0x[\da-f]{8},0x[\da-f]{4},0x[\da-f]{4},\{0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2},0x[\da-f]{2}\}\}"
            + @")\Z";

        private const string _Trn = @"{~_`:=@ ""&(+,-./\t\r\n";

        private static readonly Regex _NonAlphaNumericRegex =
            new Regex("(?i)[^a-z0-9]", _StandardRegexOptions | RegexOptions.IgnoreCase);

        private static readonly Regex _NonAlphaNumericWhiteSpaceRegex =
            new Regex("(?i)[^a-z0-9 \r\n\t\v\f]", _StandardRegexOptions | RegexOptions.IgnoreCase);

        private static readonly Regex _NonAlphaRegex =
            new Regex("(?i)[^a-z]", _StandardRegexOptions | RegexOptions.IgnoreCase);

        private static readonly Regex _ToPhraseRegex =
            new Regex("([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", _StandardRegexOptions);

        private static readonly Regex _ValidGuidRegex = new Regex(_ValidGuidExpression, _StandardRegexOptions | RegexOptions.IgnoreCase);

        /// <summary>
        ///     Converts the specified text to an object
        /// </summary>
        /// <typeparam name="T">the type to convert to</typeparam>
        /// <param name="source">specified text</param>
        /// <param name="defaultValue">default value if conversion fails</param>
        /// <returns>An Object that represents the converted text.</returns>
        /// <exception cref="ArgumentNullException">type is null</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ConvertTo<T>(this string source, T defaultValue)
        {
            return (T)typeof(T).ConvertFromString(source, defaultValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool In(this string source, params string[] values)
        {
            return source.In(StringComparison.Ordinal, values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool In(this string source, StringComparison comparisonType, params string[] values)
        {
            return source == null
                ? values.Any(s => s == null)
                : values.Any(s => source.Equals(s, comparisonType));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullIf(this string source, bool ignoreCase, string value)
        {
            return source?.NullIfIn(ignoreCase, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullIfEmpty(this string source)
        {
            return source?.NullIf(String.IsNullOrEmpty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullIfIn(this string source, bool ignoreCase, params string[] values)
        {
            return source?.NullIfIn(
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal,
                values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullIfWhitespace(this string source)
        {
            return source.NullIf(String.IsNullOrWhiteSpace);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> Split(this string source, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return source == null
                ? Array.Empty<string>()
                : source.Split(separator == null ? null : new[] { separator }, options);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StripNonAlphaCharacters(this string source)
        {
            return source == null ? null : _NonAlphaRegex.Replace(source, String.Empty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StripNonAlphanumericCharacters(this string source) =>
            source != null ? _NonAlphaNumericRegex.Replace(source, String.Empty) : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string StripNonAlphanumericOrWhiteSpaceCharacters(this string source) =>
            source != null ? _NonAlphaNumericWhiteSpaceRegex.Replace(source, String.Empty) : null;

        public static string TitleCase(this string source) =>
            source != null
                ? new string(
                    source
                        .ToUpperInvariant()
                        .ToCharArray()
                        .Select((c, i) => i > 0 && _Trn.IndexOf(source[i - 1]) == -1 ? char.ToLowerInvariant(c) : c)
                        .ToArray())
                : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime? ToDateTime(this string source, IFormatProvider provider)
        {
            return ToDateTime(source, provider, DateTimeStyles.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime? ToDateTime(this string source, IFormatProvider provider, DateTimeStyles styles)
        {
            return source != null
                ? DateTime.TryParse(source, provider, styles, out var result)
                    ? (DateTime?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime? ToDateTime(this string source, string format, IFormatProvider provider)
        {
            return ToDateTime(source, format, provider, DateTimeStyles.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime? ToDateTime(
            this string source,
            string format,
            IFormatProvider provider,
            DateTimeStyles style,
            bool adjustCentury = false)
        {
            return format != null
                ? ToDateTime(source, new[] { format }, provider, style, adjustCentury)
                : throw new ArgumentNullException(nameof(format));
        }

        public static DateTime? ToDateTime(
            this string source,
            string[] formats,
            IFormatProvider provider,
            DateTimeStyles style,
            bool adjustCentury = false)
        {
            if (formats == null)
                throw new ArgumentNullException(nameof(formats));

            if (source == null)
                return null;

            if (DateTime.TryParseExact(source, formats, provider, style, out var result))
            {
                if (adjustCentury && result.Year >= 1900 && result.Year <= 1950)
                    result = result.AddYears(100);

                return result;
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime? ToDateTimeInvariant(this string source)
        {
            return ToDateTime(source, CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime? ToDateTimeInvariant(this string source, string format)
        {
            return ToDateTime(source, format, CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal? ToDecimal(this string source)
        {
            return source != null
                ? Decimal.TryParse(source, out var result)
                    ? (decimal?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal? ToDecimal(this string source, NumberStyles style, IFormatProvider provider)
        {
            return source != null
                ? Decimal.TryParse(source, style, provider, out var result)
                    ? (decimal?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double? ToDouble(this string source)
        {
            return source != null
                ? Double.TryParse(source, out var result)
                    ? (double?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double? ToDouble(this string source, NumberStyles style, IFormatProvider provider)
        {
            return source != null
                ? Double.TryParse(source, style, provider, out var result)
                    ? (double?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum? ToEnum<TEnum>(this string value, bool ignoreCase = false)
            where TEnum : struct, Enum
        {
            return Enum.TryParse(value, ignoreCase, out TEnum result) ? result : (TEnum?)null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid? ToGuid(this string source)
        {
            return source != null
                ? _ValidGuidRegex.IsMatch(source)
                    ? (Guid?)new Guid(source)
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short? ToInt16(this string source)
        {
            return source != null
                ? Int16.TryParse(source, out var result)
                    ? (short?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short? ToInt16(this string source, NumberStyles style, IFormatProvider provider)
        {
            return source != null
                ? Int16.TryParse(source, style, provider, out var result)
                    ? (short?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int? ToInt32(this string source)
        {
            return source != null
                ? Int32.TryParse(source, out var result)
                    ? (int?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int? ToInt32(this string source, NumberStyles style, IFormatProvider provider)
        {
            return source != null
                ? Int32.TryParse(source, style, provider, out var result)
                    ? (int?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long? ToInt64(this string source)
        {
            return source != null
                ? Int64.TryParse(source, out var result)
                    ? (long?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long? ToInt64(this string source, NumberStyles style, IFormatProvider provider)
        {
            return source != null
                ? Int64.TryParse(source, style, provider, out var result)
                    ? (long?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float? ToSingle(this string source)
        {
            return source != null
                ? Single.TryParse(source, out var result)
                    ? (float?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float? ToSingle(this string source, NumberStyles style, IFormatProvider provider)
        {
            return source != null
                ? Single.TryParse(source, style, provider, out var result)
                    ? (float?)result
                    : null
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan? ToTimeSpan(this string source)
        {
            return source != null
                ? TimeSpan.TryParse(source, out var result)
                    ? (TimeSpan?)result
                    : null
                : null;
        }

        public static string Translate(this string source, string from, string to)
        {
            if (String.IsNullOrEmpty(source) || String.IsNullOrEmpty(from))
                return source;

            to = to ?? String.Empty;

            var sb = new StringBuilder(source);

            for (var i = 0 ; i < sb.Length ; i++)
            {
                var j = from.IndexOf(sb[i]);
                if (j >= 0 && j < to.Length)
                    sb[i] = to[j];
                else if (j >= 0)
                    sb.Remove(i--, 1);
            }

            return sb.ToString();
        }

        public static bool TryConvertTo<T>(this string value, out T returnValue)
        {
            if (typeof(T).TryConvertFromString(value, out var convertedValue))
            {
                returnValue = (T)convertedValue;
                return true;
            }

            returnValue = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string NullIfIn(this string source, StringComparison comparison, params string[] values)
        {
            return source?.NullIf(s => s.In(comparison, values));
        }
    }
}
