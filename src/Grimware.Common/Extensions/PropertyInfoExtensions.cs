using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Grimware.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static Attribute GetSingleAttributeOfTypeIfExists<T>(this PropertyInfo property)
        {
            var attributeType = typeof(T);

            return property?.TryGetSingleAttributeOfType(attributeType, attributes => attributes.SingleOrDefault());
        }

        public static Attribute GetSingleAttributeOfTypeIfExists(this PropertyInfo property, Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return property?.TryGetSingleAttributeOfType(attributeType, attributes => attributes.SingleOrDefault());
        }

        public static bool HasAttributeOfType(this PropertyInfo property, Type attributeType)
        {
            return HasAttributeOfType(property, attributeType, false);
        }

        public static bool HasAttributeOfType(this PropertyInfo property, Type attributeType, bool inherit)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return property?.GetCustomAttributes(attributeType, inherit).Any() ?? false;
        }

        public static bool HasAttributesOfTypes(this PropertyInfo property, IEnumerable<Type> attributeTypes)
        {
            return HasAttributesOfTypes(property, attributeTypes, false);
        }

        public static bool HasAttributesOfTypes(this PropertyInfo property, IEnumerable<Type> attributeTypes, bool inherit)
        {
            if (attributeTypes == null) throw new ArgumentNullException(nameof(attributeTypes));

            var attTypeArray = attributeTypes as Type[] ?? attributeTypes.ToArray();

            return
                attTypeArray
                    .Except(attTypeArray.Where(attType => property?.GetCustomAttributes(inherit).Any(attType.IsInstanceOfType) ?? false))
                    .Any();
        }

        public static bool HasName(this PropertyInfo property, string name)
        {
            return HasName(property, name, false);
        }

        public static bool HasName(this PropertyInfo property, string name, bool ignoreCase)
        {
            return ignoreCase
                ? property?.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false
                : property?.Name.Equals(name, StringComparison.Ordinal) ?? false;
        }

        public static Attribute TryGetSingleAttributeOfType(
            this PropertyInfo property,
            Type attributeType,
            Func<IEnumerable<Attribute>, Attribute> selector)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return selector(property?.GetCustomAttributes(attributeType, true).Cast<Attribute>());
        }
    }
}
