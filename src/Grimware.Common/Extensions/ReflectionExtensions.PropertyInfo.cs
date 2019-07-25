using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Grimware.Extensions
{
    partial class ReflectionExtensions
    {
        public static Attribute GetSingleAttributeOfTypeIfExists(this PropertyInfo property, Type attributeType)
        {
            return property.TryGetSingleAttributeOfType(attributeType, attributes => attributes.SingleOrDefault());
        }

        public static bool HasAttributeOfType(this PropertyInfo property, Type attributeType, bool inherit = false)
        {
            return property.GetCustomAttributes(attributeType, inherit).Any();
        }

        public static bool HasAttributesOfTypes(this PropertyInfo property, IEnumerable<Type> attributeTypes, bool inherit = false)
        {
            var attTypeArray = attributeTypes as Type[] ?? attributeTypes.ToArray();

            return
                attTypeArray
                    .Except(
                        attTypeArray
                            .Where(
                                attType =>
                                    property.GetCustomAttributes(inherit)
                                        .Any(attType.IsInstanceOfType)))
                    .Any();
        }

        public static bool HasName(this PropertyInfo property, string name, bool ignoreCase = false)
        {
            return
                ignoreCase
                    ? property.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                    : property.Name.Equals(name, StringComparison.Ordinal);
        }

        public static Attribute TryGetSingleAttributeOfType(this PropertyInfo property, Type attributeType, Func<IEnumerable<Attribute>, Attribute> selector)
        {
            return selector(property.GetCustomAttributes(attributeType, true).Cast<Attribute>());
        }
    }
}