using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TopDocumentDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public List<string> Tags { get; set; }
        public int UpvoteCount { get; set; }
    }
}
