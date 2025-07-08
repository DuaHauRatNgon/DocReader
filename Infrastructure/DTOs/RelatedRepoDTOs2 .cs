using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs {
    public class RelatedRepoDTOs2 {
        public Guid documentId { get; set; }
        public IEnumerable<Tag> tag { get; set; }
        public Document doc { get; set; }

    }
}
