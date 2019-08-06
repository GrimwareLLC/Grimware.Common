using System;
using System.Globalization;
using System.Runtime.CompilerServices;

#if NETCORE
using System.Diagnostics.CodeAnalysis;

#endif

namespace Grimware
{
    /// <summary>
    ///     Implements the "disposable pattern" as an abstract base class.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This class can be inherited by derived classes that need to implement
    ///         the "disposable pattern" to simplify the process of implementing the
    ///         pattern.
    ///     </para>
    ///     <para>
    ///         Note that this only helps with classes that do not need to be
    ///         part of another inheritance hierarchy already.  However, the base class
    ///         of that hierarchy can inherit from <see cref="DisposableBase" />, causing
    ///         the entire class hierarchy to implement the pattern implicitly.
    ///     </para>
    /// </remarks>
    public abstract class DisposableBase
        : IDisposable
    {
        /// <summary>
        ///     Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if this instance is disposed; otherwise,
        ///     <see langword="false" />.
        /// </value>
        public bool IsDisposed { get; private set; }

#pragma warning disable CA1063 // Implement IDisposable Correctly
        /// <summary>
        ///     Releases unmanaged resources and performs other cleanup operations
        ///     before the <see cref="DisposableBase" /> is reclaimed by garbage
        ///     collection.
        /// </summary>
#if NETCORE
        [ExcludeFromCodeCoverage] // Finalizers can't be tested properly in netcore
#endif
        ~DisposableBase()
        {
            Dispose(false);

            RaiseDisposedEvent();
        }

#pragma warning restore CA1063 // Implement IDisposable Correctly
        /// <summary>
        ///     Raised after this instance has been disposed or finalized.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        ///     Raised before this instance is disposed.
        /// </summary>
        public event EventHandler Disposing;

#pragma warning disable CA1063 // Implement IDisposable Correctly
        /// <summary>
        ///     Performs application-defined tasks associated with freeing,
        ///     releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>
        ///     Raises the <see cref="Disposing" /> event before the resources are
        ///     disposed, and the <see cref="Disposed" /> event once disposal is
        ///     completed.
        /// </remarks>
        public void Dispose()
        {
            try
            {
                RaiseDisposingEvent();
            }
            finally
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            RaiseDisposedEvent();
        }
#pragma warning restore CA1063 // Implement IDisposable Correctly

        /// <summary>
        ///     When overridden in a derived class, builds an
        ///     <see cref="ObjectDisposedException" /> with an appropriate ObjectName.
        /// </summary>
        /// <returns>
        ///     An <see cref="ObjectDisposedException" />.
        /// </returns>
        protected virtual ObjectDisposedException BuildObjectDisposedException()
        {
            var message = String.Format(
                CultureInfo.InvariantCulture,
                "{0}::{1}",
                GetType().FullName,

                // Call ToString on the Int32 to prevent boxing
                GetHashCode().ToString(CultureInfo.InvariantCulture));

            return new ObjectDisposedException(message);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true" /> to release both
        ///     managed and unmanaged resources; <see langword="false" /> to release
        ///     only unmanaged resources.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void Dispose(bool disposing) => IsDisposed = true;

        /// <summary>
        ///     Checks whether this instance has been disposed, and throws an
        ///     <see cref="ObjectDisposedException" /> if it has.
        /// </summary>
        protected void CheckDisposed()
        {
            if (IsDisposed)
            {
                throw BuildObjectDisposedException();
            }
        }

#pragma warning disable CA1030 // Use events where appropriate
        /// <summary>
        ///     Raises a generic event with safety testing for <see langword="null" />.
        /// </summary>
        /// <remarks>
        ///     Since the event object is passed in as a parameter, it can't
        ///     accidentally be converted to a <see langword="null" /> mid-flight by
        ///     another thread after it's been tested.
        /// </remarks>
        /// <typeparam name="T">
        ///     The type of the 'e' parameter, which must be derived
        ///     from <see cref="EventArgs" />.
        /// </typeparam>
        /// <param name="event">The event to be raised.</param>
        /// <param name="e">
        ///     The <see cref="EventArgs" /> (or derived class) instance
        ///     to be raised with the event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <see cref="EventArgs" /> (or derived class) instance parameter is
        ///     <see langword="null" />.
        /// </exception>
        protected void RaiseEvent<T>(EventHandler<T> @event, T e)
            where T : EventArgs
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            @event?.Invoke(this, e);
        }
#pragma warning restore CA1030 // Use events where appropriate

#pragma warning disable CA1030 // Use events where appropriate
        /// <summary>
        ///     Raises an event with safety testing for <see langword="null" />.
        /// </summary>
        /// <remarks>
        ///     Since the event object is passed in as a parameter, it can't
        ///     accidentally be converted to a <see langword="null" /> mid-flight by
        ///     another thread after it's been tested.
        /// </remarks>
        /// <param name="event">The event to be raised.</param>
        /// <param name="e">
        ///     The <see cref="EventArgs" /> (or derived class) instance
        ///     to be raised with the event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <see cref="EventArgs" /> (or derived class) instance parameter is
        ///     <see langword="null" />.
        /// </exception>
        protected void RaiseEvent(EventHandler @event, EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            @event?.Invoke(this, e);
        }
#pragma warning restore CA1030 // Use events where appropriate

        /// <summary>
        ///     Raises the Disposed event if any event handlers are attached.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RaiseDisposedEvent() => RaiseEvent(Disposed, new EventArgs());

        /// <summary>
        ///     Raises the Disposing event if any event handlers are attached.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RaiseDisposingEvent() => RaiseEvent(Disposing, new EventArgs());
    }
}
