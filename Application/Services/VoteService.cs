using Application.DTOs;
using Core.Models.Domain;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class VoteService {
        private readonly DocumentVoteRepository _voteRepository;
        private readonly DocumentRepository _documentRepository;

        public VoteService(DocumentVoteRepository voteRepository, DocumentRepository documentRepository) {
            _voteRepository = voteRepository;
            _documentRepository = documentRepository;
        }




        public async Task<bool> VoteDocumentAsync(string userId, VoteDocumentDto voteDto) {
            if (await _documentRepository.GetByIdAsync(voteDto.DocumentId) == null) return false;

            var voted = await _voteRepository.GetUserVoteAsync(userId, voteDto.DocumentId);
            if (voted != null) {
                if (voted.IsUpvote == voteDto.IsUpvote)
                    return await _voteRepository.RemoveVoteAsync(voted);
                else {
                    voted.IsUpvote = voteDto.IsUpvote;
                    voted.UpdatedAt = DateTime.UtcNow;
                    return await _voteRepository.UpdateVoteAsync(voted);
                }
            }
            else {
                var newVote = new DocumentVote {
                    Id = Guid.NewGuid(),
                    DocumentId = voteDto.DocumentId,
                    UserId = userId,
                    IsUpvote = voteDto.IsUpvote,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _voteRepository.AddVoteAsync(newVote);
                return true;
            }
        }






        public async Task<DocumentVoteStatsDto> GetDocumentVoteStatsAsync(Guid documentId) {
            var docVotes = await _voteRepository.GetVoteStatsAsync(documentId);
            return new DocumentVoteStatsDto {
                DocumentId = documentId,
                UpvoteCount = docVotes.Count(v => v.IsUpvote),
                DownvoteCount = docVotes.Count(v => !v.IsUpvote),
                TotalVotes = docVotes.Count
            };
        }





        public async Task<bool> RemoveVoteAsync(string userId, Guid documentId) {
            var voted = await _voteRepository.GetUserVoteAsync(userId, documentId);
            if (voted == null) return false;
            var res = await _voteRepository.RemoveVoteAsync(voted);
            return res;
        }
    }

}
