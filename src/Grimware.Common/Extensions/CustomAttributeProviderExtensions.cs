using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Grimware.Extensions
{
    public static class CustomAttributeProviderExtensions
    {
        public static IEnumerable<T> FindAttributesOfType<T>(this ICustomAttributeProvider attributeProvider)
            where T : Attribute
        {
            return attributeProvider != null
                ? attributeProvider.GetCustomAttributes(typeof(T), true).Cast<T>()
                : Array.Empty<T>();
        }

        public static IEnumerable<Attribute> FindAttributesOfType(this ICustomAttributeProvider attributeProvider, Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));

            return
                attributeProvider != null
                    ? attributeProvider.GetCustomAttributes(attributeType, true).Cast<Attribute>()
                    : Array.Empty<Attribute>();
        }

        public static T FindSingleAttributeOfType<T>(this ICustomAttributeProvider attributeProvider)
            where T : Attribute
        {
            return attributeProvider != null
                ? attributeProvider.FindSingleAttributeOfType<T>(attributes => attributes.SingleOrDefault())
                : default;
        }

        public static Attribute FindSingleAttributeOfType(this ICustomAttributeProvider attributeProvider, Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));

            return attributeProvider?.FindSingleAttributeOfType(attributeType, attributes => attributes.SingleOrDefault());
        }

        public static T FindSingleAttributeOfType<T>(this ICustomAttributeProvider attributeProvider, Func<IEnumerable<T>, T> selector)
            where T : Attribute
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return attributeProvider != null ? selector(attributeProvider.FindAttributesOfType<T>()) : default;
        }

        public static Attribute FindSingleAttributeOfType(
            this ICustomAttributeProvider attributeProvider,
            Type attributeType,
            Func<IEnumerable<Attribute>, Attribute> selector)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return attributeProvider != null ? selector(attributeProvider.FindAttributesOfType(attributeType)) : null;
        }
    }
}
