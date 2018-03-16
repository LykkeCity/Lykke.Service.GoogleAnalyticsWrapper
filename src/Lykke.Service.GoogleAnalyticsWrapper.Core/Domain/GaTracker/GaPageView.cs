using System.Net;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public class GaPageView : GaBaseHit
    {
        public string PageName { get; set; }
        public string PageTitle { get; set; }

        public static GaPageView Create(GaEvent src, string sessionControl = "ignore")
        {
            return new GaPageView
            {
                TrackingId = src.TrackingId,
                Type = GaHitType.GaPageView,
                PageName = WebUtility.UrlEncode($"{src.EventCategory}/{src.EventAction}"),
                PageTitle = src.EventAction,
                UserId = src.UserId,
                UserAgent = src.UserAgent ?? string.Empty,
                ScreenResolution = src.ScreenResolution ?? string.Empty,
                SessionControl = sessionControl,
                Ip = src.Ip ?? string.Empty,
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
