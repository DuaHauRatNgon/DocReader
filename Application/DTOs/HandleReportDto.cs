using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class HandleReportDto {
        public Guid ReportId { get; set; }
        public string? AdminNote { get; set; }
    }
}
