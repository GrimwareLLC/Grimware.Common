namespace Grimware.Extensions
{
    public static class Int32Extensions
    {
        public static int? CountOnBits(this int? value)
        {
            return value != null
                ? value != 0
                    ? 1 + CountOnBits(value & (value - 1))
                    : 0
                : null;
        }

        public static bool? ToBoolean(this int? source)
        {
            return source != null ? !0.Equals(source.Value) : (bool?)null;
        }
    }
}