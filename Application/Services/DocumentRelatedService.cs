using Application.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOs;

namespace Application.Services {
    public class DocumentRelatedService {
        private readonly IDocumentRepository _documentRepository;

        public DocumentRelatedService(IDocumentRepository documentRepository) {
            _documentRepository = documentRepository;
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


        public async Task<object> AnhToai(Guid documentId) {
            var res = await _documentRepository.GetRelatedDocumentsByTagAsync(documentId);
            return res;
        }
    }

}
