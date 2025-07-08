using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Domain;
using Infrastructure.Repository;

namespace Application.Services {
    public class ReportService {
        private readonly ReportRepository _repo;
        private readonly NotificationRepository _notificationRepository;

        public ReportService(ReportRepository repo) {
            _repo = repo;
        }




        public async Task ReportTargetAsync(Guid reporterId, Guid targetId, ReportTargetType type, string reason) {
            var report = new Report {
                ReporterId = reporterId,
                TargetId = targetId,
                TargetType = type,
                CustomReason = reason
            };

            await _repo.AddReportAsync(report);
        }




        public async Task<IEnumerable<Report>> GetPendingReportsAsync() {
            return await _repo.GetAllUnresolvedReportsAsync();
        }




        public async Task HandleReportAsync(Guid reportId, string? note) {
            var report = await _repo.GetReportByIdAsync(reportId);
            await _repo.HandleReportAsync(reportId, note);
            var message = $"Báo cáo của bạn về {report.TargetType} đã được xử lý.";
            await _notificationRepository.SendNotificationToUserAsync(report.ReporterId, message);
        }




    }
}
