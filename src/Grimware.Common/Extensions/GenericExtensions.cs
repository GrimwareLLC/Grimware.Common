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
        public static TDerived CreateDerivedCopy<TBase, TDerived>(this TBase source)
            where TBase : class
            where TDerived : class, TBase, new() =>
            source?.TransferTo(new TDerived());

        public static bool In<T>(this T source, params T[] values) => source == null ? values.Any(t => t == null) : values.Any(t => source.Equals(t));

        public static bool IsNullOrDefault<T>(this T? source)
            where T : struct =>
            source.NullIfDefault() == null;

        public static T? NullIfDefault<T>(this T? source)
            where T : struct =>
            source.NullIf(default(T));

        public static T? NullIf<T>(this T? source, T value)
            where T : struct =>
            NullIf(source, t => source != null && source.Value.Equals(value));

        public static T? NullIf<T>(this T? source, Predicate<T> condition)
            where T : struct
        {
            return condition != null
                ? source != null
                    ? condition(source.Value) ? null : source
                    : null
                : throw new ArgumentNullException(nameof(condition));
        }

        public static T NullIf<T>(this T source, Predicate<T> condition)
            where T : class =>
            condition != null
                ? source != null
                    ? !condition(source) ? source : null
                    : null
                : throw new ArgumentNullException(nameof(condition));

        public static T? NullIfIn<T>(this T? source, params T[] values)
            where T : struct =>
            source.NullIf(t => t.In(values));

        public static T NullIfIn<T>(this T source, params T[] values)
            where T : class =>
            source?.NullIf(t => t.In(values));

        public static TDerived TransferTo<TBase, TDerived>(this TBase source, TDerived target)
            where TBase : class
            where TDerived : class, TBase
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var derivedType = typeof(TDerived);
            var baseType = typeof(TBase);

            baseType.GetFields()
                .Intersect(derivedType.GetFields())
                .ForEach(f => f.SetValue(target, f.GetValue(source)));

            baseType.GetProperties()
                .Intersect(derivedType.GetProperties())
                .Where(p => p.CanRead && p.CanWrite)
                .ForEach(p => p.SetValue(target, p.GetValue(source, null), null));

            return target;
        }

        public static int TryGetHashCode<T>(this T source) => source == null ? 0 : source.GetHashCode();

        public static string TrySerializeAsXml<T>(this T source)
        {
            if (source == null)
                return null;

            var sb = new StringBuilder(0x1000);
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
            var xmlWriter = XmlWriter.Create(new StreamWriter(target), new XmlWriterSettings {Encoding = Encoding.UTF8, Indent = true});
            var serializer = new XmlSerializer(source.GetType());
            serializer.Serialize(xmlWriter, source);
            xmlWriter.Flush();
#pragma warning restore CA2000 // Dispose objects before losing scope
        }
    }
}