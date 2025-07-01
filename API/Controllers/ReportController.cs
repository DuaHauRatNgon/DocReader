using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Infrastructure;
using System.Security.Claims;

namespace API.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase {
        private readonly ReportService _service;


        public ReportController(ReportService service) {
            _service = service;
        }




        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Report([FromBody] CreateReportDto dto) {
            //var userId = User.GetUserId();
            //var userId = User.Identity.Name;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _service.ReportTargetAsync(_2Guid.ToGuid(userId), dto.TargetId, dto.TargetType, dto.CustomReason);
            return Ok();
        }





        [HttpGet]
        //[Authorize(Roles = "Admin,Mod")]
        public async Task<IActionResult> GetUnresolvedReports() {
            var reports = await _service.GetPendingReportsAsync();
            return Ok(reports);
        }






        [HttpPost("handle")]
        //[Authorize(Roles = "Admin,Mod")]
        public async Task<IActionResult> Handle([FromBody] HandleReportDto dto) {
            await _service.HandleReportAsync(dto.ReportId, dto.AdminNote);
            return Ok();
        }



    }


}

