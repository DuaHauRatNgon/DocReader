using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ReportReasonController : ControllerBase {
        private readonly ReportReasonOptionService _service;

        public ReportReasonController(ReportReasonOptionService service) {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var reasons = await _service.GetAllAsync();
            return Ok(reasons);
        }




        [HttpPost]
        //[Authorize(Roles = "Admin,Mod")]
        public async Task<IActionResult> Add([FromBody] CreateReasonOptionDto dto) {
            await _service.AddAsync(dto.Name);
            return Ok();
        }




        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin,Mod")]
        public async Task<IActionResult> Delete(int id) {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
