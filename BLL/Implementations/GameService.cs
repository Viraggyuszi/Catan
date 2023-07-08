using BLL.Interfaces;
using Catan.Shared.Model;
using Catan.Shared.Request;
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
		public ConcurrentDictionary<Guid, Game> GuidGamePairs { get; set; }
		public ConcurrentDictionary<Guid, Lobby> GuidLobbyPairs { get; set; }
		public GameService()
		{
			GuidGamePairs = new ConcurrentDictionary<Guid, Game>();
			GuidLobbyPairs = new ConcurrentDictionary<Guid, Lobby>();
		}
		public Lobby CreateLobby(string name)
		{
			foreach (var pair in GuidLobbyPairs)
			{
				if (pair.Value.Players.Count < 1)
				{
					GuidLobbyPairs.TryRemove(pair.Key, out _);
				}
			}
			var guid = Guid.NewGuid();
			var NewItem = new Lobby();
			NewItem.Name = name;
			NewItem.Players = new List<Player>();
			NewItem.GuID = guid;
			GuidLobbyPairs.TryAdd(guid, NewItem);
			return NewItem;
		}
		public List<Lobby> GetLobbies()
		{
			var ret = new List<Lobby>();
			foreach (var guidlobbypair in GuidLobbyPairs)
			{
				ret.Add(guidlobbypair.Value);
			}
			return ret;
		}
		public ApiDTO<string> AddPlayerToLobby(Player player, Guid guid)
		{
			var lobby = GuidLobbyPairs.GetValueOrDefault(guid);
			if (lobby is null)
			{
				return new ApiDTO<string>() { Success = false, Value = "Not valid lobby" };
			}
			else if (lobby.Players.Count >= 4)
			{
				return new ApiDTO<string>() { Success = false, Value = "Lobby is already full" };
			}
			else if (lobby.Players.Find(p => p.Name == player.Name) is not null)
			{
				return new ApiDTO<string>() { Success = false, Value = "You're already a member of the lobby" };
			}
			else
			{
				GuidLobbyPairs.Remove(guid, out _);
				lobby.Players.Add(player);
				GuidLobbyPairs.TryAdd(guid, lobby);
				return new ApiDTO<string>() { Success = true, Value = guid.ToString() };
			}
		}
		public ApiDTO<string> RemovePlayerFromLobby(Player player, Guid guid)
		{
			var lobby = GuidLobbyPairs.GetValueOrDefault(guid);
			if (lobby is null)
			{
				return new ApiDTO<string>() { Success = false, Value = "Not valid lobby" };
			}
			var element = lobby.Players.Find(p => p.Name == player.Name);
			if (element is null)
			{
				return new ApiDTO<string>() { Success = false, Value = "You're not a member of the lobby" };
			}
			else
			{
				GuidLobbyPairs.Remove(guid, out _);
				lobby.Players!.Remove(element);
				if (lobby.Players.Count > 0)
				{
					GuidLobbyPairs.TryAdd(guid, lobby);
				}
				return new ApiDTO<string>() { Success = true, Value = "ok" };
			}
		}
		public ApiDTO<string> RegisterGame(Guid guid, Map map)
		{
			var lobby = GuidLobbyPairs.GetValueOrDefault(guid);
			if (lobby is null)
			{
				return new ApiDTO<string>() { Success = false, Value = "Not valid lobby" };
			}
			Game game = new Game();
			foreach (var player in lobby.Players!)
			{
				game.PlayerList.Add(new GamePlayer(player));
			}
			Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Orange };
			int i = 0;
			foreach (var player in game.PlayerList)
			{
				player.Points = 0;
				player.Color = colors[i++].Name;
			}
			game.GameMap = map;
			if (GuidGamePairs.TryAdd(guid, game))
			{
				GuidLobbyPairs.Remove(guid, out _);
				return new ApiDTO<string>() { Success = true, Value = "ok" };
			}
			else
			{
				return new ApiDTO<string>() { Success = false, Value = "Something went wrong, try again later" };
			}
		}
		public Game GetGame(Guid guid)
		{
			return GuidGamePairs.GetValueOrDefault(guid)!;
		}
		public bool RegisterPlayerConnectionId(Guid guid, string name, string connectionId)
		{
			if (!GuidGamePairs.TryGetValue(guid, out var game))
			{
				throw new Exception("Not valid guid");
			}
			var player = game.PlayerList.FirstOrDefault(x => x.Name == name) ?? throw new Exception("Not valid playername");
			player.connectionID = connectionId;

			if (game.PlayerList.All(p => p.connectionID is not null) && !game.AlreadyInitialized)
			{
				game.AlreadyInitialized = true;
				return true;
			}
			return false;
		}
		public ApiDTO<int[]> RollDices(Guid guid)
		{
			var dices = new int[2];
			var game = GuidGamePairs.GetValueOrDefault(guid);
			if (game is null)
			{
				return new ApiDTO<int[]>() { Success = false, Value = null };
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
				foreach (var player in game.PlayerList)
				{
					if (CountResources(player.ResourcesInventory) >= 7)
					{
						if (game.PlayersWithSevenOrMore.FirstOrDefault(p => p.Name == player.Name) is null)
						{
							game.PlayersWithSevenOrMore.Add(player);
						}
					}
				}
				return new ApiDTO<int[]>() { Success = true, Value = dices };
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
			return new ApiDTO<int[]>() { Success = true, Value = dices };
		}
		private int CountResources(Dictionary<Resources, int> inventory)
		{
			int count = 0;
			//inventory.Values.Count;
			count += inventory[Resources.Brick];
			count += inventory[Resources.Ore];
			count += inventory[Resources.Sheep];
			count += inventory[Resources.Wheat];
			count += inventory[Resources.Wood];
			return count;
		}
		public ApiDTO<string[]> GetPlayersConnectionIdWithSevenOrMoreResources(Guid guid)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				return new ApiDTO<string[]>() { Success = false, Value = null };
			}
			List<string> res = new List<string>();
			foreach (var player in game.PlayerList)
			{
				if (CountResources(player.ResourcesInventory) >= 7)
				{
					res.Add(player.connectionID!);
				}
			}
			return new ApiDTO<string[]>() { Success = true, Value = res.ToArray() };
		}
		public string GetActivePlayerConnectionId(Guid guid)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				return null!;
			}
			return game.ActivePlayer.connectionID!;
		}
		public Player GetActivePlayer(Guid guid)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				return null!;
			}
			return game.ActivePlayer;
		}
		public void StartGame(Guid guid)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			GuidGamePairs[guid].InitialRound = true;
			GuidGamePairs[guid].InitialRoundCount = game.PlayerList.Count * 2;
			GuidGamePairs[guid].ActivePlayer = game.PlayerList[new Random().Next(1, game.PlayerList.Count)];
		}
		public bool IsInitialRound(Guid guid)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			return game.InitialRound;
		}
		public void NextTurn(Guid guid)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			int index = game.PlayerList.IndexOf(game.ActivePlayer);
			game.ActivePlayer = game.PlayerList[(index + 1) % game.PlayerList.Count];
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
		}
		public void ClaimInitialCorner(Guid guid, int id, string name)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			if (game.ActivePlayer.Name != name)
			{
				throw new Exception("Not valid username");
			}
			var corner = game.GameMap.CornerList.FirstOrDefault(corner => corner.Id == id);
			if (corner is null)
			{
				throw new Exception("Not valid Corner ID");
			}
			if (corner.Level != 0)
			{
				throw new Exception("You can't take that corner");
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
		}
		public void ClaimInitialRoad(Guid guid, int id, string name)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			if (game.ActivePlayer.Name != name)
			{
				throw new Exception("Not valid username");
			}
			var edge = game.GameMap.EdgeList.FirstOrDefault(edge => edge.Id == id);
			if (edge is null)
			{
				throw new Exception("Not valid edge ID");
			}
			if (edge.Owner.Name is not null)
			{
				throw new Exception("Edge is already taken");
			}
			if (edge.corners[0].Id != game.lastInitialVillageId && edge.corners[1].Id != game.lastInitialVillageId)
			{
				throw new Exception("You must place your road next to your village");
			}
			game.ActivePlayerCanPlaceInitialRoad = false;
			edge.Owner = game.ActivePlayer;
		}
		public Dictionary<Resources, int> GetPlayersInventory(Guid guid, string name)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			var player = game.PlayerList.Find(p => p.Name == name);
			if (player is null)
			{
				throw new Exception("Not valid username");
			}
			return player.ResourcesInventory;
		}
		public Dictionary<string, int> GetOtherPlayersInventory(Guid guid, string name)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			Dictionary<string, int> res = new Dictionary<string, int>();
			foreach (var player in game.PlayerList)
			{
				if (player.Name != name)
				{
					var number = CountResources(player.ResourcesInventory);
					res.Add(player.Name!, number);
				}
			}
			return res;
		}

		public void ClaimCorner(Guid guid, int id, string name)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			if (game.ActivePlayer.Name != name)
			{
				throw new Exception("You're not the current player");
			}
			if (game.ResolveResourceCount)
			{
				throw new Exception("You must resolve the resources first");
			}
			if (game.RobberNeedsMove)
			{
				throw new Exception("You must move the robber first");
			}
			var corner = game.GameMap.CornerList.FirstOrDefault(corner => corner.Id == id);
			if (corner is null)
			{
				throw new Exception("Not valid Corner ID");
			}
			var inventory = game.ActivePlayer.ResourcesInventory;
			if (corner.Player.Name == name && corner.Level == 1)
			{
				if (inventory[Resources.Ore] < 3 || inventory[Resources.Wheat] < 2)
				{
					throw new Exception("Insufficient resources to upgrade village");
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
					}
				}
				return;
			}
			if (corner.Level != 0)
			{
				throw new Exception("You can't take that corner");
			}
			if (!corner.Edges.Any(e => e.Owner.Name == name))
			{
				throw new Exception("You must have a road to the village");
			}
			if (inventory[Resources.Wood] <= 0 || inventory[Resources.Wheat] <= 0 || inventory[Resources.Sheep] <= 0 || inventory[Resources.Brick] <= 0)
			{
				throw new Exception("Insufficient resources to build village");
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
		}

		public void ClaimEdge(Guid guid, int id, string name)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			if (game.ActivePlayer.Name != name)
			{
				throw new Exception("You're not the current player");
			}
			if (game.ResolveResourceCount)
			{
				throw new Exception("You must resolve the resources first");
			}
			if (game.RobberNeedsMove)
			{
				throw new Exception("You must move the robber first");
			}
			var inventory = game.ActivePlayer.ResourcesInventory;
			if (inventory[Resources.Wood] <= 0 || inventory[Resources.Brick] <= 0)
			{
				throw new Exception("Insufficient resouces to build road");
			}
			var edge = game.GameMap.EdgeList.FirstOrDefault(edge => edge.Id == id);
			if (edge is null)
			{
				throw new Exception("Not valid edge ID");
			}

			if (edge.Owner.Name is not null)
			{
				throw new Exception("Edge is already taken");
			}
			if (edge.corners[0].Player.Name != name && edge.corners[1].Player.Name != name)
			{
				var corner1 = edge.corners[0];
				var corner2 = edge.corners[1];
				if (!corner1.Edges.Any(e => e.Owner.Name == name) && !corner2.Edges.Any(e => e.Owner.Name == name))
				{
					throw new Exception("You must place your road next to your village, or road");
				}
			}
			game.ActivePlayerCanPlaceInitialRoad = false;
			edge.Owner = game.ActivePlayer;
			inventory[Resources.Wood] -= 1;
			inventory[Resources.Brick] -= 1;
		}

		public bool IsGameOver(Guid guid)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			return game.GameOver;
		}

		public List<Player> GetPlayers(Guid guid)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			var res = new List<Player>();
			foreach (var player in game.PlayerList)
			{
				res.Add(new Player() { Name = player.Name, Color = player.Color, Points = player.Points });
			}
			return res;
		}

		public void MoveRobber(Guid guid, int id, string name)
		{
			var game = GuidGamePairs[guid];
			if (game is null)
			{
				throw new Exception("Not valid guid");
			}
			if (game.ActivePlayer.Name != name)
			{
				throw new Exception("Not valid username");
			}
			var newRobbedField = game.GameMap.FieldList.FirstOrDefault(f => f.Id == id);
			if (newRobbedField is null)
			{
				throw new Exception("Not valid field ID");
			}
			var currentlyRobbedfield = game.GameMap.FieldList.FirstOrDefault(f => f.IsRobbed == true);
			if (currentlyRobbedfield is null)
			{
				throw new Exception("Where's the robber?");
			}
			currentlyRobbedfield.IsRobbed = false;
			newRobbedField.IsRobbed = true;
			var corner = newRobbedField.Corners.FirstOrDefault(c => c.Level > 0 && c.Player.Name != game.ActivePlayer.Name);
			if (corner is not null)
			{
				var rand = new Random().Next(0, 5);
				Resources stolenResource;
				var player = game.PlayerList.First(c => c.Name == corner.Player.Name);
				if (player.ResourcesInventory[Resources.Wood] > 0)
				{
					stolenResource = Resources.Wood;
				}
				else if (player.ResourcesInventory[Resources.Wheat] > 0)
				{
					stolenResource = Resources.Wheat;
				}
				else if (player.ResourcesInventory[Resources.Sheep] > 0)
				{
					stolenResource = Resources.Sheep;
				}
				else if (player.ResourcesInventory[Resources.Ore] > 0)
				{
					stolenResource = Resources.Ore;
				}
				else if (player.ResourcesInventory[Resources.Brick] > 0)
				{
					stolenResource = Resources.Brick;
				}
				else
				{
					return;
				}
				player.ResourcesInventory[stolenResource]--;
				game.ActivePlayer.ResourcesInventory[stolenResource]++;
			}
			game.RobberNeedsMove = false;
		}
	}
}
