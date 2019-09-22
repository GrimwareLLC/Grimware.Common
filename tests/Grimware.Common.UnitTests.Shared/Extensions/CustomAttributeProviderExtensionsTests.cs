using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        [TestMethod]
        public void FindAttributesOfType()
        {
            // Arrange

            // Act

            // Assert
            TestType.FindAttributesOfType<SerializableAttribute>().Should().HaveCount(0);
            TestType.FindAttributesOfType(typeof(SerializableAttribute)).Should().HaveCount(0);

            TestType.FindAttributesOfType<ReflectionTestAttribute>().Should().HaveCount(1);
            TestType.FindAttributesOfType(typeof(ReflectionTestAttribute)).Should().HaveCount(1);

            var attribute = TestType.FindAttributesOfType<ReflectionTestAttribute>().Single();
            attribute.Should().NotBeNull();
            attribute.State.Should().Be("class");
        }

        [TestMethod]
        public void FindAttributesOfType_Exceptions()
        {
            // Arrange
            object result = null;
            
            // Act
            Action act = () => result = TestType.FindAttributesOfType(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().Where(ex => "attributeType".Equals(ex.ParamName));
            result.Should().BeNull();
        }
    }
}
