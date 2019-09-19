using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EventArgsTests
    {
        [TestMethod]
        public void State_Test()
        {
            var e = new EventArgs<int>(Int32.MaxValue);

            e.Should().NotBeNull();
            e.State.Should().Be(Int32.MaxValue);
        }
    }
}
