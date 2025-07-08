using Application.DTOs;
using Core.Models.Domain;
using Core.Models.Identity;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Application.DTOs.Comment;
using Infrastructure.DTOs;

public class CommentService {
    private readonly CommentRepository _commentRepository;
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;


    public CommentService(AppDbContext context,
                    UserManager<AppUser> userManager,
                    CommentRepository cmtrepo) {
        _context = context;
        _userManager = userManager;
        _commentRepository = cmtrepo;
    }






    public async Task<IEnumerable<CommentDTO>> GetCommentsByDocIdAsync(Guid docId, string currentUserId) {
        var comments = await _commentRepository.GetCommentsByDocIdAsync_Join(docId);

        var commentDTOs = new List<CommentDTO>();

        foreach (var comment in comments) {

            //bool isLiked = true;
            //if (!string.IsNullOrEmpty(currentUserId))
            //    await _commentRepository.HasUserLikedCommentAsync(comment.Id, currentUserId);
            //else isLiked = false;

            //cach nay hay hon
            var isLiked = !string.IsNullOrEmpty(currentUserId) ? await _commentRepository.HasUserLikedCommentAsync(comment.Id, currentUserId) : false;


            commentDTOs.Add(new CommentDTO {
                Id = comment.Id,
                Content = comment.Content,
                DocId = comment.DocId,
                UserId = comment.UserId,
                UserName = comment.UserName,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                LikeCount = comment.LikeCount,
                IsLikedByCurrentUser = isLiked
            });
        }

        return commentDTOs.OrderByDescending(c => c.CreatedAt);
    }






    public async Task<CommentDTO?> CreateCommentAsync(CreateCommentDTO createCommentDTO, string userId, string userName) {
        var comment = new Comment {
            Id = Guid.NewGuid(),
            Content = createCommentDTO.Content,
            DocId = createCommentDTO.DocId,
            UserId = userId,
            UserName = userName,
            CreatedAt = DateTime.UtcNow
        };

        var createdComment = await _commentRepository.CreateCommentAsync(comment);

        return new CommentDTO {
            Id = createdComment.Id,
            Content = createdComment.Content,
            DocId = createdComment.DocId,
            UserId = createdComment.UserId,
            UserName = createdComment.UserName,
            CreatedAt = createdComment.CreatedAt,
            UpdatedAt = createdComment.UpdatedAt,
            LikeCount = createdComment.LikeCount,
            IsLikedByCurrentUser = false
        };
    }





    public async Task<CommentDTO?> UpdateCommentAsync(Guid commentId, UpdateCommentDTO updateCommentDTO, string userId) {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);

        if (comment == null || comment.UserId != userId) {
            return null;
        }

        comment.Content = updateCommentDTO.Content;
        comment.UpdatedAt = DateTime.UtcNow;

        var updatedComment = await _commentRepository.UpdateCommentAsync(comment);

        var isLiked = await _commentRepository.HasUserLikedCommentAsync(commentId, userId);

        return new CommentDTO {
            Id = updatedComment.Id,
            Content = updatedComment.Content,
            DocId = updatedComment.DocId,
            UserId = updatedComment.UserId,
            UserName = updatedComment.UserName,
            CreatedAt = updatedComment.CreatedAt,
            UpdatedAt = updatedComment.UpdatedAt,
            LikeCount = updatedComment.LikeCount,
            IsLikedByCurrentUser = isLiked
        };
    }







    public async Task<bool> DeleteCommentAsync(Guid commentId, string userId) {
        return await _commentRepository.DeleteCommentAsync(commentId, userId);
    }








    public async Task<CommentLikeDTO?> ToggleLikeAsync(Guid commentId, string userId) {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        if (comment == null) return null;

        var hasLiked = await _commentRepository.HasUserLikedCommentAsync(commentId, userId);
        bool isLiked;

        if (hasLiked)
            isLiked = !await _commentRepository.UnlikeCommentAsync(commentId, userId);

        else
            isLiked = await _commentRepository.LikeCommentAsync(commentId, userId);


        return new CommentLikeDTO {
            CommentId = commentId,
            IsLiked = isLiked
        };
    }





    public async Task<List<LatestCommentDto>> GetLatestCommentsAsync(int count) {
        var comments = await _commentRepository.GetLatestRawCommentsAsync(count);
        foreach (var c in comments) {
            if (c.ContentPreview.Length > 50)
                c.ContentPreview = c.ContentPreview.Substring(0, 50) + "...";
        }
        return comments;
    }

}
