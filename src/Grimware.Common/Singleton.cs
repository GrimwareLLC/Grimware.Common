using System;
using System.Globalization;
using Grimware.Resources;

namespace Grimware
{
    /// <summary>
    ///     Generic Singleton abstraction.
    ///     http://andyclymer.blogspot.com/2008/02/true-generic-singleton.html
    /// </summary>
    /// <typeparam name="T">The type of Singleton being used.</typeparam>
    public abstract class Singleton<T>
        where T : Singleton<T>, new()
    {
        // ReSharper disable StaticFieldInGenericType
#pragma warning disable CA1000 // Do not declare static members on generic types

        private static readonly object _InitLock = new object();
        private static T _Instance;

        public static T Instance => GetInstance();

#pragma warning restore CA1000 // Do not declare static members on generic types
        // ReSharper restore StaticFieldInGenericType

        private static T GetInstance()
        {
            lock (_InitLock)
            {
                // ReSharper disable once ConvertToNullCoalescingCompoundAssignment
                return _Instance ?? (_Instance = CreateInstance());
            }
        }

        private static T CreateInstance()
        {
            lock (_InitLock)
            {
                var t = typeof(T);
                var constructors = t.GetConstructors();

                if (constructors.Length > 0)
                {
                    throw new InvalidOperationException(
                        String.Format(CultureInfo.InvariantCulture, ExceptionMessages.SingletonCantBeEnforcedFormat, t.Name));
                }

                return Activator.CreateInstance(t, true) as T;
            }
        }
    }
}