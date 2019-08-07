using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StringExtensionsTests
    {
        private const string _NullString = ((string)null);

        [TestMethod]
        public void ConvertTo()
        {
            var testDate = DateTime.Now;
            var testTime = DateTime.Now - DateTime.Today;

            "true".ConvertTo(false).Should().Be(true);
            "-1".ConvertTo(false).Should().Be(false);
            "1999-12-31".ConvertTo(false).Should().Be(false);
            "23:59:59.999".ConvertTo(false).Should().Be(false);

            "true".ConvertTo(0).Should().Be(0);
            "-1".ConvertTo(0).Should().Be(-1);
            "1999-12-31".ConvertTo(0).Should().Be(0);
            "23:59:59.999".ConvertTo(0).Should().Be(0);

            "true".ConvertTo(testDate).Should().Be(testDate);
            "-1".ConvertTo(testDate).Should().Be(testDate);
            "1999-12-31".ConvertTo(testDate).Should().Be(new DateTime(1999, 12, 31));
            "23:59:59.999".ConvertTo(testDate).Should().Be(DateTime.Parse("23:59:59.999"));

            "true".ConvertTo(testTime).Should().Be(testTime);
            "-1".ConvertTo(testTime).Should().Be(TimeSpan.Parse("-1"));
            "1999-12-31".ConvertTo(testTime).Should().Be(testTime);
            "23:59:59.999".ConvertTo(testTime).Should().Be(TimeSpan.Parse("23:59:59.999"));
        }

        [TestMethod]
        public void In()
        {
            _NullString.In("Lorem", "ipsum", "dolor", "sit", "amet", null).Should().BeTrue();

            _NullString.In("Lorem", "ipsum", "dolor", "sit", "amet").Should().BeFalse();
            "".In("Lorem", "ipsum", "dolor", "sit", "amet").Should().BeFalse();
            "Lorem".In("Lorem", "ipsum", "dolor", "sit", "amet").Should().BeTrue();
            "Ipsum".In("Lorem", "ipsum", "dolor", "sit", "amet").Should().BeFalse();
            "dolor".In("Lorem", "ipsum", "dolor", "sit", "amet").Should().BeTrue();

            _NullString.In(StringComparison.OrdinalIgnoreCase, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeFalse();
            "".In(StringComparison.OrdinalIgnoreCase, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeFalse();
            "Lorem".In(StringComparison.OrdinalIgnoreCase, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeTrue();
            "Ipsum".In(StringComparison.OrdinalIgnoreCase, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeTrue();
            "dolor".In(StringComparison.OrdinalIgnoreCase, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeTrue();
        }

        [TestMethod]
        public void NullIf()
        {
            _NullString.NullIf(true, null).Should().BeNull();
            "".NullIf(true, "").Should().BeNull();
            "".NullIf(true, null).Should().Be("");
            "Test".NullIf(true, "test").Should().BeNull();
            "Test".NullIf(false, "test").Should().Be("Test");
        }

        [TestMethod]
        public void NullIfEmpty()
        {
            _NullString.NullIfEmpty().Should().BeNull();
            "".NullIfEmpty().Should().BeNull();
            " ".NullIfEmpty().Should().Be(" ");
            "Test".NullIfEmpty().Should().Be("Test");
        }

        [TestMethod]
        public void NullIfIn()
        {
            _NullString.NullIfIn(false, "Lorem", "ipsum", "dolor", "sit", "amet", null).Should().BeNull();

            _NullString.NullIfIn(false, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeNull();
            "".NullIfIn(false, "Lorem", "ipsum", "dolor", "sit", "amet").Should().NotBeNull();
            "Lorem".NullIfIn(false, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeNull();
            "Ipsum".NullIfIn(false, "Lorem", "ipsum", "dolor", "sit", "amet").Should().NotBeNull();
            "dolor".NullIfIn(false, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeNull();

            _NullString.NullIfIn(true, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeNull();
            "".NullIfIn(true, "Lorem", "ipsum", "dolor", "sit", "amet").Should().NotBeNull();
            "Lorem".NullIfIn(true, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeNull();
            "Ipsum".NullIfIn(true, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeNull();
            "dolor".NullIfIn(true, "Lorem", "ipsum", "dolor", "sit", "amet").Should().BeNull();
        }

        [TestMethod]
        public void NullIfWhitespace()
        {
            _NullString.NullIfWhitespace().Should().BeNull();
            "".NullIfWhitespace().Should().BeNull();
            " ".NullIfWhitespace().Should().BeNull();
            "Test".NullIfWhitespace().Should().Be("Test");
        }

        [TestMethod]
        public void Split()
        {
            _NullString.Split(" ").Should().BeEquivalentTo(Array.Empty<string>());

            "".Split(" ").Should().BeEquivalentTo("");
            "".Split(" ", StringSplitOptions.RemoveEmptyEntries).Should().BeEquivalentTo(Array.Empty<string>());

            "Lorem ipsum dolor sit amet".Split(" ").Should().BeEquivalentTo("Lorem", "ipsum", "dolor", "sit", "amet");
        }

        [TestMethod]
        public void StripNonAlphaCharacters()
        {
            "Bazinga!".StripNonAlphaCharacters().Should().Be("Bazinga");
            "B a z i n g a !™ 1.2.3.".StripNonAlphaCharacters().Should().Be("Bazinga");
        }

        [TestMethod]
        public void StripNonAlphanumericCharacters()
        {
            "Bazinga!".StripNonAlphanumericCharacters().Should().Be("Bazinga");
            "B a z i n g a !™ 1.2.3.".StripNonAlphanumericCharacters().Should().Be("Bazinga123");

        }

        [TestMethod]
        public void StripNonAlphanumericOrWhiteSpaceCharacters()
        {
            "Bazinga!".StripNonAlphanumericOrWhiteSpaceCharacters().Should().Be("Bazinga");
            "B a z i n g a !™ 1.2.3.".StripNonAlphanumericOrWhiteSpaceCharacters().Should().Be("B a z i n g a  123");
            " \t\r\n 1.2.3.".StripNonAlphanumericOrWhiteSpaceCharacters().Should().Be(" \t\r\n 123");
        }

        [TestMethod]
        public void ToDateTime()
        {

        }



        [DataTestMethod]
        [DataRow(null)]
        [DataRow("2019 © Assurant, Inc.")]
        [DataRow("Can't Touch This")]
        [DataRow("Line\r\nBreak")]
        [DataRow("Non█Breaking Hyphen")]
        [DataRow("Non-Breaking Space")]
        public void ToSecuredString(string test)
        {
            // Act
            var secured = test.ToSecuredString();

            // Assert
            if (test != null)
                secured.Should().NotBeNull();
            else
                secured.Should().BeNull();
        }
    }
}
