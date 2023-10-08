using Catan.Shared.Model.GameState;
using Catan.Shared.Model.GameState.Inventory;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.ThrowResourcesAction.Implementations
{
    public class BaseThrowResourcesAction : IThrowResourcesAction
	{
		public GameServiceResponses Execute(Game game, Inventory thrownResources, string name)
		{
			var player = game.PlayerList.FirstOrDefault(p => p.Name == name);
			if (player is null)
			{
				return GameServiceResponses.InvalidMember;
			}
			if (player.Inventory.GetAllResourcesCount() / 2 == thrownResources.GetAllResourcesCount())
			{
				if (!player.Inventory.HasSufficientResources(thrownResources))
				{
					return GameServiceResponses.InvalidResourcesHaveBeenThrown;
				}
				player.Inventory.RemoveResources(thrownResources);
				game.PlayersWithSevenOrMoreResources.Remove(player);
				if (game.PlayersWithSevenOrMoreResources.Count() == 0)
				{
					game.ResolveResourceCount = false;
					foreach (var p in game.PlayerList)
					{
						if (p.Inventory.GetAllResourcesCount() >= 7)
						{
							game.PlayersWithSevenOrMoreResources.Add(p);
						}
					}
				}
				return GameServiceResponses.Success;
			}
			return GameServiceResponses.NotEnoughResourceThrown;
		}
	}
}
