using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Application.DTOs {
    public class UploadDocumentRequest {
        [Required]
        public IFormFile File { get; set; } = null!;
        public string Title { get; set; }
        public string Field { get; set; }
        public string Author { get; set; }
        public List<Guid> TagIds { get; set; } = new();
    }

}
