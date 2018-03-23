using JetBrains.Annotations;
using MessagePack;

namespace Lykke.Service.GoogleAnalyticsWrapper.Services.CachedModels
{
    [MessagePackObject]
    public class CachedGaUserId
    {
        [Key(0)]
        [UsedImplicitly]
        public string GaUserId { get; set; }

        [UsedImplicitly]
        public CachedGaUserId()
        {
        }

        public CachedGaUserId(string userId)
        {
            GaUserId = userId;
        }
    }
}
