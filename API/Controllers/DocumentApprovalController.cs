using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [ApiController]
    [Route("/api/documents")]
    public class DocumentApprovalController : ControllerBase {
        private readonly ApprovalService _approvalService;
        private readonly PendingDocumentService _pendingDocumentService;
        private readonly NotificationApprovalSenderService _notificationApprovalSenderService;



        public DocumentApprovalController(ApprovalService approvalService, PendingDocumentService pendingDocumentService, NotificationApprovalSenderService notificationApprovalSenderService) {
            _approvalService = approvalService;
            _pendingDocumentService = pendingDocumentService;
            _notificationApprovalSenderService = notificationApprovalSenderService;
        }










        [HttpGet("pending")]
        [Authorize(Roles = "Admin,Mod")]
        public async Task<IActionResult> GetPendingDocuments() {
            var documents = await _pendingDocumentService.GetPendingDocumentsAsync();
            return Ok(documents);
        }







        //[HttpPost("approve/{id}")]
        //[Authorize(Roles = "Admin,Mod")]
        //public async Task<IActionResult> Approve(Guid id) {
        //    var updated = await _approvalService.MarkAsApprovedAsync(id);
        //    if (!updated) return NotFound();

        //    await _approvalService.ProcessApprovedDocumentAsync(id);

        //    return Ok("Đã duyệt tài liệu");
        //}






        [HttpPost("review/{id}")]
        [Authorize(Roles = "Admin,Mod")]
        public async Task<IActionResult> Review(Guid id, [FromBody] DocumentReviewRequest request) {
            var pending = await _approvalService.GetPendingDocumentAsync(id);

            if (!request.IsApproved && string.IsNullOrWhiteSpace(request.RejectReason))
                return BadRequest("đièn lý do từ chối ");

            if (request.IsApproved) {
                var approved = await _approvalService.MarkAsApprovedAsync(id);
                if (!approved) return NotFound();
                await _approvalService.ProcessApprovedDocumentAsync(id);

                await _notificationApprovalSenderService.NotifyUserAsync(pending.UploadedByUserId, $"Tài liệu \"{pending.Title}\" của bạn đã được duyệt rồi nhé!");

                return Ok("Đã duyệt tài liệu");
            }


            else {
                var rejected = await _approvalService.MarkAsRejectedAsync(id, request.RejectReason!);
                if (!rejected) return NotFound();

                await _notificationApprovalSenderService.NotifyUserAsync(pending.UploadedByUserId, $" Tài liệu \"{pending.Title}\" của bạn đã bị từ chối: {request.RejectReason}");
                return Ok("Tài liệu đã bị từ chối");
            }
        }



    }
}
