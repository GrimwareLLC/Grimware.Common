using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static partial class DictionaryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> source) =>
            source != null
                ? new ReadOnlyDictionary<TKey, TValue>(source)
                : null;
    }
}
