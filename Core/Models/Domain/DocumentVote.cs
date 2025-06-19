using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {
    public class DocumentVote {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public string UserId { get; set; }
        public bool IsUpvote { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Document Document { get; set; }
    }
}
