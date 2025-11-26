using System.ComponentModel.DataAnnotations;

namespace MoveOn.Api.DTOs;

public class BodyRecordRequest
{
    [Required]
    [Range(1, 1000)]
    public decimal Weight { get; set; }

    [Required]
    [Range(1, 300)]
    public decimal Height { get; set; }

    [Range(0, 100)]
    public decimal? FatPercentage { get; set; }

    [Range(0, 1000)]
    public decimal? MuscleMass { get; set; }
}

public class BodyRecordResponse
{
    public Guid Id { get; set; }
    public decimal Weight { get; set; }
    public decimal Height { get; set; }
    public decimal? FatPercentage { get; set; }
    public decimal? MuscleMass { get; set; }
    public DateTime RecordedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}