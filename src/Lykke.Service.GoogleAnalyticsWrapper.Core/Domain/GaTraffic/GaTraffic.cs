using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic
{
    public class GaTraffic : IGaTraffic
    {
        public string ClientId { get; set; }
        public string Source { get; set; }
        public string Medium { get; set; }
        public string Campaign { get; set; }
        public string Keyword { get; set; }
        public string Content { get; set; }

        public static GaTraffic CreateDefault(string clientId)
        {
            return new GaTraffic
            {
                ClientId = clientId,
                Source = GaParamValue.Direct,
                Medium = GaParamValue.None,
                Campaign = GaParamValue.None,
                Keyword = GaParamValue.None,
                Content = GaParamValue.None
            };
        }
    }
}
