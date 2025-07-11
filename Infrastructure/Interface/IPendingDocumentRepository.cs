using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface {
    public interface IPendingDocumentRepository {
        Task AddAsync(PendingDocument doc);


        Task<PendingDocument?> GetByIdAsync(Guid id);


        Task SaveChangesAsync();



        Task<List<Guid>> GetValidTagIdsAsync(List<Guid> inputTagIds);



        //Task<List<PendingDocumentDto>> GetAllPendingAsync();
        Task<List<PendingDocument>> GetAllPendingAsync();



        Task<List<PendingDocument>> GetByUploaderAsync(string userId);

    }

}
