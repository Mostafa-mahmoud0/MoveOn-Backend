using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveOn.Api.DTOs;
using MoveOn.Core.Interfaces;
using System.Security.Claims;

namespace MoveOn.Api.Controllers;

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
    public async Task<ActionResult<BodyRecordResponse>> CreateBodyRecord([FromBody] BodyRecordRequest request)
    {
        var userId = GetCurrentUserId();
        var bodyRecord = await _bodyRecordService.CreateBodyRecordAsync(
            userId, 
            request.Weight, 
            request.Height, 
            request.FatPercentage, 
            request.MuscleMass);

        return Ok(new BodyRecordResponse
        {
            Id = bodyRecord.Id,
            Weight = bodyRecord.Weight,
            Height = bodyRecord.Height,
            FatPercentage = bodyRecord.FatPercentage,
            MuscleMass = bodyRecord.MuscleMass,
            RecordedAt = bodyRecord.RecordedAt,
            CreatedAt = bodyRecord.CreatedAt
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BodyRecordResponse>>> GetBodyRecords()
    {
        var userId = GetCurrentUserId();
        var bodyRecords = await _bodyRecordService.GetUserBodyRecordsAsync(userId);

        return Ok(bodyRecords.Select(br => new BodyRecordResponse
        {
            Id = br.Id,
            Weight = br.Weight,
            Height = br.Height,
            FatPercentage = br.FatPercentage,
            MuscleMass = br.MuscleMass,
            RecordedAt = br.RecordedAt,
            CreatedAt = br.CreatedAt
        }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBodyRecord(Guid id)
    {
        var userId = GetCurrentUserId();
        var success = await _bodyRecordService.DeleteBodyRecordAsync(id, userId);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}