using System;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
#if NETFRAMEWORK
using System.Drawing;

#endif

namespace Grimware.Extensions
{
    public static class ResourceManagerExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Stream TryGetStream(this ResourceManager resourceManager, string name)
        {
            return TryGetStream(resourceManager, name, CultureInfo.CurrentCulture);
        }

        public static Stream TryGetStream(this ResourceManager resourceManager, string name, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));
            if (resourceManager != null && !String.IsNullOrWhiteSpace(name))
            {
                var resource = resourceManager.GetObject(name, culture);
                switch (resource)
                {
                    case byte[] buffer:
                        return new MemoryStream(buffer);
                }
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TryGetString(this ResourceManager resourceManager, string name)
        {
            return TryGetString(resourceManager, name, CultureInfo.CurrentCulture);
        }

        public static string TryGetString(this ResourceManager resourceManager, string name, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));
            return resourceManager != null && name != null
                ? resourceManager.GetObject(name, culture) as string
                : null;
        }
#if NETFRAMEWORK
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Icon TryGetIcon(this ResourceManager resourceManager, string name)
        {
            return TryGetIcon(resourceManager, name, CultureInfo.CurrentCulture);
        }

        public static Icon TryGetIcon(this ResourceManager resourceManager, string name, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));
            return resourceManager != null && name != null
                ? resourceManager.GetObject(name, culture) as Icon
                : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Image TryGetImage(this ResourceManager resourceManager, string name)
        {
            return TryGetImage(resourceManager, name, CultureInfo.CurrentCulture);
        }

        public static Image TryGetImage(this ResourceManager resourceManager, string name, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));
            return resourceManager != null && name != null
                ? resourceManager.GetObject(name, culture) as Image
                : null;
        }
#endif
    }
}
