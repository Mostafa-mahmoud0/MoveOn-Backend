using System;

namespace MoveOn.Core.Entities;

public class Conversation
{
    public Guid Id { get; set; }
    public Guid User1Id { get; set; }
    public Guid User2Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual User User1 { get; set; } = null!;
    public virtual User User2 { get; set; } = null!;
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}