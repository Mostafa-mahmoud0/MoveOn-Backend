using Microsoft.EntityFrameworkCore;
using MoveOn.Core.Entities;
using MoveOn.Core.Interfaces;
using MoveOn.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoveOn.Services.Services;

public class BodyRecordService : IBodyRecordService
{
    private readonly MoveOnDbContext _context;

    public BodyRecordService(MoveOnDbContext context)
    {
        _context = context;
    }

    public async Task<BodyRecord> CreateBodyRecordAsync(Guid userId, decimal weight, decimal height, decimal? fatPercentage, decimal? muscleMass)
    {
        var bodyRecord = new BodyRecord
        {
            UserId = userId,
            Weight = weight,
            Height = height,
            FatPercentage = fatPercentage,
            MuscleMass = muscleMass,
            RecordedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.BodyRecords.Add(bodyRecord);
        await _context.SaveChangesAsync();

        return bodyRecord;
    }

    public async Task<IEnumerable<BodyRecord>> GetUserBodyRecordsAsync(Guid userId)
    {
        return await _context.BodyRecords
            .Where(br => br.UserId == userId)
            .OrderByDescending(br => br.RecordedAt)
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
}