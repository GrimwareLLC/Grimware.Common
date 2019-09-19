using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Grimware.Extensions
{
    public static class GenericExtensions
    {
        public static bool In<T>(this T source, params T[] values)
        {
            return source == null ? values.Any(t => t == null) : values.Any(t => source.Equals(t));
        }

        public static bool IsNullOrDefault<T>(this T source)
            where T : struct
        {
            return IsNullOrDefault((T?) source);
        }

        public static bool IsNullOrDefault<T>(this T? source)
            where T : struct
        {
            return NullIfDefault(source) == null;
        }

        public static T? NullIf<T>(this T? source, Predicate<T> condition)
            where T : struct
        {
            if (condition != null)
                if (source != null)
                    return condition(source.Value) ? null : source;
                else
                    return null;
            throw new ArgumentNullException(nameof(condition));
        }

        public static T NullIf<T>(this T source, Predicate<T> condition)
            where T : class
        {
            if (condition != null)
                if (source != null)
                    return condition(source) ? null : source;
                else
                    return null;
            throw new ArgumentNullException(nameof(condition));
        }

        public static T? NullIfDefault<T>(this T source)
            where T : struct
        {
            return NullIfDefault((T?) source);
        }

        public static T? NullIfDefault<T>(this T? source)
            where T : struct
        {
            return NullIfIn(source, default(T));
        }

        public static T? NullIfIn<T>(this T source, params T[] values)
            where T : struct
        {
            return NullIfIn((T?) source, values);
        }

        public static T? NullIfIn<T>(this T? source, params T[] values)
            where T : struct
        {
            return NullIf(source, t => values.Any(v => v.Equals(t)));
        }

        public static int TryGetHashCode<T>(this T source)
        {
            return source == null ? 0 : source.GetHashCode();
        }

        public static string TrySerializeAsXml<T>(this T source)
        {
            if (source == null)
                return null;

            var sb = new StringBuilder(1024 * 4);
            using (var xmlWriter = XmlWriter.Create(new StringWriter(sb), new XmlWriterSettings {Encoding = Encoding.UTF8, Indent = true}))
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
            var xmlWriter = XmlWriter.Create(new StreamWriter(target), new XmlWriterSettings {Encoding = Encoding.UTF8, Indent = true});
            var serializer = new XmlSerializer(source.GetType());
            serializer.Serialize(xmlWriter, source);
            xmlWriter.Flush();
#pragma warning restore CA2000 // Dispose objects before losing scope
        }
    }
}
