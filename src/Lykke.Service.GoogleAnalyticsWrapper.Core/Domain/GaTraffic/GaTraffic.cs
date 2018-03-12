namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic
{
    public class GaTraffic : IGaTraffic
    {
        public string ClientId { get; set; }
        public string Source { get; set; }
        public string Medium { get; set; }
        public string Campaign { get; set; }
        public string Keyword { get; set; }
    }
}
