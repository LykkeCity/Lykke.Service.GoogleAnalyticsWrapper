using JetBrains.Annotations;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic;
using MessagePack;

namespace Lykke.Service.GoogleAnalyticsWrapper.Services.CachedModels
{
    [MessagePackObject]
    public class CachedGaTraffic
    {
        [Key(0)]
        [UsedImplicitly]
        public string ClientId { get; set; }
        [Key(1)]
        [UsedImplicitly]
        public string Source { get; set; }
        [Key(2)]
        [UsedImplicitly]
        public string Medium { get; set; }
        [Key(3)]
        [UsedImplicitly]
        public string Campaign { get; set; }
        [Key(4)]
        [UsedImplicitly]
        public string Keyword { get; set; }

        [UsedImplicitly]
        public CachedGaTraffic()
        {
        }

        public CachedGaTraffic(IGaTraffic traffic)
        {
            ClientId = traffic.ClientId;
            Source = traffic.Source;
            Medium = traffic.Medium;
            Campaign = traffic.Campaign;
            Keyword = traffic.Keyword;
        }
    }
}
