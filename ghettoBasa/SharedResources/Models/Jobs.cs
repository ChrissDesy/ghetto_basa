using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.Models
{
    public class Jobs
    {
        public int Id { get; set; }
        public string JobId { get; set; }
        public string Description { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime? DatePosted { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public bool Negotiable { get; set; }
        public DateTime? ExpiringDate { get; set; }
        public string RequiredSkills { get; set; }
        public string Status { get; set; }
        public string SuccessfulBidder { get; set; }
        public string PosterId { get; set; }
        public string Renewed { get; set; }
        public bool Deleted { get; set; }
    }
}
