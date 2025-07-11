using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class DocumentDownloadDto {
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; } = "application/pdf";
    }

}
