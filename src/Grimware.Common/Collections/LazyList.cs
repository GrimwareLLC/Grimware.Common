using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Grimware.Collections
{
    public class LazyList<T>
        : IList<T>
    {
        private readonly Lazy<IList<T>> _lazyList;

        public LazyList()
            : this(LazyThreadSafetyMode.ExecutionAndPublication)
        {
        }

        public LazyList(IEnumerable<T> collection)
            : this(collection, LazyThreadSafetyMode.ExecutionAndPublication)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
        }

        public LazyList(Func<IList<T>> valueFactory)
            : this(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication)
        {
        }

        public LazyList(bool isThreadSafe)
            : this(isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None)
        {
        }

        public LazyList(IEnumerable<T> collection, bool isThreadSafe)
            : this(collection, isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None)
        {
        }

        public LazyList(Func<IList<T>> valueFactory, bool isThreadSafe)
            : this(valueFactory, isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None)
        {
        }

        public LazyList(LazyThreadSafetyMode mode)
        {
            _lazyList = new Lazy<IList<T>>(mode);
        }

        public LazyList(IEnumerable<T> collection, LazyThreadSafetyMode mode)
            : this(collection.ToList, mode)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
        }

        public LazyList(Func<IList<T>> valueFactory, LazyThreadSafetyMode mode)
        {
            if (valueFactory == null)
                throw new ArgumentNullException(nameof(valueFactory));

            _lazyList = new Lazy<IList<T>>(valueFactory, mode);
        }

        public T this[int index]
        {
            get => _lazyList.Value[index];
            set => _lazyList.Value[index] = value;
        }

        public int Count => _lazyList.Value.Count;

        public bool IsReadOnly => _lazyList.Value.IsReadOnly;

        public void Add(T item)
        {
            _lazyList.Value.Add(item);
        }

        public void Clear()
        {
            _lazyList.Value.Clear();
        }

        public bool Contains(T item)
        {
            return _lazyList.Value.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _lazyList.Value.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _lazyList.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _lazyList.Value.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _lazyList.Value.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return _lazyList.Value.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _lazyList.Value.RemoveAt(index);
        }
    }
}