using Application.DTOs;
using Core.Interfaces;
using Core.Models.Domain;
using Core.Models.Identity;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class PageBookmarkService {
        private readonly PageBookmarkRepository _repo;

        private readonly IUserContextService _user;
        //private readonly UserManager<AppUser> _userManager;






        public PageBookmarkService(PageBookmarkRepository repo, IUserContextService user) {
            _repo = repo;
            //_userManager = userManager;
            _user = user;
        }





        public async Task AddAsync(Guid documentId, int pageNumber) {
            var userId = _user.UserId;

            // await _context.PageBookmarks
            // .AnyAsync(x => x.UserId == userId
            // && x.DocumentId == documentId && x.PageNumber == pageNumber);
            // -> chuyen sang repo layer

            if (!await _repo.ExistsAsync(userId, documentId, pageNumber)) {
                var bookmark = new PageBookmark {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DocumentId = documentId,
                    PageNumber = pageNumber,
                    CreatedAt = DateTime.UtcNow
                };
                await _repo.AddAsync(bookmark);
            }
        }





        public async Task RemoveAsync(Guid documentId, int pageNumber) {
            var userId = _user.UserId;
            await _repo.RemoveAsync(userId, documentId, pageNumber);
        }





        public async Task<IEnumerable<PageBookmarkDTO>> GetByDocumentAsync(Guid documentId) {
            var userId = _user.UserId;
            var pageBoookmarkList = await _repo.GetByUserAndDocumentAsync(userId, documentId);

            var pageBookmarkDTOs = new List<PageBookmarkDTO>();
            foreach (var pbm in pageBoookmarkList) {
                var pageBookmarkDTO = new PageBookmarkDTO() {
                    Id = pbm.Id,
                    DocumentId = pbm.DocumentId,
                    PageNumber = pbm.PageNumber,
                    CreatedAt = pbm.CreatedAt
                };
                pageBookmarkDTOs.Add(pageBookmarkDTO);
            }
            return pageBookmarkDTOs;
        }





        public async Task<bool> IsBookmarked(Guid documentId, int pageNumber) {
            return await _repo.ExistsAsync(_user.UserId, documentId, pageNumber);
        }





        public async Task<int?> GetLatestBookmarkedPageAsync(Guid documentId) {
            var userId = _user.UserId;
            return await _repo.GetLatestPageNumberAsync(userId, documentId);
        }





        public async Task<IEnumerable<int>> GetAllBookmarkedPagesAsync(Guid documentId) {
            var userId = _user.UserId;
            return await _repo.GetAllPageNumbersAsync(userId, documentId);
        }




        public async Task<IEnumerable<PageBookmarkResponeDto>> GetAllAsyncService() {
            var userId = _user.UserId;
            var tmp = await _repo.GetAllAsyncRepo(userId);
            var r = new List<PageBookmarkResponeDto>();
            foreach (var i in tmp) {
                var j = new PageBookmarkResponeDto() {
                    DocumentId = i.DocumentId,
                    Title = i.Title,
                    Author = i.Author,
                    PageNumber = i.PageNumber
                };
                r.Add(j);
            }
            return r;
        }
    }


}
