using Microsoft.EntityFrameworkCore;
using MoveOn.Core.Models.Entities;
using MoveOn.Core.Interfaces.Services;
using MoveOn.Infrastructure.Data.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoveOn.Services.Services.Social;

public class CommentService : ICommentService
{
    private readonly MoveOnDbContext _context;

    public CommentService(MoveOnDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateCommentAsync(Guid postId, Guid userId, string content)
    {
        // Verify post exists
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            throw new ArgumentException("Post not found.");
        }

        var comment = new Comment
        {
            PostId = postId,
            UserId = userId,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        // Load user and post for response
        await _context.Entry(comment)
            .Reference(c => c.User)
            .LoadAsync();

        await _context.Entry(comment)
            .Reference(c => c.Post)
            .LoadAsync();

        return comment;
    }

    public async Task<IEnumerable<Comment>> GetPostCommentsAsync(Guid postId)
    {
        return await _context.Comments
            .Where(c => c.PostId == postId)
            .Include(c => c.User)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }
}