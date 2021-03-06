﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.Models
{
    public class Reviews
    {
        public int Id { get; set; }
        public string UserRef { get; set; }
        public string Review { get; set; }
        public DateTime? DateReviewed { get; set; }
        public bool Deleted { get; set; }
    }
}
