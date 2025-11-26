using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoveOn.Core.Interfaces;
using MoveOn.Workers.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoveOn.Workers;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;

    public Worker(
        ILogger<Worker> logger,
        IServiceProvider serviceProvider,
        IBackgroundJobClient backgroundJobClient,
        IRecurringJobManager recurringJobManager)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MoveOn Worker Service starting at {time}", DateTimeOffset.Now);

        // Register recurring jobs
        RegisterRecurringJobs();

        // Keep the worker alive
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // You can add periodic tasks here if needed
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // The operation was canceled
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the worker service");
            }
        }

        _logger.LogInformation("MoveOn Worker Service stopping at {time}", DateTimeOffset.Now);
    }

    private void RegisterRecurringJobs()
    {
        try
        {
            // Schedule daily workout reminder at 9:00 AM
            _recurringJobManager.AddOrUpdate(
                "daily-workout-reminder",
                () => SendDailyWorkoutReminder(),
                Cron.Daily(9, 0)); // 9:00 AM daily

            // Schedule weekly progress summary every Sunday at 6:00 PM
            _recurringJobManager.AddOrUpdate(
                "weekly-progress-summary",
                () => SendWeeklyProgressSummary(),
                Cron.Weekly(DayOfWeek.Sunday, 18, 0)); // Sunday 6:00 PM

            // Schedule monthly fitness challenge reminder on the 1st of each month
            _recurringJobManager.AddOrUpdate(
                "monthly-challenge-reminder",
                () => SendMonthlyChallengeReminder(),
                Cron.Monthly(1, 10, 0)); // 1st of each month at 10:00 AM

            _logger.LogInformation("Recurring jobs registered successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering recurring jobs");
        }
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task SendDailyWorkoutReminder()
    {
        using var scope = _serviceProvider.CreateScope();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        
        try
        {
            await notificationService.SendWorkoutReminderAsync();
            _logger.LogInformation("Daily workout reminder sent successfully at {time}", DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending daily workout reminder");
            throw;
        }
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task SendWeeklyProgressSummary()
    {
        using var scope = _serviceProvider.CreateScope();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        
        try
        {
            await notificationService.SendNotificationAsync(
                "Weekly Progress Summary: Check your fitness achievements and set new goals for the upcoming week!");
            
            _logger.LogInformation("Weekly progress summary sent successfully at {time}", DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending weekly progress summary");
            throw;
        }
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task SendMonthlyChallengeReminder()
    {
        using var scope = _serviceProvider.CreateScope();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        
        try
        {
            await notificationService.SendNotificationAsync(
                "New Monthly Challenge! Join our fitness challenge and compete with other members to achieve your goals!");
            
            _logger.LogInformation("Monthly challenge reminder sent successfully at {time}", DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending monthly challenge reminder");
            throw;
        }
    }

    public override void Dispose()
    {
        // Clean up any resources
        base.Dispose();
    }
}