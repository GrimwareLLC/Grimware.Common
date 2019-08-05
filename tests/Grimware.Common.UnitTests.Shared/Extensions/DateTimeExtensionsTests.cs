using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DateTimeExtensionsTests
    {
        private static Calendar TestCalendar { get; } = new JulianCalendar();

        [TestMethod]
        public void Add()
        {
            var dateTime = DateTime.Now;

            dateTime.Add(null).Should().Be(dateTime);
            dateTime.Add(TimeSpan.Zero).Should().Be(dateTime);
        }

        [TestMethod]
        public void Add_Nullable()
        {
            var dateTime = DateTime.Now;

            ((DateTime?)null).Add(null).Should().BeNull();
            ((DateTime?)null).Add(TimeSpan.Zero).Should().BeNull();
            ((DateTime?)dateTime).Add(null).Should().Be(dateTime);
        }

        [DataTestMethod]
        [DataRow(1900, 1, 1, 1)]
        [DataRow(1999, 12, 31, 5)]
        [DataRow(2001, 9, 11, 2)]
        public void GetDayOfWeekOccurence(int year, int month, int day, int result)
        {
            new DateTime(year, month, day).GetDayOfWeekOccurence().Should().Be(result);
        }

        [DataTestMethod]
        [DataRow(1900, 1, 1, 1)]
        [DataRow(1999, 12, 31, 5)]
        [DataRow(2001, 9, 11, 2)]
        public void GetDayOfWeekOccurence_AlternateCalendar(int year, int month, int day, int result)
        {
            new DateTime(year, month, day).GetDayOfWeekOccurence(TestCalendar).Should().Be(result);
        }

        [DataTestMethod]
        [DataRow(1900, 1, 1, 0)]
        [DataRow(1999, 12, 31, 4)]
        [DataRow(2001, 9, 11, 2)]
        public void GetWeekOfMonth(int year, int month, int day, int result)
        {
            new DateTime(year, month, day).GetWeekOfMonth().Should().Be(result);
        }

        [DataTestMethod]
        [DataRow(1900, 1, 1, 0)]
        [DataRow(1999, 12, 31, 4)]
        [DataRow(2001, 9, 11, 2)]
        public void GetWeekOfMonth_AlternateCalendar(int year, int month, int day, int result)
        {
            new DateTime(year, month, day).GetWeekOfMonth(TestCalendar).Should().Be(result);
        }

        [DataTestMethod]
        [DataRow(1900, 1, 1, 1)]
        [DataRow(1999, 12, 31, 53)]
        [DataRow(2001, 9, 11, 37)]
        public void GetWeekOfYear(int year, int month, int day, int result)
        {
            new DateTime(year, month, day).GetWeekOfYear().Should().Be(result);
        }

        [DataTestMethod]
        [DataRow(1900, 1, 1, 52)]
        [DataRow(1999, 12, 31, 51)]
        [DataRow(2001, 9, 11, 35)]
        public void GetWeekOfYear_AlternateCalendar(int year, int month, int day, int result)
        {
            new DateTime(year, month, day).GetWeekOfYear(TestCalendar).Should().Be(result);
        }
    }
}
