namespace MoveOn.Core.Models.Entities;

public class Conversation : BaseEntity
{
    public Guid User1Id { get; set; }
    public Guid User2Id { get; set; }
    
    // Navigation properties
    public virtual User User1 { get; set; } = null!;
    public virtual User User2 { get; set; } = null!;
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}