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

        public LazyCollection()
            : this(LazyThreadSafetyMode.ExecutionAndPublication)
        {
        }

        public LazyCollection(LazyThreadSafetyMode mode)
            : this(Array.Empty<T>().AsEnumerable(), mode)
        {
        }

        public LazyCollection(IEnumerable<T> collection)
            : this(collection, LazyThreadSafetyMode.None)
        {
        }

        public LazyCollection(IEnumerable<T> collection, LazyThreadSafetyMode mode)
            : this(collection != null ? (Func<ICollection<T>>)collection.ToList : null, mode)
        {
        }

        public LazyCollection(Func<ICollection<T>> valueFactory)
            : this(valueFactory, LazyThreadSafetyMode.None)
        {
        }

        public LazyCollection(Func<ICollection<T>> valueFactory, LazyThreadSafetyMode mode)
        {
            if (valueFactory == null)
                throw new ArgumentNullException(nameof(valueFactory));

            _lazyCollection = new Lazy<ICollection<T>>(valueFactory, mode);
        }

        public int Count => Collection.Count;

        public bool IsReadOnly => Collection.IsReadOnly;

        internal ICollection<T> Collection => _lazyCollection.Value;

        public void Add(T item)
        {
            Collection.Add(item);
        }

        public void Clear()
        {
            Collection.Clear();
        }

        public bool Contains(T item)
        {
            return Collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return Collection.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
