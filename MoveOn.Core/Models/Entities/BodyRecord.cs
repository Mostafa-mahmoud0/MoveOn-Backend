namespace MoveOn.Core.Models.Entities;

public class BodyRecord : BaseEntity
{
    public Guid UserId { get; set; }
    public decimal Weight { get; set; }
    public decimal Height { get; set; }
    public decimal? FatPercentage { get; set; }
    public decimal? MuscleMass { get; set; }
    public DateTime RecordedAt { get; set; }
    
    // Navigation property
    public virtual User User { get; set; } = null!;
}