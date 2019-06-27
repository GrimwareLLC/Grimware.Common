using System;

namespace Grimware
{
    public sealed class DisposableAdapter
        : DisposableBase
    {
        private readonly Action _disposer;

        #region Constructors & Destructor

        public DisposableAdapter(Action disposer)
        {
            _disposer = disposer;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && !IsDisposed)
                {
                    _disposer?.Invoke();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}