using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DocumentDTO {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Field { get; set; }
        public string Author { get; set; }
        public ushort PageCount { get; set; }
        public List<string> Tags { get; set; }
    }

}
