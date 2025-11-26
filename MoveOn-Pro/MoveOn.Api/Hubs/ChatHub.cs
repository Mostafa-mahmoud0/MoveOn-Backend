using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MoveOn.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private static readonly Dictionary<string, string> UserConnections = new();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            UserConnections[userId] = Context.ConnectionId;
            await Clients.Others.SendAsync("UserOnline", userId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            UserConnections.Remove(userId);
            await Clients.Others.SendAsync("UserOffline", userId);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageToUser(string receiverId, string message)
    {
        var senderId = Context.UserIdentifier;
        if (senderId != null && UserConnections.TryGetValue(receiverId, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", new
            {
                SenderId = senderId,
                Message = message,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task SendGroupMessage(string groupName, string message)
    {
        var senderId = Context.UserIdentifier;
        await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", new
        {
            GroupName = groupName,
            SenderId = senderId,
            Message = message,
            Timestamp = DateTime.UtcNow
        });
    }
}