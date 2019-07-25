using System.Drawing;
using System.IO;
using System.Resources;

namespace Grimware.Extensions
{
    public static class ResourceManagerExtensions
    {
        public static Icon TryGetIcon(this ResourceManager resourceManager, string name)
        {
            return resourceManager != null && name != null
                ? resourceManager.GetObject(name) as Icon
                : null;
        }

        public static Image TryGetImage(this ResourceManager resourceManager, string name)
        {
            return resourceManager != null && name != null
                ? resourceManager.GetObject(name) as Image
                : null;
        }

        public static Stream TryGetStream(this ResourceManager resourceManager, string name)
        {
            return resourceManager != null && name != null
                ? resourceManager.GetObject(name) as Stream
                : null;
        }

        public static string TryGetString(this ResourceManager resourceManager, string name)
        {
            return resourceManager != null && name != null
                ? resourceManager.GetObject(name) as string
                : null;
        }
    }
}