using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Grimware.Extensions
{
    partial class ReflectionExtensions
    {
        public static IEnumerable<T> FindAttributesOfType<T>(this MemberInfo member)
            where T : Attribute
        {
            return
                member == null
                    ? Array.Empty<T>()
                    : member.GetCustomAttributes(typeof(T), true)
                        .Cast<T>();
        }

        public static IEnumerable<Attribute> FindAttributesOfType(this MemberInfo member, Type attributeType)
        {
            return
                member == null || attributeType == null
                    ? Array.Empty<Attribute>()
                    : member.GetCustomAttributes(attributeType, true)
                        .Cast<Attribute>();
        }

        public static T FindSingleAttributeOfType<T>(this MemberInfo member)
            where T : Attribute
        {
            return
                member == null
                    ? default
                    : member.FindSingleAttributeOfType<T>(attributes => attributes.SingleOrDefault());
        }

        public static Attribute FindSingleAttributeOfType(this MemberInfo member, Type attributeType)
        {
            return
                member == null || attributeType == null
                    ? null
                    : member.FindSingleAttributeOfType(attributeType, attributes => attributes.SingleOrDefault());
        }

        public static T FindSingleAttributeOfType<T>(this MemberInfo member, Func<IEnumerable<T>, T> selector)
            where T : Attribute
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return
                member == null
                    ? default
                    : selector(member.FindAttributesOfType<T>());
        }

        public static Attribute FindSingleAttributeOfType(
            this MemberInfo member,
            Type attributeType,
            Func<IEnumerable<Attribute>, Attribute> selector
            )
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return
                member == null || attributeType == null
                    ? null
                    : selector(member.FindAttributesOfType(attributeType));
        }

        public static bool HasAttributeOfType<T>(this MemberInfo member, bool inherit)
            where T : Attribute
        {
            return
                member != null
                    && member.GetCustomAttributes(typeof(T), inherit)
                        .Any();
        }

        public static bool HasAttributeOfType(this MemberInfo member, Type attributeType, bool inherit)
        {
            return
                member != null
                    && attributeType != null
                    && member.GetCustomAttributes(attributeType, inherit)
                        .Any();
        }

        public static bool IsNamed(this MemberInfo member, string name)
        {
            return
                member != null
                    && !String.IsNullOrEmpty(name)
                    && member.Name.Equals(name);
        }
    }
}