using BLL.GameActions.AcceptTradeOfferAction;
using BLL.GameActions.ClaimCornerAction;
using BLL.GameActions.ClaimEdgeAction;
using BLL.GameActions.ClaimInitialCornerAction;
using BLL.GameActions.ClaimInitialEdgeAction;
using BLL.GameActions.EndTurnAction;
using BLL.GameActions.MoveRobberAction;
using BLL.GameActions.RegisterTradeOfferAction;
using BLL.GameActions.RegisterTradeOfferWithBankAction;
using BLL.GameActions.RollDiceAction;
using BLL.GameActions.ThrowResourcesAction;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions
{
    public class GameActionHandler
	{
        private readonly IClaimCornerAction _claimCornerAction;
        private readonly IClaimEdgeAction _claimEdgeAction;
        private readonly IEndTurnAction _endTurnAction;
        private readonly IClaimInitialCornerAction _claimInitialCornerAction;
        private readonly IClaimInitialEdgeAction _claimInitialEdgeAction;
        private readonly IMoveRobberAction _moveRobberAction;
        private readonly IRegisterTradeOfferWithBankAction _registerTradeOfferWithBankAction;
        private readonly IRegisterTradeOfferAction _registerTradeOfferAction;
        private readonly IAcceptTradeOfferAction _acceptTradeOfferAction;
        private readonly IThrowResourcesAction _throwResourcesAction;
        private readonly IDiceRollAction _diceRollAction;

		public GameActionHandler()
        {

        }
        public GameServiceResponses ExecuteClaimCornerAction(Game game, int cornerId, string name)
        {
            return _claimCornerAction.Execute(game, cornerId, name);
        }
        public GameServiceResponses ExecuteClaimEdgeAction(Game game, int edgeId, string name)
        {
			return _claimEdgeAction.Execute(game, edgeId, name);
		}
        public GameServiceResponses ExecuteEndTurnAction(Game game, string name)
        {
            return _endTurnAction.Execute(game, name);
        }
        public GameServiceResponses ExecuteClaimInitialCornerAction(Game game, int cornerId, string name)
        {
            return _claimInitialCornerAction.Execute(game, cornerId, name);
        }
        public GameServiceResponses ExecuteClaimInitialEdgeAction(Game game, int edgeId, string name)
        {
            return _claimInitialEdgeAction.Execute(game, edgeId, name);
        }
        public GameServiceResponses ExecuteMoveRobberAction(Game game, int fieldId, string name)
        {
            return _moveRobberAction.Execute(game, fieldId, name);
        }
        public GameServiceResponses ExecuteRegisterTradeOfferWithBankAction(Game game, TradeOffer tradeOffer)
        {
            return _registerTradeOfferWithBankAction.Execute(game, tradeOffer);
        }
		public GameServiceResponses ExecuteRegisterTradeOfferAction(Game game, TradeOffer tradeOffer)
		{
			return _registerTradeOfferAction.Execute(game, tradeOffer);
		}
		public GameServiceResponses ExecuteAcceptTradeOfferAction(Game game, TradeOffer tradeOffer, string name)
		{
            return _acceptTradeOfferAction.Execute(game, tradeOffer, name);
		}
        public GameServiceResponses ExecuteThrowResourcesAction(Game game, Inventory thrownResources, string name)
        {
            return _throwResourcesAction.Execute(game, thrownResources, name);
        }
        public GameServiceResponses ExecuteDiceRollAction(Game game, string name)
        {
            return _diceRollAction.Execute(game, name);
        }
	}
}
