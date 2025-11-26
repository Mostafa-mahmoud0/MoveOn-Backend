using MoveOn.Core.Entities;
using System.Threading.Tasks;

namespace MoveOn.Core.Interfaces;

public interface IConversationService
{
    Task<Conversation> CreateConversationAsync(Guid user1Id, Guid user2Id);
    Task<IEnumerable<Conversation>> GetUserConversationsAsync(Guid userId);
    Task<Conversation?> GetConversationAsync(Guid id, Guid userId);
    Task<Message> SendMessageAsync(Guid conversationId, Guid senderId, Guid receiverId, string content);
    Task<IEnumerable<Message>> GetConversationMessagesAsync(Guid conversationId, Guid userId);
    Task MarkMessageAsReadAsync(Guid messageId, Guid userId);
}