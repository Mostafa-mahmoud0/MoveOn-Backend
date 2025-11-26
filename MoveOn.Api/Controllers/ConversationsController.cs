using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveOn.Api.DTOs;
using MoveOn.Core.Interfaces;
using System.Security.Claims;

namespace MoveOn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConversationsController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationsController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpPost]
    public async Task<ActionResult<ConversationResponse>> CreateConversation([FromBody] Guid otherUserId)
    {
        var userId = GetCurrentUserId();
        var conversation = await _conversationService.CreateConversationAsync(userId, otherUserId);

        return Ok(MapToConversationResponse(conversation));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConversationResponse>>> GetConversations()
    {
        var userId = GetCurrentUserId();
        var conversations = await _conversationService.GetUserConversationsAsync(userId);

        return Ok(conversations.Select(MapToConversationResponse));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConversationResponse>> GetConversation(Guid id)
    {
        var userId = GetCurrentUserId();
        var conversation = await _conversationService.GetConversationAsync(id, userId);

        if (conversation == null)
        {
            return NotFound();
        }

        return Ok(MapToConversationResponse(conversation));
    }

    [HttpPost("{id}/messages")]
    public async Task<ActionResult<MessageResponse>> SendMessage(Guid id, [FromBody] MessageRequest request)
    {
        var userId = GetCurrentUserId();
        var conversation = await _conversationService.GetConversationAsync(id, userId);

        if (conversation == null)
        {
            return NotFound();
        }

        var receiverId = conversation.User1Id == userId ? conversation.User2Id : conversation.User1Id;
        var message = await _conversationService.SendMessageAsync(id, userId, receiverId, request.Content);

        return Ok(MapToMessageResponse(message));
    }

    [HttpGet("{id}/messages")]
    public async Task<ActionResult<IEnumerable<MessageResponse>>> GetMessages(Guid id)
    {
        var userId = GetCurrentUserId();
        var messages = await _conversationService.GetConversationMessagesAsync(id, userId);

        return Ok(messages.Select(MapToMessageResponse));
    }

    [HttpPost("messages/{messageId}/read")]
    public async Task<IActionResult> MarkMessageAsRead(Guid messageId)
    {
        var userId = GetCurrentUserId();
        await _conversationService.MarkMessageAsReadAsync(messageId, userId);

        return NoContent();
    }

    private ConversationResponse MapToConversationResponse(MoveOn.Core.Entities.Conversation conversation)
    {
        return new ConversationResponse
        {
            Id = conversation.Id,
            User1Id = conversation.User1Id,
            User2Id = conversation.User2Id,
            CreatedAt = conversation.CreatedAt,
            UpdatedAt = conversation.UpdatedAt,
            User1 = new UserResponse
            {
                Id = conversation.User1.Id,
                Email = conversation.User1.Email,
                FirstName = conversation.User1.FirstName,
                LastName = conversation.User1.LastName,
                ProfileImageUrl = conversation.User1.ProfileImageUrl
            },
            User2 = new UserResponse
            {
                Id = conversation.User2.Id,
                Email = conversation.User2.Email,
                FirstName = conversation.User2.FirstName,
                LastName = conversation.User2.LastName,
                ProfileImageUrl = conversation.User2.ProfileImageUrl
            },
            Messages = conversation.Messages?.Select(MapToMessageResponse).ToList() ?? new List<MessageResponse>()
        };
    }

    private MessageResponse MapToMessageResponse(MoveOn.Core.Entities.Message message)
    {
        return new MessageResponse
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderId = message.SenderId,
            ReceiverId = message.ReceiverId,
            Content = message.Content,
            IsRead = message.IsRead,
            CreatedAt = message.CreatedAt,
            Sender = new UserResponse
            {
                Id = message.Sender.Id,
                Email = message.Sender.Email,
                FirstName = message.Sender.FirstName,
                LastName = message.Sender.LastName,
                ProfileImageUrl = message.Sender.ProfileImageUrl
            },
            Receiver = new UserResponse
            {
                Id = message.Receiver.Id,
                Email = message.Receiver.Email,
                FirstName = message.Receiver.FirstName,
                LastName = message.Receiver.LastName,
                ProfileImageUrl = message.Receiver.ProfileImageUrl
            }
        };
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}