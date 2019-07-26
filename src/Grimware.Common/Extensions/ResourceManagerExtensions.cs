using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Resources;

namespace Grimware.Extensions
{
    public static class ResourceManagerExtensions
    {
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

        public static Stream TryGetStream(this ResourceManager resourceManager, string name)
        {
            return TryGetStream(resourceManager, name, CultureInfo.CurrentCulture);
        }

        public static Stream TryGetStream(this ResourceManager resourceManager, string name, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));
            return resourceManager != null && name != null
                ? resourceManager.GetObject(name, culture) as Stream
                : null;
        }

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
    }
}