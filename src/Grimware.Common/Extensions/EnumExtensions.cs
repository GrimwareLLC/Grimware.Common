using System;

namespace Grimware.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescription<TEnum>(this TEnum source)
            where TEnum : struct, Enum
        {
            return source.ToString().ToPhrase();
        }

        public static string ToDescription<TEnum>(this TEnum? source)
            where TEnum : struct, Enum
        {
            return source != null ? ToDescription(source.Value) : null;
        }

        public static TEnumOut? Translate<TEnumIn, TEnumOut>(this TEnumIn source)
            where TEnumIn : struct, Enum
            where TEnumOut : struct, Enum
        {
            return Translate<TEnumIn, TEnumOut>(source, false);
        }

        public static TEnumOut? Translate<TEnumIn, TEnumOut>(this TEnumIn source, bool ignoreCase)
            where TEnumIn : struct, Enum
            where TEnumOut : struct, Enum
        {
            return source.ToString().ToEnum<TEnumOut>(ignoreCase);
        }
    }
}
