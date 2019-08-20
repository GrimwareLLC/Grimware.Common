using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CancelEventArgsTests
    {
        [TestMethod]
        public void State_Canceled_Test()
        {
            var e = new CancelEventArgs<int>(Int32.MaxValue, true);

            e.Should().NotBeNull();
            e.Cancel.Should().BeTrue();
            e.State.Should().Be(Int32.MaxValue);
        }

        [TestMethod]
        public void State_Test()
        {
            var e = new CancelEventArgs<int>(Int32.MaxValue);

            e.Should().NotBeNull();
            e.Cancel.Should().BeFalse();
            e.State.Should().Be(Int32.MaxValue);
        }
    }
}