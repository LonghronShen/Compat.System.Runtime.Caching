namespace System.Runtime.Caching
{

    /// <summary>
    /// Represents a set of features that a cache implementation provides.
    /// </summary>
    [Flags]
    public enum DefaultCacheCapabilities
    {
        /// <summary>
        /// A cache implementation does not provide any of the features that are described in the System.Runtime.Caching.DefaultCacheCapabilities enumeration.
        /// </summary>
        None = 0,

        /// <summary>
        /// A cache implementation runs at least partially in memory.
        /// A distributed cache would not set this flag, whereas an in-memory cache such as the System.Runtime.Caching.MemoryCache class would do so.
        /// </summary>
        InMemoryProvider = 1,

        /// <summary>
        /// A cache implementation runs out-of-process. A distributed cache would set this flag,
        /// whereas an in-memory cache such as the System.Runtime.Caching.MemoryCache class would not.
        /// </summary>
        OutOfProcessProvider = 2,

        /// <summary>
        /// A cache implementation supports the ability to create change monitors that monitor entries.
        /// </summary>
        CacheEntryChangeMonitors = 4,

        /// <summary>
        /// A cache implementation supports the ability to automatically remove cache entries at a specific date and time.
        /// </summary>
        AbsoluteExpirations = 8,

        /// <summary>
        /// A cache implementation supports the ability to automatically remove cache entries that have not been accessed in a specified time span.
        /// </summary>
        SlidingExpirations = 16,

        /// <summary>
        /// A cache implementation can raise a notification that an entry is about to be
        ///    removed from the cache. This setting also indicates that a cache implementation
        ///    supports the ability to automatically replace the entry that is being removed
        ///    with a new cache entry.
        /// </summary>
        CacheEntryUpdateCallback = 32,

        /// <summary>
        /// A cache implementation can raise a notification that an entry has been removed from the cache.
        /// </summary>
        CacheEntryRemovedCallback = 64,

        /// <summary>
        /// A cache implementation supports the ability to partition its storage into cache regions,
        /// and supports the ability to insert cache entries into those regions and to retrieve cache entries from those regions.
        /// </summary>
        CacheRegions = 128
    }

}
