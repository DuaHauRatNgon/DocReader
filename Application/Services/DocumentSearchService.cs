using Application.DTOs;
using Core.Interfaces;
using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class DocumentSearchService {


        private readonly IDocumentRepository _documentRepository;
        private readonly string baseUrl;





        public DocumentSearchService(IDocumentRepository documentRepository, IConfiguration configuration) {
            _documentRepository = documentRepository;
            baseUrl = configuration["BaseUrl"] ?? "https://localhost:5225";
        }




        public async Task<PaginatedResponse<SimpleDocumentResponese>> SearchDocumentsAsync(DocumentSearchRequest request) {
            var documentsIqueryable = _documentRepository.GetQueryable();

            documentsIqueryable = ApplyFilters(documentsIqueryable, request);



            //documentsIqueryable = ApplySorting(documentsIqueryable, request.SortBy, request.SortDirection);


            var docs = await documentsIqueryable
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            // map to response
            var docResList = docs.Select(q => new SimpleDocumentResponese {
                Id = q.Id,
                Author = q.Author,
                Field = q.Field,
                Title = q.Title,
                Sumary = q.Sumary,
                PageCount = q.PageCount,
                ThumbnailUrl = $"{baseUrl}/storage/documents/{q.Id}/{q.Id}.jpg",
                Tags = q.Tags?
                    .Select(dt => new TagResponse { Id = dt.Tag.Id, Name = dt.Tag.Name })
                    .ToList() ?? new List<TagResponse>()
            });

            var totalCount = await documentsIqueryable.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            return new PaginatedResponse<SimpleDocumentResponese> {
                Data = docResList,                   //  IEnumerable<SimpleDocumentResponese>
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                HasPreviousPage = request.PageNumber > 1,
                HasNextPage = request.PageNumber < totalPages
            };
        }








        private IQueryable<Document> ApplyFilters(IQueryable<Document> documentsIq, DocumentSearchRequest request) {
            if (!string.IsNullOrWhiteSpace(request.SearchTerm)) {
                var searchTerm = request.SearchTerm.ToLower();
                documentsIq = documentsIq.Where(d => d.Title.ToLower().Contains(searchTerm) ||
                                                        d.Author.ToLower().Contains(searchTerm) ||
                                                        d.Sumary.ToLower().Contains(searchTerm) ||
                                                        d.Field.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(request.Author)) {
                documentsIq = documentsIq.Where(d => d.Author.ToLower().Contains(request.Author.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(request.Field))
                documentsIq = documentsIq.Where(d => d.Field.ToLower().Contains(request.Field.ToLower()));


            if (request.Tags != null && request.Tags.Any())
                documentsIq = documentsIq.Where(d => d.Tags.Any(dt => request.Tags.Contains(dt.Tag.Name)));


            return documentsIq;
        }

    }
}
