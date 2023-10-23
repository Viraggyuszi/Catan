using BLL.GameActions.BuildInitialVillageAction;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.BuildInitialVillageAction.Implementations
{
    public class BaseBuildInitialVillageAction : IBuildInitialVillageAction
    {
        public GameServiceResponses Execute(Game game, int cornerId, string name)
        {
            if (game.ActivePlayer.Name != name)
            {
                return GameServiceResponses.InvalidMember;
            }
            var corner = game.GameMap.CornerList.FirstOrDefault(corner => corner.Id == cornerId);
            if (corner is null)
            {
                return GameServiceResponses.InvalidCorner;
            }
            if (corner.Level != 0)
            {
                return GameServiceResponses.CornerAlreadyTaken;
            }
            game.ActivePlayerCanPlaceInitialVillage = false;
            corner.Player = game.ActivePlayer;
            corner.Level = 1;
            game.ActivePlayer.Points++;
            game.LastInitialVillageId = corner.Id;
            foreach (var edge in corner.Edges)
            {
                if (corner == edge.Corners[0])
                {
                    edge.Corners[1].Level = -1;
                }
                else
                {
                    edge.Corners[0].Level = -1;
                }
            }
            return GameServiceResponses.Success;
        }
    }
}
