using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.GraphQL {
    public class TopDocumentDto {
        public string DocumentId { get; set; }
        public string Title { get; set; }
        public int VoteCount { get; set; }
    }

}
