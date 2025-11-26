using Microsoft.EntityFrameworkCore;
using MoveOn.Core.Models.Entities;
using MoveOn.Core.Interfaces.Services;
using MoveOn.Infrastructure.Data.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoveOn.Services.Services.Social;

public class LikeService : ILikeService
{
    private readonly MoveOnDbContext _context;

    public LikeService(MoveOnDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ToggleLikeAsync(Guid postId, Guid userId)
    {
        // Verify post exists
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            throw new ArgumentException("Post not found.");
        }

        // Check if user already liked the post
        var existingLike = await _context.Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

        if (existingLike != null)
        {
            // Unlike the post
            _context.Likes.Remove(existingLike);
            await _context.SaveChangesAsync();
            return false;
        }
        else
        {
            // Like the post
            var like = new Like
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public async Task<int> GetPostLikesCountAsync(Guid postId)
    {
        return await _context.Likes
            .CountAsync(l => l.PostId == postId);
    }

    public async Task<bool> IsPostLikedByUserAsync(Guid postId, Guid userId)
    {
        return await _context.Likes
            .AnyAsync(l => l.PostId == postId && l.UserId == userId);
    }
}