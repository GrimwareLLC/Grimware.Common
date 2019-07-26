using System;

namespace Grimware
{
    public class EventArgs<TState>
        : EventArgs
    {
        public EventArgs(TState state)
        {
            State = state;
        }

        public TState State { get; }
    }
}