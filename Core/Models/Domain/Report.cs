using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {

    public enum ReportTargetType {
        Document,
        Comment
    }



    public class Report {
        public Guid Id { get; set; } = Guid.NewGuid();


        public Guid ReporterId { get; set; }
        public AppUser Reporter { get; set; }


        public ReportTargetType TargetType { get; set; }
        public Guid TargetId { get; set; }



        public int? ReasonOptionId { get; set; }
        public ReportReasonOption? ReasonOption { get; set; }
        public string? CustomReason { get; set; }



        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;



        public bool IsHandled { get; set; } = false;
        public string? AdminNote { get; set; }
    }
}
