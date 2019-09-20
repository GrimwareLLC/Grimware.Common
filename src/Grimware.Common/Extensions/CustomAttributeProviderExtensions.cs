using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Grimware.Extensions
{
    public static class CustomAttributeProviderExtensions
    {
        public static IEnumerable<T> FindAttributesOfType<T>(this ICustomAttributeProvider provider)
            where T : Attribute
        {
            return provider != null
                ? provider.GetCustomAttributes(typeof(T), true)
                          .Cast<T>()
                : Array.Empty<T>();
        }

        public static IEnumerable<Attribute> FindAttributesOfType(this ICustomAttributeProvider provider, Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return
                provider != null
                    ? provider.GetCustomAttributes(attributeType, true)
                              .Cast<Attribute>()
                    : Array.Empty<Attribute>();
        }
    }
}
