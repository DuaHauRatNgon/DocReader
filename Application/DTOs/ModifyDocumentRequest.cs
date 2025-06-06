using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class ModifyDocumentRequest {
        public string Title { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}
