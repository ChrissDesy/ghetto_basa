using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.Models
{
    public class ChangePassword
    {
        public string Type { get; set; }
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
