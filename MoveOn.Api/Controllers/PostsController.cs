using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveOn.Api.DTOs;
using MoveOn.Core.Interfaces;
using System.Security.Claims;

namespace MoveOn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    public async Task<ActionResult<PostResponse>> CreatePost([FromBody] PostRequest request)
    {
        var userId = GetCurrentUserId();
        var post = await _postService.CreatePostAsync(userId, request.Content);

        return Ok(MapToPostResponse(post));
    }

    [HttpPost("with-image")]
    public async Task<ActionResult<PostResponse>> CreatePostWithImage([FromForm] PostWithImageRequest request)
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
        return Ok(MapToPostResponse(post));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<PostResponse>>> GetPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var posts = await _postService.GetPostsAsync(page, pageSize);
        var userId = GetCurrentUserId();

        var postResponses = posts.Select(post => MapToPostResponse(post, userId)).ToList();
        return Ok(postResponses);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<PostResponse>> GetPost(Guid id)
    {
        var post = await _postService.GetPostAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        var userId = GetCurrentUserId();
        return Ok(MapToPostResponse(post, userId));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        var userId = GetCurrentUserId();
        var success = await _postService.DeletePostAsync(id, userId);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    private PostResponse MapToPostResponse(MoveOn.Core.Entities.Post post, Guid? currentUserId = null)
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
                ProfileImageUrl = post.User.ProfileImageUrl
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
                    ProfileImageUrl = c.User.ProfileImageUrl
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