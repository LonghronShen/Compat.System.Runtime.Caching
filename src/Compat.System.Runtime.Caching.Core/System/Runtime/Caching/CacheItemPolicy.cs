using System.Collections.ObjectModel;

namespace System.Runtime.Caching
{

    /// <summary>
    /// Represents a set of eviction and expiration details for a specific cache entry.
    /// </summary>
    public class CacheItemPolicy
    {

        private Collection<ChangeMonitor> _changeMonitors;

        /// <summary>
        /// Gets or sets a value that indicates whether a cache entry should be evicted after a specified duration.
        /// </summary>
        /// <value>
        /// The period of time that must pass before a cache entry is evicted.
        /// The default value is System.Runtime.Caching.ObjectCache.InfiniteAbsoluteExpiration, meaning that the entry does not expire.
        /// </value>
        public DateTimeOffset AbsoluteExpiration { get; set; }

        /// <summary>
        /// Gets or sets a priority setting that is used to determine whether to evict a  cache entry.
        /// </summary>
        /// <value>
        /// One of the enumeration values that indicates the priority for eviction.
        /// The default priority value is System.Runtime.Caching.CacheItemPriority.Default, which means no priority.
        /// </value>
        public CacheItemPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets a reference to a System.Runtime.Caching.CacheEntryRemovedCallback delegate that is called after an entry is removed from the cache.
        /// </summary>
        /// <value>
        /// A reference to a delegate that is called by a cache implementation.
        /// </value>
        public CacheEntryRemovedCallback RemovedCallback { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether a cache entry should be evicted if it has not been accessed in a given span of time.
        /// </summary>
        /// <value>
        /// A span of time within which a cache entry must be accessed before the cache entry
        ///    is evicted from the cache. The default is System.Runtime.Caching.ObjectCache.NoSlidingExpiration,
        ///    meaning that the item should not be expired based on a time span.
        /// </value>
        public TimeSpan SlidingExpiration { get; set; }

        /// <summary>
        /// Gets or sets a reference to a System.Runtime.Caching.CacheEntryUpdateCallback delegate that is called before a cache entry is removed from the cache.
        /// </summary>
        /// <value>
        /// A reference to a delegate that is called by a cache implementation.
        /// </value>
        public CacheEntryUpdateCallback UpdateCallback { get; set; }

        /// <summary>
        /// Gets a collection of System.Runtime.Caching.ChangeMonitor objects that are associated with a cache entry.
        /// </summary>
        /// <value>
        /// A collection of change monitors. The default is an empty collection.
        /// </value>
        public Collection<ChangeMonitor> ChangeMonitors
        {
            get
            {
                if (_changeMonitors == null)
                {
                    _changeMonitors = new Collection<ChangeMonitor>();
                }
                return _changeMonitors;
            }
        }

        /// <summary>
        /// Initializes a new instance of the System.Runtime.Caching.CacheItemPolicy class.
        /// </summary>
        public CacheItemPolicy()
        {
            this.AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            this.SlidingExpiration = ObjectCache.NoSlidingExpiration;
            this.Priority = CacheItemPriority.Default;
        }

    }

}
