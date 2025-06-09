using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Models.Domain;

namespace Application.DTOs {
    public class DocumentPageResponse {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public int PageNumber { get; set; }
        //public Document Document { get; set; }
        public string ContentBase64Encoded { get; set; }
    }
}
