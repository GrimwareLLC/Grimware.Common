using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Grimware.Extensions
{
    public static class GenericExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool In<T>(this T source, params T[] values) =>
            source == null ? values.Any(t => t == null) : values.Any(t => source.Equals(t));


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrDefault<T>(this T source)
            where T : struct =>
            IsNullOrDefault((T?)source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrDefault<T>(this T? source)
            where T : struct =>
            NullIfDefault(source) == null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? NullIf<T>(this T? source, Predicate<T> condition)
            where T : struct =>
            condition != null
                ? source != null
                      ? condition(source.Value)
                            ? null
                            : source
                      : null
                : throw new ArgumentNullException(nameof(condition));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NullIf<T>(this T source, Predicate<T> condition)
            where T : class =>
            condition != null
                ? source != null
                      ? condition(source)
                            ? null
                            : source
                      : null
                : throw new ArgumentNullException(nameof(condition));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? NullIfDefault<T>(this T source)
            where T : struct =>
            NullIfDefault((T?)source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? NullIfDefault<T>(this T? source)
            where T : struct =>
            NullIfIn(source, default(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? NullIfIn<T>(this T source, params T[] values)
            where T : struct =>
            NullIfIn((T?)source, values);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? NullIfIn<T>(this T? source, params T[] values)
            where T : struct =>
            NullIf(source, t => values.Any(v => v.Equals(t)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryGetHashCode<T>(this T source) => source == null ? 0 : source.GetHashCode();

        public static string TrySerializeAsXml<T>(this T source)
        {
            if (source == null)
                return null;

            var sb = new StringBuilder(1024 * 4);
            using (var xmlWriter = XmlWriter.Create(new StringWriter(sb), new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true }))
            {
                var serializer = new XmlSerializer(source.GetType());
                serializer.Serialize(xmlWriter, source);
                xmlWriter.Flush();
            }

            return sb.ToString();
        }

        public static void TrySerializeAsXml<T>(this T source, Stream target)
        {
            if (source == null)
                return;

            if (target == null)
                throw new ArgumentNullException(nameof(target));

#pragma warning disable CA2000 // Dispose objects before losing scope

            // If we dispose the XmlWriter, it would dispose the underlying stream, which we don't want.
            var xmlWriter = XmlWriter.Create(new StreamWriter(target), new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true });
            var serializer = new XmlSerializer(source.GetType());
            serializer.Serialize(xmlWriter, source);
            xmlWriter.Flush();
#pragma warning restore CA2000 // Dispose objects before losing scope
        }
    }
}
