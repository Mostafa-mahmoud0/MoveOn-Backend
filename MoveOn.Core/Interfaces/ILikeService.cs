using MoveOn.Core.Entities;
using System.Threading.Tasks;

namespace MoveOn.Core.Interfaces;

public interface ILikeService
{
    Task<bool> ToggleLikeAsync(Guid postId, Guid userId);
    Task<int> GetPostLikesCountAsync(Guid postId);
    Task<bool> IsPostLikedByUserAsync(Guid postId, Guid userId);
}