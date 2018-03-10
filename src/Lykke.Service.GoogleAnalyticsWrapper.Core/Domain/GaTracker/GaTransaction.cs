namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public class GaTransaction : GaBaseHit
    {
        public string Id { get; set; }
        public string AssetId { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
        
        public static GaTransaction Create(string trackingId, string id, string userId, double amount, double fee, string assetId)
        {
            return new GaTransaction
            {
                TrackingId = trackingId,
                Type = GaHitType.GaTransaction,
                UserId = userId,
                Id = id,
                Amount = amount,
                Fee = fee,
                AssetId = assetId
            };
        }

        public override object Transform()
        {
            return new
            {
                v = Version,
                tid = TrackingId,
                t = Type,
                uid = UserId,
                ti = Id,
                tr = Amount,
                tt = Fee,
                cu = AssetId
            };
        }
    }
}
