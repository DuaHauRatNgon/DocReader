using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Models.Domain;

namespace Infrastructure.Repository {



    public class DocumentPageRepository {

        private readonly AppDbContext _appDbContext;

        public DocumentPageRepository(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }



        public async Task<IEnumerable<DocumentPage>> GetAsync(Guid docId, int pageNumber, int batch_size) {
            var pages = (from p in _appDbContext.DocumentPages
                         where p.DocumentId == docId && p.PageNumber >= pageNumber
                         orderby p.PageNumber ascending
                         select p).Take(batch_size).ToList();
            return pages;
        }

    }
}
