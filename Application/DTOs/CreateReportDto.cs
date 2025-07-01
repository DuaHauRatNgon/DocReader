using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class CreateReportDto {
        public Guid TargetId { get; set; }
        public ReportTargetType TargetType { get; set; }
        public int? ReasonOptionId { get; set; }
        public string? CustomReason { get; set; }
    }
}
