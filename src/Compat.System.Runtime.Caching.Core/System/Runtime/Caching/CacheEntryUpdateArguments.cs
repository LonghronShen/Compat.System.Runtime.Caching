namespace System.Runtime.Caching
{

    /// <summary>
    /// Provides information about a cache entry that will be removed from the cache.
    /// </summary>
    public class CacheEntryUpdateArguments
    {

        /// <summary>
        /// Gets the unique identifier for a cache entry that is about to be removed.
        /// </summary>
        /// <value>The unique identifier for the cache entry.</value>
        public string Key { get; }

        /// <summary>
        /// Gets the reason that a cache entry is about to be removed from the cache.
        /// </summary>
        /// <value>
        /// One of the enumeration values that describes why a cache entry is being removed.
        /// </value>
        public CacheEntryRemovedReason RemovedReason { get; }

        /// <summary>
        /// Gets the name of a region in the cache that contains a cache entry.
        /// </summary>
        /// <value>
        /// The name of a region in the cache. If regions are not used, this value is null.
        /// </value>
        public string RegionName { get; }

        /// <summary>
        /// Gets a reference to the System.Runtime.Caching.ObjectCache instance that contains a cache entry that is about to be removed.
        /// </summary>
        /// <value>
        /// A reference to the cache instance.
        /// </value>
        public ObjectCache Source { get; }

        /// <summary>
        /// Gets or sets the value of System.Runtime.Caching.CacheItem entry that is used to update the cache object.
        /// </summary>
        /// <value>
        /// The cache entry to update in the cache object. The default is null.
        /// </value>
        public CacheItem UpdatedCacheItem { get; set; }

        /// <summary>
        /// Gets or sets the cache eviction or expiration policy of the System.Runtime.Caching.CacheItem entry that is updated.
        /// </summary>
        /// <value>
        /// The cache eviction or expiration policy of the cache item that was updated. The default is null.
        /// </value>
        public CacheItemPolicy UpdatedCacheItemPolicy { get; set; }

        /// <summary>
        /// Initializes a new instance of the System.Runtime.Caching.CacheEntryUpdateArguments class.
        /// </summary>
        /// <param name="source">The System.Runtime.Caching.ObjectCache instance from which the cache entry referenced by key will be removed.</param>
        /// <param name="reason">One of the enumeration values that indicate why the cache entry will be removed.</param>
        /// <param name="key">The key of the cache entry that will be removed.</param>
        /// <param name="regionName">The name of the region in the cache to remove the cache entry from. This parameter is optional. If cache regions are not defined, regionName must be null.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentNullException">key is null.</exception>
        public CacheEntryUpdateArguments(ObjectCache source, CacheEntryRemovedReason reason, string key, string regionName)
        {
            this.Source = source ?? throw new ArgumentNullException(nameof(source));
            this.Key = key ?? throw new ArgumentNullException(nameof(key));
            this.RemovedReason = reason;
            this.RegionName = regionName;
        }

    }

}
