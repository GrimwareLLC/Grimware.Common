using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Grimware.Extensions
{
    partial class ReflectionExtensions
    {
        public static object ConvertFromString(this Type type, string value, object defaultValue)
        {
            if (!TryConvertFromString(type, value, out var retVal))
                retVal = defaultValue;

            return retVal;
        }

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

        public static IEnumerable<PropertyInfo> FindPropertiesOfType(
            this Type type,
            Type propertyType,
            BindingFlags bindingAttributes
        )
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

        public static IEnumerable<PropertyInfo> FindPropertiesWithAttributeOfType(
            this Type type,
            Type attributeType,
            bool inherit
        )
        {
            if (type == null || attributeType == null)
                return Array.Empty<PropertyInfo>();

            return type.GetProperties()
                .Where(property => property.HasAttributeOfType(attributeType, inherit));
        }

        public static IEnumerable<PropertyInfo> FindPropertiesWithAttributeOfType<T>(
            this Type type,
            bool inherit,
            BindingFlags bindingAttributes
        )
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

        public static bool TryConvertFromString(this Type type, string value, out object returnValue)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var successful = true;
            object convertedValue = null;

            try
            {
                if (value == null)
                {
                    if (type.IsValueType)
                        successful = false;
                }
                else
                {
                    convertedValue = type.IsInstanceOfType(value) ? value : TryConvertFromStringInternal(type, value);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            {
                successful = false;
            }
#pragma warning restore CA1031 // Do not catch general exception types

            returnValue = successful ? convertedValue : null;
            return successful;
        }

        private static object TryConvertFromStringInternal(Type type, string value)
        {
            var typeConverter = TypeDescriptor.GetConverter(type);
            if (typeConverter.CanConvertFrom(typeof(string)))
                return typeConverter.ConvertFromString(value);

            if (type.IsEnum) return Enum.Parse(type, value, true);

            return null;
        }
    }
}