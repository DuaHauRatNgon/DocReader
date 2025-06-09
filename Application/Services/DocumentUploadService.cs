using Microsoft.AspNetCore.Mvc;

using Application.DTOs;
using Infrastructure.PdfProcessing;
using Infrastructure.Repository;
using Infrastructure.FileStorage;
using Core.Models.Domain;
using Infrastructure.ExtractTextFromPdf;
using Core.Interfaces;

namespace Application.Services {
    public class DocumentUploadService {



        private readonly IDocumentProcessor _processor;
        private readonly IFileStorage _fileStorage;
        //private readonly AppDbContext _context;
        private readonly IDocumentRepository _documentRepository;



        public DocumentUploadService(IDocumentProcessor processor, IFileStorage storage, AppDbContext context, IDocumentRepository documentRepository) {
            _processor = processor;
            _fileStorage = storage;
            //_context = context;
            _documentRepository = documentRepository;
        }





        public async Task<Guid> UploadAsync(UploadDocumentRequest request) {
            var docId = Guid.NewGuid();

            var pages = new List<DocumentPage>();

            using var streamReq = request.File.OpenReadStream();

            // quen ko await khi gọi _processor.SplitPdfAsync(streamReq), dẫn đến splitPages là một Task, ko phai la IEnumerable
            // var splitPages =  _processor.SplitPdfAsync(streamReq);

            var splitPages = await _processor.SplitPdfAsync(streamReq);

            //foreach (var page in splitPages) {
            //    Console.WriteLine(pages.);
            //}

            //for (int i = 0; i < splitPages.Result.Count; i++) {
            //    Console.WriteLine(splitPages.Result[i]);
            //}

            foreach (var (pageNumber, bytes) in splitPages) {
                var path = await _fileStorage.SavePageAsync(docId.ToString(), pageNumber, bytes);
                pages.Add(new DocumentPage {
                    Id = Guid.NewGuid(),
                    DocumentId = docId,
                    PageNumber = pageNumber,
                    FilePath = path
                });
            }



            // summarizer
            var sumary = ExtractTextFromPdf.Extract(docId.ToString(), 3);



            // goi xuong repo de chay context :v
            await _documentRepository.AddAsync(
                new Document {
                    Id = docId,
                    Title = request.Title,
                    Field = request.Field,
                    Author = request.Author,
                    CreatedAt = DateTime.UtcNow,
                    Pages = pages,
                    Sumary = sumary
                });

            return docId;
        }
    }
}
