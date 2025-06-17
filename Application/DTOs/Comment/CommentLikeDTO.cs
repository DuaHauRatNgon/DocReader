using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Comment {
    public class CommentLikeDTO {
        public Guid CommentId { get; set; }
        public bool IsLiked { get; set; }
    }
}
