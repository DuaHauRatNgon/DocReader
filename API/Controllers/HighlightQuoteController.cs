using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Authorize]
    [ApiController]
    [Route("api/highlights")]
    public class HighlightQuoteController : ControllerBase {
        private readonly HighlightQuoteService _service;

        public HighlightQuoteController(HighlightQuoteService service) {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] HighlightQuoteCreateDto dto) {
            await _service.AddAsync(dto);
            return Ok();
        }

        [HttpGet("{pageId}")]
        public async Task<IActionResult> GetByPage(Guid pageId) {
            var result = await _service.GetByPageAsync(pageId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

}
