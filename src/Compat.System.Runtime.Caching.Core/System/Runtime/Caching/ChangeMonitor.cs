using System.Threading;

namespace System.Runtime.Caching
{

    /// <summary>
    /// Provides a base class for a derived custom type that monitors changes in the state of the data which a cache item depends on.
    /// </summary>
    public abstract class ChangeMonitor
        : IDisposable
    {

        private const int INITIALIZED = 1;
        private const int CHANGED = 2;
        private const int INVOKED = 4;
        private const int DISPOSED = 8;

        private static readonly object NOT_SET = new object();

        private SafeBitVector32 _flags;
        private OnChangedCallback _onChangedCallback;
        private object _onChangedState = NOT_SET;

        /// <summary>
        /// Gets a value that indicates that the state that is monitored by the System.Runtime.Caching.ChangeMonitor class has changed.
        /// </summary>
        /// <value>
        /// true if the state that is monitored by the System.Runtime.Caching.ChangeMonitor instance has changed; otherwise, false.
        /// </value>
        public bool HasChanged => _flags[CHANGED];

        /// <summary>
        /// Gets a value that indicates that the derived instance of a System.Runtime.Caching.ChangeMonitor class is disposed.
        /// </summary>
        /// <value>
        /// true if the instance is disposed; otherwise, false.
        /// </value>
        public bool IsDisposed => _flags[DISPOSED];

        /// <summary>
        /// Gets a value that represents the System.Runtime.Caching.ChangeMonitor class instance.
        /// </summary>
        /// <value>
        /// The identifier for a change-monitor instance.
        /// </value>
        public abstract string UniqueId { get; }

        /// <summary>
        /// Initializes a new instance of the System.Runtime.Caching.ChangeMonitor class. This constructor is called from constructors in derived classes to initialize the base class.
        /// </summary>
        protected ChangeMonitor()
        {
        }

        /// <summary>
        /// Called by Cache implementers to register a callback and notify an System.Runtime.Caching.ObjectCache instance through the System.Runtime.Caching.OnChangedCallback delegate when a dependency has changed.
        /// </summary>
        /// <param name="onChangedCallback">A reference to a method that is invoked when a dependency has changed.</param>
        /// <exception cref="ArgumentNullException">onChangedCallback is null.</exception>
        /// <exception cref="InvalidOperationException">The callback method has already been invoked.</exception>
        public void NotifyOnChanged(OnChangedCallback onChangedCallback)
        {
            if (onChangedCallback == null)
            {
                throw new ArgumentNullException("onChangedCallback");
            }

            if (Interlocked.CompareExchange(ref _onChangedCallback, onChangedCallback, null) != null)
            {
                throw new InvalidOperationException("Method already invoked.");
            }

            if (_flags[CHANGED])
            {
                OnChanged(null);
            }
        }

        /// <summary>
        /// Called from the constructor of derived classes to indicate that initialization is finished.
        /// </summary>
        protected void InitializationComplete()
        {
            _flags[INITIALIZED] = true;
            if (_flags[CHANGED])
            {
                Dispose();
            }
        }

        /// <summary>
        /// Called by derived classes to raise the event when a dependency changes.
        /// </summary>
        /// <param name="state">The data for the change event. This value can be null.</param>
        protected void OnChanged(object state)
        {
            OnChangedHelper(state);
            if (_flags[INITIALIZED])
            {
                DisposeHelper();
            }
        }

        private void DisposeHelper()
        {
            if (_flags[INITIALIZED] && _flags.ChangeValue(DISPOSED, value: true))
            {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }

        private void OnChangedHelper(object state)
        {
            _flags[CHANGED] = true;
            Interlocked.CompareExchange(ref _onChangedState, state, NOT_SET);
            OnChangedCallback onChangedCallback = _onChangedCallback;
            if (onChangedCallback != null && _flags.ChangeValue(INVOKED, value: true))
            {
                onChangedCallback(_onChangedState);
            }
        }

        /// <summary>
        /// Releases all managed and unmanaged resources and any references to the System.Runtime.Caching.ChangeMonitor instance. This overload must be implemented by derived change-monitor classes.
        /// </summary>
        /// <param name="disposing">
        /// true to release managed and unmanaged resources and any references to a System.Runtime.Caching.ChangeMonitor
        /// instance; false to release only unmanaged resources. When false is passed, the
        /// System.Runtime.Caching.ChangeMonitor.Dispose(System.Boolean) method is called
        /// by a finalizer thread and any external managed references are likely no longer
        /// valid because they have already been garbage collected.
        /// </param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Releases all resources that are used by the current instance of the System.Runtime.Caching.ChangeMonitor class.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Initialization is not complete in the derived change-monitor class that called the base System.Runtime.Caching.ChangeMonitor.Dispose method.
        /// </exception>
        public void Dispose()
        {
            OnChangedHelper(null);
            if (!_flags[INITIALIZED])
            {
                throw new InvalidOperationException("Init not complete.");
            }

            DisposeHelper();
        }

    }

}
