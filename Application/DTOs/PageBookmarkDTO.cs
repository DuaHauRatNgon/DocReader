using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class PageBookmarkDTO {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public int PageNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
