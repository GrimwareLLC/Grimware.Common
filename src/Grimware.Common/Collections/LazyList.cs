using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Grimware.Collections
{
#pragma warning disable CA1710 // Identifiers should have correct suffix
    public class LazyList<T>
        : LazyCollection<T>, IList<T>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        public LazyList()
            : this(LazyThreadSafetyMode.ExecutionAndPublication)
        {
        }

        public LazyList(LazyThreadSafetyMode mode)
            : this(Array.Empty<T>().AsEnumerable(), mode)
        {
        }

        public LazyList(IEnumerable<T> collection, LazyThreadSafetyMode mode = LazyThreadSafetyMode.ExecutionAndPublication)
            : this(collection != null ? (Func<IList<T>>)collection.ToList : Array.Empty<T>, mode)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
        }

        public LazyList(Func<IList<T>> valueFactory, LazyThreadSafetyMode mode = LazyThreadSafetyMode.ExecutionAndPublication)
            : base(valueFactory ?? Array.Empty<T>, mode)
        {
            if (valueFactory == null)
                throw new ArgumentNullException(nameof(valueFactory));
        }

        public T this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }

        private IList<T> List => Collection as IList<T>;

        public int IndexOf(T item)
        {
            return List.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            List.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            List.RemoveAt(index);
        }
    }
}
