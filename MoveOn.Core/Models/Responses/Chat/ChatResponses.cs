namespace MoveOn.Core.Models.Responses.Chat;

public class ConversationResponse
{
    public Guid Id { get; set; }
    public Guid User1Id { get; set; }
    public Guid User2Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public UserResponse User1 { get; set; } = null!;
    public UserResponse User2 { get; set; } = null!;
    public List<MessageResponse> Messages { get; set; } = new();
    public MessageResponse? LastMessage { get; set; }
    public int UnreadMessageCount { get; set; }
    public bool IsOnline { get; set; }
    public string? LastMessagePreview => LastMessage?.Content.Length > 50 ? 
        LastMessage.Content.Substring(0, 50) + "..." : LastMessage?.Content;
}

public class MessageResponse
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserResponse Sender { get; set; } = null!;
    public UserResponse Receiver { get; set; } = null!;
    public TimeSpan TimeAgo => DateTime.UtcNow - CreatedAt;
    public string FormattedTimeAgo => FormatTimeAgo(TimeAgo);
    public bool IsFromCurrentUser { get; set; }
}

public class ConversationListResponse
{
    public List<ConversationResponse> Conversations { get; set; } = new();
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
    public int TotalUnreadMessages { get; set; }
}

public class MessageListResponse
{
    public List<MessageResponse> Messages { get; set; } = new();
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
}

public class UnreadMessageCountResponse
{
    public int TotalUnreadMessages { get; set; }
    public int ConversationsWithUnreadMessages { get; set; }
    public List<ConversationUnreadCount> ConversationCounts { get; set; } = new();
}

public class ConversationUnreadCount
{
    public Guid ConversationId { get; set; }
    public int UnreadCount { get; set; }
}

private static class ChatResponseHelper
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