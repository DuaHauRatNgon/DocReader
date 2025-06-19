using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository {
    public class DocumentVoteRepository {

        private readonly AppDbContext _context;

        public DocumentVoteRepository(AppDbContext appDbContext) {
            _context = appDbContext;
        }


        public async Task<DocumentVote> GetUserVoteAsync(string userId, Guid documentId) {
            var q = await _context.DocumentVotes.FirstOrDefaultAsync(v => v.UserId == userId && v.DocumentId == documentId);
            return q;
        }

        public async Task<DocumentVote> AddVoteAsync(DocumentVote vote) {
            _context.DocumentVotes.Add(vote);
            await _context.SaveChangesAsync();
            return vote;
        }

        public async Task<bool> UpdateVoteAsync(DocumentVote vote) {
            _context.DocumentVotes.Update(vote);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveVoteAsync(DocumentVote vote) {
            _context.DocumentVotes.Remove(vote);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<DocumentVote>> GetVoteStatsAsync(Guid documentId) {
            var votes = await _context.DocumentVotes
                .Where(v => v.DocumentId == documentId)
                .ToListAsync();

            //return new DocumentVoteStatsDto {
            //    DocumentId = documentId,
            //    UpvoteCount = votes.Count(v => v.IsUpvote),
            //    DownvoteCount = votes.Count(v => !v.IsUpvote),
            //    TotalVotes = votes.Count
            //};
            return votes;
        }

    }
}
