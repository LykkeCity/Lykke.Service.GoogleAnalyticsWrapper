using System.Net;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public class GaPageView : GaBaseEvent
    {
        public string PageName { get; set; }
        public string PageTitle { get; set; }

        public static GaPageView Create(EventInfo src, string trackingId, string category, string action, string sessionControl = "ignore")
        {
            return new GaPageView
            {
                TrackingId = trackingId,
                Type = "pageview",
                PageName = WebUtility.UrlEncode($"{category}/{action}"),
                PageTitle = action,
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
                dp = PageName,
                dt = PageTitle,
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
