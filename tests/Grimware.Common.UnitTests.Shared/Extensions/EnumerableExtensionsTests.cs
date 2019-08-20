using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

// ReSharper disable JoinDeclarationAndInitializer
// ReSharper disable RedundantAssignment

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable PossibleMultipleEnumeration
namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EnumerableExtensionsTests
    {
        private static readonly IEnumerable<int> _Int32TestData =
            new[] {Int32.MinValue, -1, 0, 1, Int32.MaxValue}
                .AsEnumerable();

        private static readonly IEnumerable<TestClass> _TestClassesBefore =
            new[]
            {
                new TestClass {Data = 0},
                new TestClass {Data = 1},
                new TestClass {Data = 2},
                new TestClass {Data = 3},
                new TestClass {Data = 4},
                new TestClass {Data = 5},
                new TestClass {Data = 6},
                new TestClass {Data = 7},
                new TestClass {Data = 8},
                new TestClass {Data = 9}
            };

        private static readonly IEnumerable<TestClass> _TestClassesAfter =
            _TestClassesBefore

                // Make sure we're modifying copies, not the originals
                .Select(x => x.Clone())

                // Add some
                .Concat(
                    new[]
                    {
                        new TestClass {Data = 101},
                        new TestClass {Data = 102},
                        new TestClass {Data = 103},
                        new TestClass {Data = 104},
                        new TestClass {Data = 105}
                    })

                // Modify some
                .Select(
                    x =>
                    {
                        if (x.Data % 2 != 0)
                            x.Data += 10;

                        return x;
                    })

                // Remove some
                .Where(x => x.Data % 3 != 0)

                // Enumerate
                .ToArray();


        private static Fixture _Fixture;


        private IEnumerable<int> _testEnumerable;

        [TestInitialize]
        public void Init()
        {
            _Fixture = new Fixture();

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
            indexes.Should().BeEquivalentTo(new[] {0, 1});
        }

        [TestMethod]
        public void Concat()
        {
            // Arrange

            // Act
            ((IEnumerable<int>) null).Concat(101).Should().BeEquivalentTo(new[] {101});

            _Int32TestData.Concat(101).Should().BeEquivalentTo(new[] {Int32.MinValue, -1, 0, 1, Int32.MaxValue, 101});

            // Assert
        }

        [TestMethod]
        public void Convert()
        {
            // Arrange
            IEnumerable<long> results;

            // Act
            results = _Int32TestData.Convert(i => (long) i);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEquivalentTo(_Int32TestData.Select(i => (long) i));
        }

        [TestMethod]
        public void Convert_Exceptions()
        {
            // Arrange
            IEnumerable<long> results = null;

            // Act
            Action act = () => results = _testEnumerable.Convert<int, long>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
            results.Should().BeNull();
        }


        [TestMethod]
        public void Diff()
        {
            // Arrange

            // Act
            var diff = _TestClassesBefore.Diff(_TestClassesAfter, x => x.Key, x => x.Data);

            // Assert
            diff.Should().NotBeNull();

            var added = diff.Added;
            added.Should().NotBeNull();
            added.Count().Should().Be(3);
            added.Select(x => x.Data).Should().BeEquivalentTo(new[] {113, 104, 115});

            var removed = diff.Removed;
            removed.Should().NotBeNull();
            removed.Count().Should().Be(3);
            removed.Select(x => x.Data).Should().BeEquivalentTo(new[] {0, 5, 6});

            var modified = diff.Modified;
            modified.Should().NotBeNull();
            modified.Count().Should().Be(4);
            modified.Select(x => x.Data).Should().BeEquivalentTo(new[] {11, 13, 17, 19});

            var unmodified = diff.Unmodified;
            unmodified.Should().NotBeNull();
            unmodified.Count().Should().Be(3);
            unmodified.Select(x => x.Data).Should().BeEquivalentTo(new[] {2, 4, 8});
        }

        [TestMethod]
        public void Diff_Exceptions()
        {
            // Arrange
            IDiffResult<TestClass> diff = null;

            Action act1 = () => diff = _TestClassesBefore.Diff<TestClass, Guid, int>(_TestClassesAfter, null, x => x.Data);
            Action act2 = () => diff = _TestClassesBefore.Diff<TestClass, Guid, int>(_TestClassesAfter, x => x.Key, null);

            // Act

            // Assert
            act1.Should().Throw<ArgumentNullException>().Where(ex => "keySelector".Equals(ex.ParamName));
            diff.Should().BeNull();

            act2.Should().Throw<ArgumentNullException>().Where(ex => "dataSelector".Equals(ex.ParamName));
            diff.Should().BeNull();
        }

        [TestMethod]
        public void Distinct_ByKey()
        {
            // Arrange
            IEnumerable<int> results;

            // Act
            results = _Int32TestData.Distinct(i => i);

            // Assert
            ((IEnumerable<int>) null).Distinct(i => i).Should().BeNull();

            results.Should().NotBeNull();
            results.Should().NotBeSameAs(_Int32TestData);
            results.Should().BeEquivalentTo(_Int32TestData);
        }

        [TestMethod]
        public void Distinct_BySelectors()
        {
            // Arrange
            IEnumerable<int> results;

            // Act
            results = _Int32TestData.Distinct((i, j) => i == j, i => i.GetHashCode());

            // Assert
            ((IEnumerable<int>) null).Distinct((i, j) => i == j, i => i.GetHashCode()).Should().BeNull();

            results.Should().NotBeNull();
            results.Should().NotBeSameAs(_Int32TestData);
            results.Should().BeEquivalentTo(_Int32TestData);
        }

        [TestMethod]
        public void Distinct_Exceptions()
        {
            // Arrange
            IEnumerable<int> results = null;

            // Act
            Action act1 = () => results = _testEnumerable.Distinct(null, i => i.GetHashCode());
            Action act2 = () => results = _testEnumerable.Distinct((i, j) => i == j, null);
            Action act3 = () => results = _testEnumerable.Distinct((Func<int, int>) null);

            // Assert
            act1.Should().Throw<ArgumentNullException>().Where(ex => "equalityComparison".Equals(ex.ParamName));
            results.Should().BeNull();

            act2.Should().Throw<ArgumentNullException>().Where(ex => "hashGenerator".Equals(ex.ParamName));
            results.Should().BeNull();

            act3.Should().Throw<ArgumentNullException>().Where(ex => "keySelector".Equals(ex.ParamName));
            results.Should().BeNull();
        }

        [TestMethod]
        public void Except()
        {
            // Arrange
            IEnumerable<int> results;

            // Act
            results = _Int32TestData.Except(0);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEquivalentTo(new[] {Int32.MinValue, -1, 1, Int32.MaxValue});
        }

        [TestMethod]
        public void FirstIndexWhere()
        {
            // Arrange

            // Act
            ((IEnumerable<int>) null).FirstIndexWhere(i => i == 0).Should().BeNull();
            _Int32TestData.FirstIndexWhere(i => i == 101).Should().BeNull();

            _Int32TestData.FirstIndexWhere(i => i == Int32.MinValue).Should().Be(0);
            _Int32TestData.FirstIndexWhere(i => i == -1).Should().Be(1);
            _Int32TestData.FirstIndexWhere(i => i == 0).Should().Be(2);
            _Int32TestData.FirstIndexWhere(i => i == 1).Should().Be(3);
            _Int32TestData.FirstIndexWhere(i => i == Int32.MaxValue).Should().Be(4);

            // Assert
        }

        [TestMethod]
        public void ForEach()
        {
            // Arrange
            var list = new List<int>(8);

            // Act
            ((IEnumerable<int>) null).ForEach(i => i++);
            _testEnumerable.ForEach(null);

            _Int32TestData.ForEach(i => list.Add(i));

            // Assert
            list.Should().BeEquivalentTo(_Int32TestData);
        }

        [TestMethod]
        public void Last()
        {
            // Arrange

            // Act

            // Assert
            _Int32TestData.Last(2).Should().BeEquivalentTo(new[] {1, Int32.MaxValue});
            _Int32TestData.Last(i => i <= 0, 2).Should().BeEquivalentTo(new[] {-1, 0});
        }

        [TestMethod]
        public void None()
        {
            // Arrange

            // Act

            // Assert
            ((IEnumerable<int>) null).None().Should().BeTrue();
            ((IEnumerable<int>) null).None().Should().BeTrue();

            _Int32TestData.None().Should().BeFalse();
            _Int32TestData.Where(i => i == 101).None().Should().BeTrue();

            _Int32TestData.None(i => i == 0).Should().BeFalse();
            _Int32TestData.None(i => i == 101).Should().BeTrue();
        }

        [TestMethod]
        public void OrderRandom()
        {
            // Arrange
            var testData = _Fixture.CreateMany<int>(128).ToList();

            // Act
            var random = testData.OrderRandom().ToList();

            // Assert
            random.Should().NotBeNull();
            random.Count.Should().Be(128);
            random.Should().BeEquivalentTo(testData);

            random.Select((t, i) => testData[i] == t ? 0 : 1).Sum().Should().BeGreaterThan(0);

            ((IEnumerable<int>) null).OrderRandom().Should().NotBeNull();
            ((IEnumerable<int>) null).OrderRandom().Should().BeEquivalentTo(Enumerable.Empty<int>());
        }

        [TestMethod]
        public void SelectAdded()
        {
            // Arrange

            // Act
            var added = _TestClassesBefore.SelectAdded(_TestClassesAfter, x => x.Key);

            // Assert
            added.Should().NotBeNull();
            added.Count().Should().Be(3);
            added.Select(x => x.Data).Should().BeEquivalentTo(new[] {113, 104, 115});
        }

        [TestMethod]
        public void SelectedAdded_Exceptions()
        {
            // Arrange
            IEnumerable<TestClass> added = null;

            // Act
            Action act = () => added = _TestClassesBefore.SelectAdded<TestClass, Guid>(_TestClassesAfter, null);

            // Assert
            act.Should().Throw<ArgumentNullException>().Where(ex => "keySelector".Equals(ex.ParamName));
            added.Should().BeNull();
        }

        [TestMethod]
        public void SelectModified()
        {
            // Arrange

            // Act
            var modified = _TestClassesBefore.SelectModified(_TestClassesAfter, x => x.Key, x => x.Data);

            // Assert
            modified.Should().NotBeNull();
            modified.Count().Should().Be(4);
            modified.Select(x => x.Data).Should().BeEquivalentTo(new[] {11, 13, 17, 19});
        }

        [TestMethod]
        public void SelectModified_Exceptions()
        {
            // Arrange
            IEnumerable<TestClass> modified = null;

            // Act
            Action act1 = () => modified = _TestClassesBefore.SelectModified<TestClass, Guid, int>(_TestClassesAfter, null, x => x.Data);
            Action act2 = () => modified = _TestClassesBefore.SelectModified<TestClass, Guid, int>(_TestClassesAfter, x => x.Key, null);

            // Arrange
            act1.Should().Throw<ArgumentNullException>().Where(ex => "keySelector".Equals(ex.ParamName));
            modified.Should().BeNull();

            act2.Should().Throw<ArgumentNullException>().Where(ex => "dataSelector".Equals(ex.ParamName));
            modified.Should().BeNull();
        }

        [TestMethod]
        public void SelectRemoved()
        {
            // Arrange

            // Act
            var removed = _TestClassesBefore.SelectRemoved(_TestClassesAfter, x => x.Key);

            // Assert
            removed.Should().NotBeNull();
            removed.Count().Should().Be(3);
            removed.Select(x => x.Data).Should().BeEquivalentTo(new[] {0, 5, 6});
        }

        [TestMethod]
        public void SelectRemoved_Exceptions()
        {
            // Arrange
            IEnumerable<TestClass> removed = null;

            // Act
            Action act = () => removed = _TestClassesBefore.SelectRemoved<TestClass, Guid>(_TestClassesAfter, null);

            // Assert
            act.Should().Throw<ArgumentNullException>().Where(ex => "keySelector".Equals(ex.ParamName));
            removed.Should().BeNull();
        }

        [TestMethod]
        public void TakeRandom()
        {
            // Arrange
            var testData = _Fixture.CreateMany<int>(128).ToList();

            // Act
            var random = testData.TakeRandom(64).ToList();

            // Assert
            random.Should().NotBeNull();
            random.Count.Should().Be(64);
            random.Join(testData, i => i, i => i, (t, r) => r).ToList().Should().BeEquivalentTo(random);

            random.Select((t, i) => testData[i] == t ? 0 : 1).Sum().Should().BeGreaterThan(0);

            ((IEnumerable<TestClass>) null).TakeRandom().Should().BeNull();
            ((IEnumerable<int>) null).TakeRandom().Should().Be(0);

            ((IEnumerable<TestClass>) null).TakeRandom(1).Should().NotBeNull();
            ((IEnumerable<TestClass>) null).TakeRandom(1).Should().BeEquivalentTo(Enumerable.Empty<TestClass>());
            ((IEnumerable<int>) null).TakeRandom(1).Should().NotBeNull();
            ((IEnumerable<int>) null).TakeRandom(1).Should().BeEquivalentTo(Enumerable.Empty<int>());
        }

        private class TestClass
        {
            public int Data { get; set; }
            public Guid Key { get; private set; } = Guid.NewGuid();

            public TestClass Clone()
            {
                return new TestClass {Key = Key, Data = Data};
            }
        }
    }
}