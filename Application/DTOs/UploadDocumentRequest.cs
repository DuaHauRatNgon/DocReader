using Microsoft.AspNetCore.Http;


namespace Application.DTOs {
    public class UploadDocumentRequest {
        public string Title { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public IFormFile File { get; set; } = null!;
        public List<Guid> TagIds { get; set; }
    }

}
