using System;
using System.Reflection;

namespace Grimware.Extensions
{
    partial class ReflectionExtensions
    {
        public static bool HasReturnType<T>(this MethodInfo method)
            where T : Attribute
        {
            if (method == null)
                return false;

            return method.ReturnType == typeof(T);
        }

        public static bool HasReturnType(this MethodInfo method, Type returnType)
        {
            if (method == null)
                return false;

            return method.ReturnType == returnType;
        }
    }
}