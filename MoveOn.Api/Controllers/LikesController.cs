using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveOn.Core.Interfaces;
using System.Security.Claims;

namespace MoveOn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LikesController : ControllerBase
{
    private readonly ILikeService _likeService;

    public LikesController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [HttpPost("{postId}")]
    public async Task<IActionResult> LikePost(Guid postId)
    {
        var userId = GetCurrentUserId();
        var isLiked = await _likeService.ToggleLikeAsync(postId, userId);

        return Ok(new { IsLiked = isLiked, LikeCount = await _likeService.GetPostLikesCountAsync(postId) });
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> UnlikePost(Guid postId)
    {
        var userId = GetCurrentUserId();
        var isLiked = await _likeService.ToggleLikeAsync(postId, userId);

        return Ok(new { IsLiked = isLiked, LikeCount = await _likeService.GetPostLikesCountAsync(postId) });
    }

    [HttpGet("post/{postId}/count")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPostLikesCount(Guid postId)
    {
        var count = await _likeService.GetPostLikesCountAsync(postId);
        return Ok(new { LikeCount = count });
    }

    [HttpGet("post/{postId}/status")]
    public async Task<IActionResult> GetPostLikeStatus(Guid postId)
    {
        var userId = GetCurrentUserId();
        var isLiked = await _likeService.IsPostLikedByUserAsync(postId, userId);
        var count = await _likeService.GetPostLikesCountAsync(postId);

        return Ok(new { IsLiked = isLiked, LikeCount = count });
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}