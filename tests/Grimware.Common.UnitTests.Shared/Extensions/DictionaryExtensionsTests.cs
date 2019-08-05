using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DictionaryExtensionsTests
    {
        private static readonly IDictionary<string, string> _TestDictionary =
            new Dictionary<string, string>
            {
                { "0", "Zero" },
                { "1", "One" },
                { "2", "Two" },
                { "3", "Three" },
                { "4", "Four" },
                { "5", "Five" },
                { "6", "Six" },
                { "7", "Seven" },
                { "8", "Eight" },
                { "9", "Nine" },
            };

        private readonly Lazy<IDictionary<string, string>> _lazyReadOnly =
            new Lazy<IDictionary<string, string>>(() => _TestDictionary.AsReadOnly());

        private IDictionary<string, string> ReadOnlyTarget => _lazyReadOnly.Value;

        [TestMethod]
        public void AsReadOnly()
        {
            // Act
            var result = _TestDictionary.AsReadOnly();

            // Assert
            result.Should().NotBeNull();
            result.GetType().DeclaringType.Should().Be(typeof(DictionaryExtensions));
            result.GetType().Name.Should().Be("ReadOnlyDictionary`2");
        }

        [TestMethod]
        public void AsReadOnly_Null()
        {
            // Act
            var result = ((IDictionary<string, string>)null).AsReadOnly();

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void ReadOnlyDictionary_Count()
        {
            // Assert
            ReadOnlyTarget.Count.Should().Be(10);
        }

        [TestMethod]
        public void ReadOnlyDictionary_IsReadOnly()
        {
            ReadOnlyTarget.IsReadOnly.Should().BeTrue();
        }

        [TestMethod]
        public void ReadOnlyDictionary_Keys()
        {
            // Arrange
            var keys = ReadOnlyTarget.Keys;

            // Assert
            keys.Should().NotBeNull();
            keys.Count.Should().Be(10);
            keys.Should().BeEquivalentTo(_TestDictionary.Keys);
        }

        [TestMethod]
        public void ReadOnlyDictionary_Values()
        {
            // Arrange
            var values = ReadOnlyTarget.Values;

            // Assert
            values.Should().NotBeNull();
            values.Count.Should().Be(10);
            values.Should().BeEquivalentTo(_TestDictionary.Values);
        }

        [DataTestMethod]
        [DataRow("0", "Zero", true)]
        [DataRow("1", "One", true)]
        [DataRow("2", "Two", true)]
        [DataRow("3", "Three", true)]
        [DataRow("4", "Four", true)]
        [DataRow("5", "Five", true)]
        [DataRow("6", "Six", true)]
        [DataRow("7", "Seven", true)]
        [DataRow("8", "Eight", true)]
        [DataRow("9", "Nine", true)]
        [DataRow("0", "Zilch", false)]
        [DataRow("1", "Uno", false)]
        public void ReadOnlyDictionary_Contains(string key, string value, bool expected)
        {
            ReadOnlyTarget.Contains(new KeyValuePair<string, string>(key, value)).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("0", true)]
        [DataRow("1", true)]
        [DataRow("2", true)]
        [DataRow("3", true)]
        [DataRow("4", true)]
        [DataRow("5", true)]
        [DataRow("6", true)]
        [DataRow("7", true)]
        [DataRow("8", true)]
        [DataRow("9", true)]
        [DataRow("", false)]
        [DataRow(" 0 ", false)]
        [DataRow("A", false)]
        [DataRow("Zero", false)]
        public void ReadOnlyDictionary_ContainsKey(string key, bool expected)
        {
            ReadOnlyTarget.ContainsKey(key).Should().Be(expected);
        }

        [TestMethod]
        public void ReadOnlyDictionary_CopyTo()
        {
            // Arrange
            var copy = new KeyValuePair<string, string>[10];

            // Act
            ReadOnlyTarget.CopyTo(copy, 0);

            // Assert
            copy.Should().BeEquivalentTo(ReadOnlyTarget);
        }

        [TestMethod]
        public void ReadOnlyDictionary_Enumerate()
        {
            // Arrange
            var list = new List<string>(10);

            // Act
            foreach (var kvp in ReadOnlyTarget)
                list.Add(kvp.Key);

            // Assert
            list.Count.Should().Be(10);
            list.Should().BeEquivalentTo(ReadOnlyTarget.Keys);
        }

        [TestMethod]
        public void ReadOnlyDictionary_TryGetValue()
        {
            throw new NotImplementedException();
        }
    }
}
