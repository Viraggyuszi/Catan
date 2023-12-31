﻿using BLL.GameActions.BuildInitialRoadAction;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.BuildInitialRoadAction.Implementations
{
    public class BaseBuildInitialRoadAction : IBuildInitialRoadAction
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
            game.ActivePlayerCanPlaceInitialRoad = false;
            edge.Owner = game.ActivePlayer;
            return GameServiceResponses.Success;
        }
    }
}
