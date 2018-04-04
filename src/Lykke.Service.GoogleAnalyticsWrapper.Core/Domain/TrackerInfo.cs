using System;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain
{
    public class TrackerInfo
    {
        public string UserId { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string ClientInfo { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
