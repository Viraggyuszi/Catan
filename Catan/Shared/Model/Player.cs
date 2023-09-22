using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model
{
	public class Player
	{
		public string? Name { get; set; } //TOOD refactor proper ctor for non-nullable properties
		public string? Color { get; set; }
		public int? Points { get; set; }
        public Dictionary<Resources, int> ResourcesInventory { get; set; }
        public string? connectionID;
        public Player()
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
        public Player(Player player)
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
            Color = player.Color;
            Points = player.Points;
        }
    }
}
