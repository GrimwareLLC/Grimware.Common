using System;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable UnusedMember.Local

namespace Grimware.Common.UnitTests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ReflectionExtensionsTestsBase
    {
        protected static readonly Type NullType = null;
        protected static readonly Type TestType = typeof(ReflectionTestType);

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
        public sealed class ReflectionTestAttribute
            : Attribute
        {
            public string State { get; set; }
        }

        [ReflectionTest(State = "class")]
        public class ReflectionTestType
        {
            [ReflectionTest(State = "Given Name")]
            public string FirstName { get;set; }

            [ReflectionTest(State = "Surname")]
            public string LastName { get; set; }

            [ReflectionTest(State = "yyyy-MM-dd")]
            public DateTime BirthDate { get; set; }

            public Gender Gender { get; set; }

#pragma warning disable IDE0051 // Remove unused private members
            [ReflectionTest(State = "private")]
            private string PrivateString { get; set; }

            [ReflectionTest(State = "private")]
            private string PrivateString2 { get; set; }

            [ReflectionTest(State = "internal")]
            internal string InternalString { get; set; }
#pragma warning restore IDE0051 // Remove unused private members
        }

        [ReflectionTest(State = "enum")]
        public enum Gender
        {
            Unknown = 0,

            Female,
            Male,
        }
    }
}
