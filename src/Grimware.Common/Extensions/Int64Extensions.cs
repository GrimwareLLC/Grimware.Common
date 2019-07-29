using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static class Int64Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int? CountOnBits(this long? value) =>
            value != null
                ? value != 0 ? 1 + CountOnBits(value & (value.Value - 1L)) : 0
                : null;
    }
}