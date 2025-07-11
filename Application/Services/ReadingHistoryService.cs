using Application.DTOs;
using Application.Interfaces;
using Core.Interfaces;
using Core.Models.Domain;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class ReadingHistoryService : IReadingHistoryService {
        private readonly IReadingHistoryRepository _repo;

        private readonly IUnitOfWork _unitOfWork;



        public ReadingHistoryService(IReadingHistoryRepository repo, IUnitOfWork unitOfWork) {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }


        public async Task<List<ReadingHistoryDto>> GetUserHistoryAsync(Guid userId) {
            var res = await _repo.GetByUserAsync(userId);
            var q = (from h in res
                     select new ReadingHistoryDto {
                         DocumentId = h.DocumentId,
                         DocumentTitle = h.Document.Title,
                         LastReadPage = h.LastReadPage,
                         LastReadTime = h.LastReadTime,
                         TotalReads = h.TotalReads
                     }).ToList();
            return q;
        }


        public async Task TrackReadingAsync(Guid userId, Guid documentId, int pageNumber) {
            var history = await _repo.GetAsync(userId, documentId);
            if (history == null) {
                history = new ReadingHistory {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DocumentId = documentId,
                    LastReadPage = pageNumber,
                    LastReadTime = DateTime.UtcNow,
                    TotalReads = 1
                };
                await _repo.AddAsync(history);
            }
            else {
                history.LastReadPage = pageNumber;
                history.LastReadTime = DateTime.UtcNow;
                history.TotalReads++;
                await _repo.UpdateAsync(history);
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
