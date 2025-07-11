using Application.DTOs;
using Application.Interfaces;
using Core.Interfaces;
using Infrastructure.FileStorage;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class DocumentDownloadService : IDocumentDownloadService {
        private readonly IUserContextService _userContext;
        private readonly IDocumentRepository _documentRepository;
        private readonly IFileStorageService _fileStorage;
        private readonly IAuthService _authService;

        public Task<DocumentDownloadDto?> GetFileForDownloadAsync(Guid documentId, Guid userId) {
            throw new NotImplementedException();
        }




        //public async Task<DocumentDownloadDto?> GetFileForDownloadAsync(Guid documentId, Guid userId) {
        //    var document = await _documentRepository.GetByIdAsync(documentId);

        //    var fileStream = await _fileStorage.GetFileStreamAsync(document.FilePath);
        //    if (fileStream == null)
        //        return null;

        //    return new DocumentDownloadDto {
        //        FileStream = fileStream,
        //        FileName = $"{document.Title}.pdf",
        //        ContentType = "application/pdf"
        //    };
        //}






    }
}
