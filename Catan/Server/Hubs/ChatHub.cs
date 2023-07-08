using Microsoft.AspNetCore.SignalR;

namespace Catan.Server.Hubs
{
	public class ChatHub : Hub
	{
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task SendMessageToGroup(string groupName, string name, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage",name, message);
        }
    }
}
