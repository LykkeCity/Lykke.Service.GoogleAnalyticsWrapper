namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser
{
    public interface IGaUser
    {
        string ClientId { get; }
        string TrackerUserId { get; }
        string Cid { get; }
    }
}
