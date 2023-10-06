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
		public bool HasEnoughResourcesForTradeOffer(Inventory _inventory)
		{
			if (_inventory.GetResourceCount(Resources.Wood) > inventory[Resources.Wood]) return false;
			if (_inventory.GetResourceCount(Resources.Brick) > inventory[Resources.Brick]) return false;
			if (_inventory.GetResourceCount(Resources.Ore) > inventory[Resources.Ore]) return false;
			if (_inventory.GetResourceCount(Resources.Sheep) > inventory[Resources.Wheat]) return false;
			if (_inventory.GetResourceCount(Resources.Wheat) > inventory[Resources.Wheat]) return false;
			return true;
		}
		public void AddResources(Inventory _inventory)
		{
			inventory[Resources.Wood] += _inventory.GetResourceCount(Resources.Wood);
			inventory[Resources.Brick] += _inventory.GetResourceCount(Resources.Brick);
			inventory[Resources.Ore] += _inventory.GetResourceCount(Resources.Ore);
			inventory[Resources.Sheep] += _inventory.GetResourceCount(Resources.Sheep);
			inventory[Resources.Wheat] += _inventory.GetResourceCount(Resources.Wheat);
		}
		public void RemoveResources(Inventory _inventory)
		{
			inventory[Resources.Wood] -= _inventory.GetResourceCount(Resources.Wood);
			inventory[Resources.Brick] -= _inventory.GetResourceCount(Resources.Brick);
			inventory[Resources.Ore] -= _inventory.GetResourceCount(Resources.Ore);
			inventory[Resources.Sheep] -= _inventory.GetResourceCount(Resources.Sheep);
			inventory[Resources.Wheat] -= _inventory.GetResourceCount(Resources.Wheat);
		}
	}
}
