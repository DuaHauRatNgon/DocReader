using Application.DTOs;
using Core.Interfaces;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class PendingDocumentService {
        private readonly IPendingDocumentRepository _repo;
        private readonly IUserContextService _userContext;

        public PendingDocumentService(IPendingDocumentRepository repo, IUserContextService userContext) {
            _repo = repo;
            _userContext = userContext;
        }

        public async Task<List<PendingDocumentDto>> GetPendingDocumentsAsync() {
            var pendingDocs = await _repo.GetAllPendingAsync();

            return pendingDocs.Select(p => new PendingDocumentDto {
                Id = p.Id,
                Title = p.Title,
                Author = p.Author,
                Field = p.Field,
                UploadedAt = p.UploadedAt,
                UploadedByUserId = p.UploadedByUserId,
                Status = p.Status,
                TagNames = p.Tags.Select(t => t.Tag.Name).ToList()
            }).ToList();
        }








        public async Task<List<MyUploadDto>> GetMyUploadsAsync() {
            var userId = _userContext.UserId;
            var uploads = await _repo.GetByUploaderAsync(userId);

            return uploads.Select(p => new MyUploadDto {
                Id = p.Id,
                Title = p.Title,
                Field = p.Field,
                Author = p.Author,
                UploadedAt = p.UploadedAt,
                Status = p.Status,
                RejectionReason = p.RejectReason
            }).ToList();
        }






    }

}
