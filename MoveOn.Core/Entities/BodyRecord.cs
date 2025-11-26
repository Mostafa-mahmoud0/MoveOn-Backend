using System;

namespace MoveOn.Core.Entities;

public class BodyRecord
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Weight { get; set; }
    public decimal Height { get; set; }
    public decimal? FatPercentage { get; set; }
    public decimal? MuscleMass { get; set; }
    public DateTime RecordedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation property
    public virtual User User { get; set; } = null!;
}