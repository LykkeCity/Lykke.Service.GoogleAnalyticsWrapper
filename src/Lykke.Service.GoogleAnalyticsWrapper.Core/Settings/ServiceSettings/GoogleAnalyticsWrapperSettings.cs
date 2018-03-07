namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Settings.ServiceSettings
{
    public class GoogleAnalyticsWrapperSettings
    {
        public bool IsLive { get; set; }
        public DbSettings Db { get; set; }
        public CacheSettings CacheSettings { get; set; }
        public GaSettings GaSettings { get; set; }
        public TrackAssetsSttings TrackAssets { get; set; }
    }
}
