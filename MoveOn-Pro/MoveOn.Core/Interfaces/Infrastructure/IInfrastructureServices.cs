using Microsoft.AspNetCore.Http;

namespace MoveOn.Core.Interfaces.Infrastructure;

public interface IImageUploadService
{
    Task<string> UploadImageAsync(IFormFile file, string? folder = null);
    Task<bool> DeleteImageAsync(string imageUrl);
    Task<string> UploadProfilePictureAsync(IFormFile file);
    Task<bool> ValidateImageFile(IFormFile file);
    Task<string> GenerateThumbnailAsync(string imageUrl, int width = 200, int height = 200);
    Task<bool> OptimizeImageAsync(string imageUrl);
    Task<Dictionary<string, object>> GetImageMetadataAsync(IFormFile file);
}

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    Task SendWelcomeEmailAsync(string email, string firstName, string confirmationLink);
    Task SendPasswordResetEmailAsync(string email, string resetToken, string resetLink);
    Task SendEmailVerificationAsync(string email, string verificationToken, string verificationLink);
    Task SendWorkoutReminderEmailAsync(string email, string firstName, List<string> upcomingWorkouts);
    Task SendProgressReportEmailAsync(string email, string firstName, Dictionary<string, object> progressData);
    Task SendNotificationEmailAsync(string email, string title, string message, string? actionUrl = null);
}

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);
    Task ClearAsync();
    Task ClearByPrefixAsync(string prefix);
    Task<T?> GetOrSetAsync<T>(string key, Func<T> factory, TimeSpan? expiry = null);
    Task<IEnumerable<string>> GetKeysByPrefixAsync(string prefix);
}

public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string containerName);
    Task<bool> DeleteFileAsync(string fileName, string containerName);
    Task<string> GetFileUrlAsync(string fileName, string containerName);
    Task<bool> FileExistsAsync(string fileName, string containerName);
    Task<byte[]> DownloadFileAsync(string fileName, string containerName);
    Task<Dictionary<string, object>> GetFileMetadataAsync(string fileName, string containerName);
    Task<IEnumerable<string>> ListFilesAsync(string containerName, string? prefix = null);
    Task<bool> CopyFileAsync(string sourceFileName, string sourceContainer, string destinationFileName, string destinationContainer);
}

public interface ILoggingService
{
    Task LogInformationAsync(string message, Dictionary<string, object>? parameters = null);
    Task LogWarningAsync(string message, Dictionary<string, object>? parameters = null);
    Task LogErrorAsync(string message, Exception? exception = null, Dictionary<string, object>? parameters = null);
    Task LogDebugAsync(string message, Dictionary<string, object>? parameters = null);
    Task LogAuditAsync(string action, string userId, Dictionary<string, object>? details = null);
}

public interface ISearchService
{
    Task<IEnumerable<SearchResult>> SearchAsync(SearchRequest request);
    Task<IEnumerable<SearchResult>> SearchUsersAsync(string query, int page = 1, int pageSize = 20);
    Task<IEnumerable<SearchResult>> SearchPostsAsync(string query, int page = 1, int pageSize = 20);
    Task<IEnumerable<SearchResult>> SearchHashtagsAsync(string query, int page = 1, int pageSize = 20);
    Task SaveSearchHistoryAsync(Guid userId, string query, string type);
    Task<IEnumerable<string>> GetSearchSuggestionsAsync(string query, string type);
}

public class SearchRequest
{
    public string Query { get; set; } = string.Empty;
    public string Type { get; set; } = "all"; // all, users, posts, hashtags
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? UserId { get; set; }
    public Dictionary<string, object>? Filters { get; set; }
}

public class SearchResult
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // user, post, hashtag
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
    public double RelevanceScore { get; set; }
}