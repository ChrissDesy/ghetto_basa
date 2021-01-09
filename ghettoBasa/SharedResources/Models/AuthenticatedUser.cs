using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.Models
{
    class AuthenticatedUser
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string UserId { get; set; }
        public string PhotoUrl { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserType { get; set; }
    }
}
