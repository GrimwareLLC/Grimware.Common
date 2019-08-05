using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static class Int32Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool? ToBoolean(this int? source) => source != null ? !0.Equals(source.Value) : (bool?)null;
    }
}
