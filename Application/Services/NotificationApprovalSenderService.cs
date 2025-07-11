using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class NotificationApprovalSenderService {
        private readonly NotificationRepository _repo;
        public NotificationApprovalSenderService(NotificationRepository repo) {
            _repo = repo;
        }



        public async Task NotifyUserAsync(string title, string content) {
            await _repo.SendGlobalNotificationAsync(title, content);

        }


    }
}
