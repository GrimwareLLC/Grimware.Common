using System;
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

        private static readonly object _InitLock = new object();
        private static T _Instance;

        // ReSharper restore StaticFieldInGenericType

        public static T Instance => GetInstance();

        private static T GetInstance()
        {
            lock (_InitLock)
            {
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
                        String.Format(ExceptionMessages.SingletonCantBeEnforcedFormat, t.Name));
                }

                return Activator.CreateInstance(t, true) as T;
            }
        }
    }
}