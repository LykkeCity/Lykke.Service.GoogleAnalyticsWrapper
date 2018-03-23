namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic
{
    public interface IGaTraffic
    {
        string ClientId { get; }
        string Source { get; }
        string Medium { get; }
        string Campaign { get; }
        string Keyword { get; }
        string Content { get; }
    }
}
