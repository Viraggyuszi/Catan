using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catan.Shared.Model.GameMap;
using Catan.Shared.Model.GameState.Dice;
using Catan.Shared.Model.GameState.Inventory;

namespace Catan.Shared.Model.GameState
{
    public class Game
    {
        public List<Player> PlayerList { get; set; } = new List<Player>();
        public Map GameMap { get; set; } = new Map();
        public Player ActivePlayer { get; set; } = new Player();
        public bool RobberNeedsMove { get; set; } = false;
        public bool ResolveResourceCount { get; set; } = false;
        public List<Player> PlayersWithSevenOrMoreResources { get; set; } = new List<Player>();
        public bool InitialRound { get; set; } = false;
        public int InitialRoundCount { get; set; } = 0;
        public bool AlreadyInitialized { get; set; } = false;
        public bool HaveToRollDices { get; set; } = false;
        public bool ActivePlayerCanPlaceInitialVillage { get; set; } = false;
        public bool ActivePlayerCanPlaceInitialRoad { get; set; } = false;
        public int LastInitialVillageId { get; set; } = 0;
        public int PointsToWin { get; set; } = 10;
        public Player Winner { get; set; } = new Player();
        public List<TradeOffer> TradeOfferList { get; set; } = new List<TradeOffer>();

        public Dictionary<GameType, bool> DLCs { get; set; } = new Dictionary<GameType, bool>();
        public List<IDice> Dices { get; set; } = new List<IDice>();
        public List<CardType> Cards { get; set; }
        public int FreeRoadBuilding { get; set; } = 0;
        public bool ActivePlayerChooseResource { get; set; } = false;
        public Game()
        {
            Cards=new List<CardType>();
            for (int i = 0; i < 48; i++)
            {
                Cards.Add(CardType.Knight);
            }
            for (int i = 0; i < 4; i++)
            {
                Cards.Add(CardType.RoadBuilder);
                Cards.Add(CardType.ExtraPoint);
            }
        }

        public Player BiggestKnightForce { get; set; } = new Player();
        public Player LongestPath { get; set; } = new Player();
    }
}
