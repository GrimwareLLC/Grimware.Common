using System;

namespace Grimware
{
    public sealed class DisposableAdapter
        : DisposableBase
    {
        private readonly Action _disposer;

        public DisposableAdapter(Action disposer)
        {
            _disposer = disposer;
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && !IsDisposed) _disposer?.Invoke();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}