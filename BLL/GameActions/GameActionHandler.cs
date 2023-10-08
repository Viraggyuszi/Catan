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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions
{
    public class GameActionHandler
    {
		public required IClaimCornerAction ClaimCornerAction { private get; init; }
		public required IClaimEdgeAction ClaimEdgeAction { private get; init; }
		public required IEndTurnAction EndTurnAction { private get; init; }
		public required IClaimInitialCornerAction ClaimInitialCornerAction { private get; init; }
		public required IClaimInitialEdgeAction ClaimInitialEdgeAction { private get; init; }
		public required IMoveRobberAction MoveRobberAction { private get; init; }
		public required IRegisterTradeOfferWithBankAction RegisterTradeOfferWithBankAction { private get; init; }
		public required IRegisterTradeOfferAction RegisterTradeOfferAction { private get; init; }
		public required IAcceptTradeOfferAction AcceptTradeOfferAction { private get; init; }
		public required IThrowResourcesAction ThrowResourcesAction { private get; init; }
		public required IDiceRollAction RollDiceAction { private get; init; }

		public virtual GameServiceResponses ExecuteClaimCornerAction(Game game, int cornerId, string name)
        {
            return ClaimCornerAction.Execute(game, cornerId, name);
        }
        public virtual GameServiceResponses ExecuteClaimEdgeAction(Game game, int edgeId, string name)
        {
            return ClaimEdgeAction.Execute(game, edgeId, name);
        }
        public virtual GameServiceResponses ExecuteEndTurnAction(Game game, string name)
        {
            return EndTurnAction.Execute(game, name);
        }
        public virtual GameServiceResponses ExecuteClaimInitialCornerAction(Game game, int cornerId, string name)
        {
            return ClaimInitialCornerAction.Execute(game, cornerId, name);
        }
        public virtual GameServiceResponses ExecuteClaimInitialEdgeAction(Game game, int edgeId, string name)
        {
            return ClaimInitialEdgeAction.Execute(game, edgeId, name);
        }
        public virtual GameServiceResponses ExecuteMoveRobberAction(Game game, int fieldId, string name)
        {
            return MoveRobberAction.Execute(game, fieldId, name);
        }
        public virtual GameServiceResponses ExecuteRegisterTradeOfferWithBankAction(Game game, TradeOffer tradeOffer)
        {
            return RegisterTradeOfferWithBankAction.Execute(game, tradeOffer);
        }
        public virtual GameServiceResponses ExecuteRegisterTradeOfferAction(Game game, TradeOffer tradeOffer)
        {
            return RegisterTradeOfferAction.Execute(game, tradeOffer);
        }
        public virtual GameServiceResponses ExecuteAcceptTradeOfferAction(Game game, TradeOffer tradeOffer, string name)
        {
            return AcceptTradeOfferAction.Execute(game, tradeOffer, name);
        }
        public virtual GameServiceResponses ExecuteThrowResourcesAction(Game game, Inventory thrownResources, string name)
        {
            return ThrowResourcesAction.Execute(game, thrownResources, name);
        }
        public virtual GameServiceResponses ExecuteDiceRollAction(Game game, string name)
        {
            return RollDiceAction.Execute(game, name);
        }
    }
}
