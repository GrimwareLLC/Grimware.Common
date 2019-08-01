using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    partial class ReflectionExtensions
    {
        public static Attribute GetFirstAttributeOfType(this FieldInfo field, Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return field?.TryGetSingleAttributeOfType(attributeType, attributes => attributes.First());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TAttribute GetFirstAttributeOfType<TAttribute>(this FieldInfo field)
            where TAttribute : Attribute =>
            (TAttribute)field?.GetFirstAttributeOfType(typeof(TAttribute));

        public static Attribute GetFirstAttributeOfTypeIfExists(this FieldInfo field, Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return field?.TryGetSingleAttributeOfType(attributeType, attributes => attributes.FirstOrDefault());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TAttribute GetFirstAttributeOfTypeIfExists<TAttribute>(this FieldInfo field)
            where TAttribute : Attribute =>
            (TAttribute)field?.GetFirstAttributeOfTypeIfExists(typeof(TAttribute));

        public static Attribute GetSingleAttributeOfType(this FieldInfo field, Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return field?.TryGetSingleAttributeOfType(attributeType, attributes => attributes.Single());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TAttribute GetSingleAttributeOfType<TAttribute>(this FieldInfo field)
            where TAttribute : Attribute =>
            (TAttribute)field?.GetSingleAttributeOfType(typeof(TAttribute));

        public static Attribute GetSingleAttributeOfTypeIfExists(this FieldInfo field, Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return field?.TryGetSingleAttributeOfType(attributeType, attributes => attributes.SingleOrDefault());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TAttribute GetSingleAttributeOfTypeIfExists<TAttribute>(this FieldInfo field)
            where TAttribute : Attribute =>
            (TAttribute)field?.GetSingleAttributeOfTypeIfExists(typeof(TAttribute));

        public static bool HasDeclaredAttributeOfType(this FieldInfo field, Type attributeType, bool inherit = false)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return field?.GetCustomAttributes(attributeType, inherit).Any() ?? false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasName(this FieldInfo field, string name, bool ignoreCase) =>
            ignoreCase
                ? field?.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false
                : field?.Name.Equals(name, StringComparison.Ordinal)           ?? false;

        public static Attribute TryGetSingleAttributeOfType(
            this FieldInfo                          field,
            Type                                    attributeType,
            Func<IEnumerable<Attribute>, Attribute> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return selector(field?.GetCustomAttributes(attributeType, false).Cast<Attribute>());
        }
    }
}
