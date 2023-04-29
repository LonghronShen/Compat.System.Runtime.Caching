using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text;

namespace System.Runtime.Caching
{

    /// <summary>
    /// Defines a reference to a method that handles changes to monitored items.
    /// </summary>
    /// <param name="state">The state of the dependent object that was changed.</param>
    public delegate void OnChangedCallback(object state);

    /// <summary>
    /// Defines a reference to a method that is called after a cache entry is removed from the cache.
    /// </summary>
    /// <param name="arguments">The information about the cache entry that was removed from the cache.</param>
    public delegate void CacheEntryRemovedCallback(CacheEntryRemovedArguments arguments);

    /// <summary>
    /// Defines a reference to a method that is invoked when a cache entry is about to be removed from the cache.
    /// </summary>
    /// <param name="arguments">The information about the entry that is about to be removed from the cache.</param>
    public delegate void CacheEntryUpdateCallback(CacheEntryUpdateArguments arguments);

}
