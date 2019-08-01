using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Grimware.Resources;

namespace Grimware.Extensions
{
    partial class DictionaryExtensions
    {
        private class ReadOnlyDictionary<TKey, TValue>
            : IDictionary<TKey, TValue>
        {
            private readonly IDictionary<TKey, TValue> _dictionary;

            public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
            {
                _dictionary = new Dictionary<TKey, TValue>(dictionary, (dictionary as Dictionary<TKey, TValue>)?.Comparer);
            }

            private TValue this[TKey key] => _dictionary[key];

            TValue IDictionary<TKey, TValue>.this[TKey key]
            {
                get => this[key];
                set => throw ReadOnlyException();
            }

            // ReSharper disable once UnusedMember.Local
            public static ReadOnlyDictionary<TKey, TValue> Empty { get; } =
                new ReadOnlyDictionary<TKey, TValue>(new Dictionary<TKey, TValue>());

            public int Count => _dictionary.Count;

            public bool IsReadOnly => true;

            public ICollection<TKey> Keys => _dictionary.Keys;

            public ICollection<TValue> Values => _dictionary.Values;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static Exception ReadOnlyException() => new NotSupportedException(ExceptionMessages.DictionaryIsReadOnly);

            public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

            public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

            public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

            void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => throw ReadOnlyException();

            void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => throw ReadOnlyException();

            void ICollection<KeyValuePair<TKey, TValue>>.Clear() => throw ReadOnlyException();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            bool IDictionary<TKey, TValue>.Remove(TKey key) => throw ReadOnlyException();

            bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => throw ReadOnlyException();
        }
    }
}