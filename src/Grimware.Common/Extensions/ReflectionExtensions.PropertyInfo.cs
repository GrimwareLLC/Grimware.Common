using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    partial class ReflectionExtensions
    {
        public static Attribute GetSingleAttributeOfTypeIfExists(this PropertyInfo property, Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return property?.TryGetSingleAttributeOfType(attributeType, attributes => attributes.SingleOrDefault());
        }

        public static bool HasAttributeOfType(this PropertyInfo property, Type attributeType, bool inherit = false)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return property?.GetCustomAttributes(attributeType, inherit).Any() ?? false;
        }

        public static bool HasAttributesOfTypes(this PropertyInfo property, IEnumerable<Type> attributeTypes, bool inherit = false)
        {
            if (attributeTypes == null) throw new ArgumentNullException(nameof(attributeTypes));

            var attTypeArray = attributeTypes as Type[] ?? attributeTypes.ToArray();

            return
                attTypeArray
                   .Except(attTypeArray.Where(attType => property?.GetCustomAttributes(inherit).Any(attType.IsInstanceOfType) ?? false))
                   .Any();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasName(this PropertyInfo property, string name, bool ignoreCase = false) =>
            ignoreCase
                ? property?.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false
                : property?.Name.Equals(name, StringComparison.Ordinal)           ?? false;

        public static Attribute TryGetSingleAttributeOfType(
            this PropertyInfo                       property,
            Type                                    attributeType,
            Func<IEnumerable<Attribute>, Attribute> selector)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            if (selector      == null) throw new ArgumentNullException(nameof(selector));
            return selector(property?.GetCustomAttributes(attributeType, true).Cast<Attribute>());
        }
    }
}
