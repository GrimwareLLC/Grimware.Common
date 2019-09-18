using System.Diagnostics.CodeAnalysis;
using System.Security;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SecureStringExtensionsTests
    {
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("2019 © Assurant, Inc.")]
        [DataRow("Can't Touch This")]
        [DataRow("Line\r\nBreak")]
        [DataRow("Non█Breaking Hyphen")]
        [DataRow("Non-Breaking Space")]
        public void ToUnsecuredString(string test)
        {
            // Arrange
            SecureString secured = null;

            if (test != null)
                unsafe
                {
                    fixed (char* pChars = test.ToCharArray())
                    {
                        secured = new SecureString(pChars, test.Length);
                    }
                }

            // Act
            var result = secured.ToUnsecuredString();

            // Assert
            if (test != null)
            {
                secured.Should().NotBeNull();
                result.Should().NotBeNull();
            }

            result.Should().Be(test);
        }
    }
}
