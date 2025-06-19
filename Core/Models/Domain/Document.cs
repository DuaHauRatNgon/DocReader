using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {
    public class Document {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Field { get; set; }
        public string Author { get; set; }
        public string Sumary { get; set; }
        public ushort PageCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<DocumentPage> Pages { get; set; }
        public ICollection<DocumentTag> Tags { get; set; } = new List<DocumentTag>();
        public ICollection<DocumentVote> Votes { get; set; } = new List<DocumentVote>();
    }
}
