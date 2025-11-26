using System.Threading.Tasks;

namespace MoveOn.Core.Interfaces;

public interface INotificationService
{
    Task SendWorkoutReminderAsync();
    Task SendNotificationAsync(string message, string? userId = null);
}