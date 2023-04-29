namespace System.Runtime.Caching
{

    /// <summary>
    /// Represents an individual cache entry in the cache.
    /// </summary>
    public class CacheItem
    {

        /// <summary>
        /// Gets or sets a unique identifier for a System.Runtime.Caching.CacheItem instance.
        /// </summary>
        /// <value>
        /// The identifier for a System.Runtime.Caching.CacheItem instance.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the name of a region in the cache that contains a System.Runtime.Caching.CacheItem entry.
        /// </summary>
        /// <value>
        /// The name of a region in a cache. The default is null.
        /// </value>
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the data for a System.Runtime.Caching.CacheItem instance.
        /// </summary>
        /// <value>
        /// The data for a System.Runtime.Caching.CacheItem instance. The default is null.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Initializes a new System.Runtime.Caching.CacheItem instance.
        /// </summary>
        public CacheItem()
        {
        }

        /// <summary>
        /// Initializes a new System.Runtime.Caching.CacheItem instance using the specified key of a cache entry.
        /// </summary>
        /// <param name="key">A unique identifier for a System.Runtime.Caching.CacheItem entry.</param>
        public CacheItem(string key)
            : this(key, null)
        {
        }

        /// <summary>
        /// Initializes a new System.Runtime.Caching.CacheItem instance using the specified key of a cache entry and a value of the cache entry.
        /// </summary>
        /// <param name="key">A unique identifier for a System.Runtime.Caching.CacheItem entry.</param>
        /// <param name="value">The data for a System.Runtime.Caching.CacheItem entry.</param>
        public CacheItem(string key, object value)
            : this(key, value, null)
        {
        }

        /// <summary>
        /// Initializes a new System.Runtime.Caching.CacheItem instance using the specified key, value, and region of the cache entry.
        /// </summary>
        /// <param name="key">A unique identifier for a System.Runtime.Caching.CacheItem entry.</param>
        /// <param name="value">The data for a System.Runtime.Caching.CacheItem entry.</param>
        /// <param name="regionName">The name of a region in the cache that will contain the System.Runtime.Caching.CacheItem entry.</param>
        public CacheItem(string key, object value, string regionName)
        {
            this.Key = key;
            this.Value = value;
            this.RegionName = regionName;
        }

    }

}
