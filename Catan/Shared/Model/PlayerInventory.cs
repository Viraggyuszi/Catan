using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model
{
	public class PlayerInventory : Inventory
	{
        public PlayerInventory()
        {
			
		}
        public Dictionary<Resources, int> GetResources()
        {
            return inventory;
        }
        public int GetCount()
        {
            return inventory.Values.Sum();
        }
		public bool HasEnoughForUpgrade()
		{
			return inventory[Resources.Ore]>=3 && inventory[Resources.Wheat]>=2;
		}
		public void PayForUpgrade()
		{
			inventory[Resources.Ore] -= 3;
			inventory[Resources.Wheat] -= 2;
		}
		public bool HasEnoughForVillage()
		{
			return inventory[Resources.Brick] >= 1 && inventory[Resources.Wheat] >= 1 && inventory[Resources.Wood] >= 1 && inventory[Resources.Sheep] >= 1;
		}
		public void PayForVillage()
		{
			inventory[Resources.Brick] -= 1;
			inventory[Resources.Wheat] -= 1;
			inventory[Resources.Sheep] -= 1;
			inventory[Resources.Wood] -= 1;
		}
		public bool HasEnoughForRoad()
		{
			return inventory[Resources.Brick] >= 1 && inventory[Resources.Wood] >= 1;
		}
		public void PayForRoad()
		{
			inventory[Resources.Brick] -= 1;
			inventory[Resources.Wood] -= 1;
		}
		public Resources GetRandomResource()
		{
			List<Resources> list = new List<Resources>();
			foreach (var item in inventory)
			{
				for (int i = 0; i < item.Value; i++)
				{
					list.Add(item.Key);
				}
			}
			int rnd = new Random().Next(0, list.Count());
			return list[rnd];
		}
	}
}
