using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    partial class ReflectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> FindAttributesOfType<T>(this MemberInfo member)
            where T : Attribute
        {
            return member != null
                ? member.GetCustomAttributes(typeof(T), true)
                        .Cast<T>()
                : Array.Empty<T>();
        }

        public static IEnumerable<Attribute> FindAttributesOfType(this MemberInfo member, Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return
                member != null
                    ? member.GetCustomAttributes(attributeType, true)
                            .Cast<Attribute>()
                    : Array.Empty<Attribute>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindSingleAttributeOfType<T>(this MemberInfo member)
            where T : Attribute
        {
            return member != null
                ? member.FindSingleAttributeOfType<T>(attributes => attributes.SingleOrDefault())
                : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Attribute FindSingleAttributeOfType(this MemberInfo member, Type attributeType)
        {
            return attributeType != null
                ? member?.FindSingleAttributeOfType(attributeType, attributes => attributes.SingleOrDefault())
                : throw new ArgumentNullException(nameof(attributeType));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindSingleAttributeOfType<T>(this MemberInfo member, Func<IEnumerable<T>, T> selector)
            where T : Attribute
        {
            return selector != null
                ? member == null
                    ? default
                    : selector(member.FindAttributesOfType<T>())
                : throw new ArgumentNullException(nameof(selector));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Attribute FindSingleAttributeOfType(
            this MemberInfo member,
            Type attributeType,
            Func<IEnumerable<Attribute>, Attribute> selector)
        {
            return attributeType != null
                ? selector != null
                    ? member == null
                        ? null
                        : selector(member.FindAttributesOfType(attributeType))
                    : throw new ArgumentNullException(nameof(selector))
                : throw new ArgumentNullException(nameof(attributeType));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAttributeOfType<T>(this MemberInfo member, bool inherit = false)
            where T : Attribute
        {
            return member?.GetCustomAttributes(typeof(T), inherit).Any() ?? false;
        }

        public static bool HasAttributeOfType(this MemberInfo member, Type attributeType, bool inherit = false)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return member?.GetCustomAttributes(attributeType, inherit).Any() ?? false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNamed(this MemberInfo member, string name)
        {
            return !String.IsNullOrEmpty(name)
                && (member?.Name.Equals(name, StringComparison.Ordinal) ?? false);
        }
    }
}
