using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState
{
    public class Player
    {
        public string? Name { get; set; } //TOOD refactor proper ctor for non-nullable properties
        public string? Color { get; set; }
        public int? Points { get; set; }
        [JsonIgnore]
        public PlayerInventory Inventory { get; set; }

        public string? ConnectionID { get; set; }
        public Player()
        {
            Inventory = new PlayerInventory();
        }
        public Player(Player player)
        {
            Inventory = new PlayerInventory();
            Name = player.Name;
            Color = player.Color;
            Points = player.Points;
        }
    }
}
