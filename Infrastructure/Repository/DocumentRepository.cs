using Core.Interfaces;
using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Infrastructure.Repository {


    public class DocumentRepository : IDocumentRepository {
        private readonly AppDbContext _context;



        public DocumentRepository(AppDbContext context) {
            _context = context;
        }




        public async Task<IEnumerable<Document>> GetAllAsync() {
            var res = await _context.Documents.ToListAsync();
            return res;
        }


        public async Task<Document> GetByIdAsync(Guid id) {
            var res = await _context.Documents.FindAsync(id);
            return res;
        }


        public async Task AddAsync(Document doc) {
            _context.Documents.Add(doc);
            await _context.SaveChangesAsync();
        }



        public async Task UpdateAsync(Document doc) {
            _context.Documents.Update(doc);
            await _context.SaveChangesAsync();
        }



        public async Task DeleteAsync(Guid id) {
            var doc = await _context.Documents.FindAsync(id);
            if (doc != null) {
                _context.Documents.Remove(doc);
                await _context.SaveChangesAsync();
            }
        }
    }

}
