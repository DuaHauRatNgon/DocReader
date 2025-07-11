using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface {
    public interface IReadingHistoryRepository {
        Task<ReadingHistory?> GetAsync(Guid userId, Guid documentId);
        Task<List<ReadingHistory>> GetByUserAsync(Guid userId);
        Task AddAsync(ReadingHistory history);
        Task UpdateAsync(ReadingHistory history);
    }
}

