namespace System.Runtime.Caching
{

    /// <summary>
    /// Provides information about a cache entry that was removed from the cache.
    /// </summary>
    public class CacheEntryRemovedArguments
    {

        /// <summary>
        /// Gets an instance of a cache entry that was removed from the cache.
        /// </summary>
        /// <value>
        /// An instance of the System.Runtime.Caching.CacheItem class that was removed from the cache.
        /// </value>
        public CacheItem CacheItem { get; }

        /// <summary>
        /// Gets a value that indicates why a cache entry was removed.
        /// </summary>
        /// <value>
        /// One of the enumeration values that indicates why the entry was removed.
        /// </value>
        public CacheEntryRemovedReason RemovedReason { get; }

        /// <summary>
        /// Gets a reference to the source System.Runtime.Caching.ObjectCache instance that originally contained the removed cache entry.
        /// </summary>
        /// <value>
        /// A reference to the cache that originally contained the removed cache entry.
        /// </value>
        public ObjectCache Source { get; }

        /// <summary>
        /// Initializes a new instance of the System.Runtime.Caching.CacheEntryRemovedArguments class.
        /// </summary>
        /// <param name="source">The System.Runtime.Caching.ObjectCache instance from which cacheItem was removed.</param>
        /// <param name="reason">One of the enumeration values that indicate why cacheItem was removed.</param>
        /// <param name="cacheItem">An instance of the cached entry that was removed.</param>
        /// <exception cref="ArgumentNullException">source is null. -or-cacheItem is null.</exception>
        public CacheEntryRemovedArguments(ObjectCache source, CacheEntryRemovedReason reason, CacheItem cacheItem)
        {
            this.Source = source ?? throw new ArgumentNullException(nameof(source));
            this.CacheItem = cacheItem ?? throw new ArgumentNullException(nameof(cacheItem));
            this.RemovedReason = reason;
        }

    }

}
