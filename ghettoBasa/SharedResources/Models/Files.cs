using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.Models
{
    public class Files
    {
        public IFormFile file { get; set; }
        // public string FileType { get; set; }
    }
}
