using Application.DTOs;
using Infrastructure;
using Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Services {



    public class DocumentLoadService {
        private readonly IDocumentRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public DocumentLoadService(IDocumentRepository repository,
             IHttpContextAccessor httpContextAccessor) {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }




        //public async Task<IEnumerable<SimpleDocumentResponese>> GetAllAsync() {
        //    var docs = await _repository.GetAllAsync();
        //    var docResList = from q in docs
        //                     select new SimpleDocumentResponese {
        //                         Id = q.Id,
        //                         Author = q.Author,
        //                         Field = q.Field,
        //                         Title = q.Title,
        //                         Sumary = q.Sumary,
        //                         PageCount = q.PageCount,
        //                         Tags = q.Tags?
        //                                    .Select(dt => new TagResponse { Id = dt.Tag.Id, Name = dt.Tag.Name })
        //                                      .ToList() ?? new List<TagResponse>()
        //                     };
        //    return docResList;
        //}

        public async Task<IEnumerable<SimpleDocumentResponese>> GetAllAsync() {
            var docs = await _repository.GetAllAsync();

            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "";

            var docResList = from q in docs
                             let thumbPath = $"/storage/documents/{q.Id}/{q.Id}.jpg"
                             select new SimpleDocumentResponese {
                                 Id = q.Id,
                                 Author = q.Author,
                                 Field = q.Field,
                                 Title = q.Title,
                                 Sumary = q.Sumary,
                                 PageCount = q.PageCount,
                                 ThumbnailUrl = baseUrl + thumbPath,
                                 Tags = q.Tags?
                                            .Select(dt => new TagResponse { Id = dt.Tag.Id, Name = dt.Tag.Name })
                                            .ToList() ?? new List<TagResponse>()
                             };
            return docResList;
        }




        public async Task<SimpleDocumentResponese> GetByIdAsync(string docId) {
            var doc = await _repository.GetByIdAsync(_2Guid.ToGuid(docId));
            return new SimpleDocumentResponese {
                Id = doc.Id,
                Title = doc.Title,
                Author = doc.Author,
                Field = doc.Field,
                Sumary = doc.Sumary,
                PageCount = doc.PageCount,
                Tags = doc.Tags?
                    .Select(dt => new TagResponse {
                        Id = dt.Tag.Id,
                        Name = dt.Tag.Name
                    })
                     .ToList()
                     ?? new List<TagResponse>()
            };
        }

    }
}
