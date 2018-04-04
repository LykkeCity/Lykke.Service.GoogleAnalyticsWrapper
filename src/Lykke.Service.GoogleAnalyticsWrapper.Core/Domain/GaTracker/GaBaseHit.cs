namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public abstract class GaBaseHit
    {
        public int Version => 1;
        public string TrackingId { get; set; }
        public string Type { get; set; }
        public string Cid { get; set; }
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        public string ClientInfo { get; set; }
        public string ScreenResolution { get; set; }
        public string AppVersion { get; set; }
        public string AppName => "LykkeWallet";
        public string Ip { get; set; }
        
        public abstract object Transform();
    }
}
