using Microsoft.AspNetCore.Mvc;

using Application.DTOs;
using Application.Services;

namespace DocReader.Controllers {

    [ApiController]
    [Route("/api/documents")]
    public class UploadController : ControllerBase {

        private readonly DocumentUploadService _uploadService;

        public UploadController(DocumentUploadService uploadService) {
            _uploadService = uploadService;

        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] UploadDocumentRequest request) {
            var docId = await _uploadService.UploadAsync(request);
            //return OkResult(docId);  
            // ok vs okresult 
            return Ok(new { documentId = docId });
        }




    }
}
