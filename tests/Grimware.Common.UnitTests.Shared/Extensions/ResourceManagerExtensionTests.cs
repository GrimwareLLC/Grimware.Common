using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Resources;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#if NETFRAMEWORK
using System.Drawing;

#endif

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ResourceManagerExtensionTests
    {
        private readonly ResourceManager _target = new ResourceManager(typeof(Properties.Resources));

#if NETFRAMEWORK
        [TestMethod]
        public void TryGetIcon()
        {
            // Act
            using (var result = _target.TryGetIcon("Icon"))
            {
                // Assert
                result.Should().NotBeNull();
                result.Width.Should().Be(32);
                result.Height.Should().Be(32);
            }
        }

        [TestMethod]
        public void TryGetIcon_Invariant()
        {
            // Act
            using (var result = _target.TryGetIcon("Icon", CultureInfo.InvariantCulture))
            {
                // Assert
                result.Should().NotBeNull();
                result.Width.Should().Be(32);
                result.Height.Should().Be(32);
            }
        }

        [TestMethod]
        public void TryGetIcon_Exception()
        {
            Icon icon = null;
            try
            {
                // Arrange
                Action act = () => icon = _target.TryGetIcon("Icon", null);

                // Assert
                act.Should().Throw<ArgumentNullException>();
                icon.Should().BeNull();
            }
            finally
            {
                icon?.Dispose();
            }
        }

        [TestMethod]
        public void TryGetImage()
        {
            // Act
            using (var result = _target.TryGetImage("Png"))
            {
                // Assert
                result.Should().NotBeNull();
                result.Width.Should().Be(800);
                result.Height.Should().Be(600);
            }
        }

        [TestMethod]
        public void TryGetImage_Invariant()
        {
            // Act
            using (var result = _target.TryGetImage("Png", CultureInfo.InvariantCulture))
            {
                // Assert
                result.Should().NotBeNull();
                result.Width.Should().Be(800);
                result.Height.Should().Be(600);
            }
        }

        [TestMethod]
        public void TryGetImage_Exception()
        {
            Image image = null;
            try
            {
                // Arrange
                Action act = () => image = _target.TryGetImage("Png", null);

                // Assert
                act.Should().Throw<ArgumentNullException>();
                image.Should().BeNull();
            }
            finally
            {
                image?.Dispose();
            }
        }
#endif

        [TestMethod]
        public void TryGetStream()
        {
            // Act
            using (var result = _target.TryGetStream("Pdf"))
            {
                // Assert
                result.Should().NotBeNull();
                result.Length.Should().Be(81603);
            }
        }

        [TestMethod]
        public void TryGetStream_Invariant()
        {
            // Act
            using (var result = _target.TryGetStream("Pdf", CultureInfo.InvariantCulture))
            {
                // Assert
                result.Should().NotBeNull();
                result.Length.Should().Be(81603);
            }
        }

        [TestMethod]
        public void TryGetStream_NotFound()
        {
            // Act
            using (var result = _target.TryGetStream("PdfEmbedded"))
            {
                // Assert
                result.Should().BeNull();
            }
        }

        [TestMethod]
        public void TryGetStream_Exception()
        {
            Stream stream = null;
            try
            {
                // Arrange
                Action act = () => stream = _target.TryGetStream("Pdf", null);

                // Assert
                act.Should().Throw<ArgumentNullException>();
                stream.Should().BeNull();
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [TestMethod]
        public void TryGetString()
        {
            // Act
            var result = _target.TryGetString("Text");

            // Assert
            result.Should().NotBeNull();
            result.StartsWith("Lorem ipsum").Should().BeTrue();
            result.Length.Should().Be(3475);
        }

        [TestMethod]
        public void TryGetString_Invariant()
        {
            // Act
            var result = _target.TryGetString("Text", CultureInfo.InvariantCulture);

            // Assert
            result.Should().NotBeNull();
            result.StartsWith("Lorem ipsum").Should().BeTrue();
            result.Length.Should().Be(3475);
        }

        [TestMethod]
        public void TryGetString_Exception()
        {
            string str = null;

            // Arrange
            Action act = () => str = _target.TryGetString("Text", null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
            str.Should().BeNull();
        }
    }
}
