using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository {
    public class ReportReasonOptionRepository {
        private readonly AppDbContext _context;


        public ReportReasonOptionRepository(AppDbContext context) {
            _context = context;
        }



        public async Task<List<ReportReasonOption>> GetAllAsync() {
            var r = await _context.ReportReasonOptions.ToListAsync();
            return r;

        }



        public async Task<ReportReasonOption?> GetByIdAsync(int id) {
            return await _context.ReportReasonOptions.FindAsync(id);
        }




        public async Task AddAsync(string name) {
            _context.ReportReasonOptions.Add(new ReportReasonOption { Name = name });
            await _context.SaveChangesAsync();
        }




        public async Task DeleteAsync(int id) {
            var reason = await _context.ReportReasonOptions.FindAsync(id);
            if (!(reason == null)) {
                _context.ReportReasonOptions.Remove(reason);
                await _context.SaveChangesAsync();
            }
        }

    }
}
