using BLL.GameActions.RegisterTradeOfferAction;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.RegisterTradeOfferAction.Implementations
{
    public class BaseRegisterTradeOfferAction : IRegisterTradeOfferAction
    {
        public GameServiceResponses Execute(Game game, TradeOffer offer)
        {
            if (game.ActivePlayer.Name != offer.Owner.Name)
            {
                return GameServiceResponses.InvalidMember;
            }
			if (game.HaveToRollDices)
			{
				return GameServiceResponses.RollDicesFirst;
			}
			if (game.TradeOfferList.Count < 3)
            {
                if (game.ActivePlayer.Inventory.HasSufficientResources(offer.OwnerOffer))
                {
                    int lastId=game.TradeOfferList.Count;
                    offer.Id = lastId++;
                    game.TradeOfferList.Add(offer);
                    return GameServiceResponses.Success;
                }
                else
                {
                    return GameServiceResponses.NotEnoughResourcesToCreateTrade;
                }
            }
            else
            {
                return GameServiceResponses.TradeListFull;
            }
        }
    }
}
