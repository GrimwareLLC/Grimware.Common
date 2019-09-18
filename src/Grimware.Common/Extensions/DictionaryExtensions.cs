using System.Collections.Generic;

namespace Grimware.Extensions
{
    public static partial class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            return source != null
                ? new ReadOnlyDictionary<TKey, TValue>(source)
                : null;
        }
    }
}