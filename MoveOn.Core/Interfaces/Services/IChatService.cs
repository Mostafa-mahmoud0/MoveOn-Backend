using MoveOn.Core.Models.Requests.Chat;
using MoveOn.Core.Models.Responses.Chat;

namespace MoveOn.Core.Interfaces.Services;

public interface IConversationService
{
    Task<ApiResponse<ConversationResponse>> CreateConversationAsync(Guid user1Id, Guid user2Id);
    Task<ApiResponse<ConversationListResponse>> GetUserConversationsAsync(Guid userId, GetConversationsRequest request);
    Task<ApiResponse<ConversationResponse>> GetConversationAsync(Guid id, Guid userId);
    Task<ApiResponse<MessageResponse>> SendMessageAsync(Guid conversationId, Guid senderId, SendMessageRequest request);
    Task<ApiResponse<MessageListResponse>> GetConversationMessagesAsync(Guid conversationId, Guid userId, GetMessagesRequest request);
    Task<ApiResponse<bool>> MarkMessageAsReadAsync(Guid messageId, Guid userId);
    Task<ApiResponse<bool>> MarkConversationAsReadAsync(Guid conversationId, Guid userId);
    Task<ApiResponse<UnreadMessageCountResponse>> GetUnreadMessageCountAsync(Guid userId);
    Task<ApiResponse<bool>> DeleteMessageAsync(Guid messageId, Guid userId);
    Task<ApiResponse<bool>> DeleteConversationAsync(Guid conversationId, Guid userId);
    Task<ApiResponse<bool>> ArchiveConversationAsync(Guid conversationId, Guid userId);
    Task<ApiResponse<bool>> MuteConversationAsync(Guid conversationId, Guid userId);
    Task<ApiResponse<bool>> UnmuteConversationAsync(Guid conversationId, Guid userId);
    Task<ApiResponse<ConversationListResponse>> GetArchivedConversationsAsync(Guid userId, int page = 1, int pageSize = 20);
    Task<ApiResponse<ConversationListResponse>> GetMutedConversationsAsync(Guid userId, int page = 1, int pageSize = 20);
}

public interface IChatService
{
    Task<ApiResponse<bool>> SendTypingNotificationAsync(Guid conversationId, Guid userId);
    Task<ApiResponse<bool>> SendOnlineStatusAsync(Guid userId, bool isOnline);
    Task<ApiResponse<bool>> SendDeliveryReceiptAsync(Guid messageId, Guid userId);
    Task<ApiResponse<bool>> SendReadReceiptAsync(Guid messageId, Guid userId);
    Task<ApiResponse<bool>> ForwardMessageAsync(Guid messageId, Guid userId, Guid targetConversationId);
    Task<ApiResponse<bool>> ShareMessageAsync(Guid messageId, Guid userId, List<Guid> targetUserIds);
    Task<ApiResponse<bool>> ReportMessageAsync(Guid messageId, Guid userId, string reason);
    Task<ApiResponse<bool>> BlockUserAsync(Guid userId, Guid blockUserId);
    Task<ApiResponse<bool>> UnblockUserAsync(Guid userId, Guid blockUserId);
    Task<ApiResponse<IEnumerable<UserResponse>>> GetBlockedUsersAsync(Guid userId, int page = 1, int pageSize = 20);
}