using System;
using System.Globalization;
using System.Linq;
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
            + @")\Z";

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
        /// <param name="value">specified text</param>
        /// <param name="defaultValue">default value if conversion fails</param>
        /// <returns>An Object that represents the converted text.</returns>
        /// <exception cref="ArgumentNullException">type is null</exception>
        public static T ConvertTo<T>(this string value, T defaultValue)
        {
            return (T)typeof(T).ConvertFromString(value, defaultValue);
        }

        public static bool In(this string source, params string[] values)
        {
            return source.In(StringComparison.Ordinal, values);
        }

        public static bool In(
            this string source,
            StringComparison comparisonType,
            params string[] values)
        {
            return
                source == null
                    ? values.Any(s => s == null)
                    : values.Any(s => source.Equals(s, comparisonType));
        }

        public static string NullIf(this string source, bool ignoreCase, string value)
        {
            return source.NullIfIn(ignoreCase, value);
        }

        public static string NullIfEmpty(this string source)
        {
            return source.NullIf(String.IsNullOrEmpty);
        }

        public static string NullIfIn(this string source, bool ignoreCase, params string[] values)
        {
            return
                source.NullIfIn(
                    ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal,
                    values);
        }

        public static string NullIfWhitespace(this string source)
        {
            return source.NullIf(String.IsNullOrWhiteSpace);
        }

        public static unsafe SecureString Secure(this string source)
        {
            if (source == null)
                return null;

            fixed (char* pChars = source.ToCharArray())
            {
                var secured = new SecureString(pChars, source.Length);
                return secured;
            }
        }

        public static string[] Split(
            this string source,
            string separator,
            StringSplitOptions options = StringSplitOptions.None)
        {
            return
                source == null
                    ? Array.Empty<string>()
                    : source.Split(separator == null ? null : new[] { separator }, options);
        }

        public static string StripNonAlphaCharacters(this string source)
        {
            return source == null ? null : _NonAlphaRegex.Replace(source, String.Empty);
        }

        public static string StripNonAlphanumericCharacters(this string source)
        {
            return source == null ? null : _NonAlphaNumericRegex.Replace(source, String.Empty);
        }

        public static string StripNonAlphanumericOrWhiteSpaceCharacters(this string source)
        {
            return source == null ? null : _NonAlphaNumericWhiteSpaceRegex.Replace(source, String.Empty);
        }

        public static bool? ToBoolean(this string source)
        {
            if (String.IsNullOrEmpty(source))
                return null;

            var parsedTrue =
                "true".Equals(source, StringComparison.OrdinalIgnoreCase)
                || "yes".Equals(source, StringComparison.OrdinalIgnoreCase)
                || "t".Equals(source, StringComparison.OrdinalIgnoreCase)
                || "y".Equals(source, StringComparison.OrdinalIgnoreCase)
                || "1".Equals(source, StringComparison.OrdinalIgnoreCase);
            if (parsedTrue)
                return true;

            var parsedFalse =
                "false".Equals(source, StringComparison.OrdinalIgnoreCase)
                || "no".Equals(source, StringComparison.OrdinalIgnoreCase)
                || "f".Equals(source, StringComparison.OrdinalIgnoreCase)
                || "n".Equals(source, StringComparison.OrdinalIgnoreCase)
                || "0".Equals(source, StringComparison.OrdinalIgnoreCase);
            if (parsedFalse)
                return false;

            return source.ToInt32().ToBoolean();
        }

        public static DateTime? ToDateTime(this string source)
        {
            return ToDateTime(source, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToDateTime(this string source, IFormatProvider provider)
        {
            return ToDateTime(source, provider, DateTimeStyles.None);
        }

        public static DateTime? ToDateTime(this string source, IFormatProvider provider, DateTimeStyles styles)
        {
            if (source == null)
                return null;

            return DateTime.TryParse(source, provider, styles, out var result) ? (DateTime?)result : null;
        }

        public static DateTime? ToDateTime(this string source, string format)
        {
            return ToDateTime(source, format, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToDateTime(this string source, string format, IFormatProvider provider)
        {
            return ToDateTime(source, format, provider, DateTimeStyles.None);
        }

        public static DateTime? ToDateTime(this string source, string format, IFormatProvider provider, DateTimeStyles style)
        {
            return ToDateTime(source, format, provider, style, false);
        }

        public static DateTime? ToDateTime(this string source, string format, IFormatProvider provider, DateTimeStyles style, bool adjustCentury)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));

            return ToDateTime(source, new[] { format }, provider, style, adjustCentury);
        }

        public static DateTime? ToDateTime(this string source, string[] formats, IFormatProvider provider, DateTimeStyles style)
        {
            return ToDateTime(source, formats, provider, style, false);
        }

        public static DateTime? ToDateTime(this string source, string[] formats, IFormatProvider provider, DateTimeStyles style, bool adjustCentury)
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

        public static decimal? ToDecimal(this string source)
        {
            if (source == null)
                return null;

            return Decimal.TryParse(source, out var result) ? (decimal?)result : null;
        }

        public static double? ToDouble(this string source)
        {
            if (source == null)
                return null;

            return Double.TryParse(source, out var result) ? (double?)result : null;
        }

        public static TEnum? ToEnum<TEnum>(this string value, bool ignoreCase = false)
            where TEnum : struct, Enum
        {
            return Enum.TryParse(value, ignoreCase, out TEnum result) ? result : (TEnum?)null;
        }

        public static Guid? ToGuid(this string source)
        {
            if (source == null)
                return null;

            return _ValidGuidRegex.IsMatch(source) ? (Guid?)new Guid(source) : null;
        }

        public static short? ToInt16(this string source)
        {
            if (source == null)
                return null;

            return Int16.TryParse(source, out var result) ? (short?)result : null;
        }

        public static int? ToInt32(this string source)
        {
            if (source == null)
                return null;

            return Int32.TryParse(source, out var result) ? (int?)result : null;
        }

        public static long? ToInt64(this string source)
        {
            if (source == null)
                return null;

            return Int64.TryParse(source, out var result) ? (long?)result : null;
        }

        public static string ToPhrase(this string source)
        {
            return source == null ? null : _ToPhraseRegex.Replace(source, "$1 ").Trim();
        }

        public static float? ToSingle(this string source)
        {
            if (source == null)
                return null;

            return Single.TryParse(source, out var result) ? (float?)result : null;
        }

        public static TimeSpan? ToTimeSpan(this string source)
        {
            if (source == null)
                return null;

            return TimeSpan.TryParse(source, out var result) ? (TimeSpan?)result : null;
        }

        public static string Translate(string source, string from, string to)
        {
            if (String.IsNullOrEmpty(source) || String.IsNullOrEmpty(from))
                return source;

            to = to ?? String.Empty;

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

        private static string NullIfIn(this string source, StringComparison comparison, params string[] values)
        {
            return source?.NullIf(s => s.In(comparison, values));
        }
    }
}