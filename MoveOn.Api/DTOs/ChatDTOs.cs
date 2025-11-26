namespace MoveOn.Api.DTOs;

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
}

public class MessageRequest
{
    public string Content { get; set; } = string.Empty;
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
}