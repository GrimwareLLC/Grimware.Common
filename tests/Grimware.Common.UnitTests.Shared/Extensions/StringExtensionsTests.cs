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
