using MoveOn.Core.Entities;
using System.Threading.Tasks;

namespace MoveOn.Core.Interfaces;

public interface IPostService
{
    Task<Post> CreatePostAsync(Guid userId, string content, string? imageUrl = null);
    Task<IEnumerable<Post>> GetPostsAsync(int page = 1, int pageSize = 10);
    Task<Post?> GetPostAsync(Guid id);
    Task<bool> DeletePostAsync(Guid id, Guid userId);
}