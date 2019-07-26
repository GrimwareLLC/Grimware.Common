namespace Grimware.Extensions
{
    public static class Int64Extensions
    {
        public static int? CountOnBits(this long? value) =>
            value != null
                ? value != 0
                    ? 1 + CountOnBits(value & (value - 1))
                    : 0
                : null;
    }
}