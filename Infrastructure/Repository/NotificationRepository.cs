﻿using Core.Models.Domain;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Repository {
    public class NotificationRepository {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;


        public NotificationRepository(AppDbContext context,
            IHubContext<NotificationHub> hubContext) {
            _context = context;
            _hubContext = hubContext;
        }



        public async Task SendGlobalNotificationAsync(string title, string content) {
            var notification = new Notification {
                Id = Guid.NewGuid(),
                Title = title,
                Content = content
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
        }




        //public async Task NotifyUserAsync(string userId, string message) {
        //    // userId phải trùng với client-side connection UserIdentifier
        //    await _hub.Clients.User(userId).SendAsync("ReceiveNotification", message);
        //}




        public async Task SendNotificationToUserAsync(Guid userId, string message) {
            var notification = new Notification {
                UserId = userId.ToString(),
                Content = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // gui realtime neu user dang connect
            await _hubContext.Clients
                .User(userId.ToString())
                .SendAsync("ReceiveNotification", message);
        }




    }
}
