using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs {
    public class LatestCommentDto {
        public string UserName { get; set; }
        public string ContentPreview { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DocumentTitle { get; set; }
    }

}
