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

            var ms = new MemoryStream();

            try
            {
                var originalPosition = -1L;

                if (source.CanSeek)
                {
                    originalPosition = source.Position;
                    source.Seek(0, SeekOrigin.Begin);
                }

                source.CopyTo(ms, _BufferSize);

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
    }
}
