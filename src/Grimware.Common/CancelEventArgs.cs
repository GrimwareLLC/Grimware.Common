using System.ComponentModel;

namespace Grimware
{
    public class CancelEventArgs<TState>
        : CancelEventArgs
    {
        public CancelEventArgs(TState state)
            : this(state, false)
        {
        }

        public CancelEventArgs(TState state, bool cancel)
            : base(cancel)
        {
            State = state;
        }

        public TState State { get; }
    }
}