using BLL.GameActions.EndTurnAction;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.EndTurnAction.Implementations
{
    public class BaseEndTurnAction : IEndTurnAction
    {
        public GameServiceResponses Execute(Game game, string name)
        {
            game.ActivePlayer = game.PlayerList[(game.PlayerList.IndexOf(game.ActivePlayer) + 1) % game.PlayerList.Count];
            game.TradeOfferList.Clear();
            if (game.InitialRound)
            {
                game.InitialRoundCount--;
                if (game.InitialRoundCount <= 0)
                {
                    game.InitialRound = false;
                    foreach (var corner in game.GameMap.CornerList)
                    {
                        if (corner.Level > 0)
                        {
                            foreach (var field in corner.Fields)
                            {
                                corner.Player.Inventory.AddResource(field.Type, corner.Level);
                            }
                        }
                    }
                }
                else
                {
                    game.ActivePlayerCanPlaceInitialVillage = true;
                    game.ActivePlayerCanPlaceInitialRoad = true;
                }
            }
            return GameServiceResponses.Success;
        }
    }
}
