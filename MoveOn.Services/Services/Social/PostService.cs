using Microsoft.EntityFrameworkCore;
using MoveOn.Core.Models.Entities;
using MoveOn.Core.Interfaces.Services;
using MoveOn.Infrastructure.Data.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoveOn.Services.Services.Social;

public class PostService : IPostService
{
    private readonly MoveOnDbContext _context;

    public PostService(MoveOnDbContext context)
    {
        _context = context;
    }

    public async Task<Post> CreatePostAsync(Guid userId, string content, string? imageUrl = null)
    {
        var post = new Post
        {
            UserId = userId,
            Content = content,
            ImageUrl = imageUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        // Load user for response
        await _context.Entry(post)
            .Reference(p => p.User)
            .LoadAsync();

        return post;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync(int page = 1, int pageSize = 10)
    {
        return await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
                .ThenInclude(c => c.User)
            .Include(p => p.Likes)
                .ThenInclude(l => l.User)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Post?> GetPostAsync(Guid id)
    {
        return await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
                .ThenInclude(c => c.User)
            .Include(p => p.Likes)
                .ThenInclude(l => l.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> DeletePostAsync(Guid id, Guid userId)
    {
        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        if (post == null)
        {
            return false;
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return true;
    }
}