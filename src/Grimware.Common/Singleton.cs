using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Grimware.Resources;

namespace Grimware
{
    /// <summary>
    ///     Generic Singleton abstraction.
    ///     http://andyclymer.blogspot.com/2008/02/true-generic-singleton.html
    /// </summary>
    /// <typeparam name="T">The type of Singleton being used.</typeparam>
    public abstract class Singleton<T>
        where T : Singleton<T>
    {
        private static readonly Lazy<T> _InstanceLoader = new Lazy<T>(CreateInstance);

        protected Singleton()
        {
            var t = typeof(T);

            var publicConstructors = t.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (publicConstructors.Length > 0)
                throw new InvalidOperationException(
                    String.Format(CultureInfo.CurrentCulture, ExceptionMessages.SingletonCantBeEnforcedFormat, t.Name));

            if (!t.IsSealed)
                throw new InvalidOperationException(
                    String.Format(CultureInfo.CurrentCulture, ExceptionMessages.SingletonMustBeSealedFormat, t.Name));


            if (_InstanceLoader.IsValueCreated)
                throw new InvalidOperationException(ExceptionMessages.SingletonMustBeAccessedThroughInstanceProperty);
        }

#pragma warning disable CA1000 // Do not declare static members on generic types
        public static T Instance => _InstanceLoader.Value;
#pragma warning restore CA1000 // Do not declare static members on generic types

        private static T CreateInstance()
        {
            try
            {
                return Activator.CreateInstance(typeof(T), true) as T;
            }
            catch (MissingMethodException ex)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.CurrentCulture, ExceptionMessages.SingletonMustHavePrivateDefaultConstructorFormat, typeof(T).Name),
                    ex);
            }
        }
    }
}
