using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveOn.Core.Models.Requests.BodyRecords;
using MoveOn.Core.Models.Responses.BodyRecords;
using MoveOn.Core.Interfaces.Services;
using MoveOn.Core.Models.Common;
using System.Security.Claims;

namespace MoveOn.Api.Controllers.BodyRecords;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BodyRecordsController : ControllerBase
{
    private readonly IBodyRecordService _bodyRecordService;

    public BodyRecordsController(IBodyRecordService bodyRecordService)
    {
        _bodyRecordService = bodyRecordService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BodyRecordResponse>>> CreateBodyRecord([FromBody] CreateBodyRecordRequest request)
    {
        var userId = GetCurrentUserId();
        var recordedAt = request.RecordedAt ?? DateTime.UtcNow;
        
        var bodyRecord = await _bodyRecordService.CreateBodyRecordAsync(
            userId, 
            request.Weight, 
            request.Height, 
            request.FatPercentage, 
            request.MuscleMass,
            recordedAt);

        var response = new BodyRecordResponse
        {
            Id = bodyRecord.Id,
            Weight = bodyRecord.Weight,
            Height = bodyRecord.Height,
            FatPercentage = bodyRecord.FatPercentage,
            MuscleMass = bodyRecord.MuscleMass,
            RecordedAt = bodyRecord.RecordedAt,
            CreatedAt = bodyRecord.CreatedAt
        };

        return Ok(ApiResponse<BodyRecordResponse>.SuccessResult(response));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<BodyRecordResponse>>>> GetBodyRecords([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        var bodyRecords = await _bodyRecordService.GetUserBodyRecordsAsync(userId, page, pageSize);

        var response = new PagedResponse<BodyRecordResponse>
        {
            Data = bodyRecords.Select(br => new BodyRecordResponse
            {
                Id = br.Id,
                Weight = br.Weight,
                Height = br.Height,
                FatPercentage = br.FatPercentage,
                MuscleMass = br.MuscleMass,
                RecordedAt = br.RecordedAt,
                CreatedAt = br.CreatedAt
            }),
            Page = page,
            PageSize = pageSize,
            TotalCount = bodyRecords.Count()
        };

        return Ok(ApiResponse<PagedResponse<BodyRecordResponse>>.SuccessResult(response));
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ApiResponse<BodyRecordSummaryResponse>>> GetBodyRecordSummary()
    {
        var userId = GetCurrentUserId();
        var summary = await _bodyRecordService.GetBodyRecordSummaryAsync(userId);

        return Ok(ApiResponse<BodyRecordSummaryResponse>.SuccessResult(summary));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteBodyRecord(Guid id)
    {
        var userId = GetCurrentUserId();
        var success = await _bodyRecordService.DeleteBodyRecordAsync(id, userId);

        if (!success)
        {
            return NotFound(ApiResponse<bool>.ErrorResult("Body record not found."));
        }

        return Ok(ApiResponse<bool>.SuccessResult(true, "Body record deleted successfully."));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}