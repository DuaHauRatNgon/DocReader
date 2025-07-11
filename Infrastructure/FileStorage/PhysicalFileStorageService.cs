using Infrastructure.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileStorage {
    public class PhysicalFileStorageService : IFileStorageService {
        private readonly string _rootPath;



        public PhysicalFileStorageService(IConfiguration config) {
            _rootPath = config["Storage:RootPath"] ?? "UploadedFiles";
        }




        public async Task<Stream?> GetFileStreamAsync(string filePath) {
            var fullPath = Path.Combine(_rootPath, filePath);
            if (!File.Exists(fullPath))
                return null;

            return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        }
    }




}
