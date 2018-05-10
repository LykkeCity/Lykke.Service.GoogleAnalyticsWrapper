using System.Net;
using Common;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public class GaPageView : GaBaseHit
    {
        public string PageName { get; set; }
        public string PageTitle { get; set; }

        public static GaPageView Create(GaEvent src)
        {
            return new GaPageView
            {
                TrackingId = src.TrackingId,
                Type = GaHitType.GaPageView,
                PageName = WebUtility.UrlEncode($"{src.EventCategory}/{src.EventAction}"),
                PageTitle = src.EventAction,
                UserId = src.UserId,
                Cid = src.Cid,
                UserAgent = src.UserAgent ?? string.Empty,
                ScreenResolution = src.ScreenResolution ?? string.Empty,
                Ip = src.Ip.SanitizeIp() ?? string.Empty,
                AppVersion = src.AppVersion,
                ClientInfo = src.ClientInfo
            };
        }

        public override object Transform()
        {
            return new
            {
                v = Version,
                tid = TrackingId,
                t = Type,
                dp = PageName,
                dt = PageTitle,
                cid = Cid,
                uid = UserId,
                sr = ScreenResolution,
                uip = Ip,
                ua = UserAgent,
                av = AppVersion,
                an = AppName
            };
        }
    }
}
