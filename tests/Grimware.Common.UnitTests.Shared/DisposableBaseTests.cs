using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Grimware.Common.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DisposableBaseTests
    {
        [TestMethod]
        public void Dispose_Test()
        {
            DisposableFoo foo;
            using (foo = new DisposableFoo())
            {
                foo.Should().NotBeNull();
                foo.IsDisposed.Should().BeFalse();
                foo.Check();
            }

            foo.Should().NotBeNull();
            foo.IsDisposed.Should().BeTrue();
        }

        [TestMethod]
        public void Dispose_Finalizer_Test()
        {
            var foo = new DisposableFoo();
            foo.Should().NotBeNull();
            foo.IsDisposed.Should().BeFalse();
            foo.Check();

            foo = null;
            GC.WaitForPendingFinalizers();
        }

        [TestMethod]
        public void Raise_Test()
        {
            var testState = Int32.MinValue;

            using (var foo = new DisposableFoo())
            {
                foo.Should().NotBeNull();
                foo.Bar += (f, e) => e.State.Should().Be(testState);
                foo.IsDisposed.Should().BeFalse();

                foo.RaiseBar(testState);
            }
        }

        [TestMethod]
        public void Raise_Exception()
        {
            var testState = Int32.MinValue;

            using (var foo = new DisposableFoo())
            {
                foo.Should().NotBeNull();
                foo.Bar += (f, e) => e.State.Should().Be(testState);
                foo.IsDisposed.Should().BeFalse();

                Action throwBar = () => foo.ThrowBar();
                throwBar.Should().Throw<ArgumentNullException>();

                Action throwBaz = () => foo.ThrowBaz();
                throwBaz.Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public void Check_Exception()
        {
            DisposableFoo foo;
            using (foo = new DisposableFoo())
            {
                foo.Should().NotBeNull();
                foo.IsDisposed.Should().BeFalse();
                foo.Check();
            }

            foo.Should().NotBeNull();
            foo.IsDisposed.Should().BeTrue();

            Action act = () => foo.Check();
            act.Should().Throw<ObjectDisposedException>();
        }

        private class DisposableFoo
            : DisposableBase
        {
            public event EventHandler<EventArgs<int>> Bar;
            public event EventHandler Baz;

            public void RaiseBar(int i)
            {
                RaiseEvent(Bar, new EventArgs<int>(i));
            }

            public void ThrowBar()
            {
                RaiseEvent(Bar, null);
            }

            public void ThrowBaz()
            {
                RaiseEvent(Baz, null);
            }

            public void Check()
            {
                CheckDisposed();
            }
        }
    }
}
