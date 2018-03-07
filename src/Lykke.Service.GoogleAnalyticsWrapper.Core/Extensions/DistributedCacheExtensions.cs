using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Extensions.Caching.Distributed;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Extensions
{
    public static class DistributedCacheExtensions
    {
        private static readonly IReadOnlyDictionary<int, SemaphoreSlim> Locks;

        static DistributedCacheExtensions()
        {
            Locks = Enumerable
                .Range(0, 1000)
                .ToDictionary(
                    i => i,
                    i => new SemaphoreSlim(1, 1));

        }

        public static async Task<T> TryGetFromCacheAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> getRecordFunc, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
            where T : class
        {
            var semaphore = await GetLockAsync(key);

            try
            {
                var record = await cache.TryGetFromCacheAsync<T>(key);

                if (record != null) 
                    return record;
                
                record = await getRecordFunc();

                if (record != null)
                {
                    await cache.UpdateCacheAsync(key, record, absoluteExpiration, slidingExpiration);
                }

                return record;
            }
            finally
            {
                semaphore.Release();
            }
        }

        private static async Task<T> TryGetFromCacheAsync<T>(this IDistributedCache cache, string key)
            where T : class
        {
            var value = await cache.GetAsync(key);

            if (value != null)
            {
                using (var stream = new MemoryStream(value))
                {
                    return MessagePackSerializer.Deserialize<T>(stream, StandardResolverAllowPrivate.Instance);
                }
            }

            return default(T);
        }

        private static async Task UpdateCacheAsync<T>(this IDistributedCache cache, string key, T record, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
            where T : class
        {
            var value = MessagePackSerializer.Serialize(record);

            await cache.SetAsync(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration,
                SlidingExpiration = slidingExpiration
            });
        }

        private static async Task<SemaphoreSlim> GetLockAsync(string key)
        {
            var hash = key.GetHashCode();
            var number = Math.Abs(hash % Locks.Count);
            var semaphore = Locks[number];

            await semaphore.WaitAsync();

            return semaphore;
        }
    }
}
