using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileStorage {
    public interface IFileStorage {
        Task<string> SavePageAsync(string documentId, int pageNumber, byte[] data);
    }

}
