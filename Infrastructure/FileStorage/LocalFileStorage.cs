using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileStorage {
    public class LocalFileStorage : IFileStorage {
        private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(),
                                                            "storage", "documents");

        public async Task<string> SavePageAsync(string documentId, int pageNumber, byte[] data) {
            var dir = Path.Combine(_basePath, documentId);
            Directory.CreateDirectory(dir);

            var path = Path.Combine(dir, $"page_{pageNumber}.pdf");
            await File.WriteAllBytesAsync(path, data);
            return path;
        }
    }

}
