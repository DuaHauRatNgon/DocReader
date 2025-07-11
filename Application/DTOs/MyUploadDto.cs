using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class MyUploadDto {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Field { get; set; } = "";
        public string Author { get; set; } = "";
        public DateTime UploadedAt { get; set; }
        public UploadStatus Status { get; set; }
        public string? RejectionReason { get; set; }

    }
}
