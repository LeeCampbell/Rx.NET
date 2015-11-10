// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource whose underlying disposable resource can be replaced by another disposable resource, causing automatic disposal of the previous underlying disposable resource.
    /// </summary>
    public sealed class SerialDisposable : ICancelable
    {
        private IDisposable _current;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.SerialDisposable"/> class.
        /// </summary>
        public SerialDisposable()
        {
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return ReferenceEquals(_current, BooleanDisposable.True); }
        }

        /// <summary>
        /// Gets or sets the underlying disposable.
        /// </summary>
        /// <remarks>If the SerialDisposable has already been disposed, assignment to this property causes immediate disposal of the given disposable object. Assigning this property disposes the previous disposable object.</remarks>
        public IDisposable Disposable
        {
            get
            {
                return IsDisposed
                    ? null
                    : _current;
            }

            set
            {
                IDisposable previous;
                do
                {
                    previous = _current;
                    if (ReferenceEquals(previous, BooleanDisposable.True)) break;
                }
                //If the location is still set to the value we just checked (previous), it will get replaced. Else, try again.
                while (!ReferenceEquals(Interlocked.CompareExchange(ref _current, value, previous), previous));
                var wasReplaced = !ReferenceEquals(previous, BooleanDisposable.True);

                if (wasReplaced)
                {
                    if (previous != null)
                        previous.Dispose();
                }
                else
                {
                    if (value != null)
                        value.Dispose();
                }
            }
        }

        /// <summary>
        /// Disposes the underlying disposable as well as all future replacements.
        /// </summary>
        public void Dispose()
        {
            var old = Interlocked.Exchange(ref _current, BooleanDisposable.True);
            if (old != null)
                old.Dispose();
        }
    }
}