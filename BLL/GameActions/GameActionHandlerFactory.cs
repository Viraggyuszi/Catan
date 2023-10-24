using BLL.GameActions.AcceptTradeOfferAction.Implementations;
using BLL.GameActions.BuildCityAction.Implementations;
using BLL.GameActions.BuildInitialRoadAction.Implementations;
using BLL.GameActions.BuildInitialShipAction.Implementations;
using BLL.GameActions.BuildInitialVillageAction.Implementations;
using BLL.GameActions.BuildRoadAction.Implementations;
using BLL.GameActions.BuildShipAction.Implementations;
using BLL.GameActions.BuildVillageAction.Implementations;
using BLL.GameActions.EndTurnAction.Implementations;
using BLL.GameActions.MoveRobberAction.Implementations;
using BLL.GameActions.RegisterTradeOfferAction.Implementations;
using BLL.GameActions.RegisterTradeOfferWithBankAction.Implementations;
using BLL.GameActions.RollDiceAction.Implementations;
using BLL.GameActions.ThrowResourcesAction.Implementations;
using Catan.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions
{
    public class GameActionHandlerFactory
    {
        public GameActionHandler CreateActionHandler(Dictionary<GameType,bool> DLCs)
        {
            /*
             * Since only 1 DLC is available, the choosing procedure is easy
             * but have to be improved in the future.
             */
            GameType gameType;
            if (DLCs.GetValueOrDefault(GameType.Seafarer))
            {
                gameType = GameType.Seafarer;
            }
            else
            {
                gameType= GameType.Base;
            }
            return gameType switch
            {
                GameType.Base => CreateBaseActionHandler(),
                GameType.Seafarer => CreateSeafarerActionHandler(),
                _ => throw new Exception("Not specified game type"),
            };
        }
		private GameActionHandler CreateBaseActionHandler()
        {
            var result = new GameActionHandler()
            {
                AcceptTradeOfferAction = new BaseAcceptTradeOfferAction(),
                BuildVillageAction = new BaseBuildVillageAction(),
                BuildCityAction = new BaseBuildCityAction(),
                BuildRoadAction = new BaseBuildRoadAction(),
                BuildInitialVillageAction = new BaseBuildInitialVillageAction(),
                BuildInitialRoadAction = new BaseBuildInitialRoadAction(),
                RollDiceAction = new BaseRollDiceAction(),
                EndTurnAction = new BaseEndTurnAction(),
                MoveRobberAction = new BaseMoveRobberAction(),
                RegisterTradeOfferAction = new BaseRegisterTradeOfferAction(),
                RegisterTradeOfferWithBankAction = new BaseRegisterTradeOfferWithBankAction(),
                ThrowResourcesAction = new BaseThrowResourcesAction(),
                BuildShipAction = null,
                BuildInitialShipAction = null
            };
            return result;
        }
        private GameActionHandler CreateSeafarerActionHandler()
        {
            var result = new GameActionHandler()
            {
                AcceptTradeOfferAction = new BaseAcceptTradeOfferAction(),
                BuildVillageAction = new BaseBuildVillageAction(),
                BuildCityAction = new BaseBuildCityAction(),
                BuildRoadAction = new SeafarerBuildRoadAction(),
                BuildInitialVillageAction = new BaseBuildInitialVillageAction(),
                BuildInitialRoadAction = new BaseBuildInitialRoadAction(),
                RollDiceAction = new BaseRollDiceAction(),
                EndTurnAction = new BaseEndTurnAction(),
                MoveRobberAction = new BaseMoveRobberAction(),
                RegisterTradeOfferAction = new BaseRegisterTradeOfferAction(),
                RegisterTradeOfferWithBankAction = new BaseRegisterTradeOfferWithBankAction(),
                ThrowResourcesAction = new BaseThrowResourcesAction(),
                BuildShipAction = new SeafarerBuildShipAction(),
                BuildInitialShipAction=new SeafarerBuildInitialShipAction()
			};
			return result;
		}
    }
}
