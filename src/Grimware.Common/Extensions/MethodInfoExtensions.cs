using System;
using System.Reflection;

namespace Grimware.Extensions
{
    public static class MethodInfoExtensions
    {
        public static bool HasReturnType<T>(this MethodInfo method)
            where T : Attribute
        {
            return method?.ReturnType == typeof(T);
        }

        public static bool HasReturnType(this MethodInfo method, Type returnType)
        {
            if (returnType == null) throw new ArgumentNullException(nameof(returnType));
            return method?.ReturnType == returnType;
        }
    }
}
