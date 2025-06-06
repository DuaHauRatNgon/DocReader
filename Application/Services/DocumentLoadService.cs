using Application.DTOs;
using Infrastructure.Repository;
using Infrastructure;
using Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {



    public class DocumentLoadService {
        private readonly IDocumentRepository _repository;




        public DocumentLoadService(IDocumentRepository repository) {
            _repository = repository;
        }




        public async Task<IEnumerable<SimpleDocumentResponese>> GetAllAsync() {
            var docs = await _repository.GetAllAsync();
            var docResList = from q in docs
                             select new SimpleDocumentResponese {
                                 Id = q.Id,
                                 Author = q.Author,
                                 Field = q.Field,
                                 Title = q.Title,
                             };
            return docResList;
        }




        public async Task<SimpleDocumentResponese> GetByIdAsync(string docId) {
            var doc = await _repository.GetByIdAsync(_2Guid.ToGuid(docId));
            return new SimpleDocumentResponese { Id = doc.Id, Title = doc.Title, Author = doc.Author, Field = doc.Field };
        }

    }
}
