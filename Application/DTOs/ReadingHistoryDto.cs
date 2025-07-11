using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class ReadingHistoryDto {
        public Guid DocumentId { get; set; }
        public string DocumentTitle { get; set; }
        public int LastReadPage { get; set; }
        public DateTime LastReadTime { get; set; }
        public int TotalReads { get; set; }
    }
}

