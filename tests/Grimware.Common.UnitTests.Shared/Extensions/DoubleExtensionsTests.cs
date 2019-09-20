using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DoubleExtensionsTests
    {
        [TestMethod]
        public void ToDecimalInvariant()
        {
            (-101.101D).ToDecimalInvariant().Should().Be(-101.101M);
            0D.ToDecimalInvariant().Should().Be(0M);
            101D.ToDecimalInvariant().Should().Be(101M);
            101.101D.ToDecimalInvariant().Should().Be(101.101M);
        }

        [TestMethod]
        public void ToDecimalInvariantNullable()
        {
            ((double?)null).ToDecimalInvariant().Should().BeNull();
            ((double?)-101.101D).ToDecimalInvariant().Should().Be(-101.101M);
            ((double?)0D).ToDecimalInvariant().Should().Be(0M);
            ((double?)101D).ToDecimalInvariant().Should().Be(101M);
            ((double?)101.101D).ToDecimalInvariant().Should().Be(101.101M);
        }
    }
}
