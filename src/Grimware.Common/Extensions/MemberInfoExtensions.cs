using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Grimware.Extensions
{
    public static class MemberInfoExtensions
    {
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

        public static T FindSingleAttributeOfType<T>(this MemberInfo member)
            where T : Attribute
        {
            return member != null
                ? member.FindSingleAttributeOfType<T>(attributes => attributes.SingleOrDefault())
                : default;
        }

        public static Attribute FindSingleAttributeOfType(this MemberInfo member, Type attributeType)
        {
            return attributeType != null
                ? member?.FindSingleAttributeOfType(attributeType, attributes => attributes.SingleOrDefault())
                : throw new ArgumentNullException(nameof(attributeType));
        }

        public static T FindSingleAttributeOfType<T>(this MemberInfo member, Func<IEnumerable<T>, T> selector)
            where T : Attribute
        {
            if (selector != null)
                return member == null ? default : selector(member.FindAttributesOfType<T>());

            throw new ArgumentNullException(nameof(selector));
        }

        public static Attribute FindSingleAttributeOfType(
            this MemberInfo member,
            Type attributeType,
            Func<IEnumerable<Attribute>, Attribute> selector)
        {
            if (attributeType != null)
                if (selector != null)
                    return member == null ? null : selector(member.FindAttributesOfType(attributeType));
                else
                    throw new ArgumentNullException(nameof(selector));

            throw new ArgumentNullException(nameof(attributeType));
        }

        public static bool HasAttributeOfType<T>(this MemberInfo member)
            where T : Attribute
        {
            return HasAttributeOfType<T>(member, false);
        }

        public static bool HasAttributeOfType<T>(this MemberInfo member, bool inherit)
            where T : Attribute
        {
            return member?.GetCustomAttributes(typeof(T), inherit).Any() ?? false;
        }

        public static bool HasAttributeOfType(this MemberInfo member, Type attributeType)
        {
            return HasAttributeOfType(member, attributeType, false);
        }

        public static bool HasAttributeOfType(this MemberInfo member, Type attributeType, bool inherit)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return member?.GetCustomAttributes(attributeType, inherit).Any() ?? false;
        }

        public static bool IsNamed(this MemberInfo member, string name)
        {
            return !String.IsNullOrEmpty(name)
                   && (member?.Name.Equals(name, StringComparison.Ordinal) ?? false);
        }
    }
}
