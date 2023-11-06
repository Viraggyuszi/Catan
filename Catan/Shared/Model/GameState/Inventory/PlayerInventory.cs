using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catan.Shared.Model.GameMap;

namespace Catan.Shared.Model.GameState.Inventory
{
    public class PlayerInventory : AbstractInventory
    {
        public PlayerInventory()
        {

        }
        public Dictionary<Resources, int> GetResources()
        {
            return Inventory;
        }
        public bool HasEnoughForCard()
        {
            return Inventory[Resources.Ore] >= 1 && Inventory[Resources.Wheat] >= 1 && Inventory[Resources.Sheep] >= 1;
        }
        public void PayForCard()
        {
            Inventory[Resources.Ore] -= 1;
            Inventory[Resources.Sheep] -= 1;
            Inventory[Resources.Wheat] -= 1;
        }
        public bool HasEnoughForCityUpgrade()
        {
            return Inventory[Resources.Ore] >= 3 && Inventory[Resources.Wheat] >= 2;
        }
        public void PayForCityUpgrade()
        {
            Inventory[Resources.Ore] -= 3;
            Inventory[Resources.Wheat] -= 2;
        }
        public bool HasEnoughForVillage()
        {
            return Inventory[Resources.Brick] >= 1 && Inventory[Resources.Wheat] >= 1 && Inventory[Resources.Wood] >= 1 && Inventory[Resources.Sheep] >= 1;
        }
        public void PayForVillage()
        {
            Inventory[Resources.Brick] -= 1;
            Inventory[Resources.Wheat] -= 1;
            Inventory[Resources.Sheep] -= 1;
            Inventory[Resources.Wood] -= 1;
        }
        public bool HasEnoughForRoad()
        {
            return Inventory[Resources.Brick] >= 1 && Inventory[Resources.Wood] >= 1;
        }
        public void PayForRoad()
        {
            Inventory[Resources.Brick] -= 1;
            Inventory[Resources.Wood] -= 1;
        }
		public bool HasEnoughForShip()
		{
			return Inventory[Resources.Sheep] >= 1 && Inventory[Resources.Wood] >= 1;
		}
		public void PayForShip()
		{
			Inventory[Resources.Sheep] -= 1;
			Inventory[Resources.Wood] -= 1;
		}
		public Resources GetRandomResource()
        {
            List<Resources> list = new List<Resources>();
            foreach (var item in Inventory)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    list.Add(item.Key);
                }
            }
            int rnd = new Random().Next(0, list.Count());
            return list[rnd];
        }
        public bool HasSufficientResources(AbstractInventory _inventory)
        {
            if (_inventory.GetResourceCount(Resources.Wood) > Inventory[Resources.Wood]) return false;
            if (_inventory.GetResourceCount(Resources.Brick) > Inventory[Resources.Brick]) return false;
            if (_inventory.GetResourceCount(Resources.Ore) > Inventory[Resources.Ore]) return false;
            if (_inventory.GetResourceCount(Resources.Sheep) > Inventory[Resources.Wheat]) return false;
            if (_inventory.GetResourceCount(Resources.Wheat) > Inventory[Resources.Wheat]) return false;
            return true;
        }
        public void AddResources(AbstractInventory _inventory)
        {
            Inventory[Resources.Wood] += _inventory.GetResourceCount(Resources.Wood);
            Inventory[Resources.Brick] += _inventory.GetResourceCount(Resources.Brick);
            Inventory[Resources.Ore] += _inventory.GetResourceCount(Resources.Ore);
            Inventory[Resources.Sheep] += _inventory.GetResourceCount(Resources.Sheep);
            Inventory[Resources.Wheat] += _inventory.GetResourceCount(Resources.Wheat);
        }
        public void RemoveResources(AbstractInventory _inventory)
        {
            Inventory[Resources.Wood] -= _inventory.GetResourceCount(Resources.Wood);
            Inventory[Resources.Brick] -= _inventory.GetResourceCount(Resources.Brick);
            Inventory[Resources.Ore] -= _inventory.GetResourceCount(Resources.Ore);
            Inventory[Resources.Sheep] -= _inventory.GetResourceCount(Resources.Sheep);
            Inventory[Resources.Wheat] -= _inventory.GetResourceCount(Resources.Wheat);
        }
    }
}
