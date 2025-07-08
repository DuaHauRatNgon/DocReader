using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces {
    public interface IHighlightQuoteRepository {
        Task AddAsync(HighlightQuote quote);
        Task<IEnumerable<HighlightQuote>> GetByPageAsync(Guid documentPageId, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }

}
