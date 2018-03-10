namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public abstract class GaBaseHit
    {
        public int Version => 1;
        public string TrackingId { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        public string Ip { get; set; }
        
        public abstract object Transform();
    }
}
