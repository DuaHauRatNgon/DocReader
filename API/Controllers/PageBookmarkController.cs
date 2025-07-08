using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PageBookmarkController : ControllerBase {
        private readonly PageBookmarkService _service;


        public PageBookmarkController(PageBookmarkService service) {
            _service = service;
        }


        // 
        //[HttpPost]
        //public async Task<IActionResult> Add([FromBody] Guid documentId, [FromQuery] int pageNumber) {
        //    await _service.AddAsync(documentId, pageNumber);
        //    return Ok();
        //}

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookmarkRequest request) {
            await _service.AddAsync(request.DocumentId, request.PageNumber);
            return Ok();
        }











        [HttpDelete]
        public async Task<IActionResult> Remove([FromQuery] Guid documentId, [FromQuery] int pageNumber) {
            await _service.RemoveAsync(documentId, pageNumber);
            return Ok();
        }

        [HttpGet("{documentId}")]
        public async Task<IActionResult> GetBookmarks(Guid documentId) {
            var result = await _service.GetByDocumentAsync(documentId);
            return Ok(result);
        }








        [HttpGet("{documentId}/latest")]
        public async Task<IActionResult> GetLatest(Guid documentId) {
            var latestPage = await _service.GetLatestBookmarkedPageAsync(documentId);
            if (latestPage == null) return NotFound("khong tim thay bookmark nao ca !!!");
            return Ok(new { pageNumber = latestPage });
        }









        [HttpGet("{documentId}/list")]
        public async Task<IActionResult> GetAllById(Guid documentId) {
            var pages = await _service.GetAllBookmarkedPagesAsync(documentId);
            return Ok(pages);
        }




        [HttpGet("all")]
        public async Task<IActionResult> GetAll() {
            var bms = await _service.GetAllAsyncService();
            return Ok(bms);
        }

    }
}