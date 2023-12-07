using BLL.GameActions.AcceptTradeOfferAction;
using BLL.GameActions.BuildCityAction;
using BLL.GameActions.BuildInitialRoadAction;
using BLL.GameActions.BuildInitialShipAction;
using BLL.GameActions.BuildInitialVillageAction;
using BLL.GameActions.BuildRoadAction;
using BLL.GameActions.BuildShipAction;
using BLL.GameActions.BuildVillageAction;
using BLL.GameActions.BuyCardAction;
using BLL.GameActions.EndTurnAction;
using BLL.GameActions.MoveRobberAction;
using BLL.GameActions.PlayCardAction;
using BLL.GameActions.RegisterTradeOfferAction;
using BLL.GameActions.RegisterTradeOfferWithBankAction;
using BLL.GameActions.RollDiceAction;
using BLL.GameActions.ThrowResourcesAction;
using Catan.Shared.Model.GameState;
using Catan.Shared.Model.GameState.Inventory;
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
		public IBuildVillageAction? BuildVillageAction { private get; init; } = null;
		public IBuildCityAction? BuildCityAction { private get; init; } = null;
		public IBuildRoadAction? BuildRoadAction { private get; init; } = null;
		public IEndTurnAction? EndTurnAction { private get; init; } = null;
		public IBuildInitialVillageAction? BuildInitialVillageAction { private get; init; } = null;
		public IBuildInitialRoadAction? BuildInitialRoadAction { private get; init; } = null;
        public IBuildInitialShipAction? BuildInitialShipAction { private get; init; } = null;
		public IMoveRobberAction? MoveRobberAction { private get; init; } = null;
		public IRegisterTradeOfferWithBankAction? RegisterTradeOfferWithBankAction { private get; init; } = null;
		public IRegisterTradeOfferAction? RegisterTradeOfferAction { private get; init; } = null;
		public IAcceptTradeOfferAction? AcceptTradeOfferAction { private get; init; } = null;
		public IThrowResourcesAction? ThrowResourcesAction { private get; init; } = null;
		public IDiceRollAction? RollDiceAction { private get; init; } = null;
        public IBuildShipAction? BuildShipAction { private get; init; } = null;
        public IBuyCardAction? BuyCardAction { private get; init; } = null;
        public IPlayCardAction? PlayCardAction { private get; init; } = null;

		public GameServiceResponses ExecuteBuyCardAction(Game game, string name)
		{
			if (BuyCardAction is null)
			{
				return GameServiceResponses.ForbiddenAction;
			}
			return BuyCardAction.Execute(game, name);
		}
		public GameServiceResponses ExecutePlayCardAction(Game game, CardType card, string name)
		{
			if (PlayCardAction is null)
			{
				return GameServiceResponses.ForbiddenAction;
			}
            return PlayCardAction.Execute(game, card, name);
		}

		public GameServiceResponses ExecuteBuildShipAction(Game game, int edgeId, string name)
        {
            if (BuildShipAction is null)
            {
                return GameServiceResponses.ForbiddenAction;
            }
            return BuildShipAction.Execute(game, edgeId, name);
        }
		public GameServiceResponses ExecuteBuildInitialShipAction(Game game, int cornerId, string name)
		{
			if (BuildInitialShipAction is null)
			{
				return GameServiceResponses.ForbiddenAction;
			}
			return BuildInitialShipAction.Execute(game, cornerId, name);
		}
		public GameServiceResponses ExecuteBuildVillageAction(Game game, int cornerId, string name)
		{
            if (BuildVillageAction is null)
            {
                return GameServiceResponses.ForbiddenAction;
			}
			return BuildVillageAction.Execute(game, cornerId, name);
		}
		public GameServiceResponses ExecuteBuildCityAction(Game game, int cornerId, string name)
        {
            if (BuildCityAction is null)
            {
				return GameServiceResponses.ForbiddenAction;
			}
            return BuildCityAction.Execute(game, cornerId, name) ;
        }
        public GameServiceResponses ExecuteBuildRoadAction(Game game, int edgeId, string name)
        {
            if (BuildRoadAction is null)
            {
				return GameServiceResponses.ForbiddenAction;
			}
            return BuildRoadAction.Execute(game, edgeId, name);
        }
        public GameServiceResponses ExecuteEndTurnAction(Game game, string name)
        {
            if (EndTurnAction is null)
            {
				return GameServiceResponses.ForbiddenAction;
			}
            return EndTurnAction.Execute(game, name);
        }
        public GameServiceResponses ExecuteBuildInitialVillageAction(Game game, int cornerId, string name)
        {
            if (BuildInitialVillageAction is null)
            {
                return GameServiceResponses.ForbiddenAction;
            }
            return BuildInitialVillageAction.Execute(game, cornerId, name);
        }
        public GameServiceResponses ExecuteBuildInitialRoadAction(Game game, int edgeId, string name)
        {
            if (BuildInitialRoadAction is null)
            {
				return GameServiceResponses.ForbiddenAction;
			}
            return BuildInitialRoadAction.Execute(game, edgeId, name);
        }
        public GameServiceResponses ExecuteMoveRobberAction(Game game, int fieldId, string name)
        {
            if (MoveRobberAction is null)
            {
				return GameServiceResponses.ForbiddenAction;
			}
            return MoveRobberAction.Execute(game, fieldId, name);
        }
        public GameServiceResponses ExecuteRegisterTradeOfferWithBankAction(Game game, TradeOffer tradeOffer)
        {
            if (RegisterTradeOfferWithBankAction is null)
            {
				return GameServiceResponses.ForbiddenAction;
			}
            return RegisterTradeOfferWithBankAction.Execute(game, tradeOffer);
        }
        public GameServiceResponses ExecuteRegisterTradeOfferAction(Game game, TradeOffer tradeOffer)
        {
            if (RegisterTradeOfferAction is null)
            {
				return GameServiceResponses.ForbiddenAction;
			}
            return RegisterTradeOfferAction.Execute(game, tradeOffer);
        }
        public GameServiceResponses ExecuteAcceptTradeOfferAction(Game game, TradeOffer tradeOffer, string name)
        {
            if (AcceptTradeOfferAction is null)
            {
				return GameServiceResponses.ForbiddenAction;
			}
            return AcceptTradeOfferAction.Execute(game, tradeOffer, name);
        }
        public GameServiceResponses ExecuteThrowResourcesAction(Game game, AbstractInventory thrownResources, string name)
        {
            if (ThrowResourcesAction is null)
            {
				return GameServiceResponses.ForbiddenAction;
			}
            return ThrowResourcesAction.Execute(game, thrownResources, name);
        }
        public GameServiceResponses ExecuteDiceRollAction(Game game, string name)
        {
            if (RollDiceAction is null)
            {
				return GameServiceResponses.ForbiddenAction;
			}
            return RollDiceAction.Execute(game, name);
        }
    }
}
