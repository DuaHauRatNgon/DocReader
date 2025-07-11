using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces {
    public interface IReadingHistoryService {

        Task<List<ReadingHistoryDto>> GetUserHistoryAsync(Guid userId);

        Task TrackReadingAsync(Guid userId, Guid documentId, int pageNumber);

    }
}
