using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {
    public class CommentLike {
        public Guid Id { get; set; }

        [Required]
        public Guid CommentId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;


        // ref navigation property
        public virtual Comment Comment { get; set; } = null!;
    }
}
