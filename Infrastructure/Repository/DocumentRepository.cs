using Core.Interfaces;
using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Application.DTOs;


namespace Infrastructure.Repository {


    public class DocumentRepository : IDocumentRepository {
        private readonly AppDbContext _context;



        public DocumentRepository(AppDbContext context) {
            _context = context;
        }








        public async Task<IEnumerable<Document>> GetAllAsync() {
            // code luc chua co tag
            //var res = await _context.Documents.ToListAsync();

            //c1 ===============
            // eager loading + include 
            var docList = await _context.Documents
                            .Include(d => d.Tags)
                            .ThenInclude(dt => dt.Tag)
                            .ToListAsync();

            //c2  =================
            // manual join
            //var query = from d in _context.Documents
            //            join dt in _context.DocumentTags on d.Id equals dt.DocumentId into docTags
            //            from dt in docTags.DefaultIfEmpty()
            //            join t in _context.Tags on dt.TagId equals t.Id into tags
            //            from t in tags.DefaultIfEmpty()
            //            select new {
            //                Document = d,
            //                DocumentTag = dt,
            //                Tag = t
            //            };

            //var result = await query.ToListAsync();

            //// Group và tạo lại structure
            //var documents = result
            //    .GroupBy(x => x.Document.Id)
            //    .Select(g => {
            //        var doc = g.First().Document;
            //        doc.Tags = g
            //            .Where(x => x.DocumentTag != null)
            //            .Select(x => new DocumentTag {
            //                DocumentId = x.DocumentTag.DocumentId,
            //                TagId = x.DocumentTag.TagId,
            //                Tag = x.Tag
            //            })
            //            .ToList();
            //        return doc;
            //    })
            //    .ToList();


            // c3 ===============
            // select
            //    var simpleDocumentResponeseList = await _context.Documents
            //.Select(d => new SimpleDocumentResponese {
            //    Id = d.Id,
            //    Title = d.Title,
            //    Field = d.Field,
            //    Author = d.Author,
            //    Sumary = d.Sumary,
            //    Tags = d.Tags.Select(dt => new TagResponse {
            //        Id = dt.Tag.Id,
            //        Name = dt.Tag.Name
            //    }).ToList()
            //})
            //    .ToListAsync();

            return docList;
        }







        public async Task<Document> GetByIdAsync(Guid id) {
            //var res = await _context.Documents.FindAsync(id);
            var res = await _context.Documents.Include(d => d.Tags).ThenInclude(dt => dt.Tag).FirstOrDefaultAsync(d => d.Id == id); ;
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




        public IQueryable<Document> GetQueryable() {
            return _context.Documents.AsQueryable().Include(d => d.Tags).ThenInclude(dt => dt.Tag);
        }
    }
}
