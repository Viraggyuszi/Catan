﻿using BLL.GameActions.AcceptTradeOfferAction;
using BLL.GameActions.BuildCityAction;
using BLL.GameActions.BuildInitialRoadAction;
using BLL.GameActions.BuildInitialShipAction;
using BLL.GameActions.BuildInitialVillageAction;
using BLL.GameActions.BuildRoadAction;
using BLL.GameActions.BuildShipAction;
using BLL.GameActions.BuildVillageAction;
using BLL.GameActions.EndTurnAction;
using BLL.GameActions.MoveRobberAction;
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
		public required IBuildVillageAction? BuildVillageAction { private get; init; }
		public required IBuildCityAction? BuildCityAction { private get; init; }
		public required IBuildRoadAction? BuildRoadAction { private get; init; }
		public required IEndTurnAction? EndTurnAction { private get; init; }
		public required IBuildInitialVillageAction? BuildInitialVillageAction { private get; init; }
		public required IBuildInitialRoadAction? BuildInitialRoadAction { private get; init; }
        public required IBuildInitialShipAction? BuildInitialShipAction { private get; init; }
		public required IMoveRobberAction? MoveRobberAction { private get; init; }
		public required IRegisterTradeOfferWithBankAction? RegisterTradeOfferWithBankAction { private get; init; }
		public required IRegisterTradeOfferAction? RegisterTradeOfferAction { private get; init; }
		public required IAcceptTradeOfferAction? AcceptTradeOfferAction { private get; init; }
		public required IThrowResourcesAction? ThrowResourcesAction { private get; init; }
		public required IDiceRollAction? RollDiceAction { private get; init; }
        public required IBuildShipAction? BuildShipAction { private get; init; }

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
