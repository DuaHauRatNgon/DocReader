using Infrastructure.Repository;

namespace API.GraphQL {
    //public class DashboardQuery {
    //    public async Task<int> GetTotalDocuments([Service] AppDbContext context)
    //        => await context.Documents.CountAsync();

    //    public async Task<int> GetTotalVotes([Service] AppDbContext context)
    //        => await context.DocumentVotes.CountAsync();

    //    public async Task<int> GetTotalComments([Service] AppDbContext context)
    //        => await context.Comments.CountAsync();

    //    public async Task<int> GetTotalBookmarks([Service] AppDbContext context)
    //        => await context.PageBookmarks.CountAsync();

    //    public async Task<List<TopDocumentDto>> GetTopVotedDocuments(
    //        [Service] AppDbContext context,
    //        int limit = 5) {
    //        return await context.Documents
    //            .Select(d => new TopDocumentDto {
    //                DocumentId = d.Id,
    //                Title = d.Title,
    //                VoteCount = d.Votes.Count()
    //            })
    //            .OrderByDescending(d => d.VoteCount)
    //            .Take(limit)
    //            .ToListAsync();
    //    }
    //}

}
