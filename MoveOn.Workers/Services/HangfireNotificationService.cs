using Hangfire;
using MoveOn.Core.Interfaces;

namespace MoveOn.Workers.Services;

public class HangfireNotificationService
{
    private readonly INotificationService _notificationService;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public HangfireNotificationService(INotificationService notificationService, IBackgroundJobClient backgroundJobClient)
    {
        _notificationService = notificationService;
        _backgroundJobClient = backgroundJobClient;
    }

    // This method will be called by Hangfire as a recurring job
    [AutomaticRetry(Attempts = 3)]
    public async Task SendDailyWorkoutReminder()
    {
        try
        {
            await _notificationService.SendWorkoutReminderAsync();
            Console.WriteLine($"[{DateTime.UtcNow}] Daily workout reminder sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{DateTime.UtcNow}] Error sending workout reminder: {ex.Message}");
            throw;
        }
    }

    // Schedule a one-time notification
    public string ScheduleNotification(string message, string? userId = null, TimeSpan? delay = null)
    {
        var scheduleTime = delay.HasValue ? DateTime.UtcNow.Add(delay.Value) : DateTime.UtcNow;
        
        return _backgroundJobClient.Schedule(
            () => _notificationService.SendNotificationAsync(message, userId),
            scheduleTime);
    }

    // Schedule recurring notifications
    public void ScheduleRecurringNotification(string jobId, string message, string cronExpression, string? userId = null)
    {
        RecurringJob.AddOrUpdate(
            jobId,
            () => _notificationService.SendNotificationAsync(message, userId),
            cronExpression);
    }

    // Remove a scheduled job
    public bool RemoveScheduledJob(string jobId)
    {
        return _backgroundJobClient.Delete(jobId);
    }

    // Remove a recurring job
    public void RemoveRecurringJob(string jobId)
    {
        RecurringJob.RemoveIfExists(jobId);
    }
}