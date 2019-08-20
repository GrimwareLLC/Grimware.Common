using System.ComponentModel;

namespace Grimware
{
    public class CancelEventArgs<TState>
        : CancelEventArgs
    {
        public CancelEventArgs(TState state, bool cancel = false)
            : base(cancel)
        {
            State = state;
        }

        public TState State { get; }
    }
}