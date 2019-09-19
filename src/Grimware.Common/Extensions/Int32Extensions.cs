namespace Grimware.Extensions
{
    public static class Int32Extensions
    {
        public static bool ToBoolean(this int source)
        {
            return !0.Equals(source);
        }

        public static bool? ToBoolean(this int? source)
        {
            return source != null ? !0.Equals(source.Value) : (bool?) null;
        }
    }
}
