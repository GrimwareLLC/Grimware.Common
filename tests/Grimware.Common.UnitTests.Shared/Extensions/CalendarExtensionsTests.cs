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
    public class CalendarExtensionsTests
    {
        [TestMethod]
        public void GetWeekOfYear()
        {
            // Arrange
            var calendar = new GregorianCalendar(GregorianCalendarTypes.Localized);
            var target = new DateTime(1999, 12, 31, 23, 59, 59, 0, calendar, DateTimeKind.Local);

            // Act & Assert
            calendar.GetWeekOfYear(target).Should().Be(53);
        }

        [TestMethod]
        public void GetWeekOfYear_Exception()
        {
            var week = 0;

            Action act = () => week = ((Calendar) null).GetWeekOfYear(DateTime.Now);

            act.Should().Throw<ArgumentNullException>();
            week.Should().Be(0);
        }
    }
}