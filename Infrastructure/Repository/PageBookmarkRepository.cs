using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository {
    public class PageBookmarkRepository {
        private readonly AppDbContext _context;

        public PageBookmarkRepository(AppDbContext context) {
            _context = context;
        }





        public async Task AddAsync(PageBookmark bookmark) {
            _context.PageBookmarks.Add(bookmark);
            await _context.SaveChangesAsync();
        }




        public async Task RemoveAsync(string userId, Guid documentId, int pageNumber) {
            var item = await _context.PageBookmarks
                .FirstOrDefaultAsync(x => x.UserId == userId && x.DocumentId == documentId && x.PageNumber == pageNumber);
            if (item != null) {
                _context.PageBookmarks.Remove(item);
                await _context.SaveChangesAsync();
            }
        }





        public async Task<bool> ExistsAsync(string userId, Guid documentId, int pageNumber) {
            return await _context.PageBookmarks
                .AnyAsync(x => x.UserId == userId && x.DocumentId == documentId && x.PageNumber == pageNumber);
        }







        public async Task<IEnumerable<PageBookmark>> GetByUserAndDocumentAsync(string userId, Guid documentId) {
            //return await _context.PageBookmarks
            //    .Where(x => x.UserId == userId && x.DocumentId == documentId)
            //    .Select(x => new PageBookmarkDTO {
            //        Id = x.Id,
            //        DocumentId = x.DocumentId,
            //        PageNumber = x.PageNumber,
            //        CreatedAt = x.CreatedAt
            //    }).ToListAsync();
            var pbms = await (from p in _context.PageBookmarks
                              where p.UserId == userId && p.DocumentId == documentId
                              select p).ToListAsync();
            return pbms;
        }






        public async Task<int?> GetLatestPageNumberAsync(string userId, Guid documentId) {
            return await _context.PageBookmarks
                            .Where(x => x.UserId == userId && x.DocumentId == documentId)
                            .OrderByDescending(x => x.CreatedAt)
                                    .Select(x => (int?)x.PageNumber)
                                            .FirstOrDefaultAsync();
        }






        public async Task<IEnumerable<int>> GetAllPageNumbersAsync(string userId, Guid documentId) {
            var pages = await (from p in _context.PageBookmarks
                               where p.DocumentId == documentId && p.UserId == userId
                               orderby p.PageNumber ascending
                               select p.PageNumber).ToListAsync();
            return pages;
        }

    }
}
