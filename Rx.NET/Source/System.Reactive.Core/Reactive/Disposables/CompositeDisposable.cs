// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a group of disposable resources that are disposed together.
    /// </summary>
    public sealed class CompositeDisposable : IDisposable
    {
        private IDisposable[] _disposables;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.CompositeDisposable"/> class from a group of disposables.
        /// </summary>
        /// <param name="disposables">Disposables that will be disposed together.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposables"/> is null.</exception>
        public CompositeDisposable(params IDisposable[] disposables)
        {
            if (disposables == null)
                throw new ArgumentNullException("disposables");

            _disposables = disposables;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.CompositeDisposable"/> class from a group of disposables.
        /// </summary>
        /// <param name="disposables">Disposables that will be disposed together.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposables"/> is null.</exception>
        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            if (disposables == null)
                throw new ArgumentNullException("disposables");

            _disposables = disposables.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.CompositeDisposable"/> class with no disposables contained by it initially.
        /// </summary>
        [Obsolete("This constructor is no longer supported on the CompositeDisposable type. Use the DisposableCollection type instead.", true)]
        public CompositeDisposable()
        {
            throw new NotSupportedException("This constructor is no longer supported on the CompositeDisposable type. Use the DisposableCollection type instead.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.CompositeDisposable"/> class with the specified number of disposables.
        /// </summary>
        /// <param name="capacity">The number of disposables that the new CompositeDisposable can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than zero.</exception>
        [Obsolete("This constructor is no longer supported on the CompositeDisposable type. Use the DisposableCollection type instead.", true)]
        public CompositeDisposable(int capacity)
        {
            throw new NotSupportedException("This constructor is no longer supported on the CompositeDisposable type. Use the DisposableCollection type instead.");
        }

        /// <summary>
        /// Disposes all disposables all child <see cref="IDisposable"/> instances.
        /// </summary>
        public void Dispose()
        {
            var currentDisposables = Interlocked.Exchange(ref _disposables, null);
            
            if (currentDisposables != null)
            {
                foreach (var d in currentDisposables)
                    if (d != null)
                        d.Dispose();
            }
        }
    }
}
