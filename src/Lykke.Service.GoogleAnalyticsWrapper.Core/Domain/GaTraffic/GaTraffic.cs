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

        private const string None = "(none)";
        private const string DirectSource = "(direct)";

        public static GaTraffic CreateDefault(string clientId)
        {
            return new GaTraffic
            {
                ClientId = clientId,
                Source = DirectSource,
                Medium = None,
                Campaign = None,
                Keyword = None,
                Content = None
            };
        }
    }
}
