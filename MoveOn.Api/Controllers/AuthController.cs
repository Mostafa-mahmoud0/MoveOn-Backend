using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveOn.Core.Models.Requests.Authentication;
using MoveOn.Core.Models.Responses.Authentication;
using MoveOn.Core.Interfaces.Services;
using System.Security.Claims;

namespace MoveOn.Api.Controllers.Authentication;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var token = await _authService.RegisterAsync(request.Email, request.Password, request.FirstName, request.LastName);
            var user = await _authService.GetUserByEmailAsync(request.Email);
            
            if (user == null)
            {
                return BadRequest(ApiResponse<AuthResponse>.ErrorResult("Registration failed."));
            }

            var authResponse = new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            return Ok(ApiResponse<AuthResponse>.SuccessResult(authResponse));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<AuthResponse>.ErrorResult(ex.Message));
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            var user = await _authService.GetUserByEmailAsync(request.Email);
            
            if (user == null)
            {
                return BadRequest(ApiResponse<AuthResponse>.ErrorResult("Login failed."));
            }

            var authResponse = new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            return Ok(ApiResponse<AuthResponse>.SuccessResult(authResponse));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<AuthResponse>.ErrorResult(ex.Message));
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetCurrentUser()
    {
        var userId = GetCurrentUserId();
        var user = await _authService.GetUserByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound(ApiResponse<UserResponse>.ErrorResult("User not found."));
        }

        var userResponse = new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfileImageUrl = user.ProfileImageUrl,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };

        return Ok(ApiResponse<UserResponse>.SuccessResult(userResponse));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}