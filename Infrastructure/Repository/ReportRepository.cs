using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository {
    public class ReportRepository {
        private readonly AppDbContext _context;



        public ReportRepository(AppDbContext context) {
            _context = context;
        }





        public async Task AddReportAsync(Report report) {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
        }






        public async Task<IEnumerable<Report>> GetAllUnresolvedReportsAsync() {
            var q = await _context.Reports
                .Where(r => !r.IsHandled)
                .Include(r => r.Reporter)
                .ToListAsync();
            return q;
        }






        public async Task<Report?> GetReportByIdAsync(Guid id) => await _context.Reports.FindAsync(id);





        public async Task HandleReportAsync(Guid id, string? adminNote) {
            var report = await _context.Reports.FindAsync(id);
            if (report == null) return;
            report.IsHandled = true;
            report.AdminNote = adminNote;
            await _context.SaveChangesAsync();
        }








    }
}
