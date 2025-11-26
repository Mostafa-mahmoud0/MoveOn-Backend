using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveOn.Api.DTOs;
using MoveOn.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MoveOn.Api.Controllers;

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
    public async Task<ActionResult<CommentResponse>> CreateComment([FromBody] CommentRequest request, [FromQuery] Guid postId)
    {
        var userId = GetCurrentUserId();
        
        var comment = await _commentService.CreateCommentAsync(postId, userId, request.Content);

        return Ok(new CommentResponse
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
                ProfileImageUrl = comment.User.ProfileImageUrl
            }
        });
    }

    [HttpGet("post/{postId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CommentResponse>>> GetPostComments(Guid postId)
    {
        var comments = await _commentService.GetPostCommentsAsync(postId);

        return Ok(comments.Select(c => new CommentResponse
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
                ProfileImageUrl = c.User.ProfileImageUrl
            }
        }));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}