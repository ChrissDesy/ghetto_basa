using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string NationalId { get; set; }
        public string UserId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string Biography { get; set; }
        public string PhotoUrl { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ProofOfResidenceUrl { get; set; }
        public string NationalIdPhotoUrl { get; set; }
        public string Skills { get; set; }
        public string Status { get; set; }
        public bool Deleted { get; set; }
        public string UserType { get; set; }
    }
}
