using System;
using System.IO;

namespace Grimware.Extensions
{
    public static class StreamExtensions
    {
        private const int _BufferSize = 1024 * 16;

        public static Stream Copy(this Stream source)
        {
            if (source == null)
                return null;

            var originalPosition = -1L;

            if (source.CanSeek)
                originalPosition = source.Position;

            var ms = new MemoryStream();

            try
            {
                Copy(source, ms);

                ms.Seek(
                    originalPosition > -1
                        ? originalPosition
                        : 0,
                    SeekOrigin.Begin);

                return ms;
            }
            catch
            {
                ms.Dispose();
                throw;
            }
        }

        public static void Copy(this Stream source, Stream target)
        {
            int count;

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var originalPosition = -1L;
            var buffer = new byte[_BufferSize];

            if (source.CanSeek)
            {
                originalPosition = source.Position;
                source.Seek(0, SeekOrigin.Begin);
            }

            while ((count = source.Read(buffer, 0, buffer.Length)) > 0)
                target.Write(buffer, 0, count);

            if (originalPosition > -1)
                source.Seek(originalPosition, SeekOrigin.Begin);
        }
    }
}
