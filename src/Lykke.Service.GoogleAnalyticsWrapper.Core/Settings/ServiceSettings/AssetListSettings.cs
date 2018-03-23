using System.Collections.Generic;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Settings.ServiceSettings
{
    public class AssetListSettings
    {
        public IReadOnlyCollection<string> Assets { get; set; }
        public BaseAssetSettings BaseAsset { get; set; }
    }
}
