using Application.DTOs;
using Application.Interfaces;
using Core.Interfaces;
using Core.Models.Domain;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class UploadService : IUploadService {
        private readonly IPendingDocumentRepository _repo;
        private readonly IUserContextService _userContext;


        public UploadService(IPendingDocumentRepository repo, IUserContextService userContext) {
            _repo = repo;
            _userContext = userContext;
        }




        //public async Task<Guid> UploadAsync(UploadDocumentRequest request) {
        //    var id = Guid.NewGuid();
        //    var userId = _userContext.UserId;
        //    var dir = Path.Combine("storage", "pending", id.ToString());
        //    Directory.CreateDirectory(dir);



        //    var filePath = Path.Combine(dir, "original.pdf");
        //    using (var stream = new FileStream(filePath, FileMode.Create)) {
        //        await request.File.CopyToAsync(stream);
        //    }



        //    var pending = new PendingDocument {
        //        Id = id,
        //        Title = request.Title,
        //        Author = request.Author,
        //        Field = request.Field,
        //        PdfPath = filePath,
        //        UploadedAt = DateTime.UtcNow,
        //        UploadedByUserId = userId,
        //        Tags = request.TagIds.Select(tagId => new PendingDocumentTag { TagId = tagId }).ToList()
        //    };


        //    await _repo.AddAsync(pending);
        //    await _repo.SaveChangesAsync();
        //    return id;
        //}






        public async Task<Guid> UploadAsync(UploadDocumentRequest request) {
            // Validate bắt buộc
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required");

            var id = Guid.NewGuid();
            var userId = _userContext.UserId;

            var dir = Path.Combine("storage", "pending", id.ToString());
            Directory.CreateDirectory(dir);

            var filePath = Path.Combine(dir, "original.pdf");
            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await request.File.CopyToAsync(stream);
            }

            // Lọc TagIds hợp lệ
            var tagIds = request.TagIds?.Distinct().ToList() ?? new List<Guid>();
            var validTags = await _repo.GetValidTagIdsAsync(tagIds); // Bạn cần hàm này trong repo

            var pending = new PendingDocument {
                Id = id,
                Title = request.Title,
                Author = request.Author,
                Field = request.Field,
                PdfPath = filePath,
                UploadedAt = DateTime.UtcNow,
                UploadedByUserId = userId,
                Tags = validTags.Select(tagId => new PendingDocumentTag {
                    TagId = tagId,
                    PendingDocumentId = id // bắt buộc nếu bạn muốn set FK thủ công
                }).ToList()
            };

            await _repo.AddAsync(pending);
            await _repo.SaveChangesAsync();
            return id;
        }

    }
}

