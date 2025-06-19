using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {
    public class PageBookmark {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid DocumentId { get; set; }
        public int PageNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public AppUser User { get; set; }
        public Document Document { get; set; }
    }
}
