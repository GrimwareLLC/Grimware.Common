using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ExceptionExtensionsTests
    {
        [TestMethod]
        public void ToStringVerbose()
        {
            // Arrange
            AppDomain.CurrentDomain.SetData(@".appId", $"{GetType().FullName}");
            string result = null;

            // Act
            try
            {
                ThrowInnerException();
            }
            catch (Exception ix)
            {
                try
                {
                    throw new TestException("Outer Exception", ix);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    result = ex.ToStringVerbose();
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }

            // Assert
            result.Should().NotBeNull();
            result.Should().StartWith($"{GetType().FullName}\r\n");
            result.Should().Contain($"{nameof(ExceptionExtensionsTests)}.{nameof(ThrowInnerException)}");
            result.Should().Contain($"{nameof(ExceptionExtensionsTests)}.{nameof(ToStringVerbose)}");
        }

        private static void ThrowInnerException()
        {
            throw new TestException("Inner Exception");
        }
    }
}
