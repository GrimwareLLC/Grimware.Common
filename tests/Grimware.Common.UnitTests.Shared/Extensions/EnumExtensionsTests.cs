using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EnumExtensionsTests
    {
        [TestMethod]
        public void ToDescription()
        {
            // Assert
            TestEnumeration.DescriptiveText.ToDescription().Should().Be("Descriptive Text");

            ((TestEnumeration?)TestEnumeration.DescriptiveText).ToDescription().Should().Be("Descriptive Text");
        }

        [TestMethod]
        public void Translate()
        {
            // Arrange
            const TestEnumeration TestValue = TestEnumeration.TranslatableValue;

            // Act
            var result = TestValue.Translate<TestEnumeration, AnotherTestEnumeration>();

            // Assert
            result.Should().Be(AnotherTestEnumeration.TranslatableValue);
        }

        private enum TestEnumeration
        {
            DescriptiveText = 1,
            TranslatableValue = 2,
        }

        private enum AnotherTestEnumeration
        {
            TranslatableValue = 10,
        }
    }
}
