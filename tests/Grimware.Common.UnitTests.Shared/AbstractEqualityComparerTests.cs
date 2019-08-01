using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AbstractEqualityComparerTests
    {
        private static readonly Func<int, int, bool> _EqualityFunction = (a, b) => a == b;
        private static readonly Func<int, int> _GetHashCodeFunction = i => i.GetHashCode();
        private static readonly AbstractEqualityComparer<int> _IntegerComparer =
            new AbstractEqualityComparer<int>(_EqualityFunction, _GetHashCodeFunction);

        [TestMethod]
        public void Constructor_EqualityComparisonParameter_ArgumentNullException()
        {
            // Arrange
            Func<AbstractEqualityComparer<int>> action = () => new AbstractEqualityComparer<int>(null, _GetHashCodeFunction);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_HashFunctionParameter_ArgumentNullException()
        {
            // Arrange
            Func<AbstractEqualityComparer<int>> act = () => new AbstractEqualityComparer<int>(_EqualityFunction, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Equals_Test()
        {
            _IntegerComparer.Should().NotBeNull();

            _IntegerComparer.Equals(0, 0).Should().BeTrue();
            _IntegerComparer.Equals(0, 1).Should().BeFalse();

            _IntegerComparer.Equals(Int32.MinValue, Int32.MinValue).Should().BeTrue();
            _IntegerComparer.Equals(Int32.MaxValue, Int32.MaxValue).Should().BeTrue();
            _IntegerComparer.Equals(Int32.MinValue, Int32.MaxValue).Should().BeFalse();
        }

        [TestMethod]
        public void Equals_Exception()
        {
            // Arrange
            var comparer = new AbstractEqualityComparer<int>((a, b) => throw new InvalidOperationException(), _GetHashCodeFunction);
            Func<bool> act = () => comparer.Equals(0, 0);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void GetHashCode_Test()
        {
            _IntegerComparer.Should().NotBeNull();

            _IntegerComparer.GetHashCode(0).Should().Be(0);
        }

        [TestMethod]
        public void GetHashCode_Exception()
        {
            // Arrange
            var comparer = new AbstractEqualityComparer<int>(_EqualityFunction, i => throw new InvalidOperationException());
            Func<int> act = () => comparer.GetHashCode(0);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}
