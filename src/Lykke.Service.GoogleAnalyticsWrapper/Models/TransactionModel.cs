using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.GoogleAnalyticsWrapper.Models
{
    public class TransactionModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string AssetId { get; set; }
        [Required]
        public double Fee { get; set; }
        [Required]
        public string FeeAssetId { get; set; }
    }
}
