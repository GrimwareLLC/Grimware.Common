using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable StringLiteralTypo
namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StringExtensionsTests
    {
        private const NumberStyles _CurrencyNumberStyles = NumberStyles.AllowCurrencySymbol
            | NumberStyles.AllowDecimalPoint
            | NumberStyles.AllowLeadingSign
            | NumberStyles.AllowThousands;

        private const string _NullString = null;

        private static readonly CultureInfo _CurrentCulture = CultureInfo.CurrentCulture;
        private static readonly CultureInfo _English_Us_Culture = CultureInfo.GetCultureInfo("en-US");
        private static readonly CultureInfo _InvariantCulture = CultureInfo.InvariantCulture;

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

#if NET46
        [TestMethod]
        public void Split()
        {
            _NullString.Split(" ").Should().BeEquivalentTo(Array.Empty<string>());

            "".Split(" ").Should().BeEquivalentTo("");
            "".Split(" ", StringSplitOptions.RemoveEmptyEntries).Should().BeEquivalentTo(Array.Empty<string>());

            "Lorem ipsum dolor sit amet".Split(" ").Should().BeEquivalentTo("Lorem", "ipsum", "dolor", "sit", "amet");
        }
#endif

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

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("test", "Test")]
        [DataRow("the catcher in the rye", "The Catcher In The Rye")]
        [DataRow("rick o'shea", "Rick O'Shea")]
        public void TitleCase(string test, string expected)
        {
            test.TitleCase().Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, 0, 0, 0, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("", 0, 0, 0, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("Test", 0, 0, 0, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("1900-01-01", 1900, 1, 1, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("1999-12-31", 1999, 12, 31, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("2001-09-11", 2001, 9, 11, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("2000-01-01T01:02:03", 2000, 1, 1, 1, 2, 3, DateTimeStyles.None)]
        [DataRow("2000-01-01T13:14:15", 2000, 1, 1, 13, 14, 15, DateTimeStyles.None)]
        [DataRow(" 2000-01-01 T 01:02:03 ", 2000, 1, 1, 1, 2, 3, DateTimeStyles.AllowWhiteSpaces)]
        [DataRow(" 2000-01-01 T 13:14:15 ", 2000, 1, 1, 13, 14, 15, DateTimeStyles.AllowWhiteSpaces)]
        public void ToDateTime(string source, int year, int month, int day, int hour, int minute, int second, DateTimeStyles styles)
        {
            if (year == 0 && month == 0 && day == 0)
                source.ToDateTime(_CurrentCulture).Should().BeNull();
            else if (styles == DateTimeStyles.None)
                source.ToDateTime(_CurrentCulture).Should().Be(_CurrentCulture.Calendar.ToDateTime(year, month, day, hour, minute, second, 0));
            else
                source
                    .ToDateTime(_CurrentCulture, styles)
                    .Should()
                    .Be(_CurrentCulture.Calendar.ToDateTime(year, month, day, hour, minute, second, 0));
        }

        [TestMethod]
        public void ToDateTime_AdjustCentury()
        {
            "45/12/31"
                .ToDateTime("yy/MM/dd", _CurrentCulture, DateTimeStyles.None, true)
                .Should()
                .Be(_CurrentCulture.Calendar.ToDateTime(2045, 12, 31, 0, 0, 0, 0));
        }

        [DataTestMethod]
        [DataRow(null, "", 0, 0, 0, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("", "", 0, 0, 0, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("Test", "", 0, 0, 0, 0, 0, 0, DateTimeStyles.None)]
        [DataRow(" 2000/1/1 01:02:03 ", "yyyy/M/d hh:mm:ss", 0, 0, 0, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("1900/1/1", "yyyy/M/d", 1900, 1, 1, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("1999/12/31", "yyyy/M/d", 1999, 12, 31, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("2001/9/11", "yyyy/M/d", 2001, 9, 11, 0, 0, 0, DateTimeStyles.None)]
        [DataRow("2000/1/1 01:02:03", "yyyy/M/d hh:mm:ss", 2000, 1, 1, 1, 2, 3, DateTimeStyles.None)]
        [DataRow("2000/1/1 13:14:15", "yyyy/M/d HH:mm:ss", 2000, 1, 1, 13, 14, 15, DateTimeStyles.None)]
        [DataRow(" 2000 / 01 / 01 01:02:03 pm ", "yyyy/MM/dd hh:mm:ss tt", 2000, 1, 1, 13, 2, 3, DateTimeStyles.AllowWhiteSpaces)]
        [DataRow(" 2000/1/1 13:14:15 ", "yyyy/M/d HH:mm:ss", 2000, 1, 1, 13, 14, 15, DateTimeStyles.AllowWhiteSpaces)]
        public void ToDateTime_ExactFormat(
            string source,
            string format,
            int year,
            int month,
            int day,
            int hour,
            int minute,
            int second,
            DateTimeStyles styles)
        {
            if (year == 0 && month == 0 && day == 0)
            {
                source.ToDateTime(format, _CurrentCulture).Should().BeNull();
            }
            else if (styles == DateTimeStyles.None)
            {
                source.ToDateTime(format, _CurrentCulture)
                    .Should()
                    .Be(_CurrentCulture.Calendar.ToDateTime(year, month, day, hour, minute, second, 0));
            }
            else
            {
                source
                    .ToDateTime(format, _CurrentCulture, styles)
                    .Should()
                    .Be(_CurrentCulture.Calendar.ToDateTime(year, month, day, hour, minute, second, 0));

                source
                    .ToDateTime(new[] {format}, _CurrentCulture, styles)
                    .Should()
                    .Be(_CurrentCulture.Calendar.ToDateTime(year, month, day, hour, minute, second, 0));
            }
        }

        [TestMethod]
        public void ToDateTime_Exceptions()
        {
            DateTime? result = null;

            // Arrange
            Action act1 = () => result = "".ToDateTime((string) null, _CurrentCulture);
            Action act2 = () => result = "".ToDateTime((string[]) null, _CurrentCulture);

            // Act

            // Assert
            act1.Should().Throw<ArgumentNullException>().Where(ex => "format".Equals(ex.ParamName));
            result.Should().BeNull();

            act2.Should().Throw<ArgumentNullException>().Where(ex => "formats".Equals(ex.ParamName));
            result.Should().BeNull();
        }

        [DataTestMethod]
        [DataRow(null, 0, 0, 0, 0, 0, 0)]
        [DataRow("", 0, 0, 0, 0, 0, 0)]
        [DataRow("Test", 0, 0, 0, 0, 0, 0)]
        [DataRow("1900-01-01", 1900, 1, 1, 0, 0, 0)]
        [DataRow("1999-12-31", 1999, 12, 31, 0, 0, 0)]
        [DataRow("2001-09-11", 2001, 9, 11, 0, 0, 0)]
        [DataRow("2000-01-01T01:02:03", 2000, 1, 1, 1, 2, 3)]
        [DataRow("2000-01-01T13:14:15", 2000, 1, 1, 13, 14, 15)]
        [DataRow(" 2000-01-01 T 01:02:03 ", 2000, 1, 1, 1, 2, 3)]
        [DataRow(" 2000-01-01 T 13:14:15 ", 2000, 1, 1, 13, 14, 15)]
        public void ToDateTimeInvariant(string source, int year, int month, int day, int hour, int minute, int second)
        {
            if (year == 0 && month == 0 && day == 0)
                source.ToDateTimeInvariant().Should().BeNull();
            else
                source.ToDateTimeInvariant().Should().Be(_InvariantCulture.Calendar.ToDateTime(year, month, day, hour, minute, second, 0));
        }

        [DataTestMethod]
        [DataRow(null, "", 0, 0, 0, 0, 0, 0)]
        [DataRow("", "", 0, 0, 0, 0, 0, 0)]
        [DataRow("Test", "", 0, 0, 0, 0, 0, 0)]
        [DataRow(" 2000/1/1 01:02:03 ", "yyyy/M/d hh:mm:ss", 0, 0, 0, 0, 0, 0)]
        [DataRow("1900/1/1", "yyyy/M/d", 1900, 1, 1, 0, 0, 0)]
        [DataRow("1999/12/31", "yyyy/M/d", 1999, 12, 31, 0, 0, 0)]
        [DataRow("2001/9/11", "yyyy/M/d", 2001, 9, 11, 0, 0, 0)]
        [DataRow("2000/1/1 01:02:03", "yyyy/M/d hh:mm:ss", 2000, 1, 1, 1, 2, 3)]
        [DataRow("2000/1/1 13:14:15", "yyyy/M/d HH:mm:ss", 2000, 1, 1, 13, 14, 15)]
        public void ToDateTimeInvariant_ExactFormat(string source, string format, int year, int month, int day, int hour, int minute, int second)
        {
            if (year == 0 && month == 0 && day == 0)
                source.ToDateTimeInvariant(format).Should().BeNull();
            else
                source.ToDateTimeInvariant(format)
                    .Should()
                    .Be(_InvariantCulture.Calendar.ToDateTime(year, month, day, hour, minute, second, 0));
        }

        [TestMethod]
        public void ToDateTimeInvariant_Exceptions()
        {
            DateTime? result = null;

            // Arrange
            Action act1 = () => result = "".ToDateTimeInvariant(null);

            // Act

            // Assert
            act1.Should().Throw<ArgumentNullException>().Where(ex => "format".Equals(ex.ParamName));
            result.Should().BeNull();
        }

        [TestMethod]
        public void ToDecimal()
        {
            ((string) null).ToDecimal().Should().BeNull();
            "".ToDecimal().Should().BeNull();

            "0".ToDecimal().Should().Be(0M);
            "101".ToDecimal().Should().Be(101M);
            "101.101".ToDecimal().Should().Be(101.101M);
            "-101.101".ToDecimal().Should().Be(-101.101M);

            "$1,001.1234".ToDecimal(_CurrencyNumberStyles, _English_Us_Culture).Should().Be(1001.1234M);
            "$-1,001.1234".ToDecimal(_CurrencyNumberStyles, _English_Us_Culture).Should().Be(-1001.1234M);
        }

        [TestMethod]
        public void ToDouble()
        {
            ((string) null).ToDouble().Should().BeNull();
            "".ToDouble().Should().BeNull();

            "0".ToDouble().Should().Be(0D);
            "101".ToDouble().Should().Be(101D);
            "101.101".ToDouble().Should().Be(101.101D);
            "-101.101".ToDouble().Should().Be(-101.101D);

            "$1,001.1234".ToDouble(_CurrencyNumberStyles, _English_Us_Culture).Should().Be(1001.1234D);
            "$-1,001.1234".ToDouble(_CurrencyNumberStyles, _English_Us_Culture).Should().Be(-1001.1234D);
        }

        [TestMethod]
        public void ToEnum()
        {
            ((string) null).ToEnum<Digit>().Should().BeNull();
            "".ToEnum<Digit>().Should().BeNull();
            "Test".ToEnum<Digit>().Should().BeNull();

            "Zero".ToEnum<Digit>().Should().Be(Digit.Zero);
            "Nine".ToEnum<Digit>().Should().Be(Digit.Nine);

            "Public, NonPublic".ToEnum<BindingFlags>().Should().Be(BindingFlags.Public | BindingFlags.NonPublic);
        }

        [TestMethod]
        public void ToGuid()
        {
            // Arrange
            var guid = Guid.NewGuid();

            ((string) null).ToGuid().Should().BeNull();
            "".ToGuid().Should().BeNull();
            "Test".ToGuid().Should().BeNull();

            guid.ToString(null).ToGuid().Should().Be(guid);
            guid.ToString("").ToGuid().Should().Be(guid);
            guid.ToString("N").ToGuid().Should().Be(guid);
            guid.ToString("D").ToGuid().Should().Be(guid);
            guid.ToString("B").ToGuid().Should().Be(guid);
            guid.ToString("P").ToGuid().Should().Be(guid);
            guid.ToString("X").ToGuid().Should().Be(guid);
        }

        [TestMethod]
        public void ToInt16()
        {
            ((string) null).ToInt16().Should().BeNull();
            "".ToInt16().Should().BeNull();
            "Test".ToInt16().Should().BeNull();
            "Test0".ToInt16().Should().BeNull();
            "101.101".ToInt16().Should().BeNull();

            ((string) null).ToInt16(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "".ToInt16(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "Test".ToInt16(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "Test0".ToInt16(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "101.101".ToInt16(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();

            "-1".ToInt16().Should().Be(-1);
            "0".ToInt16().Should().Be(0);
            "1".ToInt16().Should().Be(1);
            "101".ToInt16().Should().Be(101);

            "-1".ToInt16(_CurrencyNumberStyles, _CurrentCulture).Should().Be(-1);
            "0".ToInt16(_CurrencyNumberStyles, _CurrentCulture).Should().Be(0);
            "1".ToInt16(_CurrencyNumberStyles, _CurrentCulture).Should().Be(1);
            "101".ToInt16(_CurrencyNumberStyles, _CurrentCulture).Should().Be(101);
            "$-1,001".ToInt16(_CurrencyNumberStyles, _CurrentCulture).Should().Be(-1001);
        }

        [TestMethod]
        public void ToInt32()
        {
            ((string) null).ToInt32().Should().BeNull();
            "".ToInt32().Should().BeNull();
            "Test".ToInt32().Should().BeNull();
            "Test0".ToInt32().Should().BeNull();
            "101.101".ToInt32().Should().BeNull();

            ((string) null).ToInt32(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "".ToInt32(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "Test".ToInt32(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "Test0".ToInt32(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "101.101".ToInt32(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();

            "-1".ToInt32().Should().Be(-1);
            "0".ToInt32().Should().Be(0);
            "1".ToInt32().Should().Be(1);
            "101".ToInt32().Should().Be(101);

            "-1".ToInt32(_CurrencyNumberStyles, _CurrentCulture).Should().Be(-1);
            "0".ToInt32(_CurrencyNumberStyles, _CurrentCulture).Should().Be(0);
            "1".ToInt32(_CurrencyNumberStyles, _CurrentCulture).Should().Be(1);
            "101".ToInt32(_CurrencyNumberStyles, _CurrentCulture).Should().Be(101);
            "$-1,001".ToInt32(_CurrencyNumberStyles, _CurrentCulture).Should().Be(-1001);
        }

        [TestMethod]
        public void ToInt64()
        {
            ((string) null).ToInt64().Should().BeNull();
            "".ToInt64().Should().BeNull();
            "Test".ToInt64().Should().BeNull();
            "Test0".ToInt64().Should().BeNull();
            "101.101".ToInt64().Should().BeNull();

            ((string) null).ToInt64(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "".ToInt64(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "Test".ToInt64(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "Test0".ToInt64(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "101.101".ToInt64(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();

            "-1".ToInt64().Should().Be(-1);
            "0".ToInt64().Should().Be(0);
            "1".ToInt64().Should().Be(1);
            "101".ToInt64().Should().Be(101);

            "-1".ToInt64(_CurrencyNumberStyles, _CurrentCulture).Should().Be(-1);
            "0".ToInt64(_CurrencyNumberStyles, _CurrentCulture).Should().Be(0);
            "1".ToInt64(_CurrencyNumberStyles, _CurrentCulture).Should().Be(1);
            "101".ToInt64(_CurrencyNumberStyles, _CurrentCulture).Should().Be(101);
            "$-1,001".ToInt64(_CurrencyNumberStyles, _CurrentCulture).Should().Be(-1001);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("Test", "Test")]
        [DataRow("TestString", "Test String")]
        [DataRow("EnumName", "Enum Name")]
        [DataRow("ABCD", "ABCD")]
        public void ToPhrase(string test, string expected)
        {
            test.ToPhrase().Should().Be(expected);
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

        [TestMethod]
        public void ToSingle()
        {
            ((string) null).ToSingle().Should().BeNull();
            "".ToSingle().Should().BeNull();
            "Test".ToSingle().Should().BeNull();
            "Test0".ToSingle().Should().BeNull();

            ((string) null).ToSingle(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "".ToSingle(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "Test".ToSingle(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();
            "Test0".ToSingle(_CurrencyNumberStyles, _CurrentCulture).Should().BeNull();

            "-1".ToSingle().Should().Be(-1);
            "0".ToSingle().Should().Be(0);
            "1".ToSingle().Should().Be(1);
            "101".ToSingle().Should().Be(101);
            "101.101".ToSingle().Should().Be((float) 101.101);

            "-1".ToSingle(_CurrencyNumberStyles, _CurrentCulture).Should().Be(-1);
            "0".ToSingle(_CurrencyNumberStyles, _CurrentCulture).Should().Be(0);
            "1".ToSingle(_CurrencyNumberStyles, _CurrentCulture).Should().Be(1);
            "101".ToSingle(_CurrencyNumberStyles, _CurrentCulture).Should().Be(101);
            "101.101".ToSingle(_CurrencyNumberStyles, _CurrentCulture).Should().Be((float) 101.101);
            "$-1,001".ToSingle(_CurrencyNumberStyles, _CurrentCulture).Should().Be(-1001);
            "$101.1234".ToSingle(_CurrencyNumberStyles, _CurrentCulture).Should().Be((float) 101.1234);
        }

        [TestMethod]
        public void ToTimespan()
        {
            ((string) null).ToTimeSpan().Should().BeNull();
            "".ToTimeSpan().Should().BeNull();
            "Test".ToTimeSpan().Should().BeNull();

            "1.2:03:04.789".ToTimeSpan().Should().Be(new TimeSpan(1, 2, 3, 4, 789));
        }

        [TestMethod]
        public void Translate()
        {
            const string InvalidFilenameCharacters = "<>:\"/\\|?*";

            ((string) null).Translate(null, null).Should().BeNull();
            "".Translate(null, null).Should().Be("");
            " ".Translate("", "").Should().Be(" ");
            " ".Translate(" ", "").Should().Be("");

            "5*[2+6]/{9-3}".Translate("[]{}", "()()").Should().Be("5*(2+6)/(9-3)");
            "[127.8,75.6]".Translate("[,]", "( )").Should().Be("(127.8 75.6)");
            "(127.8 75.6)".Translate("( )", "[,]").Should().Be("[127.8,75.6]");

            "FileName: <ROOT>BLAH<\\ROOT>".Translate(InvalidFilenameCharacters, "_________").Should().Be("FileName_ _ROOT_BLAH__ROOT_");
            "1234567890".Translate("13579", "!#%&(").Should().Be("!2#4%6&8(0");
            "1234567890".Translate("24680", "").Should().Be("13579");
        }

        [TestMethod]
        public void TryConvertTo()
        {
            ((string) null).TryConvertTo<int>(out var int32).Should().BeFalse();
            int32.Should().Be(default);
            "32".TryConvertTo(out int32).Should().BeTrue();
            int32.Should().Be(32);

            "Seven".TryConvertTo<Digit>(out var digit).Should().BeTrue();
            digit.Should().Be(Digit.Seven);

            "true".TryConvertTo<bool>(out var boolean).Should().BeTrue();
            boolean.Should().BeTrue();
            "false".TryConvertTo(out boolean).Should().BeTrue();
            boolean.Should().BeFalse();
        }
    }
}
