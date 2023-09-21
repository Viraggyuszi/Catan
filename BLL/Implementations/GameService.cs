﻿using BLL.Interfaces;
using Catan.Shared.Model;
using Catan.Shared.Request;
using Catan.Shared.Response;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
	public class GameService : IGameService
	{
		private readonly IInMemoryDatabaseGame _inMemoryDatabaseGame;
		private readonly IMapService _mapService;
		public GameService(IInMemoryDatabaseGame inMemoryDatabaseGame, IMapService mapService) 
		{
			_inMemoryDatabaseGame = inMemoryDatabaseGame;
			_mapService = mapService;
		}
		public InMemoryDatabaseGameResponses RegisterGame(Guid guid, Game game)
		{
            var newMap = _mapService.GenerateMap();
			game.GameMap= newMap;
			return _inMemoryDatabaseGame.AddGame(guid, game);
		}
		public Map? GetGameMap(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return null;
			}
            return game.GameMap;
		}
		public GameServiceResponses RegisterPlayerConnectionId(Guid guid, string name, string connectionId)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			var player = game.PlayerList.FirstOrDefault(x => x.Name == name);
			if (player is null)
			{
				return GameServiceResponses.InvalidMember;
			}
			player.connectionID = connectionId;

			if (game.PlayerList.All(p => p.connectionID is not null) && !game.AlreadyInitialized)
			{
				game.AlreadyInitialized = true;
				return GameServiceResponses.GameCanStart;
			}
			return GameServiceResponses.GameCantStart;
		}
		public int[]? RollDices(Guid guid)
		{
			var dices = new int[2];
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return null;
			}
			int sum = 0;
			for (int i = 0; i < dices.Length; i++)
			{
				dices[i] = new Random().Next(1, 7);
				sum += dices[i];
			}
			if (sum == 7)
			{
				game.RobberNeedsMove = true;
				//game.ResolveResourceCount = true;
				game.PlayersWithSevenOrMoreResources.Clear();
				foreach (var player in game.PlayerList)
				{
					if (player.ResourcesInventory.Values.Sum() >= 7)
					{
						game.PlayersWithSevenOrMoreResources.Add(player);
					}
				}
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
								var inventory = corner.Player.ResourcesInventory;
								switch (field.Type)
								{
									case TerrainType.Desert:
										throw new Exception("why does desert have a number?");
									case TerrainType.Forest:
										inventory[Resources.Wood] += corner.Level;
										break;
									case TerrainType.Mountains:
										inventory[Resources.Ore] += corner.Level;
										break;
									case TerrainType.Cropfield:
										inventory[Resources.Wheat] += corner.Level;
										break;
									case TerrainType.Grassland:
										inventory[Resources.Sheep] += corner.Level;
										break;
									case TerrainType.Quarry:
										inventory[Resources.Brick] += corner.Level;
										break;
									default:
										throw new Exception("why doesn't have a matching type?");
								}
							}
						}
					}
				}
			}
			return dices;
		}
		public List<string>? GetPlayersConnectionIdWithSevenOrMoreResources(Guid guid)
		{
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return null;
            }
			List<string> res = new List<string>();
			foreach (var player in game.PlayersWithSevenOrMoreResources)
			{
				res.Add(player.connectionID!);
			}
			return res;
		}
		public string? GetActivePlayerConnectionId(Guid guid)
		{
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return null;
            }
            return game.ActivePlayer.connectionID;
		}
		public string? GetActivePlayerName(Guid guid)
		{
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return null;
            }
            return game.ActivePlayer.Name;
		}
		public GameServiceResponses StartGame(Guid guid)
		{
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return GameServiceResponses.InvalidGame;
            }
            game.InitialRound = true;
			game.InitialRoundCount = game.PlayerList.Count * 2;
			game.ActivePlayer = game.PlayerList[new Random().Next(1, game.PlayerList.Count)];
			return GameServiceResponses.Success;
		}
		public GameServiceResponses IsInitialRound(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return game.InitialRound? GameServiceResponses.InitialRound: GameServiceResponses.NotInitialRound;
		}
		public GameServiceResponses NextTurn(Guid guid)
		{
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return GameServiceResponses.InvalidGame;
            }
			game.ActivePlayer = game.PlayerList[(game.PlayerList.IndexOf(game.ActivePlayer) + 1) % game.PlayerList.Count];
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
							var inventory = corner.Player.ResourcesInventory;
							foreach (var field in corner.Fields)
							{
								switch (field.Type)
								{
									case TerrainType.Desert:
										break;
									case TerrainType.Forest:
										inventory[Resources.Wood] += corner.Level;
										break;
									case TerrainType.Mountains:
										inventory[Resources.Ore] += corner.Level;
										break;
									case TerrainType.Cropfield:
										inventory[Resources.Wheat] += corner.Level;
										break;
									case TerrainType.Grassland:
										inventory[Resources.Sheep] += corner.Level;
										break;
									case TerrainType.Quarry:
										inventory[Resources.Brick] += corner.Level;
										break;
									default:
										throw new Exception("why doesn't have a matching type?");
								}
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
		public GameServiceResponses ClaimInitialCorner(Guid guid, int id, string name)
		{
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return GameServiceResponses.InvalidGame;
            }
            if (game.ActivePlayer.Name != name)
			{
				return GameServiceResponses.InvalidMember;
            }
			var corner = game.GameMap.CornerList.FirstOrDefault(corner => corner.Id == id);
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
			game.lastInitialVillageId = corner.Id;
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
		public GameServiceResponses ClaimInitialRoad(Guid guid, int id, string name)
		{
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return GameServiceResponses.InvalidGame;
            }
            if (game.ActivePlayer.Name != name)
			{
                return GameServiceResponses.InvalidMember;
            }
			var edge = game.GameMap.EdgeList.FirstOrDefault(edge => edge.Id == id);
			if (edge is null)
			{
                return GameServiceResponses.InvalidEdge;
            }
			if (edge.Owner.Name is not null)
			{
				return GameServiceResponses.EdgeAlreadyTaken;
            }
			if (edge.corners[0].Id != game.lastInitialVillageId && edge.corners[1].Id != game.lastInitialVillageId)
			{
                return GameServiceResponses.InitialRoadNotPlacedCorrectly;
            }
			game.ActivePlayerCanPlaceInitialRoad = false;
			edge.Owner = game.ActivePlayer;
			return GameServiceResponses.Success;
		}
        public GameServiceResponses ClaimCorner(Guid guid, int id, string name)
        {
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return GameServiceResponses.InvalidGame;
            }
            if (game.ActivePlayer.Name != name)
            {
                return GameServiceResponses.InvalidMember;
            }
            if (game.ResolveResourceCount || game.RobberNeedsMove)
            {
                return GameServiceResponses.CantPlaceCornerAtTheMoment;
            }
            var corner = game.GameMap.CornerList.FirstOrDefault(corner => corner.Id == id);
            if (corner is null)
            {
                return GameServiceResponses.InvalidCorner;
            }
            var inventory = game.ActivePlayer.ResourcesInventory;
            if (corner.Player.Name == name && corner.Level == 1)
            {
                if (inventory[Resources.Ore] < 3 || inventory[Resources.Wheat] < 2)
                {
					return GameServiceResponses.NotEnoughResourcesForUpgrade;
                }
                else
                {
                    inventory[Resources.Ore] -= 3;
                    inventory[Resources.Wheat] -= 2;
                    corner.Level = 2;
                    game.ActivePlayer.Points++;
                    if (game.ActivePlayer.Points >= 10)
                    {
                        game.GameOver = true;
                        game.Winner = new Player { Name = game.ActivePlayer.Name };
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
            if (inventory[Resources.Wood] <= 0 || inventory[Resources.Wheat] <= 0 || inventory[Resources.Sheep] <= 0 || inventory[Resources.Brick] <= 0)
            {
				return GameServiceResponses.NotEnoughResourcesForVillage;
            }
            inventory[Resources.Wood] -= 1;
            inventory[Resources.Wheat] -= 1;
            inventory[Resources.Sheep] -= 1;
            inventory[Resources.Brick] -= 1;
            corner.Player = game.ActivePlayer;
            corner.Level = 1;
            game.ActivePlayer.Points++;
            if (game.ActivePlayer.Points >= 10)
            {
                game.GameOver = true;
                game.Winner = new Player { Name = game.ActivePlayer.Name };
            }
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
        public GameServiceResponses ClaimEdge(Guid guid, int id, string name)
        {
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return GameServiceResponses.InvalidGame;
            }
            if (game.ActivePlayer.Name != name)
            {
				return GameServiceResponses.InvalidMember;
            }
            if (game.ResolveResourceCount || game.RobberNeedsMove)
            {
                return GameServiceResponses.CantPlaceEdgeAtTheMoment;
            }
            var edge = game.GameMap.EdgeList.FirstOrDefault(edge => edge.Id == id);
            if (edge is null)
            {
                return GameServiceResponses.InvalidEdge;
            }
            var inventory = game.ActivePlayer.ResourcesInventory;
            if (inventory[Resources.Wood] <= 0 || inventory[Resources.Brick] <= 0)
            {
				return GameServiceResponses.NotEnoughResourcesForRoad;
            }
            if (edge.Owner.Name is not null)
            {
				return GameServiceResponses.EdgeAlreadyTaken;
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
            inventory[Resources.Wood] -= 1;
            inventory[Resources.Brick] -= 1;
			return GameServiceResponses.Success;
        }
        public Dictionary<Resources, int>? GetPlayersInventory(Guid guid, string name)
		{
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
				return null;
            }
            var player = game.PlayerList.Find(p => p.Name == name);
			if (player is null)
			{
				return null;
			}
			return player.ResourcesInventory;
		}
		public Dictionary<string, int>? GetOtherPlayersInventory(Guid guid, string name)
		{
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return null;
            }
            Dictionary<string, int> res = new Dictionary<string, int>();
			foreach (var player in game.PlayerList)
			{
				if (player.Name != name)
				{
					var number = player.ResourcesInventory.Values.Sum();
                    res.Add(player.Name, number);
				}
			}
			return res;
		}
		public GameServiceResponses IsGameOver(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
				return GameServiceResponses.InvalidGame;
            }
			return GameServiceResponses.GameOver; //TODO why game over???
		}
		public List<Player>? GetPlayers(Guid guid)
		{
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return null;
            }
			return game.PlayerList;
		}
		public GameServiceResponses MoveRobber(Guid guid, int id, string name)
		{
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return GameServiceResponses.InvalidGame;
            }
            if (game.ActivePlayer.Name != name)
			{
                return GameServiceResponses.InvalidMember;
            }
			var newRobbedField = game.GameMap.FieldList.FirstOrDefault(f => f.Id == id);
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
				if (player.ResourcesInventory.Values.Sum() > 0)
				{
					Resources stolenResource = (Resources)new Random().Next(0, 5);
					while (player.ResourcesInventory[stolenResource] <= 0)
					{
						stolenResource = (Resources)new Random().Next(0, 5);
					}
					player.ResourcesInventory[stolenResource]--;
					game.ActivePlayer.ResourcesInventory[stolenResource]++;
				}
			}
			game.RobberNeedsMove = false;
			return GameServiceResponses.Success;
		}
	}
}
