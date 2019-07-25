using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Grimware.Extensions
{
    public static class XPathNavigableExtensions
    {
        public static XPathDocument ToXPathDocument(this IXPathNavigable source)
        {
            var nav = source?.CreateNavigator();

            return
                nav != null
                    ? new XPathDocument(nav.ReadSubtree())
                    : null;
        }

        public static XmlDocument ToXmlDocument(this IXPathNavigable source)
        {
            if (source == null)
                return null;

            var nav = source.CreateNavigator();
            var doc = new XmlDocument();
            if (nav != null)
                doc.Load(nav.ReadSubtree());

            return doc;
        }

        public static Stream WriteToStream(this IXPathNavigable source)
        {
            if (source == null)
                return null;

            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(1024);
                WriteToStream(source, ms);

                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
            catch
            {
                ms?.Dispose();
                throw;
            }
        }

        public static void WriteToStream(this IXPathNavigable source, Stream stream)
        {
            if (source == null)
                return;
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var textWriter = new StreamWriter(stream);
            var xmlWriter = XmlWriter.Create(textWriter, new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8
            });

            var nav = source.CreateNavigator();
            nav?.WriteSubtree(xmlWriter);

            xmlWriter.Flush();
        }
    }
}