using Lykke.Service.GoogleAnalyticsWrapper.Core.Settings.ClientSettings;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Settings.ServiceSettings;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Settings.SlackNotifications;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Settings
{
    public class AppSettings
    {
        public GoogleAnalyticsWrapperSettings GoogleAnalyticsWrapperService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
        public RateCalculatorServiceSettings RateCalculatorServiceClient { get; set; }
    }
}
