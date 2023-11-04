using Catan.Shared.Model.GameMap;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.BuildInitialShipAction.Implementations
{
	public class SeafarerBuildInitialShipAction : IBuildInitialShipAction
	{
		public GameServiceResponses Execute(Game game, int edgeId, string name)
		{
			if (game.ActivePlayer.Name != name)
			{
				return GameServiceResponses.InvalidMember;
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
			if (edge.Corners[0].Id != game.LastInitialVillageId && edge.Corners[1].Id != game.LastInitialVillageId)
			{
				return GameServiceResponses.InitialRoadNotPlacedCorrectly;
			}
			if (!edge.Corners[0].Fields.Any(f => edge.Corners[1].Fields.Contains(f) && f.Type==TerrainType.Sea))
			{
				return GameServiceResponses.ShipCantBePlacedHere;
			}
			game.ActivePlayerCanPlaceInitialRoad = false;
			edge.Owner = game.ActivePlayer;
			edge.EdgeType = EdgeType.Ship;

			var inventory = game.ActivePlayer.Inventory;
			foreach (var corner in edge.Corners)
			{
				foreach (var field in corner.Fields)
				{
					if (field.Type == TerrainType.Unknown)
					{
						field.RevealField();
						inventory.AddResource(field.Type, 1);
					}
				}
			}
			return GameServiceResponses.Success;
		}

	}
}
