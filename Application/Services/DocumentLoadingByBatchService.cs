using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Models.Domain;
using Application.DTOs;
using Infrastructure.Repository;

namespace Application.Services {



    public class DocumentLoadingByBatchService {

        private readonly DocumentPageRepository _documentPageRepository;


        public DocumentLoadingByBatchService(DocumentPageRepository documentPageRepository) {
            _documentPageRepository = documentPageRepository;
        }



        public async Task<IEnumerable<DocumentPageResponse>> GetsSinglePageAsync(Guid id, int pageNumber = 0, int batch_size = 5) {
            var documentPages = await _documentPageRepository.GetAsync(id, pageNumber, batch_size);
            var singleDocumentPageList = new List<DocumentPageResponse>();
            foreach (var documentPage in documentPages) {
                byte[] asBytes = File.ReadAllBytes(documentPage.FilePath);
                var tmppage = new DocumentPageResponse() {
                    Id = documentPage.Id,
                    DocumentId = documentPage.DocumentId,
                    PageNumber = documentPage.PageNumber,
                    ContentBase64Encoded = Convert.ToBase64String(asBytes)
                };
                singleDocumentPageList.Add(tmppage);
            }
            return singleDocumentPageList;
        }
    }
}