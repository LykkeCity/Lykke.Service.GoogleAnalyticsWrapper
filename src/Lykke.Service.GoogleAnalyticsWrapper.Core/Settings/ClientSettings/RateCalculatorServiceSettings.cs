using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Settings.ClientSettings
{
    public class RateCalculatorServiceSettings
    {
        [HttpCheck("/api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
