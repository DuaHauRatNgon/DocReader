using Core.Models.Domain.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {
    public class DocumentTag {
        public Guid DocumentId { get; set; }
        public Document Document { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }


}
