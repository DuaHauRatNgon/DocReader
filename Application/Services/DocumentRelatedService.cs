using Application.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Infrastructure.Repository;

namespace Application.Services {
    public class DocumentRelatedService {
        private readonly DocumentRepository _documentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DocumentRelatedService(DocumentRepository documentRepository, IHttpContextAccessor httpContextAccessor) {
            _documentRepository = documentRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        //public async Task<List<SimpleDocumentResponese>> GetRelatedDocumentsAsync(Guid documentId) {
        //    var relatedDocs = await _documentRepository.GetRelatedDocumentsByTagAsync(documentId);

        //    return relatedDocs.Select(d => new SimpleDocumentResponese {
        //        Id = d.Id,
        //        Title = d.Title,
        //        Author = d.Author,
        //        Field = d.Field,
        //        PageCount = d.PageCount,
        //        Sumary = d.Sumary,
        //        ThumbnailUrl = $"/storage/documents/{d.Id}/{d.Id}.jpg"
        //    }).ToList();
        //}


        public async Task<object> GetRelatedDocAsync(Guid documentId) {
            //var res = await _documentRepository.GetRelatedDocumentsByTagAsync(documentId);
            //return res;

            var docs = await _documentRepository.GetRelatedDocumentsByTagAsyncWtf(documentId);
            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "";
            var docResList = from q in docs
                             let thumbPath = $"/storage/documents/{q.documentId}/{q.documentId}.jpg"
                             select new SimpleDocumentResponese {
                                 Id = q.doc.Id,
                                 Author = q.doc.Author,
                                 Field = q.doc.Field,
                                 Title = q.doc.Title,
                                 Sumary = q.doc.Sumary,
                                 PageCount = q.doc.PageCount,
                                 ThumbnailUrl = baseUrl + thumbPath,
                                 Tags = q.doc.Tags?
                                            .Select(dt => new TagResponse { Id = dt.Tag.Id, Name = dt.Tag.Name })
                                            .ToList() ?? new List<TagResponse>()
                             };
            return docResList;
        }
    }

}
