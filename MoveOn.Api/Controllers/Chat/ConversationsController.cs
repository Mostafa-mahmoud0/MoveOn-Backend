using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoveOn.Core.Models.Requests.Chat;
using MoveOn.Core.Models.Responses.Chat;
using MoveOn.Core.Interfaces.Services;
using MoveOn.Core.Models.Common;
using System.Security.Claims;

namespace MoveOn.Api.Controllers.Chat;

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
    public async Task<ActionResult<ApiResponse<ConversationResponse>>> CreateConversation([FromBody] CreateConversationRequest request)
    {
        var userId = GetCurrentUserId();
        var conversation = await _conversationService.CreateConversationAsync(userId, request.OtherUserId);

        return Ok(ApiResponse<ConversationResponse>.SuccessResult(MapToConversationResponse(conversation)));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ConversationResponse>>>> GetConversations()
    {
        var userId = GetCurrentUserId();
        var conversations = await _conversationService.GetUserConversationsAsync(userId);

        var response = conversations.Select(MapToConversationResponse);
        return Ok(ApiResponse<IEnumerable<ConversationResponse>>.SuccessResult(response));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ConversationResponse>>> GetConversation(Guid id)
    {
        var userId = GetCurrentUserId();
        var conversation = await _conversationService.GetConversationAsync(id, userId);

        if (conversation == null)
        {
            return NotFound(ApiResponse<ConversationResponse>.ErrorResult("Conversation not found."));
        }

        return Ok(ApiResponse<ConversationResponse>.SuccessResult(MapToConversationResponse(conversation)));
    }

    [HttpPost("{id}/messages")]
    public async Task<ActionResult<ApiResponse<MessageResponse>>> SendMessage(Guid id, [FromBody] SendMessageRequest request)
    {
        var userId = GetCurrentUserId();
        var conversation = await _conversationService.GetConversationAsync(id, userId);

        if (conversation == null)
        {
            return NotFound(ApiResponse<MessageResponse>.ErrorResult("Conversation not found."));
        }

        var receiverId = conversation.User1Id == userId ? conversation.User2Id : conversation.User1Id;
        var message = await _conversationService.SendMessageAsync(id, userId, receiverId, request.Content);

        return Ok(ApiResponse<MessageResponse>.SuccessResult(MapToMessageResponse(message)));
    }

    [HttpGet("{id}/messages")]
    public async Task<ActionResult<ApiResponse<IEnumerable<MessageResponse>>>> GetMessages(Guid id)
    {
        var userId = GetCurrentUserId();
        var messages = await _conversationService.GetConversationMessagesAsync(id, userId);

        var response = messages.Select(MapToMessageResponse);
        return Ok(ApiResponse<IEnumerable<MessageResponse>>.SuccessResult(response));
    }

    [HttpPost("messages/{messageId}/read")]
    public async Task<ActionResult<ApiResponse<bool>>> MarkMessageAsRead(Guid messageId)
    {
        var userId = GetCurrentUserId();
        await _conversationService.MarkMessageAsReadAsync(messageId, userId);

        return Ok(ApiResponse<bool>.SuccessResult(true, "Message marked as read."));
    }

    private ConversationResponse MapToConversationResponse(MoveOn.Core.Models.Entities.Conversation conversation)
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
                ProfileImageUrl = conversation.User1.ProfileImageUrl,
                Role = conversation.User1.Role.ToString(),
                CreatedAt = conversation.User1.CreatedAt
            },
            User2 = new UserResponse
            {
                Id = conversation.User2.Id,
                Email = conversation.User2.Email,
                FirstName = conversation.User2.FirstName,
                LastName = conversation.User2.LastName,
                ProfileImageUrl = conversation.User2.ProfileImageUrl,
                Role = conversation.User2.Role.ToString(),
                CreatedAt = conversation.User2.CreatedAt
            },
            Messages = conversation.Messages?.Select(MapToMessageResponse).ToList() ?? new List<MessageResponse>()
        };
    }

    private MessageResponse MapToMessageResponse(MoveOn.Core.Models.Entities.Message message)
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
                ProfileImageUrl = message.Sender.ProfileImageUrl,
                Role = message.Sender.Role.ToString(),
                CreatedAt = message.Sender.CreatedAt
            },
            Receiver = new UserResponse
            {
                Id = message.Receiver.Id,
                Email = message.Receiver.Email,
                FirstName = message.Receiver.FirstName,
                LastName = message.Receiver.LastName,
                ProfileImageUrl = message.Receiver.ProfileImageUrl,
                Role = message.Receiver.Role.ToString(),
                CreatedAt = message.Receiver.CreatedAt
            }
        };
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}