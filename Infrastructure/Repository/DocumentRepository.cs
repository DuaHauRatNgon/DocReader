using Core.Interfaces;
using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Application.DTOs;
using Infrastructure.DTOs;


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

            //// group và tạo lại structure
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





        public async Task<List<Document>> GetDocumentsByTagIdAsync(Guid tagId) {
            // Use direct join approach similar to raw SQL
            var documents = await _context.Documents
                .Join(_context.DocumentTags,
                      d => d.Id,
                      dt => dt.DocumentId,
                      (d, dt) => new { Document = d, DocumentTag = dt })
                .Where(x => x.DocumentTag.TagId == tagId)
                .Select(x => x.Document)
                .Include(d => d.Tags)
                .ThenInclude(dt => dt.Tag)
                .Include(d => d.Pages)
                .Include(d => d.Votes)
                .Distinct()
                .ToListAsync();

            return documents;
        }






        public async Task<List<TopDocumentDto>> TakeTopDocumentUpvote() {

            // c1   linq to entities            
            var topdocs1 = await _context.Documents
                    .Select(d => new TopDocumentDto {
                        Title = d.Title,
                        Author = d.Author,
                        Tags = d.Tags.Select(x => x.Tag.Name).ToList(),
                        UpvoteCount = d.Votes.Count(c => c.IsUpvote)
                    })
                    .OrderByDescending(d => d.UpvoteCount)
                    .Take(5)
                    .ToListAsync();



            // c2 xu li linq in memory 
            // toan bo data duoc laod vao ram  
            var documents = await _context.Documents
                                    .Include(d => d.Votes)
                                    .Include(d => d.Tags).ThenInclude(dt => dt.Tag)
                                    .ToListAsync();

            var topdocs2 = documents.Select(d => new {
                d.Title,
                d.Author,
                Tags = d.Tags.Select(t => t.Tag.Name).ToList(),
                UpvoteCount = d.Votes.Count(v => v.IsUpvote)
            })
                                            .OrderByDescending(d => d.UpvoteCount)
                                            .Take(5)
                                            .ToList();


            // c3 group join + linq 
            var topdocs3 = from doc in _context.Documents
                           join vote in _context.DocumentVotes on doc.Id equals vote.DocumentId into voteGroup
                           let upvoteCount = voteGroup.Count(v => v.IsUpvote)
                           orderby upvoteCount descending
                           select new {
                               doc.Title,
                               doc.Author,
                               UpvoteCount = upvoteCount,
                               Tags = doc.Tags.Select(dt => dt.Tag.Name).ToList()
                           };



            return topdocs1;
        }





        //
        public async Task<object> GetRelatedDocumentsByTagAsync(Guid documentId, int limit = 5) {
            var currentTagIds = await (from t in _context.DocumentTags
                                       where t.DocumentId == documentId
                                       select t.TagId).ToListAsync();

            // dung roi nhung ma linq phuc tap qua nen ef k dich duoc =)))
            //var res = await (from dt in _context.DocumentTags
            //                 where currentTagIds.Contains(dt.TagId)
            //                       && dt.DocumentId != documentId

            //                 group dt by dt.DocumentId into docId_tagIds
            //                 //let commonTagCnt = docId_tagIds.Count()

            //                 select new {
            //                     DocumentId = docId_tagIds.Key,
            //                     commonTagCount = docId_tagIds.Count()
            //                 }

            //                 into grouped
            //                 /*
            //                  IEnumerable <{Guid DocumentId, int commonTagCount}>
            //                 */
            //                 join d in _context.Documents on grouped.DocumentId equals d.Id
            //                 select d).ToListAsync();


            // ====================
            // anh toai
            var q1 = from dt in _context.DocumentTags
                     join t in _context.Tags on dt.TagId equals t.Id
                     join d in _context.Documents on dt.DocumentId equals d.Id
                     where currentTagIds.Contains(dt.TagId)
                     select new {
                         DocumentId = dt.DocumentId,
                         Tag = t,
                         Doc = d
                     };
            var q2 = await q1.GroupBy(x => x.DocumentId).OrderBy(x => x.Count()).Take(5).Select(x => new {
                documentId = x.Key,
                doc = x.FirstOrDefault().Doc,
                tag = x.Select(s => s.Tag),

            }).ToListAsync();

            //return q2;




            // ======================




            // tach linq ra tung phan
            var query = (from dt in _context.DocumentTags
                         where currentTagIds.Contains(dt.TagId) && dt.DocumentId != documentId
                         group dt by dt.DocumentId into grouped
                         select new {
                             DocumentId = grouped.Key,
                             Score = grouped.Count()
                         }
                    );

            var scoredDocs = query
                .OrderByDescending(x => x.Score)
                .Take(limit)
                .ToList(); // load len ram =)))

            var res = (
                from s in scoredDocs
                join d in _context.Documents on s.DocumentId equals d.Id
                select d
            ).ToList();

            return res;
        }
    }
}
