using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CustomAttributeProviderExtensionsTests
        : ReflectionExtensionsTestsBase
    {
        private static ICustomAttributeProvider CustomAttributeProvider => TestType;

        [TestMethod]
        public void FindAttributesOfType()
        {
            // Arrange

            // Act

            // Assert
            CustomAttributeProvider.FindAttributesOfType<SerializableAttribute>().Should().HaveCount(0);
            CustomAttributeProvider.FindAttributesOfType(typeof(SerializableAttribute)).Should().HaveCount(0);

            CustomAttributeProvider.FindAttributesOfType<ReflectionTestAttribute>().Should().HaveCount(1);
            CustomAttributeProvider.FindAttributesOfType(typeof(ReflectionTestAttribute)).Should().HaveCount(1);

            var attribute = CustomAttributeProvider.FindAttributesOfType<ReflectionTestAttribute>().Single();
            attribute.Should().NotBeNull();
            attribute.State.Should().Be("class");
        }

        [TestMethod]
        public void FindAttributesOfType_Exceptions()
        {
            // Arrange
            object result = null;

            // Act
            Action act = () => result = CustomAttributeProvider.FindAttributesOfType(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().Where(ex => "attributeType".Equals(ex.ParamName));
            result.Should().BeNull();
        }
    }
}
