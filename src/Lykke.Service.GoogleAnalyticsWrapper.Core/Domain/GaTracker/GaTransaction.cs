namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public class GaTransaction : GaBaseHit
    {
        public string Id { get; set; }
        public string AssetId { get; set; }
        public double Amount { get; set; }
        public string Name { get; set; }
        
        public static GaTransaction Create(TransactionInfo transaction, double amount, string assetId)
        {
            return new GaTransaction
            {
                Type = GaHitType.GaTransaction,
                UserId = transaction.UserId,
                Id = transaction.Id,
                Amount = amount,
                AssetId = assetId,
                Name = transaction.Name,
                UserAgent = transaction.UserAgent,
                ClientInfo = transaction.ClientInfo,
                Ip = transaction.Ip
            };
        }

        public override object Transform()
        {
            return new
            {
                v = Version,
                tid = TrackingId,
                t = Type,
                cid = Cid,
                uid = UserId,
                ti = Id,
                ta = GaParamValue.AffiliateLykke,
                tr = Amount,
                ts = 0,
                tt = 0,
                cu = AssetId,
                cs = Traffic?.Source,
                cm = Traffic?.Medium,
                cn = Traffic?.Campaign,
                ck = Traffic?.Keyword,
                uip = Ip,
                ua = UserAgent
            };
        }
    }
}
