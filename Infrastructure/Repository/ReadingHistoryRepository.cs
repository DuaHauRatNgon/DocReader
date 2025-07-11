using Core.Models.Domain;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository {
    public class ReadingHistoryRepository : IReadingHistoryRepository {
        private readonly AppDbContext _context;

        public ReadingHistoryRepository(AppDbContext context) {
            _context = context;
        }




        public async Task<ReadingHistory?> GetAsync(Guid userId, Guid documentId) {
            return await _context.ReadingHistory.FirstOrDefaultAsync(r => r.UserId == userId && r.DocumentId == documentId);
        }




        public async Task<List<ReadingHistory>> GetByUserAsync(Guid userId) {
            var q = await (from r in _context.ReadingHistory
                           join d in _context.Documents on r.DocumentId equals d.Id
                           where r.UserId == userId
                           orderby r.LastReadTime descending
                           select new ReadingHistory {
                               DocumentId = d.Id,
                               LastReadPage = r.LastReadPage,
                               LastReadTime = r.LastReadTime,
                               TotalReads = r.TotalReads,
                               Document = r.Document,
                           }
                         ).ToListAsync();
            return q.ToList();
        }






        public async Task AddAsync(ReadingHistory history) {
            await _context.ReadingHistory.AddAsync(history);
        }





        public async Task UpdateAsync(ReadingHistory history) {
            _context.ReadingHistory.Update(history);
        }

    }
}
