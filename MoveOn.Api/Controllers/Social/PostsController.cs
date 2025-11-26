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
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ILikeService _likeService;

    public PostsController(IPostService postService, ILikeService likeService)
    {
        _postService = postService;
        _likeService = likeService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApiResponse<PostResponse>>> CreatePost([FromBody] CreatePostRequest request)
    {
        var userId = GetCurrentUserId();
        var post = await _postService.CreatePostAsync(userId, request.Content);

        return Ok(ApiResponse<PostResponse>.SuccessResult(MapToPostResponse(post, userId)));
    }

    [HttpPost("with-image")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<PostResponse>>> CreatePostWithImage([FromForm] CreatePostWithImageRequest request)
    {
        var userId = GetCurrentUserId();
        string? imageUrl = null;

        if (request.Image != null)
        {
            // Note: In a real implementation, you would inject IImageUploadService
            // For now, we'll simulate image upload by returning a placeholder URL
            imageUrl = $"/uploads/{Guid.NewGuid()}{System.IO.Path.GetExtension(request.Image.FileName)}";
        }

        var post = await _postService.CreatePostAsync(userId, request.Content, imageUrl);
        return Ok(ApiResponse<PostResponse>.SuccessResult(MapToPostResponse(post, userId)));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PagedResponse<PostResponse>>>> GetPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var posts = await _postService.GetPostsAsync(page, pageSize);
        var userId = GetCurrentUserId();

        var postResponses = posts.Select(post => MapToPostResponse(post, userId)).ToList();
        
        var response = new PagedResponse<PostResponse>
        {
            Data = postResponses,
            Page = page,
            PageSize = pageSize,
            TotalCount = postResponses.Count
        };

        return Ok(ApiResponse<PagedResponse<PostResponse>>.SuccessResult(response));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PostResponse>>> GetPost(Guid id)
    {
        var post = await _postService.GetPostAsync(id);
        if (post == null)
        {
            return NotFound(ApiResponse<PostResponse>.ErrorResult("Post not found."));
        }

        var userId = GetCurrentUserId();
        return Ok(ApiResponse<PostResponse>.SuccessResult(MapToPostResponse(post, userId)));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<bool>>> DeletePost(Guid id)
    {
        var userId = GetCurrentUserId();
        var success = await _postService.DeletePostAsync(id, userId);

        if (!success)
        {
            return NotFound(ApiResponse<bool>.ErrorResult("Post not found."));
        }

        return Ok(ApiResponse<bool>.SuccessResult(true, "Post deleted successfully."));
    }

    private PostResponse MapToPostResponse(MoveOn.Core.Models.Entities.Post post, Guid? currentUserId = null)
    {
        return new PostResponse
        {
            Id = post.Id,
            UserId = post.UserId,
            Content = post.Content,
            ImageUrl = post.ImageUrl,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            User = new UserResponse
            {
                Id = post.User.Id,
                Email = post.User.Email,
                FirstName = post.User.FirstName,
                LastName = post.User.LastName,
                ProfileImageUrl = post.User.ProfileImageUrl,
                Role = post.User.Role.ToString(),
                CreatedAt = post.User.CreatedAt
            },
            Comments = post.Comments?.Select(c => new CommentResponse
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
            }).ToList() ?? new List<CommentResponse>(),
            LikeCount = post.Likes?.Count ?? 0,
            IsLikedByCurrentUser = currentUserId.HasValue && post.Likes?.Any(l => l.UserId == currentUserId.Value) == true
        };
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}