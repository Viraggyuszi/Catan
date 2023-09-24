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
        public Inventory Inventory { get; set; }

        public string? ConnectionID { get; set; }
        public Player()
        {
            Inventory = new Inventory();
        }
        public Player(Player player)
        {
            Inventory=new Inventory();
            Name = player.Name;
            Color = player.Color;
            Points = player.Points;
        }
    }
}
