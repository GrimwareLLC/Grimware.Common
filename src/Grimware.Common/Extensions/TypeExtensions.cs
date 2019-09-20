using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Grimware.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> FindPropertiesOfType<T>(this Type type)
        {
            if (type == null)
                return Array.Empty<PropertyInfo>();

            return type.GetProperties()
                       .Where(property => typeof(T).IsAssignableFrom(property.PropertyType));
        }

        public static IEnumerable<PropertyInfo> FindPropertiesOfType(this Type type, Type propertyType)
        {
            if (type == null || propertyType == null)
                return Array.Empty<PropertyInfo>();

            return type.GetProperties()
                       .Where(property => propertyType.IsAssignableFrom(property.PropertyType));
        }

        public static IEnumerable<PropertyInfo> FindPropertiesOfType<T>(this Type type, BindingFlags bindingAttributes)
        {
            if (type == null)
                return Array.Empty<PropertyInfo>();

            return
                type.GetProperties(bindingAttributes)
                    .Where(property => typeof(T).IsAssignableFrom(property.PropertyType));
        }

        public static IEnumerable<PropertyInfo> FindPropertiesOfType(this Type type, Type propertyType, BindingFlags bindingAttributes)
        {
            if (type == null || propertyType == null)
                return Array.Empty<PropertyInfo>();

            return type.GetProperties(bindingAttributes)
                       .Where(property => propertyType.IsAssignableFrom(property.PropertyType));
        }

        public static IEnumerable<PropertyInfo> FindPropertiesWithAttributeOfType<T>(this Type type, bool inherit)
            where T : Attribute
        {
            if (type == null)
                return Array.Empty<PropertyInfo>();

            return type.GetProperties()
                       .Where(property => property.HasAttributeOfType<T>(inherit));
        }

        public static IEnumerable<PropertyInfo> FindPropertiesWithAttributeOfType(this Type type, Type attributeType, bool inherit)
        {
            if (type == null || attributeType == null)
                return Array.Empty<PropertyInfo>();

            return type.GetProperties()
                       .Where(property => property.HasAttributeOfType(attributeType, inherit));
        }

        public static IEnumerable<PropertyInfo> FindPropertiesWithAttributeOfType<T>(this Type type, bool inherit, BindingFlags bindingAttributes)
            where T : Attribute
        {
            if (type == null)
                return Array.Empty<PropertyInfo>();

            return type.GetProperties(bindingAttributes)
                       .Where(property => property.HasAttributeOfType<T>(inherit));
        }

        public static IEnumerable<PropertyInfo> FindPropertiesWithAttributeOfType(
            this Type type,
            Type attributeType,
            bool inherit,
            BindingFlags bindingAttributes)
        {
            return type != null && attributeType != null
                ? type.GetProperties(bindingAttributes)
                      .Where(property => property.HasAttributeOfType(attributeType, inherit))
                : Array.Empty<PropertyInfo>();
        }

        public static bool IsSubclassOf<T>(this Type type)
        {
            return type != null && type.IsSubclassOf(typeof(T));
        }
    }
}
