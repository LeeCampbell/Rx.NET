// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource whose underlying disposable resource can be swapped for another disposable resource.
    /// </summary>
    public sealed class MultipleAssignmentDisposable : IDisposable
    {
        private IDisposable _current;
        private bool _hasDisposeBeenRequested = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.MultipleAssignmentDisposable"/> class with no current underlying disposable.
        /// </summary>
        public MultipleAssignmentDisposable()
        {
        }

        /// <summary>
        /// Gets or sets the underlying disposable. After disposal, the result of getting this property is undefined.
        /// </summary>
        /// <remarks>If the MultipleAssignmentDisposable has already been disposed, assignment to this property causes immediate disposal of the given disposable object.</remarks>
        public IDisposable Disposable
        {
            get { return _current; }

            set
            {
                if (_hasDisposeBeenRequested)
                {
                    if (value != null)
                    {
                        value.Dispose();
                    }
                }
                else
                {
                    Interlocked.Exchange(ref _current, value);

                    //Mitigate race condition
                    if (_hasDisposeBeenRequested)
                    {
                        PerformDisposal();
                    }
                }
            }
        }

        /// <summary>
        /// Disposes the underlying disposable as well as all future replacements.
        /// </summary>
        public void Dispose()
        {
            _hasDisposeBeenRequested = true;
            PerformDisposal();
        }

        private void PerformDisposal()
        {
            var old = Interlocked.Exchange(ref _current, DefaultDisposable.Instance);
            if (old != null)
                old.Dispose();
        }
    }
}
