using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SingletonTests
    {
        [TestMethod]
        public void Bar_Test()
        {
            Bar bar = null;

            Action act1 = () => bar = new Bar();
            act1.Should().Throw<InvalidOperationException>();

            Action act2 = () => bar = Bar.Instance;
            act2.Should().Throw<TargetInvocationException>().WithInnerException<InvalidOperationException>();

            bar.Should().BeNull();
        }

        [TestMethod]
        public void Baz_Test()
        {
            Baz baz = null;

            Action act1 = () => baz = new Baz(0);
            act1.Should().Throw<InvalidOperationException>();

            Action act2 = () => baz = Baz.Instance;
            act2.Should().Throw<TargetInvocationException>().WithInnerException<InvalidOperationException>();

            baz.Should().BeNull();
        }

        [TestMethod]
        public void Foo_Test()
        {
            var foo1 = Foo.Instance;
            var foo2 = Foo.Instance;

            foo1.Should().NotBeNull();
            foo2.Should().NotBeNull();

            foo1.Should().Be(Foo.Instance);
            foo2.Should().Be(Foo.Instance);

            Foo foo3 = null;
            Action act = () => foo3 = Activator.CreateInstance(typeof(Foo), true) as Foo;
            act.Should().Throw<TargetInvocationException>().WithInnerException<InvalidOperationException>();
            foo3.Should().BeNull();
        }

        [TestMethod]
        public void Fred_Test()
        {
            Fred fred = null;

            Action act = () => fred = Fred.Instance;
            act.Should().Throw<TargetInvocationException>().WithInnerException<InvalidOperationException>();

            fred.Should().BeNull();
        }

        [TestMethod]
        public void Waldo_Test()
        {
            Waldo waldo = null;

            Action act = () => waldo = Waldo.Instance;
            act.Should().Throw<InvalidOperationException>().WithInnerException<MissingMethodException>();

            waldo.Should().BeNull();
        }

        private sealed class Bar
            : Singleton<Bar>
        {
        }

        private sealed class Baz
            : Singleton<Baz>
        {
            // ReSharper disable once UnusedParameter.Local
#pragma warning disable IDE0060 // Remove unused parameter
            public Baz(int i)
                : this()
            {
            }
#pragma warning restore IDE0060 // Remove unused parameter
            private Baz()
            {
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class Foo
            : Singleton<Foo>
        {
            private Foo()
            {
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class Fred
            : Singleton<Fred>
        {
            private Fred()
            {
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        // ReSharper disable once UnusedParameter.Local
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0051 // Remove unused private members
        private sealed class Waldo
            : Singleton<Waldo>
        {
            private Waldo(int i)
            {
            }
        }
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
