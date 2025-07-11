using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Domain {
    public class PendingDocument {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Field { get; set; }
        public string Author { get; set; }
        public DateTime UploadedAt { get; set; }

        public string PdfPath { get; set; }

        public string UploadedByUserId { get; set; }
        public AppUser UploadedByUser { get; set; }

        public UploadStatus Status { get; set; } = UploadStatus.Pending;
        public string? RejectReason { get; set; }

        public ICollection<PendingDocumentTag> Tags { get; set; } = new List<PendingDocumentTag>();
    }


    public enum UploadStatus { Pending, Approved, Rejected }
}
