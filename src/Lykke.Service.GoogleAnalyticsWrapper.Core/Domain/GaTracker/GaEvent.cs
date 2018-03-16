namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public class GaEvent : GaBaseHit
    {
        public string EventCategory { get; set; }
        public string EventAction { get; set; }
        public string EventValue { get; set; }

        public static GaEvent Create(TrackerInfo src, string category, string action, string value)
        {
            return new GaEvent
            {
                Type = GaHitType.GaEvent,
                EventCategory = category,
                EventAction = action,
                EventValue = value,
                UserId = src.UserAgent,
                UserAgent = src.UserAgent,
                Ip = src.Ip
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
                ua = UserAgent,
                av = AppVersion,
                an = AppName,
                sc = SessionControl
            };
        }
    }
}
