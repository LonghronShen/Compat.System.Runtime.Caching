using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Caching
{

    /// <summary>
    /// Represents an object cache and provides the base methods and properties for accessing the object cache.
    /// </summary>
    public abstract class ObjectCache
        : IEnumerable<KeyValuePair<string, object>>
    {

        private static IServiceProvider _host;

        /// <summary>
        /// Gets a value that indicates that a cache entry has no absolute expiration.
        /// </summary>
        /// <returns>
        /// A date-time value that is set to the maximum possible value.
        /// </returns>
        public static readonly DateTimeOffset InfiniteAbsoluteExpiration = DateTimeOffset.MaxValue;

        /// <summary>
        /// Indicates that a cache entry has no sliding expiration time.
        /// </summary>
        /// <returns>
        /// A time-duration value that is set to zero.
        /// </returns>
        public static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;

        /// <summary>
        /// Gets or set a reference to a managed hosting environment that is available to
        ///    System.Runtime.Caching.ObjectCache implementations and that can provide host-specific
        ///    services to System.Runtime.Caching.ObjectCache implementations.
        /// </summary>
        /// <returns>
        /// A reference to a cache-aware managed hosting environment.
        /// </returns>
        /// <exception cref="ArgumentNullException">The value being assigned to the property is null.</exception>
        /// <exception cref="InvalidOperationException">An attempt was made to set the property value more than one time.</exception>
        public static IServiceProvider Host
        {
            [PermissionSet(SecurityAction.Demand, Unrestricted = true)]
            [SecurityCritical]
            get { return _host; }

            [PermissionSet(SecurityAction.Demand, Unrestricted = true)]
            [SecurityCritical]
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (Interlocked.CompareExchange(ref _host, value, null) != null)
                {
                    throw new InvalidOperationException("Property already set.");
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a description of the features that a cache implementation provides.
        /// </summary>
        /// <returns>
        /// A bitwise combination of flags that indicate the default capabilities of a cache implementation.
        /// </returns>
        public abstract DefaultCacheCapabilities DefaultCacheCapabilities { get; }

        /// <summary>
        /// Gets the name of a specific System.Runtime.Caching.ObjectCache instance.
        /// </summary>
        /// <returns>
        /// The name of a specific cache instance.
        /// </returns>
        public abstract string Name { get; }

        /// <summary>
        /// Gets or sets the default indexer for the System.Runtime.Caching.ObjectCache class.
        /// </summary>
        /// <param name="key">A unique identifier for a cache entry in the cache.</param>
        /// <returns>A key that serves as an indexer into the cache instance.</returns>
        public abstract object this[string key] { get; set; }

        /// <summary>
        /// When overridden in a derived class, creates a System.Runtime.Caching.CacheEntryChangeMonitor object that can trigger events in response to changes to specified cache entries.
        /// </summary>
        /// <param name="keys">The unique identifiers for cache entries to monitor.</param>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <returns>A change monitor that monitors cache entries in the cache.</returns>
        public abstract CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, checks whether the cache entry already exists in the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache can be found, if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <returns>true if the cache contains a cache entry with the same key value as key; otherwise, false.</returns>
        public abstract bool Contains(string key, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache without overwriting any existing cache entry.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The fixed date and time at which the cache entry will expire.</param>
        /// <param name="regionName">
        /// Optional. A named region in the cache to which the cache entry can be added, if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <returns>
        /// true if the insertion try succeeds, or false if there is an already an entry in the cache with the same key as key.
        /// </returns>
        public virtual bool Add(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            return AddOrGetExisting(key, value, absoluteExpiration, regionName) == null;
        }

        /// <summary>
        /// When overridden in a derived class,
        /// tries to insert a cache entry into the cache as a System.Runtime.Caching.CacheItem instance,
        /// and adds details about how the entry should be evicted.
        /// </summary>
        /// <param name="item">The object to add.</param>
        /// <param name="policy">
        /// An object that contains eviction details for the cache entry.
        /// This object provides more options for eviction than a simple absolute expiration.</param>
        /// <returns>true if insertion succeeded, or false if there is an already an entry in the cache that has the same key as item.</returns>
        public virtual bool Add(CacheItem item, CacheItemPolicy policy)
        {
            return AddOrGetExisting(item, policy) == null;
        }

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, specifying information about how the entry will be evicted.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">
        /// An object that contains eviction details for the cache entry.
        /// This object provides more options for eviction than a simple absolute expiration.
        /// </param>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <returns>true if the insertion try succeeds, or false if there is an already an entry in the cache with the same key as key.</returns>
        public virtual bool Add(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            return AddOrGetExisting(key, value, policy, regionName) == null;
        }

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, by using a key, an object for the cache entry, an absolute expiration value, and an optional region to add the cache into.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The fixed date and time at which the cache entry will expire.</param>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <returns>If a cache entry with the same key exists, the specified cache entry's value; otherwise, null.</returns>
        public abstract object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, inserts the specified System.Runtime.Caching.CacheItem object into the cache, specifying information about how the entry will be evicted.
        /// </summary>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry. This object provides more options for eviction than a simple absolute expiration.</param>
        /// <returns>If a cache entry with the same key exists, the specified cache entry; otherwise, null.</returns>
        public abstract CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy);

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache,
        /// specifying a key and a value for the cache entry,
        /// and information about how the entry will be evicted.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry. This object provides more options for eviction than a simple absolute expiration.</param>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <returns>If a cache entry with the same key exists, the specified cache entry's value; otherwise, null.</returns>
        public abstract object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, gets the specified cache entry from the cache as an object.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <returns>The cache entry that is identified by key.</returns>
        public abstract object Get(string key, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, gets the specified cache entry from the cache as a System.Runtime.Caching.CacheItem instance.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <returns>The cache entry that is identified by key.</returns>
        public abstract CacheItem GetCacheItem(string key, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache, specifying time-based expiration details.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="absoluteExpiration">The fixed date and time at which the cache entry will expire.</param>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        public abstract void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);

        /// <summary>
        /// When overridden in a derived class,
        /// inserts the cache entry into the cache as a System.Runtime.Caching.CacheItem instance,
        /// specifying information about how the entry will be evicted.
        /// </summary>
        /// <param name="item">The cache item to add.</param>
        /// <param name="policy">
        /// An object that contains eviction details for the cache entry.
        /// This object provides more options for eviction than a simple absolute expiration.
        /// </param>
        public abstract void Set(CacheItem item, CacheItemPolicy policy);

        /// <summary>
        /// When overridden in a derived class, inserts a cache entry into the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="value">The object to insert.</param>
        /// <param name="policy">
        /// An object that contains eviction details for the cache entry.
        /// This object provides more options for eviction than a simple absolute expiration.
        /// </param>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        public abstract void Set(string key, object value, CacheItemPolicy policy, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, gets a set of cache entries that correspond to the specified keys.
        /// </summary>
        /// <param name="keys">A collection of unique identifiers for the cache entries to get.</param>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <returns>A dictionary of key/value pairs that represent cache entries.</returns>
        public abstract IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null);

        /// <summary>
        /// Gets a set of cache entries that correspond to the specified keys.
        /// </summary>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <param name="keys">A collection of unique identifiers for the cache entries to get.</param>
        /// <returns>A dictionary of key/value pairs that represent cache entries.</returns>
        public virtual IDictionary<string, object> GetValues(string regionName, params string[] keys)
        {
            return GetValues(keys, regionName);
        }

        /// <summary>
        /// When overridden in a derived class, removes the cache entry from the cache.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <returns>
        /// An object that represents the value of the removed cache entry that was specified by the key,
        /// or null if the specified entry was not found.
        /// </returns>
        public abstract object Remove(string key, string regionName = null);

        /// <summary>
        /// When overridden in a derived class, gets the total number of cache entries in the cache.
        /// </summary>
        /// <param name="regionName">
        /// Optional. A named region in the cache where the cache keys in the keys parameter exist,
        /// if regions are implemented.
        /// The default value for the optional parameter is null.
        /// </param>
        /// <returns>
        /// The number of cache entries in the cache.
        /// If regionName is not null, the count indicates the number of entries that are in the specified cache region.
        /// </returns>
        public abstract long GetCount(string regionName = null);

        #region IEnumerator
        /// <summary>
        /// When overridden in a derived class, creates an enumerator that can be used to iterate through a collection of cache entries.
        /// </summary>
        /// <returns>The enumerator object that provides access to the cache entries in the cache.</returns>
        protected abstract IEnumerator<KeyValuePair<string, object>> GetEnumerator();

        /// <summary>
        /// Supports iteration over a generic collection.
        /// </summary>
        /// <returns>The enumerator object that provides access to the items in the cache.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, object>>)this).GetEnumerator();
        }

        /// <summary>
        /// When overridden in a derived class, creates an enumerator that can be used to iterate through a collection of cache entries.
        /// </summary>
        /// <returns>The enumerator object that provides access to the cache entries in the cache.</returns>
        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

    }

}
