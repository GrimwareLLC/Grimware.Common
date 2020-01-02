using System;
using System.Linq;
using System.Reflection;

namespace Grimware.Extensions
{
    public static class MemberInfoExtensions
    {
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
