using System;

namespace Grimware
{
    public class EventArgs<TState>
        : EventArgs
    {
        #region Constructors & Destructor

        public EventArgs(TState state)
        {
            State = state;
        }

        #endregion

        public TState State { get; }
    }
}