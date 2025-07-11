﻿using Application.DTOs;
using Application.DTOs;
using Azure.Core;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.DTOs;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services {



    public class DocumentLoadService {
        private readonly DocumentRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public DocumentLoadService(DocumentRepository repository,
             IHttpContextAccessor httpContextAccessor) {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }






        public async Task<IEnumerable<SimpleDocumentResponese>> GetAllAsync() {
            var docs = await _repository.GetAllAsync();

            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "";

            var docResList = from q in docs
                             let thumbPath = $"/storage/documents/{q.Id}/{q.Id}.jpg"
                             select new SimpleDocumentResponese {
                                 Id = q.Id,
                                 Author = q.Author,
                                 Field = q.Field,
                                 Title = q.Title,
                                 Sumary = q.Sumary,
                                 PageCount = q.PageCount,
                                 ThumbnailUrl = baseUrl + thumbPath,
                                 Tags = q.Tags?
                                            .Select(dt => new TagResponse { Id = dt.Tag.Id, Name = dt.Tag.Name })
                                            .ToList() ?? new List<TagResponse>()
                                 // or
                                 //Tags = q.Tags != null
                                 //                   ? q.Tags.Select(...).ToList()
                                 //                   : new List<TagResponse>();

                             };
            return docResList;
        }






        public async Task<PagedResult<SimpleDocumentResponese>> GetPagedAsync(PagedRequest request) {
            var pagedDocs = await _repository.GetPagedAsync(request.Page, request.PageSize);

            var httpRequest = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = httpRequest != null ? $"{httpRequest.Scheme}://{httpRequest.Host}" : "";

            var docResList = pagedDocs.Items.Select(
                q => new SimpleDocumentResponese {
                    Id = q.Id,
                    Author = q.Author,
                    Field = q.Field,
                    Title = q.Title,
                    Sumary = q.Sumary,
                    PageCount = q.PageCount,
                    ThumbnailUrl = baseUrl + $"/storage/documents/{q.Id}/{q.Id}.jpg",
                    Tags = q.Tags?
                          .Select(dt => new TagResponse { Id = dt.Tag.Id, Name = dt.Tag.Name })
                          .ToList() ?? new List<TagResponse>()
                }).ToList();

            return new PagedResult<SimpleDocumentResponese> {
                Items = docResList,
                TotalItems = pagedDocs.TotalItems,
                Page = pagedDocs.Page,
                PageSize = pagedDocs.PageSize
            };
        }









        public async Task<SimpleDocumentResponese> GetByIdAsync(string docId) {
            var doc = await _repository.GetByIdAsync(_2Guid.ToGuid(docId));

            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "";
            var thumbPath = $"/storage/documents/{doc.Id}/{doc.Id}.jpg";

            return new SimpleDocumentResponese {
                Id = doc.Id,
                Title = doc.Title,
                Author = doc.Author,
                Field = doc.Field,
                Sumary = doc.Sumary,
                PageCount = doc.PageCount,
                ThumbnailUrl = baseUrl + thumbPath,
                Tags = doc.Tags?
                    .Select(dt => new TagResponse {
                        Id = dt.Tag.Id,
                        Name = dt.Tag.Name
                    })
                     .ToList()
                     ?? new List<TagResponse>()
            };
        }







        public async Task<IEnumerable<SimpleDocumentResponese>> GetByTagIdAsync(Guid tagId) {
            var docs = await _repository.GetDocumentsByTagIdAsync(tagId);

            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "";

            var docResList = from q in docs
                             let thumbPath = $"/storage/documents/{q.Id}/{q.Id}.jpg"
                             select new SimpleDocumentResponese {
                                 Id = q.Id,
                                 Author = q.Author,
                                 Field = q.Field,
                                 Title = q.Title,
                                 Sumary = q.Sumary,
                                 PageCount = q.PageCount,
                                 ThumbnailUrl = baseUrl + thumbPath,
                                 Tags = q.Tags?
                                            .Select(dt => new TagResponse { Id = dt.Tag.Id, Name = dt.Tag.Name })
                                            .ToList() ?? new List<TagResponse>()
                             };

            return docResList;
        }






        public async Task<List<TopDocumentDTO>> GetTopDocumentUpvote() {
            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "";
            var listInfraTopDdcoDtos = await _repository.TakeTopDocumentUpvote();
            var r = listInfraTopDdcoDtos.Select(x => new TopDocumentDTO {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                Tags = x.Tags,
                UpvoteCount = x.UpvoteCount,
                ThumbnailUrl = $"{baseUrl}/storage/documents/{x.Id}/{x.Id}.jpg"
            }).ToList();
            return r;
        }







        public async Task<List<SimpleDocumentResponese>> GetLatestDocumentsAsync(int count) {
            var docs = await _repository.GetLatestDocumentsAsync(count);

            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "";

            var result = docs.Select(d => new SimpleDocumentResponese {
                Id = d.Id,
                Title = d.Title,
                Author = d.Author,
                ThumbnailUrl = $"{baseUrl}/storage/documents/{d.Id}/{d.Id}.jpg"
            }).ToList();

            return result;
        }









    }
}
