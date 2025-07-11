using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {
    public class PendingDocumentTag {
        public Guid PendingDocumentId { get; set; }
        public PendingDocument PendingDocument { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }

}
