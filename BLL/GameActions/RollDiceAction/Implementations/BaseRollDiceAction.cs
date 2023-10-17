using BLL.GameActions.RollDiceAction;
using Catan.Shared.Model.GameState;
using Catan.Shared.Model.GameState.Dice;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.RollDiceAction.Implementations
{
    public class BaseRollDiceAction : IDiceRollAction
    {
        public GameServiceResponses Execute(Game game, string name)
        {
            if (game.ActivePlayer.Name != name)
            {
                return GameServiceResponses.InvalidMember;
            }
            int sum = 0;
            foreach (var dice in game.Dices)
            {
                var value = dice.RollDice();
                sum += Convert.ToInt32(value);
            }
            if (sum == 7)
            {
                game.RobberNeedsMove = true;
                game.ResolveResourceCount = true;
                game.PlayersWithSevenOrMoreResources.Clear();
                foreach (var player in game.PlayerList)
                {
                    if (player.Inventory.GetAllResourcesCount() >= 7)
                    {
                        game.PlayersWithSevenOrMoreResources.Add(player);
                    }
                }
                return GameServiceResponses.SuccessWithSevenRoll;
            }
            else
            {
                foreach (var field in game.GameMap.FieldList)
                {
                    if (field.Number == sum && !field.IsRobbed)
                    {
                        foreach (var corner in field.Corners)
                        {
                            if (corner.Level > 0)
                            {
                                corner.Player.Inventory.AddResource(field.Type, corner.Level);
                            }
                        }
                    }
                }
            }
            return GameServiceResponses.Success;
        }
    }
}
