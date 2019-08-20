using System;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static class EnumExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToDescription<TEnum>(this TEnum source)
            where TEnum : struct, Enum
        {
            return source.ToString().ToPhrase();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToDescription<TEnum>(this TEnum? source)
            where TEnum : struct, Enum
        {
            return source != null ? ToDescription(source.Value) : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnumOut? Translate<TEnumIn, TEnumOut>(this TEnumIn source, bool ignoreCase = false)
            where TEnumIn : struct, Enum
            where TEnumOut : struct, Enum
        {
            return source.ToString().ToEnum<TEnumOut>(ignoreCase);
        }
    }
}