namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain
{
    public class TransactionInfo : TrackerInfo
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public string AssetId { get; set; }
        public string Name { get; set; }
    }
}
