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

        public LazyCollection(IEnumerable<T> collection)
            : this(collection, LazyThreadSafetyMode.ExecutionAndPublication)
        {
        }

        public LazyCollection(Func<ICollection<T>> valueFactory)
            : this(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication)
        {
        }

        public LazyCollection(bool isThreadSafe)
            : this(isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None)
        {
        }

        public LazyCollection(IEnumerable<T> collection, bool isThreadSafe)
            : this(collection, isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None)
        {
        }

        public LazyCollection(Func<ICollection<T>> valueFactory, bool isThreadSafe)
            : this(valueFactory, isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None)
        {
        }

        public LazyCollection(LazyThreadSafetyMode mode)
        {
            _lazyCollection = new Lazy<ICollection<T>>(mode);
        }

        public LazyCollection(IEnumerable<T> collection, LazyThreadSafetyMode mode)
            : this(collection.ToList, mode)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
        }

        public LazyCollection(Func<ICollection<T>> valueFactory, LazyThreadSafetyMode mode)
        {
            if (valueFactory == null)
                throw new ArgumentNullException(nameof(valueFactory));

            _lazyCollection = new Lazy<ICollection<T>>(valueFactory, mode);
        }

        public int Count => _lazyCollection.Value.Count;

        public bool IsReadOnly => _lazyCollection.Value.IsReadOnly;

        public void Add(T item) => _lazyCollection.Value.Add(item);

        public void Clear() => _lazyCollection.Value.Clear();

        public bool Contains(T item) => _lazyCollection.Value.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _lazyCollection.Value.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _lazyCollection.Value.GetEnumerator();

        public bool Remove(T item) => _lazyCollection.Value.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
