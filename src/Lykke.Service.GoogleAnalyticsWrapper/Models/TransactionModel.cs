using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.GoogleAnalyticsWrapper.Models
{
    public class TransactionModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string AssetId { get; set; }
        [Required]
        public string UserId { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string ClientInfo { get; set; }
    }
}
