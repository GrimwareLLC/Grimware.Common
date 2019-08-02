using System;
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
    public class LazyListTests
    {
        private static readonly IEnumerable<int> _Int32TestData =
            new[] { Int32.MinValue, -1, 0, 1, Int32.MaxValue }
               .AsEnumerable();

        [TestMethod]
        public void Constructor()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var lazy = new LazyList<int>();
            lazy.Should().NotBeNull();
            lazy.Count.Should().Be(0);
            lazy.IsReadOnly.Should().BeFalse();
        }

        [TestMethod]
        public void Constructor_ThreadSafetyMode()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var lazy = new LazyList<int>(LazyThreadSafetyMode.ExecutionAndPublication);
            lazy.Should().NotBeNull();
            lazy.Count.Should().Be(0);
            lazy.IsReadOnly.Should().BeFalse();
        }

        [TestMethod]
        public void Constructor_FromEnumerable()
        {
            var lazy = new LazyList<int>(_Int32TestData);
            lazy.Should().NotBeNull();
            lazy.Count.Should().Be(5);
            lazy.IsReadOnly.Should().BeFalse();
        }

        [TestMethod]
        public void Constructor_FromFactory()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var lazy = new LazyList<int>(() => _Int32TestData.ToList());
            lazy.Should().NotBeNull();
            lazy.Count.Should().Be(5);
            lazy.IsReadOnly.Should().BeFalse();
        }

        [TestMethod]
        public void Constructor_Exceptions()
        {
            // Arrange
            LazyList<int> lazy = null;

            // Act
            Action act1 = () => lazy = new LazyList<int>((IEnumerable<int>)null);
            Action act2 = () => lazy = new LazyList<int>((Func<IList<int>>)null);

            // Assert
            act1.Should().Throw<ArgumentNullException>();
            lazy.Should().BeNull();

            act2.Should().Throw<ArgumentNullException>();
            lazy.Should().BeNull();
        }

        [TestMethod]
        public void Indexer()
        {
            // Arrange
            var lazy = new LazyList<int>(_Int32TestData);
            lazy.Should().NotBeNull();

            // Act & Assert
            lazy[0].Should().Be(Int32.MinValue);
            lazy[1].Should().Be(-1);
            lazy[2].Should().Be(0);
            lazy[3].Should().Be(1);
            lazy[4].Should().Be(Int32.MaxValue);


            lazy[2] = 101;
            lazy.Should().NotBeEquivalentTo(_Int32TestData);

            lazy[2] = 0;
            lazy.Should().BeEquivalentTo(_Int32TestData);
        }

        [TestMethod]
        public void IndexOf()
        {
            // Arrange
            var lazy = new LazyList<int>(_Int32TestData);
            lazy.Should().NotBeNull();

            // Act & Assert
            lazy.IndexOf(Int32.MinValue).Should().Be(0);
            lazy.IndexOf(-1).Should().Be(1);
            lazy.IndexOf(0).Should().Be(2);
            lazy.IndexOf(1).Should().Be(3);
            lazy.IndexOf(Int32.MaxValue).Should().Be(4);
        }

        [TestMethod]
        public void Insert_RemoveAt()
        {
            // Arrange
            var lazy = new LazyList<int>(_Int32TestData);
            lazy.Should().NotBeNull();
            lazy.Count.Should().Be(5);
            lazy.IsReadOnly.Should().BeFalse();

            lazy.RemoveAt(lazy.IndexOf(0));
            lazy.Count.Should().Be(4);
            lazy.IndexOf(0).Should().Be(-1);

            lazy.Insert(2, 0);
            lazy.Count.Should().Be(5);
            lazy.Should().BeEquivalentTo(_Int32TestData);
        }
    }
}
