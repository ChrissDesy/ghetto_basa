using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.Models
{
    class Ratings
    {
        public int Id { get; set; }
        public string UserRefer { get; set; }
        public int Rating { get; set; }
        public DateTime? DateRated { get; set; }
    }
}
