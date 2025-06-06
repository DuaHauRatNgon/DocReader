using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

using Infrastructure;

namespace Application.Services {
    public class DocumentModifyService {
        private readonly IDocumentRepository _documentRepository;


        public DocumentModifyService(IDocumentRepository documentRepository) {
            _documentRepository = documentRepository;

        }


        public async Task ModifyAsync(string docId, [FromBody] ModifyDocumentRequest req) {
            var docToUpdate = await _documentRepository.GetByIdAsync(_2Guid.ToGuid(docId));
            if (docToUpdate != null) {
                docToUpdate.Title = req.Title;
                docToUpdate.Author = req.Author;
                docToUpdate.Field = req.Field;
                docToUpdate.UpdatedAt = DateTime.UtcNow;
            }
            await _documentRepository.UpdateAsync(docToUpdate);
        }
        //public async Task ModifyAsync(string docId, ModifyDocumentRequest req) {
        //    if (req == null)
        //        throw new ArgumentNullException(nameof(req), "Request body is null.");

        //    var guid = _2Guid.ToGuid(docId); // nên kiểm tra null/error trong ToGuid
        //    var docToUpdate = await _documentRepository.GetByIdAsync(guid);

        //    if (docToUpdate == null)
        //        throw new Exception("Document not found.");

        //    docToUpdate.Title = req.Title;
        //    docToUpdate.Author = req.Author;
        //    docToUpdate.Field = req.Field;
        //    docToUpdate.UpdatedAt = DateTime.UtcNow;

        //    await _documentRepository.UpdateAsync(docToUpdate);
        //}

    }
}
