using Application.DTOs;
using Core.Interfaces;
using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class HighlightQuoteService {
        private readonly IHighlightQuoteRepository _repo;
        private readonly IUserContextService _userContext;

        public HighlightQuoteService(IHighlightQuoteRepository repo, IUserContextService userContext) {
            _repo = repo;
            _userContext = userContext;
        }

        public async Task AddAsync(HighlightQuoteCreateDto dto) {
            var entity = new HighlightQuote {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(_userContext.UserId),
                DocumentPageId = dto.DocumentPageId,
                Content = dto.Content,
                Top = dto.Top,
                Left = dto.Left,
                Width = dto.Width,
                Height = dto.Height,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(entity);
        }

        public async Task<IEnumerable<HighlightQuoteResponseDto>> GetByPageAsync(Guid pageId) {
            var result = await _repo.GetByPageAsync(pageId, Guid.Parse(_userContext.UserId));
            return result.Select(q => new HighlightQuoteResponseDto {
                Id = q.Id,
                Content = q.Content,
                Top = q.Top,
                Left = q.Left,
                Width = q.Width,
                Height = q.Height,
                CreatedAt = q.CreatedAt
            });
        }

        public async Task DeleteAsync(Guid id) {
            await _repo.DeleteAsync(id, Guid.Parse(_userContext.UserId));
        }

    }


}
