using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class PendingDocumentDto {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public string UploadedByUserId { get; set; } = string.Empty;
        public List<string> TagNames { get; set; } = new();
        public UploadStatus Status { get; set; }
    }
}

