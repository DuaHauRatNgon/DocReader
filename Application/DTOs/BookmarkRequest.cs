using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class BookmarkRequest {
        public Guid DocumentId { get; set; }
        public int PageNumber { get; set; }
    }

}
