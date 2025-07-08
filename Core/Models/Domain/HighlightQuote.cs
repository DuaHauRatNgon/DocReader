using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {
    public class HighlightQuote {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DocumentPageId { get; set; }


        public string Content { get; set; }


        public float Top { get; set; }
        public float Left { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
