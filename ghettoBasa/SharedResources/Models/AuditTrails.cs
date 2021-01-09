using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.Models
{
    public class AuditTrails
    {
        public int Id { get; set; }
        public string UserRefere { get; set; }
        public string Username { get; set; }
        public string Action { get; set; }
        public DateTime? Date { get; set; }
        public string Service { get; set; }
        public string Description { get; set; }
    }
}
