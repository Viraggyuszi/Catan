using BLL.GameActions.AcceptTradeOfferAction.Implementations;
using BLL.GameActions.ClaimCornerAction.Implementations;
using BLL.GameActions.ClaimEdgeAction.Implementations;
using BLL.GameActions.ClaimInitialCornerAction.Implementations;
using BLL.GameActions.ClaimInitialEdgeAction.Implementations;
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
        public GameActionHandler CreateActionHandler(GameType gameType)
        {
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
                ClaimCornerAction = new BaseClaimCornerAction(),
                ClaimEdgeAction = new BaseClaimEdgeAction(),
                ClaimInitialCornerAction = new BaseClaimInitialCornerAction(),
                ClaimInitialEdgeAction = new BaseClaimInitialEdgeAction(),
                RollDiceAction = new BaseRollDiceAction(),
                EndTurnAction = new BaseEndTurnAction(),
                MoveRobberAction = new BaseMoveRobberAction(),
                RegisterTradeOfferAction = new BaseRegisterTradeOfferAction(),
                RegisterTradeOfferWithBankAction = new BaseRegisterTradeOfferWithBankAction(),
                ThrowResourcesAction = new BaseThrowResourcesAction()
            };
            return result;
        }
        private GameActionHandler CreateSeafarerActionHandler()
        {
            throw new NotImplementedException();
        }
    }
}
