using BLL.Services.Interfaces;
using Catan.Shared.Model.GameState;
using Catan.Shared.Request;
using Catan.Shared.Response;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations
{
    public class InMemoryDatabaseLobby : IInMemoryDatabaseLobby
    {
        private ConcurrentDictionary<Guid, Lobby> GuidLobbyPairs { get; set; }

        public InMemoryDatabaseLobby()
        {
            GuidLobbyPairs = new ConcurrentDictionary<Guid, Lobby>();
        }
        public InMemoryDatabaseLobbyResponses AddLobby(Guid guid, Lobby lobby)
        {
            return GuidLobbyPairs.TryAdd(guid, lobby) ? InMemoryDatabaseLobbyResponses.Success : InMemoryDatabaseLobbyResponses.CreateLobbyFailed;
        }
        public InMemoryDatabaseLobbyResponses RemoveLobby(Guid guid)
        {
            return GuidLobbyPairs.TryRemove(guid, out _) ? InMemoryDatabaseLobbyResponses.Success : InMemoryDatabaseLobbyResponses.RemoveLobbyFailed;
        }
        public InMemoryDatabaseLobbyResponses AddPlayerToLobby(Player player, Guid guid)
        {
            var lobby = GuidLobbyPairs.GetValueOrDefault(guid);
            if (lobby is null)
            {
                return InMemoryDatabaseLobbyResponses.InvalidLobby;
            }
            else if (lobby.Players.Count >= 4)
            {
                return InMemoryDatabaseLobbyResponses.AlreadyFull;
            }
            else if (lobby.Players.Find(p => p.Name == player.Name) is not null)
            {
                return InMemoryDatabaseLobbyResponses.AlreadyMember;
            }
            else
            {
                lobby.Players.Add(player);
                return InMemoryDatabaseLobbyResponses.Success;
            }
        }
        public InMemoryDatabaseLobbyResponses RemovePlayerFromLobby(Player player, Guid guid)
        {
            var lobby = GuidLobbyPairs.GetValueOrDefault(guid);
            if (lobby is null)
            {
                return InMemoryDatabaseLobbyResponses.InvalidLobby;
            }
            var element = lobby.Players.Find(p => p.Name == player.Name);
            if (element is null)
            {
                return InMemoryDatabaseLobbyResponses.NotAMember;
            }
            else
            {
                lobby.Players.Remove(element);
                if (lobby.Players.Count < 1)
                {
                    GuidLobbyPairs.Remove(guid, out _);
                }
                return InMemoryDatabaseLobbyResponses.Success;
            }
        }
        public List<Lobby> GetLobbies()
        {
            return GuidLobbyPairs.Values.ToList();
        }
        public Game? CreateGame(Guid guid)
        {
            var lobby = GuidLobbyPairs.GetValueOrDefault(guid);
            if (lobby is null)
            {
                return null;
            }
            Game game = new Game();
            foreach (var player in lobby.Players!)
            {
                game.PlayerList.Add(new Player(player));
            }
            Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Orange };
            int i = 0;
            foreach (var player in game.PlayerList)
            {
                player.Points = 0;
                player.Color = colors[i++].Name;
            }
            game.DLCs = lobby.DLCs;
            return game;
        }
    }
}
