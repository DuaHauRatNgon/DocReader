using Application.DTOs;
using Application.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReadingHistoryController : ControllerBase {
        private readonly IReadingHistoryService _service;

        public ReadingHistoryController(IReadingHistoryService service) {
            _service = service;
        }

        [HttpPost("track")]
        public async Task<IActionResult> Track([FromBody] TrackReadingRequest request) {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _service.TrackReadingAsync(_2Guid.ToGuid(userId),
                request.DocumentId,
                request.PageNumber);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<ReadingHistoryDto>>> Get() {
            //var userId = User.GetUserId();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _service.GetUserHistoryAsync(_2Guid.ToGuid(userId));
            return Ok(result);
        }
    }

    public class TrackReadingRequest {
        public Guid DocumentId { get; set; }
        public int PageNumber { get; set; }
    }

}
