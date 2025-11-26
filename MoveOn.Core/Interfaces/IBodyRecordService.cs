using MoveOn.Core.Entities;
using System.Threading.Tasks;

namespace MoveOn.Core.Interfaces;

public interface IBodyRecordService
{
    Task<BodyRecord> CreateBodyRecordAsync(Guid userId, decimal weight, decimal height, decimal? fatPercentage, decimal? muscleMass);
    Task<IEnumerable<BodyRecord>> GetUserBodyRecordsAsync(Guid userId);
    Task<BodyRecord?> GetBodyRecordAsync(Guid id, Guid userId);
    Task<bool> DeleteBodyRecordAsync(Guid id, Guid userId);
}