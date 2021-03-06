﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.GoogleAnalyticsWrapper.Models
{
    public class TrackEventModel
    {
        [Required]
        public string UserId { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string ClientInfo { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Cid { get; set; }
        public string Traffic { get; set; }
    }
}
