using Microsoft.EntityFrameworkCore;
using MoveOn.Core.Models.Entities;
using MoveOn.Core.Interfaces.Services;
using MoveOn.Infrastructure.Data.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoveOn.Services.Services.BodyRecords;

public class BodyRecordService : IBodyRecordService
{
    private readonly MoveOnDbContext _context;

    public BodyRecordService(MoveOnDbContext context)
    {
        _context = context;
    }

    public async Task<BodyRecord> CreateBodyRecordAsync(Guid userId, decimal weight, decimal height, decimal? fatPercentage, decimal? muscleMass, DateTime? recordedAt = null)
    {
        var bodyRecord = new BodyRecord
        {
            UserId = userId,
            Weight = weight,
            Height = height,
            FatPercentage = fatPercentage,
            MuscleMass = muscleMass,
            RecordedAt = recordedAt ?? DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.BodyRecords.Add(bodyRecord);
        await _context.SaveChangesAsync();

        return bodyRecord;
    }

    public async Task<IEnumerable<BodyRecord>> GetUserBodyRecordsAsync(Guid userId, int page = 1, int pageSize = 10)
    {
        return await _context.BodyRecords
            .Where(br => br.UserId == userId)
            .OrderByDescending(br => br.RecordedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<BodyRecord?> GetBodyRecordAsync(Guid id, Guid userId)
    {
        return await _context.BodyRecords
            .FirstOrDefaultAsync(br => br.Id == id && br.UserId == userId);
    }

    public async Task<bool> DeleteBodyRecordAsync(Guid id, Guid userId)
    {
        var bodyRecord = await _context.BodyRecords
            .FirstOrDefaultAsync(br => br.Id == id && br.UserId == userId);

        if (bodyRecord == null)
        {
            return false;
        }

        _context.BodyRecords.Remove(bodyRecord);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<MoveOn.Core.Models.Responses.BodyRecords.BodyRecordSummaryResponse> GetBodyRecordSummaryAsync(Guid userId)
    {
        var records = await _context.BodyRecords
            .Where(br => br.UserId == userId)
            .OrderByDescending(br => br.RecordedAt)
            .ToListAsync();

        if (!records.Any())
        {
            return new MoveOn.Core.Models.Responses.BodyRecords.BodyRecordSummaryResponse();
        }

        var latest = records.First();
        return new MoveOn.Core.Models.Responses.BodyRecords.BodyRecordSummaryResponse
        {
            CurrentWeight = latest.Weight,
            CurrentHeight = latest.Height,
            CurrentFatPercentage = latest.FatPercentage,
            CurrentMuscleMass = latest.MuscleMass,
            CurrentBmi = latest.Height > 0 ? Math.Round(latest.Weight / ((latest.Height / 100) * (latest.Height / 100)), 2) : null,
            TotalRecords = records.Count,
            LastRecordedAt = latest.RecordedAt
        };
    }
}