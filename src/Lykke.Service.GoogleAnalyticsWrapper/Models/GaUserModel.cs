using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.GoogleAnalyticsWrapper.Models
{
    public class GaUserModel
    {
        [Required]
        public string ClientId { get; set; }
        public string Cid { get; set; }
    }
}
