namespace System.Runtime.Caching
{

    /// <summary>
    /// Specifies the reason why a cache entry was removed or an entry is about to be removed.
    /// </summary>
    public enum CacheEntryRemovedReason
    {
        /// <summary>
        /// A cache entry was removed by using the System.Runtime.Caching.ObjectCache.Remove(System.String,System.String)
        /// or System.Runtime.Caching.ObjectCache.Set(System.String,System.Object,System.DateTimeOffset,System.String) method.
        /// </summary>
        Removed = 0,

        /// <summary>
        /// A cache entry was removed because it expired. Expiration can be based on an absolute time or on a sliding expiration time.
        /// </summary>
        Expired = 1,

        /// <summary>
        /// A cache entry was removed to free memory in the cache.
        /// This occurs when a cache instance approaches cache-specific memory limits,
        /// or when a process or cache instance approaches computer-wide memory limits.
        /// </summary>
        Evicted = 2,

        /// <summary>
        /// A cache entry was removed because a related dependency (such as a file or another cache entry) triggered eviction of the cache entry.
        /// </summary>
        ChangeMonitorChanged = 3,

        /// <summary>
        /// A cache entry was evicted for as reason that is defined by a particular cache implementation.
        /// </summary>
        CacheSpecificEviction = 4
    }

}
