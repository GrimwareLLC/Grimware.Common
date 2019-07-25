namespace Grimware.Extensions
{
    public static class Int16Extensions
    {
        public static int? CountOnBits(this short? value)
        {
            return value != null
                ? value != 0
                    ? 1 + (value & (value - 1)).CountOnBits()
                    : 0
                : null;
        }
    }
}