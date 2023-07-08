using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model
{
	public class GamePlayer : Player
	{
		public Dictionary<Resources, int> ResourcesInventory { get; set; }
		public string? connectionID;
		public GamePlayer() 
		{
			ResourcesInventory = new Dictionary<Resources, int>
			{
				{ Resources.Brick, 0 },
				{ Resources.Ore, 0 },
				{ Resources.Sheep, 0 },
				{ Resources.Wheat, 0 },
				{ Resources.Wood, 0 }
			};
		}
		public GamePlayer(Player player)
		{
			ResourcesInventory = new Dictionary<Resources, int>
			{
				{ Resources.Brick, 0 },
				{ Resources.Ore, 0 },
				{ Resources.Sheep, 0 },
				{ Resources.Wheat, 0 },
				{ Resources.Wood, 0 }
			};
			Name = player.Name;
			Color=player.Color;
			Points = player.Points;
		}
	}
}
