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
            string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "storage", "documents");
            string dir = Path.Combine(_basePath, docId.ToString().ToLower());
            DeleteFolderByName(dir);
        }




        public static void DeleteFolderByName(string folderName) {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (Directory.Exists(folderPath)) {
                Directory.Delete(folderPath, recursive: true);
                Console.WriteLine($"Delete: {folderPath}");
            }
            else Console.WriteLine($"K tim they folder: {folderPath}");
        }

    }
}
