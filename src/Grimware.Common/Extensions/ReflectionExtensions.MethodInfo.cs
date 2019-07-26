using System;
using System.Reflection;

namespace Grimware.Extensions
{
    partial class ReflectionExtensions
    {
        public static bool HasReturnType<T>(this MethodInfo method)
            where T : Attribute =>
            method?.ReturnType == typeof(T);

        public static bool HasReturnType(this MethodInfo method, Type returnType)
        {
            if (returnType == null) throw new ArgumentNullException(nameof(returnType));
            return method?.ReturnType == returnType;
        }
    }
}