using Core.Interfaces;
using Core.Models.Domain;
using Infrastructure.ExtractTextFromPdf;
using Infrastructure.FileStorage;
using Infrastructure.PdfProcessing;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class ApprovalService {
        private readonly AppDbContext _context;
        private readonly IDocumentProcessor _processor;
        private readonly IFileStorage _fileStorage;
        private readonly IDocumentRepository _documentRepository;



        public ApprovalService(IDocumentProcessor processor, IFileStorage storage, AppDbContext context, IDocumentRepository documentRepository) {
            _context = context;
            _processor = processor;
            _fileStorage = storage;
            _documentRepository = documentRepository;
        }




        //public async Task<bool> ProcessApprovedDocumentAsync(Guid pendingId) {
        //    var pending = await _context.PendingDocuments.Include(p => p.Tags)

        //        .FirstOrDefaultAsync(p => p.Id == pendingId);

        //    if (pending == null || pending.Status != UploadStatus.Approved)
        //        return false;

        //    var newDocId = Guid.NewGuid();
        //    //var newDir = Path.Combine("storage", "documents", newDocId.ToString());
        //    var newDir = newDocId.ToString();
        //    Directory.CreateDirectory(newDir);

        //    await using var pdfStream = new FileStream(pending.PdfPath, FileMode.Open, FileAccess.Read);

        //    var pages = new List<DocumentPage>();
        //    var splitResult = await _processor.SplitPdfAsync(pdfStream);

        //    foreach (var (pageNumber, bytes) in splitResult) {
        //        var filePath = Path.Combine(newDir, $"page_{pageNumber}.pdf");
        //        await File.WriteAllBytesAsync(filePath, bytes);

        //        pages.Add(new DocumentPage {
        //            PageNumber = pageNumber,
        //            FilePath = filePath
        //        });
        //    }

        //    var firstPagePath = Path.Combine(newDir, "page_1.pdf");
        //    var coverImagePath = Path.Combine(newDir, $"{newDocId}.jpg");
        //    PdfToImageSharpConverter.ConvertPdfToJpeg(firstPagePath, coverImagePath);



        //    var summary = ExtractTextFromPdf.Extract(newDir, maxPage: 3);

        //    var document = new Document {
        //        Id = newDocId,
        //        Title = pending.Title,
        //        Author = pending.Author,
        //        Field = pending.Field,
        //        Sumary = summary,
        //        PageCount = (ushort)pages.Count,
        //        CreatedAt = DateTime.UtcNow,
        //        Pages = pages,
        //        Tags = pending.Tags.Select(t => new DocumentTag { TagId = t.TagId }).ToList()

        //    };

        //    await _context.Documents.AddAsync(document);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        public async Task<bool> ProcessApprovedDocumentAsync(Guid pendingId) {
            var pending = await _context.PendingDocuments
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == pendingId);

            if (pending == null || pending.Status != UploadStatus.Approved)
                return false;

            var newDocId = Guid.NewGuid();

            var newDir = Path.Combine("storage", "documents", newDocId.ToString());
            Directory.CreateDirectory(newDir);

            await using var pdfStream = new FileStream(pending.PdfPath, FileMode.Open, FileAccess.Read);
            var pages = new List<DocumentPage>();
            var splitResult = await _processor.SplitPdfAsync(pdfStream);

            foreach (var (pageNumber, bytes) in splitResult) {
                var pagePath = Path.Combine(newDir, $"page_{pageNumber}.pdf");
                await File.WriteAllBytesAsync(pagePath, bytes);
                pages.Add(new DocumentPage {
                    PageNumber = pageNumber,
                    FilePath = pagePath
                });
            }

            var firstPagePath = Path.Combine(newDir, "page_1.pdf");
            if (!File.Exists(firstPagePath))
                throw new FileNotFoundException($"Không tìm thấy {firstPagePath}");

            var coverImagePath = Path.Combine(newDir, $"{newDocId}.jpg");
            PdfToImageSharpConverter.ConvertPdfToJpeg(firstPagePath, coverImagePath);

            var summary = ExtractTextFromPdf.Extract(pending.PdfPath, pageStart: 1, pageEnd: 3);

            var document = new Document {
                Id = newDocId,
                Title = pending.Title,
                Author = pending.Author,
                Field = pending.Field,
                Sumary = summary,
                PageCount = (ushort)pages.Count,
                CreatedAt = DateTime.UtcNow,
                Pages = pages,
                Tags = pending.Tags.Select(t => new DocumentTag { TagId = t.TagId }).ToList()
            };

            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            return true;
        }








        public async Task<PendingDocument> GetPendingDocumentAsync(Guid id) {
            var pending = await _context.PendingDocuments.FirstOrDefaultAsync(p => p.Id == id);
            return pending == null ? null : pending;
        }








        public async Task<bool> MarkAsRejectedAsync(Guid id, string reason) {
            var pending = await _context.PendingDocuments.FirstOrDefaultAsync(p => p.Id == id);
            if (pending == null || pending.Status != UploadStatus.Pending)
                return false;

            pending.Status = UploadStatus.Rejected;
            pending.RejectReason = reason;
            await _context.SaveChangesAsync();
            return true;
        }







        public async Task<bool> MarkAsApprovedAsync(Guid id) {
            var pending = await _context.PendingDocuments.FirstOrDefaultAsync(p => p.Id == id);
            if (pending == null || pending.Status != UploadStatus.Pending)
                return false;

            pending.Status = UploadStatus.Approved;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
