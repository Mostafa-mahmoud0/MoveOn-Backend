using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveOn.Core.Models.Requests.Social;
using MoveOn.Core.Models.Responses.Social;
using MoveOn.Core.Interfaces.Services;
using MoveOn.Core.Models.Common;
using System.Security.Claims;

namespace MoveOn.Api.Controllers.Social;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CommentResponse>>> CreateComment([FromBody] CreateCommentRequest request, [FromQuery] Guid postId)
    {
        var userId = GetCurrentUserId();
        
        var comment = await _commentService.CreateCommentAsync(postId, userId, request.Content);

        var response = new CommentResponse
        {
            Id = comment.Id,
            PostId = comment.PostId,
            UserId = comment.UserId,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            User = new UserResponse
            {
                Id = comment.User.Id,
                Email = comment.User.Email,
                FirstName = comment.User.FirstName,
                LastName = comment.User.LastName,
                ProfileImageUrl = comment.User.ProfileImageUrl,
                Role = comment.User.Role.ToString(),
                CreatedAt = comment.User.CreatedAt
            }
        };

        return Ok(ApiResponse<CommentResponse>.SuccessResult(response));
    }

    [HttpGet("post/{postId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<CommentResponse>>>> GetPostComments(Guid postId)
    {
        var comments = await _commentService.GetPostCommentsAsync(postId);

        var response = comments.Select(c => new CommentResponse
        {
            Id = c.Id,
            PostId = c.PostId,
            UserId = c.UserId,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            User = new UserResponse
            {
                Id = c.User.Id,
                Email = c.User.Email,
                FirstName = c.User.FirstName,
                LastName = c.User.LastName,
                ProfileImageUrl = c.User.ProfileImageUrl,
                Role = c.User.Role.ToString(),
                CreatedAt = c.User.CreatedAt
            }
        });

        return Ok(ApiResponse<IEnumerable<CommentResponse>>.SuccessResult(response));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}