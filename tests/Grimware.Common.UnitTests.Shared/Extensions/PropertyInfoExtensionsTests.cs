using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentAssertions.Common;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PropertyInfoExtensionsTests
        : ReflectionExtensionsTestsBase
    {
        [TestMethod]
        public void GetSingleAttributeOfTypeIfExists()
        {
            // Arrange

            // Act

            // Assert
        }

        [TestMethod]
        public void GetSingleAttributeOfTypeIfExists_Exceptions()
        {
            // Arrange
            object result = null;

            // Act
            Action act = () => TestType.FindProperty("LastName", typeof(string)).GetSingleAttributeOfTypeIfExists(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().Where(ex => "attributeType".Equals(ex.ParamName));
        }

        // Arrange

        // Act

        // Assert
    }
}
