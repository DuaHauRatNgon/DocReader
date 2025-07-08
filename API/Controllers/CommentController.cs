using Application.DTOs;
using Application.DTOs.Comment;
using Core.Models.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase {
    private readonly CommentService _commentService;

    public CommentController(CommentService commentService) {
        _commentService = commentService;
    }





    [HttpGet("document/{docId}")]
    public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsByDocId(Guid docId) {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var comments = await _commentService.GetCommentsByDocIdAsync(docId, currentUserId ?? "");
        return Ok(comments);
    }





    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CommentDTO>> CreateComment([FromBody] CreateCommentDTO createCommentDTO) {

        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = User.FindFirst(ClaimTypes.Name)?.Value ??
                           User.FindFirst("name")?.Value ??
                           User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName)) {
            return Unauthorized(new { message = "Login moi duoc comment nha" });
        }

        var comment = await _commentService.CreateCommentAsync(createCommentDTO, userId, userName);
        return CreatedAtAction(nameof(GetCommentsByDocId), new { docId = comment.DocId }, comment);
    }








    [HttpPut("{commentId}")]
    [Authorize]
    public async Task<ActionResult<CommentDTO>> UpdateComment(Guid commentId, [FromBody] UpdateCommentDTO updateCommentDTO) {

        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) {
            return Unauthorized(new { message = "Login de sua bl nha" });
        }

        var updatedComment = await _commentService.UpdateCommentAsync(commentId, updateCommentDTO, userId);

        if (updatedComment == null) {
            return NotFound(new { message = "K tim thay bl or ban k co quyen sua" });
        }

        return Ok(updatedComment);
    }







    [HttpDelete("{commentId}")]
    [Authorize]
    public async Task<ActionResult> DeleteComment(Guid commentId) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "Login de xoa bl nha" });

        var result = await _commentService.DeleteCommentAsync(commentId, userId);

        if (!result) return NotFound(new { message = "asadaafasasdf" });

        return Ok(new { message = "Xoa bl oke" });
    }







    [HttpPost("{commentId}/like")]
    [Authorize]
    public async Task<ActionResult<CommentLikeDTO>> ToggleLike(Guid commentId) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(new { message = "Login truoc da" });

        var result = await _commentService.ToggleLikeAsync(commentId, userId);
        return Ok(result);
    }





    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestComments([FromQuery] int count = 6) {
        var comments = await _commentService.GetLatestCommentsAsync(count);
        return Ok(comments);
    }

}
