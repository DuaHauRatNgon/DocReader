using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class SimpleDocumentResponese {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Sumary { get; set; } = string.Empty;
        public ushort PageCount { get; set; }

        public List<TagResponse> Tags { get; set; } = new List<TagResponse>();
    }
}
