using MoveOn.Core.Models.Enums;

namespace MoveOn.Core.Interfaces.Services;

public interface INotificationService
{
    Task SendWorkoutReminderAsync();
    Task SendNotificationAsync(string message, string? userId = null, NotificationType type = NotificationType.WorkoutReminder);
    Task SendWeeklyProgressSummaryAsync();
    Task SendMonthlyChallengeReminderAsync();
    Task SendNewFollowerNotificationAsync(Guid userId, Guid followerId);
    Task SendPostLikedNotificationAsync(Guid userId, Guid postId, Guid likerId);
    Task SendCommentReceivedNotificationAsync(Guid userId, Guid postId, Guid commenterId);
    Task SendMentionNotificationAsync(Guid userId, Guid postId, Guid mentionerId);
    Task SendWorkoutCompletedNotificationAsync(Guid userId, string workoutType, int duration);
    Task SendGoalAchievedNotificationAsync(Guid userId, string goalType);
    Task SendBadgeEarnedNotificationAsync(Guid userId, string badgeName);
    Task SendChallengeInviteNotificationAsync(Guid userId, Guid challengeId, string challengeName);
    Task SendSystemNotificationAsync(string title, string message, List<string>? userIds = null);
    Task SendEmailNotificationAsync(string email, string subject, string message);
    Task SendPushNotificationAsync(string deviceToken, string title, string message, Dictionary<string, object>? data = null);
    Task SendSmsNotificationAsync(string phoneNumber, string message);
    Task<ApiResponse<bool>> MarkNotificationAsReadAsync(Guid userId, Guid notificationId);
    Task<ApiResponse<bool>> MarkAllNotificationsAsReadAsync(Guid userId);
    Task<ApiResponse<IEnumerable<NotificationResponse>>> GetUserNotificationsAsync(Guid userId, int page = 1, int pageSize = 20);
    Task<ApiResponse<int>> GetUnreadNotificationCountAsync(Guid userId);
    Task<ApiResponse<bool>> UpdateNotificationSettingsAsync(Guid userId, NotificationSettingsRequest settings);
    Task<ApiResponse<NotificationSettingsResponse>> GetNotificationSettingsAsync(Guid userId);
}

public class NotificationSettingsRequest
{
    public bool EmailNotifications { get; set; } = true;
    public bool PushNotifications { get; set; } = true;
    public bool SmsNotifications { get; set; } = false;
    public bool WorkoutReminders { get; set; } = true;
    public bool SocialNotifications { get; set; } = true;
    public bool MarketingEmails { get; set; } = false;
}

public class NotificationResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public Dictionary<string, object>? Data { get; set; }
    public string? ActionUrl { get; set; }
    public TimeSpan TimeAgo => DateTime.UtcNow - CreatedAt;
    public string FormattedTimeAgo => FormatTimeAgo(TimeAgo);
}

public class NotificationSettingsResponse
{
    public bool EmailNotifications { get; set; }
    public bool PushNotifications { get; set; }
    public bool SmsNotifications { get; set; }
    public bool WorkoutReminders { get; set; }
    public bool SocialNotifications { get; set; }
    public bool MarketingEmails { get; set; }
    public string? PreferredLanguage { get; set; }
    public string? TimeZone { get; set; }
}

private static class NotificationHelper
{
    public static string FormatTimeAgo(TimeSpan timeAgo)
    {
        if (timeAgo.TotalMinutes < 1)
            return "Just now";
        if (timeAgo.TotalMinutes < 60)
            return $"{(int)timeAgo.TotalMinutes} minute{(timeAgo.TotalMinutes > 1 ? "s" : "")} ago";
        if (timeAgo.TotalHours < 24)
            return $"{(int)timeAgo.TotalHours} hour{(timeAgo.TotalHours > 1 ? "s" : "")} ago";
        if (timeAgo.TotalDays < 30)
            return $"{(int)timeAgo.TotalDays} day{(timeAgo.TotalDays > 1 ? "s" : "")} ago";
        if (timeAgo.TotalDays < 365)
            return $"{(int)(timeAgo.TotalDays / 30)} month{(timeAgo.TotalDays > 30 ? "s" : "")} ago";
        
        return $"{(int)(timeAgo.TotalDays / 365)} year{(timeAgo.TotalDays > 365 ? "s" : "")} ago";
    }
}