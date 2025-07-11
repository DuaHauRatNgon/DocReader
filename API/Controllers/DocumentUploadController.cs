using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers {
    [ApiController]
    [Route("api/documents")]
    public class DocumentUploadController : ControllerBase {
        private readonly IUploadService _uploadService;
        private readonly PendingDocumentService _pendingUploadService;

        public DocumentUploadController(IUploadService uploadService, PendingDocumentService pendingUploadService) {
            _uploadService = uploadService;
            _pendingUploadService = pendingUploadService;
        }




        [HttpPost("upload")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Upload([FromForm] UploadDocumentRequest request) {
            var id = await _uploadService.UploadAsync(request);
            return Ok(new { pendingId = id });
        }




        [HttpGet("my-uploads")]
        [Authorize] // Yêu cầu đăng nhập
        public async Task<IActionResult> GetMyUploads() {
            var result = await _pendingUploadService.GetMyUploadsAsync();
            return Ok(result);
        }

    }
}
