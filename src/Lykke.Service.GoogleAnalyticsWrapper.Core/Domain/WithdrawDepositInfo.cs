namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain
{
    public class WithdrawDepositInfo : TrackerInfo
    {
        public double Amount { get; set; }
        public string AssetId { get; set; }
    }
}
