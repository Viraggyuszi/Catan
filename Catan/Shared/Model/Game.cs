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
		public List<GamePlayer> PlayerList { get; set; } = new List<GamePlayer>();
		public Map GameMap { get; set; } = new Map();
		public GamePlayer ActivePlayer { get; set; } = new GamePlayer();
		public bool RobberNeedsMove = false;
		public bool ResolveResourceCount = false;
		public List<Player> PlayersWithSevenOrMore { get; set; } = new List<Player>();
		
		public bool InitialRound = false;
		public int InitialRoundCount = 0;
		public bool AlreadyInitialized = false;
		public bool ActivePlayerCanPlaceInitialVillage = false;
		public bool ActivePlayerCanPlaceInitialRoad = false;
		public int lastInitialVillageId = 0;
		public bool GameOver = false;
		public Player? Winner;
	}
}
