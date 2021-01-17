using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResources.DTOs
{
    public class Response
    {
        public int totalElements { get; set; }
        public IEnumerable<object> content { get; set; }
        public int totalPages { get; set; }
        public int currentPage { get; set; }
    }
}
