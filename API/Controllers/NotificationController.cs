using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase  {
        private readonly SendNotificationAllUserService _notificationService;


        public NotificationController(SendNotificationAllUserService service) {
            _notificationService = service;     
        }





        //[HttpPost("broadcast")]
        //public async Task<IActionResult> Broadcast([FromBody] string message) {
        //    await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        //    return Ok(new { Message = "Thong bao gui toi toan bo user" });
        //}







        [HttpPost("broadcast")]
        public async Task<IActionResult> Broadcast([FromBody] NotificationDto dto)
        {
            await _notificationService.SendNoti(dto.Title, dto.Content);
            return Ok();
        }

    }
}





/*

      ┌────────────┐             HTTP              ┌──────────────────────┐
      │ Admin User │ ────────────────────────────▶ │ NotificationController│
      └────────────┘     POST /api/notification    └─────────┬────────────┘
                                                              │
                                                              ▼
                                                    ┌────────────────────┐
                                                    │  NotificationHub   │◀────────┐
                                                    └─────────┬──────────┘         │
                                                              │                    │
                                              SignalR         │     Kết nối WebSocket/SSE/LongPolling
                                              Broadcast        ▼                    ▼
                                                     ┌─────────────────┐    ┌─────────────────┐
                                                     │  Client/User A  │    │  Client/User B  │
                                                     └─────────────────┘    └─────────────────┘


*/