using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Grimware.Collections
{
    public class LazyCollection<T>
        : ICollection<T>
    {
        private readonly Lazy<ICollection<T>> _lazyCollection;

        internal ICollection<T> Collection => _lazyCollection.Value;

        public LazyCollection()
            : this(LazyThreadSafetyMode.ExecutionAndPublication)
        {
        }

        public LazyCollection(LazyThreadSafetyMode mode)
            : this(Array.Empty<T>().AsEnumerable(), mode)
        {
        }

        public LazyCollection(IEnumerable<T> collection, LazyThreadSafetyMode mode = LazyThreadSafetyMode.ExecutionAndPublication)
            : this(collection != null ? (Func<ICollection<T>>)collection.ToList : null, mode)
        {
        }

        public LazyCollection(Func<ICollection<T>> valueFactory, LazyThreadSafetyMode mode = LazyThreadSafetyMode.ExecutionAndPublication)
        {
            if (valueFactory == null)
                throw new ArgumentNullException(nameof(valueFactory));

            _lazyCollection = new Lazy<ICollection<T>>(valueFactory, mode);
        }

        public int Count => Collection.Count;

        public bool IsReadOnly => Collection.IsReadOnly;

        public void Add(T item) => Collection.Add(item);

        public void Clear() => Collection.Clear();

        public bool Contains(T item) => Collection.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => Collection.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();

        public bool Remove(T item) => Collection.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
