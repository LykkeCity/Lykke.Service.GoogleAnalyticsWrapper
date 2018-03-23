namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain
{
    public class DeviceInfo
    {
        public string DeviceType { get; set; }
        public string DeviceModel { get; set; }
        public string Os { get; set; }
        public string OsVersion { get; set; }
        public string AppVersion { get; set; }
        public string ScreenResolution { get; set; }
        public string RawUserAgent { get; set; }
    }
}
