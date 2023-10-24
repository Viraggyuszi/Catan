using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState.Inventory
{
	public class CardInventory
	{
		Dictionary<CardType,int> Inventory = new Dictionary<CardType,int>();
		public CardInventory() 
		{
			Inventory.Clear();
			Inventory.Add(CardType.Knight, 0);
			Inventory.Add(CardType.RoadBuilder, 0);
			Inventory.Add(CardType.ExtraPoint, 0);
		}
		public bool HasCard(CardType type)
		{
			return Inventory[type] >= 1;
		}
		public void AddCard(CardType type)
		{
			Inventory[type] += 1;
		}
		public void RemoveCard(CardType type)
		{
			Inventory[type] -= 1;
		}
	}
}
