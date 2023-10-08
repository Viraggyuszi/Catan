using BLL.GameActions.MoveRobberAction;
using Catan.Shared.Model.GameMap;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.MoveRobberAction.Implementations
{
    public class BaseMoveRobberAction : IMoveRobberAction
    {
        public GameServiceResponses Execute(Game game, int fieldId, string name)
        {
            if (game.ActivePlayer.Name != name)
            {
                return GameServiceResponses.InvalidMember;
            }
            var newRobbedField = game.GameMap.FieldList.FirstOrDefault(f => f.Id == fieldId);
            if (newRobbedField is null)
            {
                return GameServiceResponses.InvalidField;
            }
            var currentlyRobbedfield = game.GameMap.FieldList.FirstOrDefault(f => f.IsRobbed == true);
            if (currentlyRobbedfield is null)
            {
                return GameServiceResponses.SomeHowTheRobberIsMissing;
            }
            currentlyRobbedfield.IsRobbed = false;
            newRobbedField.IsRobbed = true;
            var corner = newRobbedField.Corners.FirstOrDefault(c => c.Level > 0 && c.Player.Name != game.ActivePlayer.Name);
            if (corner is not null)
            {
                var player = game.PlayerList.First(c => c.Name == corner.Player.Name);
                if (player.Inventory.GetAllResourcesCount() > 0)
                {
                    Resources stolenResource = player.Inventory.GetRandomResource();
                    player.Inventory.RemoveResource(stolenResource, 1);
                    game.ActivePlayer.Inventory.AddResource(stolenResource, 1);
                }
            }
            game.RobberNeedsMove = false;
            return GameServiceResponses.Success;
        }
    }
}
