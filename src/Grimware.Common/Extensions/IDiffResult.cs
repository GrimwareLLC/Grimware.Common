using System.Collections.Generic;

namespace Grimware.Extensions
{
    public interface IDiffResult<out T>
    {
        IEnumerable<T> Added { get; }

        IEnumerable<T> Modified { get; }

        IEnumerable<T> Removed { get; }

        IEnumerable<T> Unmodified { get; }
    }
}
