using System;
using Grimware.Extensions;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class Int32ExtensionsTests
    {
        [DataTestMethod]
        [DataRow(Int32.MinValue, true)]
        [DataRow(-1, true)]
        [DataRow(0, false)]
        [DataRow(1, true)]
        [DataRow(Int32.MaxValue, true)]
        public void ToBoolean(int test, bool expected)
        {
            test.ToBoolean().Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow(Int32.MinValue, true)]
        [DataRow(-1, true)]
        [DataRow(0, false)]
        [DataRow(1, true)]
        [DataRow(Int32.MaxValue, true)]
        public void ToBoolean_Nullable(int? test, bool? expected)
        {
            if (expected == null)
                test.ToBoolean().Should().BeNull();
            else
                test.ToBoolean().Should().Be(expected);
        }
    }
}
