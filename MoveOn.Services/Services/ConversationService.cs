using Microsoft.EntityFrameworkCore;
using MoveOn.Core.Entities;
using MoveOn.Core.Interfaces;
using MoveOn.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoveOn.Services.Services;

public class ConversationService : IConversationService
{
    private readonly MoveOnDbContext _context;

    public ConversationService(MoveOnDbContext context)
    {
        _context = context;
    }

    public async Task<Conversation> CreateConversationAsync(Guid user1Id, Guid user2Id)
    {
        // Check if conversation already exists
        var existingConversation = await _context.Conversations
            .FirstOrDefaultAsync(c => 
                (c.User1Id == user1Id && c.User2Id == user2Id) ||
                (c.User1Id == user2Id && c.User2Id == user1Id));

        if (existingConversation != null)
        {
            return existingConversation;
        }

        var conversation = new Conversation
        {
            User1Id = user1Id,
            User2Id = user2Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();

        // Load users for response
        await _context.Entry(conversation)
            .Reference(c => c.User1)
            .LoadAsync();

        await _context.Entry(conversation)
            .Reference(c => c.User2)
            .LoadAsync();

        return conversation;
    }

    public async Task<IEnumerable<Conversation>> GetUserConversationsAsync(Guid userId)
    {
        return await _context.Conversations
            .Where(c => c.User1Id == userId || c.User2Id == userId)
            .Include(c => c.User1)
            .Include(c => c.User2)
            .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync();
    }

    public async Task<Conversation?> GetConversationAsync(Guid id, Guid userId)
    {
        return await _context.Conversations
            .Where(c => (c.Id == id) && (c.User1Id == userId || c.User2Id == userId))
            .Include(c => c.User1)
            .Include(c => c.User2)
            .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync();
    }

    public async Task<Message> SendMessageAsync(Guid conversationId, Guid senderId, Guid receiverId, string content)
    {
        // Verify conversation exists and user is part of it
        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId && 
                (c.User1Id == senderId || c.User2Id == senderId));

        if (conversation == null)
        {
            throw new ArgumentException("Conversation not found or access denied.");
        }

        var message = new Message
        {
            ConversationId = conversationId,
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);

        // Update conversation timestamp
        conversation.UpdatedAt = DateTime.UtcNow;
        _context.Conversations.Update(conversation);

        await _context.SaveChangesAsync();

        // Load navigation properties for response
        await _context.Entry(message)
            .Reference(m => m.Sender)
            .LoadAsync();

        await _context.Entry(message)
            .Reference(m => m.Receiver)
            .LoadAsync();

        return message;
    }

    public async Task<IEnumerable<Message>> GetConversationMessagesAsync(Guid conversationId, Guid userId)
    {
        // Verify user is part of the conversation
        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId && 
                (c.User1Id == userId || c.User2Id == userId));

        if (conversation == null)
        {
            throw new ArgumentException("Conversation not found or access denied.");
        }

        return await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkMessageAsReadAsync(Guid messageId, Guid userId)
    {
        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.Id == messageId && m.ReceiverId == userId);

        if (message != null && !message.IsRead)
        {
            message.IsRead = true;
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }
    }
}