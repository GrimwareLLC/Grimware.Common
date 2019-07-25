using System;
using System.Globalization;
using Grimware.Resources;

namespace Grimware.Extensions
{
    public static class EnumExtensions
    {
        #region Constants & Pseudo-Constants

        private static readonly CultureInfo _CI = CultureInfo.CurrentCulture;

        #endregion

        public static TEnumOut? Translate<TEnumIn, TEnumOut>(this TEnumIn source, bool ignoreCase = false)
            where TEnumIn : struct, IComparable, IFormattable, IConvertible
            where TEnumOut : struct, IComparable, IFormattable, IConvertible
        {
            ThrowIfNotEnum<TEnumIn>();
            ThrowIfNotEnum<TEnumOut>();

            return source.ToString(CultureInfo.InvariantCulture).ToEnum<TEnumOut>(ignoreCase);
        }

        public static TEnum Add<TEnum>(this Enum source, TEnum value)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            ThrowIfNotEnum<TEnum>();

            if (source == null)
                return value;

            if (typeof(TEnum).IsEnum && typeof(TEnum) == source.GetType())
            {
                // Boxing, ugh.  But for this to work it's our only choice.
                return (TEnum)(object)((long)(object)source | (long)(object)value);
            }

            throw new InvalidOperationException(
                String.Format(_CI, ExceptionMessages.EnumValueNotAddedFormat, source.GetType().Name));
        }

        public static bool Has<TEnum>(this Enum source, TEnum value)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            return
                source != null
                    && typeof(TEnum).IsEnum
                    && typeof(TEnum) == source.GetType()
                    // Boxing, ugh.  But for this to work it's our only choice.
                    && ((long)(object)source & (long)(object)value) == (long)(object)value;
        }

        public static bool Is<TEnum>(this Enum source, TEnum value)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            return
                source != null
                    && typeof(TEnum).IsEnum
                    && typeof(TEnum) == source.GetType()
                    // Boxing, ugh.  But for this to work it's our only choice.
                    && (long)(object)source == (long)(object)value;
        }

        public static TEnum Remove<TEnum>(this Enum source, TEnum value)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            ThrowIfNotEnum<TEnum>();

            if (source == null)
                return default;

            if (typeof(TEnum).IsEnum && typeof(TEnum) == source.GetType())
            {
                // Boxing, ugh.  But for this to work it's our only choice.
                return (TEnum)(object)(((long)(object)source & ~(long)(object)value));
            }

            throw new InvalidOperationException(
                String.Format(_CI, ExceptionMessages.EnumValueNotRemovedFormat, source.GetType().Name));
        }

        public static string ToDescription(this Enum source)
        {
            return source?.ToString().ToPhrase();
        }

        private static void ThrowIfNotEnum<TEnum>()
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new InvalidOperationException(
                    String.Format(_CI, ExceptionMessages.NotAnEnumTypeFormat, typeof(TEnum)));
            }
        }
    }
}