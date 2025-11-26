using MoveOn.Core.Models.Requests.Social;
using MoveOn.Core.Models.Responses.Social;

namespace MoveOn.Core.Interfaces.Services;

public interface IPostService
{
    Task<ApiResponse<PostResponse>> CreatePostAsync(Guid userId, CreatePostRequest request);
    Task<ApiResponse<PostResponse>> CreatePostWithImageAsync(Guid userId, CreatePostWithImageRequest request);
    Task<ApiResponse<PostResponse>> UpdatePostAsync(Guid userId, Guid postId, UpdatePostRequest request);
    Task<ApiResponse<PostListResponse>> GetPostsAsync(GetPostsRequest request);
    Task<ApiResponse<PostResponse>> GetPostAsync(Guid id, Guid? currentUserId = null);
    Task<ApiResponse<bool>> DeletePostAsync(Guid id, Guid userId);
    Task<ApiResponse<PostListResponse>> GetUserPostsAsync(Guid userId, int page = 1, int pageSize = 10);
    Task<ApiResponse<PostListResponse>> GetFeedAsync(Guid userId, int page = 1, int pageSize = 10);
    Task<ApiResponse<PostListResponse>> SearchPostsAsync(string searchTerm, int page = 1, int pageSize = 10);
    Task<ApiResponse<bool>> BookmarkPostAsync(Guid userId, Guid postId);
    Task<ApiResponse<bool>> RemoveBookmarkAsync(Guid userId, Guid postId);
    Task<ApiResponse<PostListResponse>> GetBookmarkedPostsAsync(Guid userId, int page = 1, int pageSize = 10);
    Task<ApiResponse<bool>> ReportPostAsync(Guid userId, Guid postId, string reason);
}

public interface ICommentService
{
    Task<ApiResponse<CommentResponse>> CreateCommentAsync(Guid postId, Guid userId, CreateCommentRequest request);
    Task<ApiResponse<CommentResponse>> UpdateCommentAsync(Guid userId, Guid commentId, UpdateCommentRequest request);
    Task<ApiResponse<CommentListResponse>> GetPostCommentsAsync(Guid postId, int page = 1, int pageSize = 20);
    Task<ApiResponse<bool>> DeleteCommentAsync(Guid id, Guid userId);
    Task<ApiResponse<CommentListResponse>> GetUserCommentsAsync(Guid userId, int page = 1, int pageSize = 20);
    Task<ApiResponse<bool>> ReportCommentAsync(Guid userId, Guid commentId, string reason);
}

public interface ILikeService
{
    Task<ApiResponse<bool>> ToggleLikeAsync(Guid postId, Guid userId);
    Task<ApiResponse<int>> GetPostLikesCountAsync(Guid postId);
    Task<ApiResponse<bool>> IsPostLikedByUserAsync(Guid postId, Guid userId);
    Task<ApiResponse<LikeListResponse>> GetPostLikesAsync(Guid postId, int page = 1, int pageSize = 20);
    Task<ApiResponse<PostListResponse>> GetLikedPostsAsync(Guid userId, int page = 1, int pageSize = 10);
}

public interface ISocialService
{
    Task<ApiResponse<bool>> FollowUserAsync(Guid followerId, Guid followingId);
    Task<ApiResponse<bool>> UnfollowUserAsync(Guid followerId, Guid followingId);
    Task<ApiResponse<bool>> IsFollowingAsync(Guid followerId, Guid followingId);
    Task<ApiResponse<int>> GetFollowersCountAsync(Guid userId);
    Task<ApiResponse<int>> GetFollowingCountAsync(Guid userId);
    Task<ApiResponse<IEnumerable<UserResponse>>> GetFollowersAsync(Guid userId, int page = 1, int pageSize = 20);
    Task<ApiResponse<IEnumerable<UserResponse>>> GetFollowingAsync(Guid userId, int page = 1, int pageSize = 20);
    Task<ApiResponse<IEnumerable<UserResponse>>> GetSuggestedUsersAsync(Guid userId, int count = 10);
    Task<ApiResponse<bool>> BlockUserAsync(Guid userId, Guid blockUserId);
    Task<ApiResponse<bool>> UnblockUserAsync(Guid userId, Guid blockUserId);
    Task<ApiResponse<IEnumerable<UserResponse>>> GetBlockedUsersAsync(Guid userId, int page = 1, int pageSize = 20);
}