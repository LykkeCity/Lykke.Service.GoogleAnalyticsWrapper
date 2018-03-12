using System;
using System.Threading.Tasks;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Extensions;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Services;
using Lykke.Service.GoogleAnalyticsWrapper.Services.CachedModels;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;

namespace Lykke.Service.GoogleAnalyticsWrapper.Services
{
    public class GaUserService : IGaUserService
    {
        private readonly IDistributedCache _cache;
        private readonly IGaUserRepository _trackerUserRepository;
        private readonly IGaTrafficRepository _trafficRepository;

        public GaUserService(
            IDistributedCache cache,
            IGaUserRepository trackerUserRepository,
            IGaTrafficRepository trafficRepository
            )
        {
            _cache = cache;
            _trackerUserRepository = trackerUserRepository;
            _trafficRepository = trafficRepository;
        }
        
        public async Task<string> GetGaUserIdAsync(string clientId)
        {
            var cachedValue = await _cache.TryGetFromCacheAsync(
                GetUserIdKey(clientId),
                async () =>
                {
                    var cachedGaUserId = new CachedGaUserId();
                    var trackerUser = await _trackerUserRepository.GetGaUserAsync(clientId);

                    if (trackerUser == null)
                    {
                        cachedGaUserId.GaUserId = Guid.NewGuid().ToString();
                        await _trackerUserRepository.AddAsync(new GaUser{ClientId = clientId, TrackerUserId = cachedGaUserId.GaUserId});
                    }
                    else
                    {
                        cachedGaUserId.GaUserId = trackerUser.TrackerUserId;
                    }
                    
                    return cachedGaUserId;
                });

            return cachedValue?.GaUserId;
        }
        
        public async Task<GaTraffic> GetGaUserTrafficAsync(string clientId)
        {
            var cachedValue = await _cache.TryGetFromCacheAsync(
                GetTrafficKey(clientId), async () =>
                {
                    var traffic = await _trafficRepository.GetAsync(clientId);

                    return traffic == null
                        ? null
                        : new CachedGaTraffic(traffic);
                });

            return cachedValue == null 
                ? null
                : new GaTraffic
                {
                    ClientId = cachedValue.ClientId,
                    Source = cachedValue.Source,
                    Medium = cachedValue.Medium,
                    Campaign = cachedValue.Campaign,
                    Keyword = cachedValue.Keyword
                };
        }
        
        public async Task AddGaUserTrafficAsync(IGaTraffic traffic)
        {
            await _trafficRepository.AddAsync(traffic);
            var value = MessagePackSerializer.Serialize(new CachedGaTraffic(traffic));
            await _cache.SetAsync(GetTrafficKey(traffic.ClientId), value);
        }
        
        private static string GetUserIdKey(string clientId) => $"userId:{clientId}";
        private static string GetTrafficKey(string clientId) => $"traffic:{clientId}";
    }
}
