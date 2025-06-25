//using Core.Interfaces;
//using Infrastructure.Repository;
//using System.Reflection.Metadata;

//namespace API.GraphQL
//{
//    public class Query
//    {
//        [UseProjection]
//        [UseFiltering]      
//        [UseSorting]
//        public IQueryable<Document> GetTopViewedDocuments([Service] IDocumentRepository repo)  {
//            repo.GetAll().OrderByDescending(d => d.ViewCount).Take(10);

//        }




//        public async Task<int> GetTotalDocumentCount([Service] IDocumentRepository repo)
//            => await repo.CountAsync();



//        public async Task<List<TagStatsDTO>> GetTagStatistics([Service] ITagRepository tagRepo)
//            => await tagRepo.GetTagStatsAsync(); // viet  method này ở repo

//    }
//}
