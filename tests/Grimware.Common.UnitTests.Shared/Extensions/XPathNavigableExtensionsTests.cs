using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.XPath;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class XPathNavigableExtensionsTests
    {
        private static readonly IXPathNavigable _TestDocument =
            new XPathDocument(
                new StringReader(
                    @"<?xml version='1.0'?>
                        <notes>
                            <note>
                                <to>Waldo</to>
                                <from>Fred</from>
                                <subject>Foo</subject>
                                <body>Lorem ipsum</body>
                            </note>
                            <note>
                                <to>Fred</to>
                                <from>Waldo</from>
                                <subject>Bar</subject>
                                <body>Lorem ipsum</body>
                            </note>
                        </notes>
                    "));


        [TestMethod]
        public void ToXmlDocument()
        {
            ((IXPathNavigable)null).ToXmlDocument().Should().BeNull();

            var xml = _TestDocument.ToXmlDocument();
            xml.Should().NotBeNull();
            xml.IsReadOnly.Should().BeFalse();
        }

        [TestMethod]
        public void ToXPathDocument()
        {
            ((IXPathNavigable)null).ToXPathDocument().Should().BeNull();

            var xpDoc = _TestDocument.ToXPathDocument();
            xpDoc.Should().NotBeNull();
        }

        [TestMethod]
        public void WriteToStream_New()
        {
            ((IXPathNavigable)null).WriteToStream().Should().BeNull();

            using (var stream = _TestDocument.WriteToStream())
            {
                stream.Should().NotBeNull();
                stream.Length.Should().Be(301);
            }
        }

        [TestMethod]
        public void WriteToStream_ExceptionHandler()
        {
            // Arrange
            Stream stream = null;
            var nav = Substitute.For<IXPathNavigable>();
            nav.CreateNavigator().Returns(x => throw new TestException());
            
            // Act
            Action act = () => stream = nav.WriteToStream();

            // Assert
            act.Should().Throw<TestException>();
            stream.Should().BeNull();

            stream?.Dispose();
        }

        [TestMethod]
        public void WriteToStream_Existing()
        {
            ((IXPathNavigable)null).WriteToStream(null);

            using (var stream = new MemoryStream())
            {
                _TestDocument.WriteToStream(stream);
                stream.Length.Should().Be(301);
            }
        }

        [TestMethod]
        public void WriteToStream_Exception()
        {
            Action act = () => _TestDocument.WriteToStream(null);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
