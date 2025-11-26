using System.ComponentModel.DataAnnotations;

namespace MoveOn.Api.DTOs;

public class CommentRequest
{
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;
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
}