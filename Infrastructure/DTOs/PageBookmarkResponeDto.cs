using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs {
    public class PageBookmarkResponeDto {

        public Guid DocumentId { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }

        public int PageNumber { get; set; }

    }
}
