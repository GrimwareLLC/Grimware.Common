using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HasherTests
    {
        private readonly IEnumerable<string> _testStrings = new[] { "This", " ", "is", " ", "a", " ", "test", "." };
        private readonly IEnumerable<byte> _testBytes = new byte[] { 0, 25, 66, 99, 101, 123 };

        private readonly IEnumerable<int> _testIntegers =
            new[]
            {
                -2147483648, -64384, -70, 0, 25, 66, 99, 101, 123, 321, 789,
                987, 123987, 2147483647
            };

        private readonly IEnumerable<long> _testLongs =
            new[]
            {
                -9223372036854775808, -2147483649, -2147483648, -64384, -70,
                0, 25, 66, 99, 101, 123, 321, 789, 987, 123987, 2147483647,
                2147483648, 9223372036854775807
            };

        [TestMethod]
        public void Hash_Bytes()
        {
            // Act
            Hasher.Hash((byte[])null).Should().Be(0);

            Hasher.Hash(_testBytes as byte[]).Should().Be(1966513476);
        }

        [TestMethod]
        public void Hash_Integers()
        {
            // Act
            Hasher.Hash((IEnumerable<int>)null).Should().Be(0);
            Hasher.Hash((int[])null).Should().Be(0);

            Hasher.Hash(_testIntegers).Should().Be(-1059381649);
            Hasher.Hash(_testIntegers as int[]).Should().Be(-1059381649);
            Hasher.Hash(-2147483648, -64384, -70, 0, 25, 66, 99, 101, 123, 321, 789, 987, 123987, 2147483647).Should().Be(-1059381649);

            Hasher.Hash(_testIntegers.Reverse()).Should().Be(1696247953);
        }

        [TestMethod]
        public void Hash_Longs()
        {
            // Act
            Hasher.Hash((IEnumerable<long>)null).Should().Be(0);
            Hasher.Hash((long[])null).Should().Be(0);

            Hasher.Hash(_testLongs).Should().Be(-1141000696);
            Hasher.Hash(_testLongs as long[]).Should().Be(-1141000696);
            Hasher.Hash(
                    -9223372036854775808, -2147483649, -2147483648, -64384, -70, 0,
                    25, 66, 99, 101, 123, 321, 789, 987, 123987, 2147483647,
                    2147483648, 9223372036854775807)
                .Should()
                .Be(-1141000696);

            Hasher.Hash(_testLongs.Reverse()).Should().Be(1503195488);
        }

        [TestMethod]
        public void Hash_Strings()
        {
            Hasher.Hash((IEnumerable<string>)null).Should().Be(0);
            Hasher.Hash((string[])null).Should().Be(0);

            Hasher.Hash((string)null).Should().Be(191);
            Hasher.Hash(String.Empty).Should().Be(191);

            Hasher.Hash(null, null).Should().Be(72962);
            Hasher.Hash(String.Empty, String.Empty).Should().Be(72962);

            Hasher.Hash("This is a test.").Should().Be(2044637889);

            Hasher.Hash(_testStrings).Should().Be(1344175977);
            Hasher.Hash(_testStrings as string[]).Should().Be(1344175977);

            Hasher.Hash(_testStrings.Reverse()).Should().Be(-918506397);

            Hasher.Hash("This", " ", "a", " ", "test", " ", "is", ".").Should().Be(-102517415);
        }
    }
}
