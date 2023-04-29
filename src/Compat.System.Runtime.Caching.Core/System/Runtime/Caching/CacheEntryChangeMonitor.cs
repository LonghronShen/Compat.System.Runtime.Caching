using System.Collections.ObjectModel;

namespace System.Runtime.Caching
{

    /// <summary>
    /// Provides a base class that represents a System.Runtime.Caching.ChangeMonitor type that can be implemented in order to monitor changes to cache entries.
    /// </summary>
    public abstract class CacheEntryChangeMonitor
        : ChangeMonitor
    {

        /// <summary>
        /// Gets a collection of cache keys that are monitored for changes.
        /// </summary>
        /// <value>
        /// A collection of cache keys.</value>
        public abstract ReadOnlyCollection<string> CacheKeys { get; }

        /// <summary>
        /// Gets a value that indicates the latest time (in UTC time) that the monitored cache entry was changed.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        public abstract DateTimeOffset LastModified { get; }

        /// <summary>
        /// Gets the name of a region of the cache.
        /// </summary>
        /// <value>
        /// The name of a region in the cache.
        /// </value>
        public abstract string RegionName { get; }

        /// <summary>
        /// Initializes a new instance of the System.Runtime.Caching.CacheEntryChangeMonitor class. This constructor is called from constructors in derived classes to initialize the base class.
        /// </summary>
        protected CacheEntryChangeMonitor()
        {
        }

    }

}
