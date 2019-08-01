using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EqualityComparerTests
    {
        private readonly Func<int, int, bool> _int32EqualityComparison = (a, b) => a == b;
        private readonly Func<int, int> _int32HashFunction = i => i.GetHashCode();
        private readonly EqualityComparer<int> _int32EqualityComparer = EqualityComparer<int>.Default;

        [TestMethod]
        public void Create_FromLambdas()
        {
            var eq = EqualityComparer.Create(_int32EqualityComparison, _int32HashFunction);

            eq.Should().NotBeNull();
            eq.GetType().Should().Be<AbstractEqualityComparer<int>>();

            eq.Equals(0, 0).Should().BeTrue();
            eq.Equals(Int32.MinValue, Int32.MaxValue).Should().BeFalse();

            eq.GetHashCode(0).Should().Be(0);
            eq.GetHashCode(Int32.MinValue).Should().Be(Int32.MinValue.GetHashCode());
            eq.GetHashCode(Int32.MaxValue).Should().Be(Int32.MaxValue.GetHashCode());
        }

        [TestMethod]
        public void Create_FromLambdas_Exceptions()
        {
            object o = null;

            Action act = () => o = EqualityComparer.Create(null, _int32HashFunction);
            act.Should().Throw<ArgumentNullException>();
            o.Should().BeNull();

            act = () => o = EqualityComparer.Create(_int32EqualityComparison, null);
            act.Should().Throw<ArgumentNullException>();
            o.Should().BeNull();
        }

        [TestMethod]
        public void Create_WithKeySelector()
        {
            var eq = EqualityComparer.Create<int, int>(i => i);

            eq.Should().NotBeNull();
            eq.GetType().Should().Be<AbstractEqualityComparer<int>>();

            eq.Equals(0, 0).Should().BeTrue();
            eq.Equals(Int32.MinValue, Int32.MaxValue).Should().BeFalse();

            eq.GetHashCode(0).Should().Be(0);
            eq.GetHashCode(Int32.MinValue).Should().Be(Int32.MinValue.GetHashCode());
            eq.GetHashCode(Int32.MaxValue).Should().Be(Int32.MaxValue.GetHashCode());
        }

        [TestMethod]
        public void Create_WithKeySelector_Exceptions()
        {
            object o = null;

            Action act = () => o = EqualityComparer.Create<int, int>(null);
            act.Should().Throw<ArgumentNullException>();
            o.Should().BeNull();
        }

        [TestMethod]
        public void Create_KeySelectorWithLambdas()
        {
            var eq = EqualityComparer.Create<int, int>(i => i, _int32EqualityComparison, _int32HashFunction);

            eq.Should().NotBeNull();
            eq.GetType().Should().Be<AbstractEqualityComparer<int>>();

            eq.Equals(0, 0).Should().BeTrue();
            eq.Equals(Int32.MinValue, Int32.MaxValue).Should().BeFalse();

            eq.GetHashCode(0).Should().Be(0);
            eq.GetHashCode(Int32.MinValue).Should().Be(Int32.MinValue.GetHashCode());
            eq.GetHashCode(Int32.MaxValue).Should().Be(Int32.MaxValue.GetHashCode());
        }

        [TestMethod]
        public void Create_KeySelectorWithLambdas_Exceptions()
        {
            object o = null;

            Action act = () => o = EqualityComparer.Create<int, int>(null, _int32EqualityComparison, _int32HashFunction);
            act.Should().Throw<ArgumentNullException>();
            o.Should().BeNull();

            act = () => o = EqualityComparer.Create<int, int>(i => i, null, _int32HashFunction);
            act.Should().Throw<ArgumentNullException>();
            o.Should().BeNull();

            act = () => o = EqualityComparer.Create<int, int>(i => i, _int32EqualityComparison, null);
            act.Should().Throw<ArgumentNullException>();
            o.Should().BeNull();
        }

        [TestMethod]
        public void Create_WithKeyEqualityComparer()
        {
            var eq = EqualityComparer.Create<int, int>(i => i, _int32EqualityComparer);

            eq.Should().NotBeNull();
            eq.GetType().Should().Be<AbstractEqualityComparer<int>>();

            eq.Equals(0, 0).Should().BeTrue();
            eq.Equals(Int32.MinValue, Int32.MaxValue).Should().BeFalse();

            eq.GetHashCode(0).Should().Be(0);
            eq.GetHashCode(Int32.MinValue).Should().Be(Int32.MinValue.GetHashCode());
            eq.GetHashCode(Int32.MaxValue).Should().Be(Int32.MaxValue.GetHashCode());
        }

        [TestMethod]
        public void Create_WithKeyEqualityComparer_Exceptions()
        {
            object o = null;

            Action act = () => o = EqualityComparer.Create<int, int>(null, _int32EqualityComparer);
            act.Should().Throw<ArgumentNullException>();
            o.Should().BeNull();

            act = () => o = EqualityComparer.Create<int, int>(i => i, null);
            act.Should().Throw<ArgumentNullException>();
            o.Should().BeNull();
        }
    }
}
