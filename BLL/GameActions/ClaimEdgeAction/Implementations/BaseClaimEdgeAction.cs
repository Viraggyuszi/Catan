using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.ClaimEdgeAction.Implementations
{
    public class BaseClaimEdgeAction : IClaimEdgeAction
	{
		public GameServiceResponses Execute(Game game, int edgeId, string name)
		{
			if (game.ActivePlayer.Name != name)
			{
				return GameServiceResponses.InvalidMember;
			}
			if (game.ResolveResourceCount || game.RobberNeedsMove)
			{
				return GameServiceResponses.CantPlaceEdgeAtTheMoment;
			}
			var edge = game.GameMap.EdgeList.FirstOrDefault(edge => edge.Id == edgeId);
			if (edge is null)
			{
				return GameServiceResponses.InvalidEdge;
			}
			if (edge.Owner.Name is not null)
			{
				return GameServiceResponses.EdgeAlreadyTaken;
			}
			var inventory = game.ActivePlayer.Inventory;
			if (!inventory.HasEnoughForRoad())
			{
				return GameServiceResponses.NotEnoughResourcesForRoad;
			}
			if (edge.corners[0].Player.Name != name && edge.corners[1].Player.Name != name)
			{
				var corner1 = edge.corners[0];
				var corner2 = edge.corners[1];
				if (!corner1.Edges.Any(e => e.Owner.Name == name) && !corner2.Edges.Any(e => e.Owner.Name == name))
				{
					return GameServiceResponses.CantPlaceCornerWithoutPath;
				}
			}
			game.ActivePlayerCanPlaceInitialRoad = false;
			edge.Owner = game.ActivePlayer;
			inventory.PayForRoad();
			return GameServiceResponses.Success;
		}
	}
}
