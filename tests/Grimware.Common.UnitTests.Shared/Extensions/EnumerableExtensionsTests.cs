using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable PossibleMultipleEnumeration
namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EnumerableExtensionsTests
    {
        private static readonly IEnumerable<int> _Int32TestData =
            new[] { Int32.MinValue, -1, 0, 1, Int32.MaxValue }
               .AsEnumerable();

        private IEnumerable<int> _testEnumerable;

        [TestInitialize]
        public void Init()
        {
            _testEnumerable = Substitute.For<IEnumerable<int>>();
        }

        [TestMethod]
        public void AllIndexesWhere()
        {
            // Arrange

            // Act
            var indexes = _Int32TestData.AllIndexesWhere(i => i < 0);

            // Assert
            indexes.Should().NotBeNull();
            indexes.Count().Should().Be(2);
            indexes.Should().BeEquivalentTo(new[] { 0, 1 });
        }

        [TestMethod]
        public void Concat()
        {
            // Arrange

            // Act
            ((IEnumerable<int>)null).Concat(101).Should().BeEquivalentTo(new[] { 101 });

            _Int32TestData.Concat(101).Should().BeEquivalentTo(new[] { Int32.MinValue, -1, 0, 1, Int32.MaxValue, 101 });

            // Assert
        }
    }
}
