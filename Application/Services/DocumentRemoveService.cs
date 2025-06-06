using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Interfaces;

namespace Application.Services {
    public class DocumentRemoveService {
        private readonly IDocumentRepository _documentrepository;


        public DocumentRemoveService(IDocumentRepository documentrepository) {
            _documentrepository = documentrepository;
        }



        public async Task Remove(Guid docId) {
            await _documentrepository.DeleteAsync(docId);
        }

    }
}
