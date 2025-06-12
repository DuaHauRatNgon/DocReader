using Microsoft.AspNetCore.Mvc;

using Application.DTOs;
using Infrastructure.PdfProcessing;
using Infrastructure.Repository;
using Infrastructure.FileStorage;
using Core.Models.Domain;
using Infrastructure.ExtractTextFromPdf;
using Core.Interfaces;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Core.Models.Domain.Core.Models.Domain;
using System.Drawing.Imaging;
using UglyToad.PdfPig.Graphics;
using Infrastructure.PdfProcessing;

namespace Application.Services {
    public class DocumentUploadService {



        private readonly AppDbContext _context;
        private readonly IDocumentProcessor _processor;
        private readonly IFileStorage _fileStorage;
        private readonly IDocumentRepository _documentRepository;



        public DocumentUploadService(IDocumentProcessor processor, IFileStorage storage, AppDbContext context, IDocumentRepository documentRepository) {
            _context = context;
            _processor = processor;
            _fileStorage = storage;
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

            string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "storage", "documents");
            var dir = Path.Combine(_basePath, docId.ToString());

            //them anh bia
            var imagePath = Path.Combine(dir, $"{docId}.jpg");
            var pdfPath = Path.Combine(dir, "page_1.pdf");
            //Pdf2Img.ConvertFirstPageToJpeg(pdfPath, imagePath);
            PdfToImageSharpConverter.ConvertPdfToJpeg(pdfPath, imagePath);


            // summarizer
            var sumary = ExtractTextFromPdf.Extract(docId.ToString(), 3);


            // dem so trang max length
            var pageCount = (ushort)Directory.GetFiles(dir).Length;



            var document = new Core.Models.Domain.Document {
                Id = docId,
                Title = request.Title,
                Field = request.Field,
                Author = request.Author,
                CreatedAt = DateTime.UtcNow,
                Pages = pages,
                Sumary = sumary,
                PageCount = pageCount
            };

            // add document vao context trước
            await _documentRepository.AddAsync(document);

            // 
            if (request.TagIds != null && request.TagIds.Count > 0) {
                var validTagIds = await _context.Tags
                    .Where(t => request.TagIds.Contains(t.Id))
                    .Select(t => t.Id)
                    .ToListAsync();

                var documentTags = validTagIds.Select(tagId => new DocumentTag {
                    DocumentId = docId,
                    TagId = tagId
                }).ToList();

                await _context.DocumentTags.AddRangeAsync(documentTags);
            }

            await _context.SaveChangesAsync();

            return docId;
        }
    }
}
