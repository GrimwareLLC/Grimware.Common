using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DisposableAdapterTests
    {
        [TestMethod]
        public void Using()
        {
            var v = 0;
            DisposableAdapter adapter;
            using (adapter = new DisposableAdapter(() => v = Int32.MaxValue))
            {
                adapter.Should().NotBeNull();
                adapter.IsDisposed.Should().BeFalse();
                v.Should().Be(0);
            }

            adapter.Should().NotBeNull();
            adapter.IsDisposed.Should().BeTrue();
            v.Should().Be(Int32.MaxValue);
        }
    }
}