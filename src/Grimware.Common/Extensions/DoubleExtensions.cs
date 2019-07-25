namespace Grimware.Extensions
{
    public static class DoubleExtensions
    {
        public static decimal? ToDecimal(this double? source)
        {
            return source != null ? (decimal)source : (decimal?)null;
        }
    }
}