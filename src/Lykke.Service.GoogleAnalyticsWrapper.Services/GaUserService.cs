using System;
using System.Threading.Tasks;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Extensions;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Services;
using Lykke.Service.GoogleAnalyticsWrapper.Services.CachedModels;
using Microsoft.Extensions.Caching.Distributed;

namespace Lykke.Service.GoogleAnalyticsWrapper.Services
{
    public class GaUserService : IGaUserService
    {
        private readonly IDistributedCache _cache;
        private readonly IGaUserRepository _trackerUserRepository;

        public GaUserService(
            IDistributedCache cache,
            IGaUserRepository trackerUserRepository
            )
        {
            _cache = cache;
            _trackerUserRepository = trackerUserRepository;
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
        
        private static string GetUserIdKey(string clientId) => $"userId:{clientId}";
    }
}
