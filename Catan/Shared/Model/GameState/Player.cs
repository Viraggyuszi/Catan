using Catan.Shared.Model.GameState.Inventory;
using System.Text.Json.Serialization;

namespace Catan.Shared.Model.GameState
{
	public class Player
    {
        public string? Name { get; set; } //TOOD refactor proper ctor for non-nullable properties
        public string? Color { get; set; }
        public int? Points { get; set; }
        [JsonIgnore]
        public PlayerInventory Inventory { get; set; }
        [JsonIgnore]
        public CardInventory CardInventory { get; set; }
        public string? ConnectionID { get; set; }
        public int KnightForce { get; set; } = 0;
        public int LongestPath { get; set; } = 0;
        public Player()
        {
            Inventory = new PlayerInventory();
            CardInventory= new CardInventory();
        }
        public Player(Player player)
        {
            Inventory = new PlayerInventory();
            Name = player.Name;
            Color = player.Color;
            Points = player.Points;
            CardInventory = new CardInventory();
        }
    }
}
