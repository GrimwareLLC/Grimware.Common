using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Grimware.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Collections
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LazyCollectionTests
    {
        private static readonly IEnumerable<int> _Int32TestData =
            new[] {Int32.MinValue, -1, 0, 1, Int32.MaxValue}
                .AsEnumerable();

        [TestMethod]
        public void Add_Remove_Clear()
        {
            // Arrange
            var lazy = new LazyCollection<int>(_Int32TestData);
            lazy.Should().NotBeNull();
            lazy.Count.Should().Be(5);
            lazy.IsReadOnly.Should().BeFalse();

            lazy.Remove(Int32.MinValue);
            lazy.Count.Should().Be(4);

            lazy.Add(Int32.MinValue);
            lazy.Add(101);
            lazy.Count.Should().Be(6);

            lazy.Clear();
            lazy.Count.Should().Be(0);
        }

        [TestMethod]
        public void Constructor()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var lazy = new LazyCollection<int>();
            lazy.Should().NotBeNull();
            lazy.Count.Should().Be(0);
            lazy.IsReadOnly.Should().BeFalse();
        }

        [TestMethod]
        public void Constructor_Exceptions()
        {
            // Arrange
            LazyCollection<int> lazy = null;

            // Act
            Action act1 = () => lazy = new LazyCollection<int>((IEnumerable<int>) null);
            Action act2 = () => lazy = new LazyCollection<int>((Func<ICollection<int>>) null);

            // Assert
            act1.Should().Throw<ArgumentNullException>();
            lazy.Should().BeNull();

            act2.Should().Throw<ArgumentNullException>();
            lazy.Should().BeNull();
        }

        [TestMethod]
        public void Constructor_FromEnumerable()
        {
            var lazy = new LazyCollection<int>(_Int32TestData);
            lazy.Should().NotBeNull();
            lazy.Count.Should().Be(5);
            lazy.IsReadOnly.Should().BeFalse();
        }

        [TestMethod]
        public void Constructor_FromFactory()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var lazy = new LazyCollection<int>(() => _Int32TestData.ToList());
            lazy.Should().NotBeNull();
            lazy.Count.Should().Be(5);
            lazy.IsReadOnly.Should().BeFalse();
        }

        [TestMethod]
        public void Constructor_ThreadSafetyMode()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var lazy = new LazyCollection<int>(LazyThreadSafetyMode.ExecutionAndPublication);
            lazy.Should().NotBeNull();
            lazy.Count.Should().Be(0);
            lazy.IsReadOnly.Should().BeFalse();
        }

        [TestMethod]
        public void Contains()
        {
            // Arrange
            var lazy = new LazyCollection<int>(_Int32TestData);
            lazy.Should().NotBeNull();

            // Assert
            lazy.Contains(0).Should().BeTrue();
            lazy.Contains(101).Should().BeFalse();
        }

        [TestMethod]
        public void CopyTo()
        {
            // Arrange
            var buffer = new int[5];

            var lazy = new LazyCollection<int>(_Int32TestData);
            lazy.Should().NotBeNull();

            // Act
            lazy.CopyTo(buffer, 0);

            buffer.Should().BeEquivalentTo(_Int32TestData);
        }

        [TestMethod]
        public void Enumerate()
        {
            // Arrange
            var list = new List<object>(8);

            var lazy = new LazyCollection<int>(_Int32TestData);
            lazy.Should().NotBeNull();

            foreach (var i in (IEnumerable) lazy)
                list.Add(i);

            list.OfType<int>().Count().Should().Be(5);
            list.OfType<int>().Should().BeEquivalentTo(_Int32TestData);
        }
    }
}