using System.ComponentModel.DataAnnotations;

namespace MoveOn.Core.Models.Requests.Chat;

public class SendMessageRequest
{
    [Required(ErrorMessage = "Message content is required")]
    [MaxLength(2000, ErrorMessage = "Message cannot exceed 2000 characters")]
    public string Content { get; set; } = string.Empty;
}

public class CreateConversationRequest
{
    [Required(ErrorMessage = "User ID is required")]
    public Guid OtherUserId { get; set; }
}

public class GetMessagesRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public bool UnreadOnly { get; set; } = false;
}

public class GetConversationsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}