using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CollectionsExtensionsTests
    {
        private static readonly IEnumerable<int> _Int32TestData =
            new[] {Int32.MinValue, -1, 0, 1, Int32.MaxValue}
                .AsEnumerable();

        private ICollection<int> _testCollection;

        [TestInitialize]
        public void Init()
        {
            _testCollection = Substitute.For<ICollection<int>>();
        }

        [TestMethod]
        public void AddIf()
        {
            // Arrange

            // Act
            _testCollection.AddIf(101, true);
            _testCollection.AddIf(-101, false);

            _testCollection.AddIf(1001, i => i > 0);
            _testCollection.AddIf(-1001, i => i > 0);

            // Assert
            _testCollection.Received(1).Add(101);
            _testCollection.Received(1).Add(1001);
        }

        [TestMethod]
        public void AddIf_Exceptions()
        {
            // Arrange

            // Act
            Action act = () => _testCollection.AddIf(0, null);

            // Assert
            act.Should().Throw<ArgumentNullException>().Where(ex => "predicate".Equals(ex.ParamName));
        }

        [TestMethod]
        public void AddIfNotExists()
        {
            // Arrange
            _testCollection.Contains(Arg.Any<int>()).Returns(c => (int) c[0] > 0);

            // Act
            foreach (var i in _Int32TestData)
                _testCollection.AddIfNotExists(i);

            // Assert
            _testCollection.Received(5).Contains(Arg.Any<int>());
            _testCollection.Received(3).Add(Arg.Any<int>());
        }

        [TestMethod]
        public void AddRange()
        {
            // Arrange

            // Act
            ((ICollection<int>) null).AddRange(_Int32TestData);
            _testCollection.AddRange(null);

            _testCollection.AddRange(_Int32TestData);

            // Assert
            _testCollection.Received(5).Add(Arg.Any<int>());
        }

        [TestMethod]
        public void AddRangeIfNotExists()
        {
            // Arrange
            _testCollection.Contains(Arg.Any<int>()).Returns(c => (int) c[0] > 0);

            // Act
            ((ICollection<int>) null).AddRangeIfNotExists(_Int32TestData);
            _testCollection.AddRangeIfNotExists(null);

            _testCollection.AddRangeIfNotExists(_Int32TestData);

            // Assert
            _testCollection.Received(5).Contains(Arg.Any<int>());
            _testCollection.Received(3).Add(Arg.Any<int>());
        }

        [TestMethod]
        public void RemoveIf()
        {
            // Arrange

            // Act
            _testCollection.RemoveIf(101, false);
            _testCollection.RemoveIf(-101, true);

            _testCollection.RemoveIf(1001, i => i < 0);
            _testCollection.RemoveIf(-1001, i => i < 0);

            // Assert
            _testCollection.Received(1).Remove(-101);
            _testCollection.Received(1).Remove(-1001);
        }

        [TestMethod]
        public void RemoveIf_Exceptions()
        {
            // Arrange
            Action act = () => _testCollection.RemoveIf(0, null);

            // Act & Assert
            act.Should().Throw<ArgumentNullException>().Where(ex => "predicate".Equals(ex.ParamName));
        }

        [TestMethod]
        public void RemoveIfExists()
        {
            // Arrange
            _testCollection.Contains(Arg.Any<int>()).Returns(c => (int) c[0] < 0);

            // Act
            foreach (var i in _Int32TestData)
                _testCollection.RemoveIfExists(i);

            // Assert
            _testCollection.Received(5).Contains(Arg.Any<int>());
            _testCollection.Received(2).Remove(Arg.Any<int>());
        }

        [TestMethod]
        public void RemoveRange()
        {
            // Arrange

            // Act
            ((ICollection<int>) null).RemoveRange(_Int32TestData);
            _testCollection.RemoveRange(null);

            _testCollection.RemoveRange(_Int32TestData);

            // Assert
            _testCollection.Received(5).Remove(Arg.Any<int>());
        }

        [TestMethod]
        public void RemoveRangeIfExists()
        {
            // Arrange
            _testCollection.Contains(Arg.Any<int>()).Returns(c => (int) c[0] > 0);

            // Act
            ((ICollection<int>) null).RemoveRangeIfExists(_Int32TestData);
            _testCollection.RemoveRangeIfExists(null);

            _testCollection.RemoveRangeIfExists(_Int32TestData);

            // Assert
            _testCollection.Received(5).Contains(Arg.Any<int>());
            _testCollection.Received(2).Remove(Arg.Any<int>());
        }
    }
}