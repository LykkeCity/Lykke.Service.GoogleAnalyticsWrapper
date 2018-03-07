namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain
{
    public class EventInfo
    {
        public string ClientId { get; set; }
        public string UserAgent { get; set; }
        public string ClientInfo { get; set; }
        public string Ip { get; set; }
        public string ScreenResolution { get; set; }
        public string AppVersion { get; set; }
        public string EventCategory { get; set; }
        public string EventName { get; set; }
        public string EventValue { get; set; }

        public static EventInfo Create(TrackerInfo src)
        {
            return new EventInfo
            {
                ClientId = src.UserId,
                UserAgent = src.UserAgent,
                ClientInfo = src.ClientInfo,
                Ip = src.Ip
            };
        }
    }
}
