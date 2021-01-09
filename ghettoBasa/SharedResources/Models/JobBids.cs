using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.Models
{
    public class JobBids
    {
        public int Id { get; set; }
        public string JobRef { get; set; }
        public string BidderId { get; set; }
        public string Status { get; set; }
        public string BidderName { get; set; }
        public DateTime? BidDate { get; set; }
        public string BidderPhotoUrl { get; set; }
        public bool Deleted { get; set; }
    }
}
