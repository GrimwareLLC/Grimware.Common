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

        private static readonly object InitLock = new object();
        private static T _instance;

        // ReSharper restore StaticFieldInGenericType

        public static T Instance => _instance ?? CreateInstance();

        private static T CreateInstance()
        {
            lock (InitLock)
            {
                if (_instance == null)
                {
                    var t = typeof(T);
                    var constructors = t.GetConstructors();

                    if (constructors.Length > 0)
                    {
                        throw new InvalidOperationException(
                            String.Format(ExceptionMessages.SingletonCantBeEnforcedFormat, t.Name));
                    }

                    _instance = Activator.CreateInstance(t, true) as T;
                }

                return _instance;
            }
        }
    }
}