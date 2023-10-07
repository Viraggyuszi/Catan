﻿using Catan.Shared.Model;
using Catan.Shared.Request;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IGameService
    {
        public InMemoryDatabaseGameResponses RegisterGame(Guid guid, Game game);
        public Map? GetGameMap(Guid guid);
        public GameServiceResponses RegisterPlayerConnectionId(Guid guid, string name, string connectionId);
        public int[]? RollDices(Guid guid);
        public List<string>? GetPlayersConnectionIdWithSevenOrMoreResources(Guid guid);
        public string? GetActivePlayerConnectionId(Guid guid);
        public string? GetActivePlayerName(Guid guid);
        public GameServiceResponses StartGame(Guid guid);
        public GameServiceResponses IsInitialRound(Guid guid);
        public GameServiceResponses EndPlayerTurn(Guid guid, string name);
        public GameServiceResponses ClaimInitialCorner(Guid guid, int id, string name);
        public GameServiceResponses ClaimInitialRoad(Guid guid, int id, string name);
        public GameServiceResponses ClaimCorner(Guid guid, int id, string name);
        public GameServiceResponses ClaimEdge(Guid guid, int id, string name);
        public Dictionary<Resources, int>? GetPlayersInventory(Guid guid, string name);
        public Dictionary<string, int>? GetOtherPlayersInventory(Guid guid, string name);
        public GameServiceResponses IsGameOver(Guid guid);
        public List<Player>? GetPlayers(Guid guid);
        public GameServiceResponses MoveRobber(Guid guid, int id, string name);

        public List<TradeOffer>? GetTradeOffers(Guid guid);
        public GameServiceResponses RegisterTradeOffer(Guid guid, TradeOffer offer);
        public GameServiceResponses RegisterTradeOfferWithBank(Guid guid, TradeOffer offer);
        public GameServiceResponses AcceptTradeOffer(Guid guid, TradeOffer offer, string name);

        public GameServiceResponses ThrowResources(Guid guid, Inventory thrownResources, string name);
    }
}
