namespace MoveOn.Core.Models.Enums;

public enum Role
{
    User = 0,
    Admin = 1,
    Trainer = 2
}

public enum MessageStatus
{
    Sent = 0,
    Delivered = 1,
    Read = 2
}

public enum NotificationType
{
    WorkoutReminder = 0,
    ProgressSummary = 1,
    ChallengeReminder = 2,
    NewFollower = 3,
    PostLiked = 4,
    CommentReceived = 5
}