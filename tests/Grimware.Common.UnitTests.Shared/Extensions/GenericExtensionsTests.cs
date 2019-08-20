using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GenericExtensionsTests
    {
        [TestMethod]
        public void In()
        {
            var obj = new object();

            ((object) null).In(new object(), new object(), null, new object()).Should().BeTrue();
            ((object) null).In(new object(), new object(), new object()).Should().BeFalse();

            obj.In(new object(), new object(), obj, new object()).Should().BeTrue();
            obj.In(new object(), new object(), null, new object()).Should().BeFalse();

            0.In(Int32.MinValue, -1, 0, 1, Int32.MaxValue).Should().BeTrue();
            0.In(Int32.MinValue, -1, 1, Int32.MaxValue).Should().BeFalse();

            "Test".In("Lorem", "ipsum", "Test").Should().BeTrue();
            "Test".In("Lorem", "ipsum").Should().BeFalse();

            Digit.Zero.In(Digit.Zero, Digit.Two, Digit.Four, Digit.Six, Digit.Eight).Should().BeTrue();
            Digit.Five.In(Digit.Zero, Digit.Two, Digit.Four, Digit.Six, Digit.Eight).Should().BeFalse();
        }

        [TestMethod]
        public void IsNullOrDefault()
        {
            Digit.Zero.IsNullOrDefault().Should().BeTrue();
            Digit.One.IsNullOrDefault().Should().BeFalse();
            Digit.Two.IsNullOrDefault().Should().BeFalse();
            Digit.Three.IsNullOrDefault().Should().BeFalse();
            Digit.Four.IsNullOrDefault().Should().BeFalse();
            Digit.Five.IsNullOrDefault().Should().BeFalse();

            ((Digit?) null).IsNullOrDefault().Should().BeTrue();
            ((Digit?) Digit.Zero).IsNullOrDefault().Should().BeTrue();
            ((Digit?) Digit.Six).IsNullOrDefault().Should().BeFalse();
        }

        [TestMethod]
        public void NullIf_Class()
        {
            var obj = new object();

            ((object) null).NullIf(o => o == obj).Should().BeNull();
            obj.NullIf(o => o == obj).Should().BeNull();
            new object().NullIf(o => o == obj).Should().NotBeNull();
        }

        [TestMethod]
        public void NullIf_Struct()
        {
            ((Digit?) null).NullIf(d => Digit.Zero.Equals(d)).Should().BeNull();
            ((Digit?) Digit.Zero).NullIf(d => Digit.Zero.Equals(d)).Should().BeNull();
            ((Digit?) Digit.One).NullIf(d => Digit.Zero.Equals(d)).Should().Be(Digit.One);
            ((Digit?) Digit.Nine).NullIf(d => Digit.Zero.Equals(d)).Should().Be(Digit.Nine);
        }

        [TestMethod]
        public void NullIfDefault()
        {
            ((int?) null).NullIfDefault().Should().BeNull();
            ((int?) 0).NullIfDefault().Should().BeNull();
            ((int?) 1).NullIfDefault().Should().Be(1);

            0.NullIfDefault().Should().BeNull();
            1.NullIfDefault().Should().Be(1);

            ((Digit?) null).NullIfDefault().Should().BeNull();
            ((Digit?) Digit.Zero).NullIfDefault().Should().BeNull();
            ((Digit?) Digit.Eight).NullIfDefault().Should().Be(Digit.Eight);

            Digit.Zero.NullIfDefault().Should().BeNull();
            Digit.Six.NullIfDefault().Should().Be(Digit.Six);
        }

        [TestMethod]
        public void NullIfEquals()
        {
            ((int?) null).NullIfIn(10).Should().BeNull();
            ((int?) 10).NullIfIn(10).Should().BeNull();
            ((int?) 11).NullIfIn(10).Should().Be(11);

            0.NullIfIn(0).Should().BeNull();
            0.NullIfIn(10).Should().Be(0);

            ((Digit?) null).NullIfIn(Digit.Zero).Should().BeNull();
            ((Digit?) Digit.One).NullIfIn(Digit.One).Should().BeNull();
            ((Digit?) Digit.Zero).NullIfIn(Digit.One).Should().Be(Digit.Zero);

            Digit.Seven.NullIfIn(Digit.Seven).Should().BeNull();
            Digit.Eight.NullIfIn(Digit.Seven).Should().Be(Digit.Eight);
        }

        [TestMethod]
        public void TryGetHashCode()
        {
            var obj = new object();
            var str = "Dummy String";

            ((object) null).TryGetHashCode().Should().Be(0);
            obj.TryGetHashCode().Should().Be(obj.GetHashCode());

            0.TryGetHashCode().Should().Be(0.GetHashCode());
            1.TryGetHashCode().Should().Be(1.GetHashCode());
            ((int?) 7).TryGetHashCode().Should().Be(7.GetHashCode());

            Digit.Zero.TryGetHashCode().Should().Be(Digit.Zero.GetHashCode());
            Digit.Nine.TryGetHashCode().Should().Be(Digit.Nine.GetHashCode());
            ((Digit?) Digit.Five).TryGetHashCode().Should().Be(Digit.Five.GetHashCode());

            ((string) null).TryGetHashCode().Should().Be(0);
            str.TryGetHashCode().Should().Be(str.GetHashCode());
        }

        [TestMethod]
        public void TrySerializeAsXml()
        {
            // Arrange
            var note = new Note {From = "Waldo", To = "Fred", Subject = "FooBar", Body = "Lorem ipsum dolor sit amet."};
            var notes = Enumerable.Repeat(note, 10).ToArray();

            // Act
            var noteXml = note.TrySerializeAsXml();
            var notesXml = notes.TrySerializeAsXml();

            // Assert
            ((object) null).TrySerializeAsXml().Should().BeNull();

            noteXml.Should().NotBeNull();
            noteXml.Length.Should().Be(267);

            notesXml.Should().NotBeNull();
            notesXml.Length.Should().Be(1579);
        }

        [TestMethod]
        public void TrySerializeAsXml_Exceptions()
        {
            // Arrange
            var note = new Note {From = "Waldo", To = "Fred", Subject = "FooBar", Body = "Lorem ipsum dolor sit amet."};

            // Act
            Action act = () => note.TrySerializeAsXml(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void TrySerializeAsXml_Stream()
        {
            // Arrange
            var note = new Note {From = "Waldo", To = "Fred", Subject = "FooBar", Body = "Lorem ipsum dolor sit amet."};
            var notes = Enumerable.Repeat(note, 10).ToArray();
            var stream = new MemoryStream(2048);

            // Act & Assert
            try
            {
                ((object) null).TrySerializeAsXml(stream);
                stream.Length.Should().Be(0);

                stream.SetLength(0);
                note.TrySerializeAsXml(stream);
                stream.Length.Should().Be(266);

                stream.SetLength(0);
                notes.TrySerializeAsXml(stream);
                stream.Length.Should().Be(1578);
            }
            finally
            {
                stream.Dispose();
            }
        }

#pragma warning disable CA1034 // Nested types should not be visible

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public class Note
        {
            public string Body { get; set; }
            public string From { get; set; }
            public string Subject { get; set; }
            public string To { get; set; }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Global
#pragma warning restore CA1034 // Nested types should not be visible
    }
}