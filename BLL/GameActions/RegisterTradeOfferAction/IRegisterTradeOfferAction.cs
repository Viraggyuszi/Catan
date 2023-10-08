using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.RegisterTradeOfferAction
{
    public interface IRegisterTradeOfferAction
	{
		public GameServiceResponses Execute(Game game, TradeOffer offer);
	}
}
