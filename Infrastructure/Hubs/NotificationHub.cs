using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs
{
    public class NotificationHub : Hub {
        public override async Task OnConnectedAsync() {
            Console.WriteLine($"client da ket noi : {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Client ngat ket noi: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
