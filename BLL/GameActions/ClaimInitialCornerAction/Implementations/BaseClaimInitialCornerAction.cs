using Catan.Shared.Model;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.ClaimInitialCornerAction.Implementations
{
	public class BaseClaimInitialCornerAction : IClaimInitialCornerAction
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
				if (corner == edge.corners[0])
				{
					edge.corners[1].Level = -1;
				}
				else
				{
					edge.corners[0].Level = -1;
				}
			}
			return GameServiceResponses.Success;
		}
	}
}
