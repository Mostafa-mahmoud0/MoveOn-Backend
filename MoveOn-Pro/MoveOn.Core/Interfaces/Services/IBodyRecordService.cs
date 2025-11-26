using MoveOn.Core.Models.Requests.BodyRecords;
using MoveOn.Core.Models.Responses.BodyRecords;

namespace MoveOn.Core.Interfaces.Services;

public interface IBodyRecordService
{
    Task<ApiResponse<BodyRecordResponse>> CreateBodyRecordAsync(Guid userId, CreateBodyRecordRequest request);
    Task<ApiResponse<BodyRecordResponse>> UpdateBodyRecordAsync(Guid userId, Guid recordId, UpdateBodyRecordRequest request);
    Task<ApiResponse<IEnumerable<BodyRecordResponse>>> GetUserBodyRecordsAsync(Guid userId, int page = 1, int pageSize = 10);
    Task<ApiResponse<BodyRecordResponse>> GetBodyRecordAsync(Guid id, Guid userId);
    Task<ApiResponse<bool>> DeleteBodyRecordAsync(Guid id, Guid userId);
    Task<ApiResponse<BodyRecordSummaryResponse>> GetBodyRecordSummaryAsync(Guid userId);
    Task<ApiResponse<BodyRecordProgressResponse>> GetBodyRecordProgressAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null);
    Task<ApiResponse<IEnumerable<BodyRecordResponse>>> GetBodyRecordsByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
    Task<ApiResponse<byte[]>> ExportBodyRecordsAsync(Guid userId, string format = "csv");
}

public interface IBodyAnalysisService
{
    Task<ApiResponse<Dictionary<string, object>>> GetBodyAnalysisAsync(Guid userId);
    Task<ApiResponse<List<WeightProgressPoint>>> GetWeightTrendAsync(Guid userId, int days = 30);
    Task<ApiResponse<List<BmiProgressPoint>>> GetBmiTrendAsync(Guid userId, int days = 30);
    Task<ApiResponse<Dictionary<string, decimal>>> GetBodyCompositionAsync(Guid userId);
    Task<ApiResponse<Dictionary<string, object>>> GetGoalsProgressAsync(Guid userId);
}