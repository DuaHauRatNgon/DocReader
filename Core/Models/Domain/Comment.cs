using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {
    public class Comment {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public Guid DocId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public int LikeCount { get; set; } = 0;

        public virtual ICollection<CommentLike> Likes { get; set; } = new List<CommentLike>();
    }
}
