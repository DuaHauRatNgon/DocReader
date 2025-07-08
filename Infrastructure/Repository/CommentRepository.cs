using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DTOs;

namespace Infrastructure.Repository {
    public class CommentRepository {

        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context) {
            _context = context;
        }




        // c1
        public async Task<IEnumerable<Comment>> GetCommentsByDocIdAsync_Include(Guid docId) {
            return await _context.Comments
                .Where(c => c.DocId == docId)
                .Include(c => c.Likes)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }






        /* c2
         oke doan nay thich dung query syntax hon, dung group join - tuong duong include()
           join ...into de lay tat ca likes cua moi comment
         dung mothod syntax : _context.Comments
                                        .Where(c => c.BookId == bookId)
                                        .GroupJoin(...)
                                            ....
         */
        public async Task<IEnumerable<Comment>> GetCommentsByDocIdAsync_Join(Guid docId) {
            var q = from c in _context.Comments
                    where c.DocId == docId
                    join l in _context.CommentLikes on c.Id equals l.CommentId into likes
                    orderby c.CreatedAt descending
                    select new Comment {
                        Id = c.Id,
                        Content = c.Content,
                        DocId = c.DocId,
                        UserId = c.UserId,
                        UserName = c.UserName,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        LikeCount = c.LikeCount,
                        Likes = likes.ToList() ///// group join
                    };

            return await q.ToListAsync();
        }





        //c3 
        //   cach nay hay -  tao them dto thay vi dung truc tiep comment :>
        //  IsLikedByCurrentUser = c.Likes.Any(l => l.UserId == currentUserId),
        // LikedUserIds = c.Likes.Select(l => l.UserId).ToList()
        /*
        public async Task<IEnumerable<CommentWithLikesDTO>> GetCommentsByBookIdAsync_DTO_QuerySyntax(Guid bookId, string currentUserId = "") {
            var query = from c in _context.Comments
                        where c.BookId == bookId
                        orderby c.CreatedAt descending
                        select new CommentWithLikesDTO {
                            Id = c.Id,
                            Content = c.Content,
                            BookId = c.BookId,
                            UserId = c.UserId,
                            UserName = c.UserName,
                            CreatedAt = c.CreatedAt,
                            UpdatedAt = c.UpdatedAt,
                            // hay
                            LikeCount = c.LikeCount,
                            IsLikedByCurrentUser = c.Likes.Any(l => l.UserId == currentUserId),
                            LikedUserIds = c.Likes.Select(l => l.UserId).ToList()
                        };

            return await query.ToListAsync();
        }
        */








        // c4 
        // Left Join
        // dung .DefaultIfEmpty() 
        // flatten 







        // c5 
        /*
                model không có Navigation Property 
                (chỉ chứa khóa ngoại Id, không chứa object tham chiếu)
        //*/
        //public async Task<IEnumerable<CommentWithUserInfoDTO>> GetCommentsWithUserInfo_QuerySyntax(Guid docId) {
        //    var query = from c in _context.Comments
        //                join u in _context.Users on c.UserId equals u.Id // Join với Users table
        //                join l in _context.CommentLikes on c.Id equals l.CommentId into likes
        //                where c.DocId == docId
        //                orderby c.CreatedAt descending
        //                select new CommentWithUserInfoDTO {
        //                    Id = c.Id,
        //                    Content = c.Content,
        //                    BookId = c.BookId,
        //                    UserId = c.UserId,
        //                    UserName = u.UserName, // lay tu  Users table thay vì denormalized
        //                    UserEmail = u.Email,
        //                    UserAvatar = u.Avatar,
        //                    CreatedAt = c.CreatedAt,
        //                    UpdatedAt = c.UpdatedAt,
        //                    LikeCount = likes.Count(),
        //                    LikedUserNames = likes.Join(_context.Users,
        //                                                  like => like.UserId,
        //                                                  user => user.Id,
        //                                                  (like, user) => user.UserName).ToList()
        //                };

        //    return await query.ToListAsync();
        //}






        public async Task<Comment> GetCommentByIdAsync(Guid commentId) {
            //  var r1 = await _context.Comments.Include(c => c.Likes).FirstOrDefaultAsync(c => c.Id == commentId);
            var r2 = await (from c in _context.Comments
                            where c.Id == commentId
                            join l in _context.CommentLikes on c.Id equals l.CommentId into commentLikes
                            orderby c.CreatedAt descending
                            select new Comment {
                                Id = c.Id,
                                Content = c.Content,
                                DocId = c.DocId,
                                UserId = c.UserId,
                                UserName = c.UserName,
                                CreatedAt = c.CreatedAt,
                                UpdatedAt = c.UpdatedAt,
                                LikeCount = c.LikeCount,
                                Likes = commentLikes.ToList()
                            }).FirstOrDefaultAsync();

            return r2;
        }






        public async Task<Comment> CreateCommentAsync(Comment commnent) {
            _context.Add(commnent);
            await _context.SaveChangesAsync();
            return commnent;
        }






        public async Task<Comment> UpdateCommentAsync(Comment commnent) {
            _context.Update(commnent);
            await _context.SaveChangesAsync();
            return commnent;
        }





        public async Task<bool> DeleteCommentAsync(Guid cmtId, string userId) {
            var cmt = await (
                                from q in _context.Comments
                                where q.Id == cmtId && q.UserId == userId
                                select q
                ).FirstOrDefaultAsync();
            //return cmts.Any();  

            if (cmt == null) return false;

            var likes = await (
                        from cl in _context.CommentLikes
                        where cl.Id == cmtId
                        select cl
                        )
                        .ToListAsync();

            _context.CommentLikes.RemoveRange(likes);
            _context.Comments.Remove(cmt);
            await _context.SaveChangesAsync();
            return true;
        }







        public async Task<bool> LikeCommentAsync(Guid commentId, string userId) {
            var existingLike = await _context.CommentLikes.FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);

            if (existingLike != null) return false;


            var commentLike = new CommentLike {
                Id = Guid.NewGuid(),
                CommentId = commentId,
                UserId = userId,
            };

            _context.CommentLikes.Add(commentLike);

            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null) {
                comment.LikeCount++;
            }

            await _context.SaveChangesAsync();
            return true;
        }





        public async Task<bool> UnlikeCommentAsync(Guid commentId, string userId) {
            var commentLike = await _context.CommentLikes
                .FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);

            if (commentLike == null)
                return false;


            _context.CommentLikes.Remove(commentLike);

            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null && comment.LikeCount > 0) comment.LikeCount--;

            await _context.SaveChangesAsync();
            return true;
        }





        /// quen mat cai nay =))
        public async Task<bool> HasUserLikedCommentAsync(Guid commentId, string userId) {
            return await _context.CommentLikes.AnyAsync(cl => cl.CommentId == commentId
                                                                        && cl.UserId == userId);
        }








        public async Task<List<LatestCommentDto>> GetLatestRawCommentsAsync(int count) {
            return await _context.Comments
                .OrderByDescending(c => c.CreatedAt)
                .Take(count)
                .Join(_context.Documents,
                      comment => comment.DocId,
                      doc => doc.Id,
                      (comment, doc) => new LatestCommentDto {
                          UserName = comment.UserName,
                          ContentPreview = comment.Content,
                          CreatedAt = comment.CreatedAt,
                          DocumentTitle = doc.Title
                      })
                .ToListAsync();
        }
        ////

    }
}
