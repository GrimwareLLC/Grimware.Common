using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StreamExtensionsTests
    {
        private Stream TestStream
        {
            get
            {
                var ms = new MemoryStream(
                    Encoding.UTF8.GetBytes(
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."));

                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
        }

        [TestMethod]
        public void Copy()
        {
            using (var result = TestStream.Copy())
            {
                result.Should().NotBeNull();
                result.Length.Should().Be(123);
            }
        }

        [TestMethod]
        public void Copy_ExceptionHandler()
        {
            // Arrange
            Stream copy = null;
            var stream = Substitute.For<Stream>();
            stream.CanSeek.Returns(x => throw new TestException());
            Action act = () => copy = stream.Copy();

            // Act
            act.Should().Throw<TestException>();

            // Assert
            copy.Should().BeNull();
        }

        [TestMethod]
        public void Copy_Null()
        {
            ((Stream)null).Copy().Should().BeNull();
        }
    }
}
