using Core.Models.Domain;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository {
    public class PendingDocumentRepository : IPendingDocumentRepository {
        private readonly AppDbContext _context;

        public PendingDocumentRepository(AppDbContext context) {
            _context = context;
        }

        public async Task AddAsync(PendingDocument doc) =>
            await _context.PendingDocuments.AddAsync(doc);

        public async Task<PendingDocument?> GetByIdAsync(Guid id) =>
            await _context.PendingDocuments.Include(p => p.UploadedByUser).FirstOrDefaultAsync(p => p.Id == id);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();





        public async Task<List<Guid>> GetValidTagIdsAsync(List<Guid> inputTagIds) {
            return await _context.Tags
                .Where(t => inputTagIds.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync();
        }





        public async Task<List<PendingDocument>> GetAllPendingAsync() {
            return await _context.PendingDocuments
                .Include(p => p.Tags).ThenInclude(pt => pt.Tag)
                .Where(p => p.Status == UploadStatus.Pending)
                .ToListAsync();
        }







        public async Task<List<PendingDocument>> GetByUploaderAsync(string userId) {
            var q = await (from pd in _context.PendingDocuments
                           where pd.UploadedByUserId == userId
                           orderby pd.UploadedAt
                           select pd)
                .ToListAsync();
            return q;
        }
    }
}

