namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain
{
    public class TransactionInfo
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public double Amount { get; set; }
        public string AssetId { get; set; }
        public double Fee { get; set; }
        public string FeeAssetId { get; set; }
    }
}
