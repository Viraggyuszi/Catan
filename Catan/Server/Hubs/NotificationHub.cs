using Microsoft.AspNetCore.SignalR;

namespace Catan.Server.Hubs
{
	public class NotificationHub : Hub
	{
		public async Task JoinGroup(string groupName)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
		}

		public async Task LeaveGroup(string groupName)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
		}

		public async Task LobbiesChanged()
		{
			await Clients.Group("default").SendAsync("RefreshLobbiesList");
		}

		public async Task GameHasStarted(string groupName)
		{
			await Clients.Group(groupName).SendAsync("StartGame",groupName);
		}
		public override async Task OnConnectedAsync()
		{
			await base.OnConnectedAsync();
		}
	}
}
