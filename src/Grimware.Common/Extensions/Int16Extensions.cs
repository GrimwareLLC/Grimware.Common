using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static class Int16Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int? CountOnBits(this short? value) =>
            value != null
                ? ((int?)value.Value).CountOnBits()
                : null;
    }
}
