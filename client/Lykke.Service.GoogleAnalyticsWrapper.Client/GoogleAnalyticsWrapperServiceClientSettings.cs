using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.GoogleAnalyticsWrapper.Client 
{
    public class GoogleAnalyticsWrapperServiceClientSettings 
    {
        [HttpCheck("/api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
