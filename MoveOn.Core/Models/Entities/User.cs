using MoveOn.Core.Models.Enums;

namespace MoveOn.Core.Models.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public Role Role { get; set; }
    
    // Navigation properties
    public virtual ICollection<BodyRecord> BodyRecords { get; set; } = new List<BodyRecord>();
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    public virtual ICollection<Conversation> Conversations1 { get; set; } = new List<Conversation>();
    public virtual ICollection<Conversation> Conversations2 { get; set; } = new List<Conversation>();
    public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
}