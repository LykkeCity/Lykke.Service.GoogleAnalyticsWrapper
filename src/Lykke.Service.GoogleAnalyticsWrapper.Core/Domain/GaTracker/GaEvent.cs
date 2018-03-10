namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public class GaEvent : GaBaseEvent
    {
        public string EventCategory { get; set; }
        public string EventAction { get; set; }
        public string EventValue { get; set; }

        public static GaEvent Create(EventInfo src, string trackingId, string category, string action, string value = null, string sessionControl = "ignore")
        {
            return new GaEvent
            {
                TrackingId = trackingId,
                Type = GaHitType.GaEvent,
                EventCategory = category,
                EventAction = action,
                EventValue = value,
                UserId = src.ClientId,
                UserAgent = src.UserAgent ?? string.Empty,
                ScreenResolution = src.ScreenResolution ?? string.Empty,
                SessionControl = sessionControl,
                Ip = src.Ip ?? string.Empty,
                AppVersion = src.AppVersion
            };
        }

        public override object Transform()
        {
            return new
            {
                v = Version,
                tid = TrackingId,
                t = Type,
                ec = EventCategory,
                ea = EventAction,
                ev = EventValue,
                uid = UserId,
                sr = ScreenResolution,
                ip = Ip,
                av = AppVersion,
                an = AppName,
                sc = SessionControl
            };
        }
    }
}
