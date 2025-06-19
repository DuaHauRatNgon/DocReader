using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class DocumentVoteStatsDto {
        public Guid DocumentId { get; set; }
        public int UpvoteCount { get; set; }
        public int DownvoteCount { get; set; }
        public int TotalVotes { get; set; }
    }
}
