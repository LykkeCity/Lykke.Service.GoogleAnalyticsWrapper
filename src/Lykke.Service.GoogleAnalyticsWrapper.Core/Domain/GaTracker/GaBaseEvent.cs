namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public abstract class GaBaseEvent : GaBaseHit
    {
        public string ScreenResolution { get; set; }
        public string AppVersion { get; set; }
        public string AppName => "LykkeWallet";
        public string SessionControl { get; set; }
    }
}
