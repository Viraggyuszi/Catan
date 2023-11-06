using Catan.Shared.Model.GameMap;
using Catan.Shared.Model.GameState.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Request
{
	public class FetchCardInventoryDTO
	{
		public Dictionary<CardType, int> CardInventory { get; set; }
		public Dictionary<string, int> OthersInventory { get; set; }
	}
}
