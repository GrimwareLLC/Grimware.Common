using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Grimware.Resources;

namespace Grimware.Extensions
{
    public static class EnumExtensions
    {
        private static readonly CultureInfo _CultureInfo = CultureInfo.CurrentCulture;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum Add<TEnum>(this TEnum source, TEnum value)
            where TEnum : struct, Enum =>
            typeof(TEnum).HasAttributeOfType<FlagsAttribute>()
                ? (TEnum)(object)((long)(object)source | (long)(object)value)
                : throw new InvalidOperationException(
                    String.Format(_CultureInfo, ExceptionMessages.EnumValueNotAddedFormat, source.GetType().Name));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<TEnum>(this TEnum source, TEnum value)
            where TEnum : struct, Enum => // Boxing, ugh.  But for this to work it's our only choice.
            typeof(TEnum).HasAttributeOfType<FlagsAttribute>() && ((long)(object)source & (long)(object)value) == (long)(object)value;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is<TEnum>(this TEnum source, TEnum value)
            where TEnum : struct, Enum => // Boxing, ugh.  But for this to work it's our only choice.
            typeof(TEnum).HasAttributeOfType<FlagsAttribute>() && (long)(object)source == (long)(object)value;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum Remove<TEnum>(this TEnum source, TEnum value)
            where TEnum : struct, Enum =>
            typeof(TEnum).HasAttributeOfType<FlagsAttribute>()
                ? (TEnum)(object)(((long)(object)source & ~(long)(object)value))
                : throw new InvalidOperationException(
                    String.Format(_CultureInfo, ExceptionMessages.EnumValueNotRemovedFormat, source.GetType().Name));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToDescription<TEnum>(this TEnum? source)
            where TEnum : struct, Enum =>
            source?.ToString().ToPhrase();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnumOut? Translate<TEnumIn, TEnumOut>(this TEnumIn source, bool ignoreCase = false)
            where TEnumIn : struct, Enum
            where TEnumOut : struct, Enum =>
            source.ToString().ToEnum<TEnumOut>(ignoreCase);
    }
}