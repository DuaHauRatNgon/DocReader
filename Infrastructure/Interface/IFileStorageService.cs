using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface {


    public interface IFileStorageService {
        Task<Stream?> GetFileStreamAsync(string filePath);
    }

}
