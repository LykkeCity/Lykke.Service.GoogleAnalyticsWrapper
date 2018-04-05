using System.Threading.Tasks;
using JetBrains.Annotations;
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

        [UsedImplicitly]
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
        
        public async Task<GaUser> GetGaUserAsync(string clientId, string cid = null)
        {
            var cachedValue = await _cache.TryGetOrAddAsync(
                GetUserInfoKey(clientId),
                async () => await GetCachedUserIdAsync(clientId, cid));

            return new GaUser
            {
                ClientId = clientId,
                TrackerUserId = cachedValue.GaUserId,
                Cid = cachedValue.GaCid
            };
        }

        public async Task AddGaUserAsync(string clientId, string cid)
        {
            var trackerUser = await _trackerUserRepository.GetGaUserAsync(clientId);

            var gaUser = new GaUser {ClientId = clientId};
            
            if (trackerUser == null)
            {
                gaUser.TrackerUserId = GaUser.GenerateNewUserId();
                gaUser.Cid = cid;
            }
            else
            {
                gaUser.TrackerUserId = trackerUser.TrackerUserId;
                
                if (string.IsNullOrEmpty(trackerUser.Cid))
                    gaUser.Cid = cid;
            }

            await _trackerUserRepository.AddAsync(gaUser);
            
            var value = MessagePackSerializer.Serialize(new CachedGaUserId(gaUser.TrackerUserId, gaUser.Cid));
            await _cache.SetAsync(GetUserInfoKey(clientId), value);
        }

        public async Task<GaTraffic> GetGaUserTrafficAsync(string clientId)
        {
            var cachedValue = await _cache.TryGetOrAddAsync(
                GetTrafficKey(clientId), async () => await GetTrafficAsync(clientId));

            return cachedValue == null 
                ? GaTraffic.CreateDefault(clientId)
                : new GaTraffic
                {
                    ClientId = cachedValue.ClientId,
                    Source = cachedValue.Source,
                    Medium = cachedValue.Medium,
                    Campaign = cachedValue.Campaign,
                    Keyword = cachedValue.Keyword,
                    Content = cachedValue.Content
                };
        }
        
        public async Task AddGaUserTrafficAsync(IGaTraffic traffic)
        {
            await _trafficRepository.AddAsync(traffic);
            var value = MessagePackSerializer.Serialize(new CachedGaTraffic(traffic));
            await _cache.SetAsync(GetTrafficKey(traffic.ClientId), value);
        }

        private async Task<CachedGaUserId> GetCachedUserIdAsync(string clientId, string cid = null)
        {
            var cachedGaUserId = new CachedGaUserId();
            var trackerUser = await _trackerUserRepository.GetGaUserAsync(clientId);

            if (trackerUser == null)
            {
                var gaUser = GaUser.CreateNew(clientId, cid);
                
                await _trackerUserRepository.AddAsync(gaUser);
                
                cachedGaUserId.GaUserId = gaUser.TrackerUserId;
                cachedGaUserId.GaCid = gaUser.Cid;
            }
            else
            {
                if (string.IsNullOrEmpty(trackerUser.Cid))
                {
                    cachedGaUserId.GaCid = string.IsNullOrEmpty(cid) ? GaUser.GenerateNewCid() : cid;
                    
                    await _trackerUserRepository.AddAsync(new GaUser{ClientId = trackerUser.ClientId, Cid = cachedGaUserId.GaCid, TrackerUserId = trackerUser.TrackerUserId});
                }
                
                cachedGaUserId.GaUserId = trackerUser.TrackerUserId;
            }
                    
            return cachedGaUserId;
        }

        private async Task<CachedGaTraffic> GetTrafficAsync(string clientId)
        {
            var traffic = await _trafficRepository.GetAsync(clientId);

            return traffic == null
                ? new CachedGaTraffic(GaTraffic.CreateDefault(clientId))
                : new CachedGaTraffic(traffic);
        }
        
        private static string GetUserInfoKey(string clientId) => $"userInfo:{clientId}";
        private static string GetTrafficKey(string clientId) => $"traffic:{clientId}";
    }
}
