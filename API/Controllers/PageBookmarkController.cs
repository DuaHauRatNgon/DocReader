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
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Guid documentId, [FromQuery] int pageNumber) {
            await _service.AddAsync(documentId, pageNumber);
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

        //[HttpGet("check")]
        //public async Task<IActionResult> Check(Guid documentId, int pageNumber) {
        //    var bookmarked = await _service.IsBookmarked(documentId, pageNumber);
        //    return Ok(new { bookmarked });
        //}







        //Khi mo document, call API GET /api/PageBookmark/{docId  }/latest, chuyển đến pageNumber tra ve
        [HttpGet("{documentId}/latest")]
        public async Task<IActionResult> GetLatest(Guid documentId) {
            var latestPage = await _service.GetLatestBookmarkedPageAsync(documentId);
            if (latestPage == null) return NotFound("khong tim thay bookmark nao ca !!!");
            return Ok(new { pageNumber = latestPage });
        }








        // Call GET /api/PageBookmark/{docId}/ list, highlight các page đã bookmark / hien thi ds lề  trai phai.

        [HttpGet("{documentId}/list")]
        public async Task<IActionResult> GetAll(Guid documentId) {
            var pages = await _service.GetAllBookmarkedPagesAsync(documentId);
            return Ok(pages);
        }




    }
}