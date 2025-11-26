using System.ComponentModel.DataAnnotations;

namespace MoveOn.Api.DTOs;

public class PostRequest
{
    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
}

public class PostWithImageRequest
{
    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;

    public IFormFile? Image { get; set; }
}

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
    public bool IsLikedByCurrentUser { get; set; }
}

public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
}