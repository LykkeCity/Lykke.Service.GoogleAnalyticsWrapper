using System;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public class GaEvent : GaBaseHit
    {
        public string EventCategory { get; set; }
        public string EventAction { get; set; }
        public string EventValue { get; set; }
        public DateTime? CreatedAt { get; set; }

        public static GaEvent Create(TrackerInfo src, string category, string action, string value)
        {
            return new GaEvent
            {
                Type = GaHitType.GaEvent,
                EventCategory = category,
                EventAction = action,
                EventValue = value,
                UserId = src.UserId,
                UserAgent = src.UserAgent,
                ClientInfo = src.ClientInfo,
                Ip = src.Ip,
                CreatedAt = src.CreatedAt,
                Cid = src.Cid
            };
        }

        public override object Transform()
        {
            if (CreatedAt.HasValue && EventAction == TrackerEvents.UserRegistered)
            {
                var ms = (DateTime.UtcNow - CreatedAt.Value).Milliseconds;
                
                if (Traffic != null)
                {
                    return new
                    {
                        v = Version,
                        tid = TrackingId,
                        t = Type,
                        ec = EventCategory,
                        ea = EventAction,
                        ev = EventValue,
                        qt = ms,
                        cid = Cid,
                        uid = UserId,
                        cs = Traffic.Source,
                        cm = Traffic.Medium ,
                        cn = Traffic.Campaign,
                        ck = Traffic.Keyword,
                        sr = ScreenResolution,
                        uip = Ip,
                        ua = UserAgent,
                        av = AppVersion,
                        an = AppName
                    };
                }

                return new
                {
                    v = Version,
                    tid = TrackingId,
                    t = Type,
                    ec = EventCategory,
                    ea = EventAction,
                    ev = EventValue,
                    qt = ms,
                    cid = Cid,
                    uid = UserId,
                    sr = ScreenResolution,
                    uip = Ip,
                    ua = UserAgent,
                    av = AppVersion,
                    an = AppName
                };

            }

            return new
            {
                v = Version,
                tid = TrackingId,
                t = Type,
                ec = EventCategory,
                ea = EventAction,
                ev = EventValue,
                cid = Cid,
                uid = UserId,
                sr = ScreenResolution,
                uip = Ip,
                ua = UserAgent,
                av = AppVersion,
                an = AppName,
                cs = GaParamValue.Undefined,
                cm = GaParamValue.None,
                cn = GaParamValue.None,
                ck = GaParamValue.None
            };
        }
    }
}
