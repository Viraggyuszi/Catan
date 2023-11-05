using BLL.GameActions.AcceptTradeOfferAction;
using Catan.Shared.Model.GameState;
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
            var targetPlayer = game.PlayerList.FirstOrDefault(p => p.Name == name);
            if (targetPlayer is null)
            {
                return GameServiceResponses.InvalidMember;
            }
            var ownerPlayer = game.PlayerList.FirstOrDefault(p => p.Name == offer.Owner.Name);
			if (ownerPlayer is null)
			{
				return GameServiceResponses.InvalidMember;
			}
			if (targetPlayer.Inventory.HasSufficientResources(offer.TargetOffer))
            {
				ownerPlayer.Inventory.RemoveResources(offer.OwnerOffer);
				ownerPlayer.Inventory.AddResources(offer.TargetOffer);
                targetPlayer.Inventory.RemoveResources(offer.TargetOffer);
                targetPlayer.Inventory.AddResources(offer.OwnerOffer);
                game.TradeOfferList.Remove(game.TradeOfferList.First(to => to.Id == offer.Id));
                return GameServiceResponses.Success;
            }
            return GameServiceResponses.NotEnoughResourcesToAcceptTrade;
        }
    }
}
