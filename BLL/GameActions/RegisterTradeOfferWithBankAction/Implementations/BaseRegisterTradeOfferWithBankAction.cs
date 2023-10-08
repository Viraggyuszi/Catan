using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.RegisterTradeOfferWithBankAction.Implementations
{
    public class BaseRegisterTradeOfferWithBankAction : IRegisterTradeOfferWithBankAction
	{
		public GameServiceResponses Execute(Game game, TradeOffer offer)
		{
			if (game.ActivePlayer.Name != offer.Owner.Name)
			{
				return GameServiceResponses.InvalidMember;
			}
			if (game.ActivePlayer.Inventory.HasSufficientResources(offer.OwnerOffer))
			{
				if (offer.OwnerOffer.GetAllResourcesCount() == offer.TargetOffer.GetAllResourcesCount() * 4)
				{
					offer.Owner.Inventory.RemoveResources(offer.OwnerOffer);
					offer.Owner.Inventory.AddResources(offer.TargetOffer);
					return GameServiceResponses.Success;
				}
				return GameServiceResponses.BadResourceCountForTradingWithBank;
			}
			return GameServiceResponses.NotEnoughResourcesToCreateTrade;
		}
	}
}
