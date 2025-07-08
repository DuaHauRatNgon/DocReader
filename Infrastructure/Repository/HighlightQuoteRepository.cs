using Core.Interfaces;
using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository {
    public class HighlightQuoteRepository : IHighlightQuoteRepository {
        private readonly AppDbContext _context;

        public HighlightQuoteRepository(AppDbContext context) {
            _context = context;
        }

        public async Task AddAsync(HighlightQuote quote) {
            await _context.HighlightQuotes.AddAsync(quote);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HighlightQuote>> GetByPageAsync(Guid pageId, Guid userId) {
            return await _context.HighlightQuotes
                .Where(q => q.DocumentPageId == pageId && q.UserId == userId)
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id, Guid userId) {
            var quote = await _context.HighlightQuotes.FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);
            if (quote != null) {
                _context.HighlightQuotes.Remove(quote);
                await _context.SaveChangesAsync();
            }
        }
    }


}
