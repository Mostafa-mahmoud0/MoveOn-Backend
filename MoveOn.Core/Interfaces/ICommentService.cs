using MoveOn.Core.Entities;
using System.Threading.Tasks;

namespace MoveOn.Core.Interfaces;

public interface ICommentService
{
    Task<Comment> CreateCommentAsync(Guid postId, Guid userId, string content);
    Task<IEnumerable<Comment>> GetPostCommentsAsync(Guid postId);
}