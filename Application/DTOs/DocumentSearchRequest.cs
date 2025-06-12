using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class DocumentSearchRequest {
        public string? SearchTerm { get; set; }
        public string? Author { get; set; }
        public string? Field { get; set; }
        public List<string>? Tags { get; set; }
        public string SortBy { get; set; } = "Title";
        public string SortDirection { get; set; } = "Asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
