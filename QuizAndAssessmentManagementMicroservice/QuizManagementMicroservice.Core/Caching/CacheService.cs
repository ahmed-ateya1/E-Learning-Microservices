using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.Json;

namespace QuizManagementMicroservice.Core.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<CacheService> _logger;

        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();
        private static readonly ConcurrentDictionary<string, bool> _keyValuePairs = new();

        public CacheService(IDistributedCache distributedCache, ILogger<CacheService> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task<T> GetByAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
                return cachedValue == null ? null : JsonSerializer.Deserialize<T>(cachedValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cache for key {Key}", key);
                return null;
            }
        }

        public async Task<T> SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");

            try
            {
                var cacheValue = JsonSerializer.Serialize(value);


                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                };

                await _distributedCache.SetStringAsync(key, cacheValue, options, cancellationToken);
                _keyValuePairs.TryAdd(key, false);
                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache for key {Key}", key);
                throw;
            }
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await _distributedCache.RemoveAsync(key, cancellationToken);
                _keyValuePairs.TryRemove(key, out _);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache for key {Key}", key);
            }
        }

        public async Task RemoveByPrefix(string prefix, CancellationToken cancellationToken = default)
        {
            var keys = _keyValuePairs.Keys.Where(k => k.StartsWith(prefix));
            foreach (var key in keys)
            {
                await RemoveAsync(key, cancellationToken);
            }
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
        {
            if (!_locks.TryGetValue(key, out var semaphore))
            {
                semaphore = new SemaphoreSlim(1, 1);
                _locks[key] = semaphore;
            }

            await semaphore.WaitAsync(cancellationToken);
            try
            {
                var cacheValue = await GetByAsync<T>(key, cancellationToken);
                if (cacheValue != null)
                {
                    return cacheValue;
                }

                cacheValue = await factory();
                await SetAsync(key, cacheValue, cancellationToken);
                return cacheValue;
            }
            finally
            {
                semaphore.Release();
                _locks.TryRemove(key, out _);
            }
        }
    }
}
