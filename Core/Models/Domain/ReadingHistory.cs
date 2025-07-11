using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {
    public class ReadingHistory {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DocumentId { get; set; }
        public int LastReadPage { get; set; }
        public DateTime LastReadTime { get; set; }
        public int TotalReads { get; set; }

        public AppUser User { get; set; }
        public Document Document { get; set; }

    }
}
