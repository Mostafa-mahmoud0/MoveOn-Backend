using System.ComponentModel.DataAnnotations;

namespace MoveOn.Core.Models.Requests.BodyRecords;

public class CreateBodyRecordRequest
{
    [Required(ErrorMessage = "Weight is required")]
    [Range(1, 1000, ErrorMessage = "Weight must be between 1 and 1000 kg")]
    public decimal Weight { get; set; }

    [Required(ErrorMessage = "Height is required")]
    [Range(1, 300, ErrorMessage = "Height must be between 1 and 300 cm")]
    public decimal Height { get; set; }

    [Range(0, 100, ErrorMessage = "Fat percentage must be between 0 and 100")]
    public decimal? FatPercentage { get; set; }

    [Range(0, 1000, ErrorMessage = "Muscle mass must be between 0 and 1000 kg")]
    public decimal? MuscleMass { get; set; }

    public DateTime? RecordedAt { get; set; }
}

public class UpdateBodyRecordRequest
{
    [Range(1, 1000, ErrorMessage = "Weight must be between 1 and 1000 kg")]
    public decimal? Weight { get; set; }

    [Range(1, 300, ErrorMessage = "Height must be between 1 and 300 cm")]
    public decimal? Height { get; set; }

    [Range(0, 100, ErrorMessage = "Fat percentage must be between 0 and 100")]
    public decimal? FatPercentage { get; set; }

    [Range(0, 1000, ErrorMessage = "Muscle mass must be between 0 and 1000 kg")]
    public decimal? MuscleMass { get; set; }

    public DateTime? RecordedAt { get; set; }
}