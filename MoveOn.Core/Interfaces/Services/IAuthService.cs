using MoveOn.Core.Models.Requests.Authentication;
using MoveOn.Core.Models.Responses.Authentication;
using MoveOn.Core.Models.Entities;

namespace MoveOn.Core.Interfaces.Services;

public interface IAuthService
{
    Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<UserResponse>> GetUserByIdAsync(Guid userId);
    Task<ApiResponse<UserResponse>> GetUserByEmailAsync(string email);
    Task<ApiResponse<bool>> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<ApiResponse<bool>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);
    Task<ApiResponse<bool>> DeleteAccountAsync(Guid userId, string password);
    Task<string> GenerateJwtToken(User user);
    Task<ApiResponse<bool>> VerifyEmailAsync(string token);
    Task<ApiResponse<string>> ForgotPasswordAsync(string email);
    Task<ApiResponse<bool>> ResetPasswordAsync(string token, string newPassword);
}

public interface ITokenService
{
    string GenerateToken(User user);
    bool ValidateToken(string token);
    Guid? GetUserIdFromToken(string token);
    string GenerateRefreshToken(User user);
    bool ValidateRefreshToken(string token);
    Guid? GetUserIdFromRefreshToken(string token);
}

public interface IUserService
{
    Task<ApiResponse<UserResponse>> UpdateUserAsync(Guid userId, UpdateProfileRequest request);
    Task<ApiResponse<bool>> DeactivateUserAsync(Guid userId);
    Task<ApiResponse<bool>> ReactivateUserAsync(Guid userId);
    Task<ApiResponse<IEnumerable<UserResponse>>> SearchUsersAsync(string searchTerm, int page = 1, int pageSize = 20);
    Task<ApiResponse<bool>> FollowUserAsync(Guid followerId, Guid followingId);
    Task<ApiResponse<bool>> UnfollowUserAsync(Guid followerId, Guid followingId);
    Task<ApiResponse<IEnumerable<UserResponse>>> GetFollowersAsync(Guid userId, int page = 1, int pageSize = 20);
    Task<ApiResponse<IEnumerable<UserResponse>>> GetFollowingAsync(Guid userId, int page = 1, int pageSize = 20);
}