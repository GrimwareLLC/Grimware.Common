using System.ComponentModel;

namespace Grimware
{
    public class CancelEventArgs<TState>
        : CancelEventArgs
    {
        #region Constructors & Destructor

        public CancelEventArgs(TState state)
            : this(state, false)
        {
        }

        public CancelEventArgs(TState state, bool cancel)
            : base(cancel)
        {
            State = state;
        }

        #endregion

        public TState State { get; }
    }
}