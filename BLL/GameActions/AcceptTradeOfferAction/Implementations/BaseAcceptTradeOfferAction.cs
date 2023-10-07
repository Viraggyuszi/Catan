using Catan.Shared.Model;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.AcceptTradeOfferAction.Implementations
{
	public class BaseAcceptTradeOfferAction : IAcceptTradeOfferAction
	{
		public GameServiceResponses Execute(Game game, TradeOffer offer, string name)
		{
			if (offer.Owner.Name == name)
			{
				return GameServiceResponses.InvalidMember;
			}
			var player = game.PlayerList.FirstOrDefault(p => p.Name == name);
			if (player is null)
			{
				return GameServiceResponses.InvalidMember;
			}
			if (player.Inventory.HasSufficientResources(offer.TargetOffer))
			{
				offer.Owner.Inventory.RemoveResources(offer.OwnerOffer);
				offer.Owner.Inventory.AddResources(offer.TargetOffer);
				player.Inventory.RemoveResources(offer.TargetOffer);
				player.Inventory.AddResources(offer.OwnerOffer);
				game.TradeOfferList.Remove(offer);
				return GameServiceResponses.Success;
			}
			return GameServiceResponses.NotEnoughResourcesToAcceptTrade;
		}
	}
}
