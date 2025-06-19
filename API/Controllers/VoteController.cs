using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers {


    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase {
        private readonly VoteService _voteService;

        public VoteController(VoteService voteService) {
            _voteService = voteService;
        }




        [HttpPost("document")]
        public async Task<IActionResult> VoteDocument([FromBody] VoteDocumentDto voteDto) {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var userName = User.FindFirst(ClaimTypes.Name)?.Value ??
            //                   User.FindFirst("name")?.Value ??
            //                   User.FindFirst(ClaimTypes.Email)?.Value;

            var userId = User.Identity.Name;
            var res = await _voteService.VoteDocumentAsync(userId, voteDto);

            if (res) return Ok(new { message = "Vote oke" });
            return BadRequest(new { message = "Vote loi" });
        }






        [HttpGet("document/{documentId}/stats")]
        public async Task<IActionResult> GetDocumentVoteStats(Guid documentId) {
            var stats = await _voteService.GetDocumentVoteStatsAsync(documentId);
            return Ok(stats);
        }






        [HttpDelete("document/{documentId}")]
        public async Task<IActionResult> RemoveVote(Guid documentId) {
            var userId = User.Identity.Name;
            var result = await _voteService.RemoveVoteAsync(userId, documentId);

            if (result)
                return Ok(new { message = "Xoa vote oke" });

            return BadRequest(new { message = "Xoa vote failed" });
        }
    }
}
