﻿using Catan.Shared.Model.GameMap;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.BuildRoadAction.Implementations
{
	public class SeafarerBuildRoadAction : IBuildRoadAction
	{
		public GameServiceResponses Execute(Game game, int edgeId, string name)
		{
			if (game.ActivePlayer.Name != name)
			{
				return GameServiceResponses.InvalidMember;
			}
			if (game.HaveToRollDices)
			{
				return GameServiceResponses.RollDicesFirst;
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
			if (edge.Corners.Any(c => c.Fields.All(f => f.Type == TerrainType.Sea)))
			{
				return GameServiceResponses.CantPlaceEdgeToSea;
			}
			if (edge.Corners[0].Player.Name != name && edge.Corners[1].Player.Name != name)
			{
				var corner1 = edge.Corners[0];
				var corner2 = edge.Corners[1];
				if (!corner1.Edges.Any(e => e.Owner.Name == name && e.EdgeType == EdgeType.Road) && !corner2.Edges.Any(e => e.Owner.Name == name && e.EdgeType == EdgeType.Road))
				{
					return GameServiceResponses.CantPlaceCornerWithoutPath;
				}
			}
			var inventory = game.ActivePlayer.Inventory;
			if (game.FreeRoadBuilding > 0)
			{
				game.FreeRoadBuilding -= 1;
				edge.Owner = game.ActivePlayer;
				edge.EdgeType = EdgeType.Road;
				return GameServiceResponses.Success;
			}
			if (!inventory.HasEnoughForRoad())
			{
				return GameServiceResponses.NotEnoughResourcesForRoad;
			}
			edge.Owner = game.ActivePlayer;
			edge.EdgeType = EdgeType.Road;
			inventory.PayForRoad();
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
