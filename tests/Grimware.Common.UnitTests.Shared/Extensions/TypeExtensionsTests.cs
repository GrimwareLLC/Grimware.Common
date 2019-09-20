using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TypeExtensionsTests
        : ReflectionExtensionsTestsBase
    {
        [TestMethod]
        public void FindPropertiesOfType()
        {
            // Arrange

            // Act

            // Assert
            NullType.FindPropertiesOfType<string>().Should().BeEmpty();
            NullType.FindPropertiesOfType(typeof(string)).Should().BeEmpty();
            NullType.FindPropertiesOfType<string>(BindingFlags.NonPublic).Should().BeEmpty();
            NullType.FindPropertiesOfType(typeof(string), BindingFlags.NonPublic).Should().BeEmpty();

            TestType.FindPropertiesOfType<string>().Should().HaveCount(2);
            TestType.FindPropertiesOfType(typeof(string)).Should().HaveCount(2);
            TestType.FindPropertiesOfType<string>(BindingFlags.Instance | BindingFlags.NonPublic).Should().HaveCount(3);
            TestType.FindPropertiesOfType(typeof(string), BindingFlags.Instance | BindingFlags.NonPublic).Should().HaveCount(3);

            TestType.FindPropertiesOfType<Gender>().Should().HaveCount(1);
            TestType.FindPropertiesOfType(typeof(Gender)).Should().HaveCount(1);
            TestType.FindPropertiesOfType<Gender>(BindingFlags.Instance | BindingFlags.NonPublic).Should().HaveCount(0);
            TestType.FindPropertiesOfType(typeof(Gender), BindingFlags.Instance | BindingFlags.NonPublic).Should().HaveCount(0);
        }

        [TestMethod]
        public void FindPropertiesWithAttributeOfType()
        {
            // Arrange

            // Act

            // Assert
            NullType.FindPropertiesWithAttributeOfType<ReflectionTestAttribute>(false).Should().BeEmpty();
            NullType.FindPropertiesWithAttributeOfType(typeof(ReflectionTestAttribute), false).Should().BeEmpty();
            NullType.FindPropertiesWithAttributeOfType<ReflectionTestAttribute>(false, BindingFlags.Instance | BindingFlags.NonPublic)
                    .Should()
                    .BeEmpty();
            NullType.FindPropertiesWithAttributeOfType(typeof(ReflectionTestAttribute), false, BindingFlags.Instance | BindingFlags.NonPublic)
                    .Should()
                    .BeEmpty();

            TestType.FindPropertiesWithAttributeOfType<ReflectionTestAttribute>(false).Should().HaveCount(3);
            TestType.FindPropertiesWithAttributeOfType(typeof(ReflectionTestAttribute), false).Should().HaveCount(3);

            TestType.FindPropertiesWithAttributeOfType<ReflectionTestAttribute>(false, BindingFlags.Instance | BindingFlags.NonPublic)
                    .Should()
                    .HaveCount(3);
            TestType.FindPropertiesWithAttributeOfType(typeof(ReflectionTestAttribute), false, BindingFlags.Instance | BindingFlags.NonPublic)
                    .Should()
                    .HaveCount(3);

        }

        [TestMethod]
        public void IsSubclassOf()
        {
            // Arrange

            // Act

            // Assert
            NullType.IsSubclassOf<object>().Should().BeFalse();

            TestType.IsSubclassOf<object>().Should().BeTrue();

            typeof(ArgumentNullException).IsSubclassOf<ArgumentException>().Should().BeTrue();
            typeof(Exception).IsSubclassOf<ArgumentException>().Should().BeFalse();
        }
    }
}
