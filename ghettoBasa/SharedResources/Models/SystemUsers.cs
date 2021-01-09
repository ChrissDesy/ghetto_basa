using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.Models
{
    class SystemUsers
    {
        public int Id { get; set; }
        public string UserType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string Hash { get; set; }
        public string UserRefe { get; set; }
        public string ResetRequest { get; set; }
        public bool Deleted { get; set; }
    }
}
