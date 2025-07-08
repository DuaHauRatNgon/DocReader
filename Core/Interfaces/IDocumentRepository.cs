using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Models.Domain;



namespace Core.Interfaces {
    public interface IDocumentRepository {
        Task<IEnumerable<Document>> GetAllAsync();
        Task<Document> GetByIdAsync(Guid id);
        Task AddAsync(Document doc);
        Task UpdateAsync(Document doc);
        Task DeleteAsync(Guid id);


        IQueryable<Document> GetQueryable();

        Task<List<Document>> GetDocumentsByTagIdAsync(Guid tagId);

        Task<object> GetRelatedDocumentsByTagAsync(Guid documentId, int limit = 5);

    }

}
