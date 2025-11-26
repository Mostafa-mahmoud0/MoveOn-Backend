namespace MoveOn.Core.Models.Responses.Social;

public class PostResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public UserResponse User { get; set; } = null!;
    public List<CommentResponse> Comments { get; set; } = new();
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public TimeSpan TimeAgo => DateTime.UtcNow - CreatedAt;
    public string FormattedTimeAgo => FormatTimeAgo(TimeAgo);
}

public class CommentResponse
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public UserResponse User { get; set; } = null!;
    public TimeSpan TimeAgo => DateTime.UtcNow - CreatedAt;
    public string FormattedTimeAgo => FormatTimeAgo(TimeAgo);
}

public class LikeResponse
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserResponse User { get; set; } = null!;
    public TimeSpan TimeAgo => DateTime.UtcNow - CreatedAt;
    public string FormattedTimeAgo => FormatTimeAgo(TimeAgo);
}

public class PostListResponse
{
    public List<PostResponse> Posts { get; set; } = new();
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
}

public class CommentListResponse
{
    public List<CommentResponse> Comments { get; set; } = new();
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
}

public class LikeListResponse
{
    public List<LikeResponse> Likes { get; set; } = new();
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
}

private static class ResponseHelper
{
    public static string FormatTimeAgo(TimeSpan timeAgo)
    {
        if (timeAgo.TotalMinutes < 1)
            return "Just now";
        if (timeAgo.TotalMinutes < 60)
            return $"{(int)timeAgo.TotalMinutes} minute{(timeAgo.TotalMinutes > 1 ? "s" : "")} ago";
        if (timeAgo.TotalHours < 24)
            return $"{(int)timeAgo.TotalHours} hour{(timeAgo.TotalHours > 1 ? "s" : "")} ago";
        if (timeAgo.TotalDays < 30)
            return $"{(int)timeAgo.TotalDays} day{(timeAgo.TotalDays > 1 ? "s" : "")} ago";
        if (timeAgo.TotalDays < 365)
            return $"{(int)(timeAgo.TotalDays / 30)} month{(timeAgo.TotalDays > 30 ? "s" : "")} ago";
        
        return $"{(int)(timeAgo.TotalDays / 365)} year{(timeAgo.TotalDays > 365 ? "s" : "")} ago";
    }
}