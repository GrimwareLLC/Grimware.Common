using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static class Int32Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int? CountOnBits(this int? value) =>
            value != null
                ? value != 0 ? 1 + CountOnBits(value & (value - 1)) : 0
                : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool? ToBoolean(this int? source) => source != null ? !0.Equals(source.Value) : (bool?)null;
    }
}