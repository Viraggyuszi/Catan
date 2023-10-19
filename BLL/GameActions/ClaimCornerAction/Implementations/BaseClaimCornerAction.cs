﻿using BLL.GameActions.ClaimCornerAction;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.ClaimCornerAction.Implementations
{
    public class BaseClaimCornerAction : IClaimCornerAction
    {
        public GameServiceResponses Execute(Game game, int cornerId, string name)
        {
            if (game.ActivePlayer.Name != name)
            {
                return GameServiceResponses.ActorIsNotActivePlayer;
            }
            if (game.ResolveResourceCount || game.RobberNeedsMove)
            {
                return GameServiceResponses.CantPlaceCornerAtTheMoment;
            }
            var corner = game.GameMap.CornerList.FirstOrDefault(corner => corner.Id == cornerId);
            if (corner is null)
            {
                return GameServiceResponses.InvalidCorner;
            }
            if (corner.Fields.All(f=>f.Type==Catan.Shared.Model.GameMap.TerrainType.Sea))
            {
				return GameServiceResponses.CantPlaceCornerToSea;
			}
            var inventory = game.ActivePlayer.Inventory;
            if (corner.Player.Name == name && corner.Level == 1)
            {
                if (!inventory.HasEnoughForUpgrade())
                {
                    return GameServiceResponses.NotEnoughResourcesForUpgrade;
                }
                else
                {
                    inventory.PayForUpgrade();
                    corner.Level = 2;
                    game.ActivePlayer.Points++;
                    if (game.ActivePlayer.Points >= 10)
                    {
                        game.GameOver = true;
                        game.Winner = game.ActivePlayer;
                        return GameServiceResponses.GameOver;
                    }
                }
            }
            if (corner.Level != 0)
            {
                return GameServiceResponses.CornerAlreadyTaken;
            }
            if (!corner.Edges.Any(e => e.Owner.Name == name))
            {
                return GameServiceResponses.CantPlaceCornerWithoutPath;
            }
            if (!inventory.HasEnoughForVillage())
            {
                return GameServiceResponses.NotEnoughResourcesForVillage;
            }
            inventory.PayForVillage();
            corner.Player = game.ActivePlayer;
            corner.Level = 1;
            game.ActivePlayer.Points++;
            if (game.ActivePlayer.Points >= 10)
            {
                game.GameOver = true;
                game.Winner = game.ActivePlayer;
            }
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
