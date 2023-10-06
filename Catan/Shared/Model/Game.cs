using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model
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
		public bool ActivePlayerCanPlaceInitialVillage { get; set; } = false;
		public bool ActivePlayerCanPlaceInitialRoad { get; set; } = false;
		public int lastInitialVillageId { get; set; } = 0;
		public bool GameOver { get; set; } = false;
		public Player? Winner { get; set; }
		public List<TradeOffer> TradeOfferList { get; set; } = new List<TradeOffer>();
	}
}
