using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model
{
	public class Inventory
	{
		private Dictionary<Resources, int> inventory;
        public Inventory()
        {
            inventory = new Dictionary<Resources, int>

			{
				{ Resources.Brick, 0 },
                { Resources.Ore, 0 },
                { Resources.Sheep, 0 },
                { Resources.Wheat, 0 },
                { Resources.Wood, 0 }
			};
		}
        public Dictionary<Resources, int> GetResources()
        {
            return inventory;
        }
        public int GetCount()
        {
            return inventory.Values.Sum();
        }
        public void AddResource(TerrainType terrainType, int count)
		{
			switch (terrainType)
			{
				case TerrainType.Desert:
					throw new Exception("why does desert have a number?");
				case TerrainType.Forest:
					inventory[Resources.Wood] += count;
					break;
				case TerrainType.Mountains:
					inventory[Resources.Ore] += count;
					break;
				case TerrainType.Cropfield:
					inventory[Resources.Wheat] += count;
					break;
				case TerrainType.Grassland:
					inventory[Resources.Sheep] += count;
					break;
				case TerrainType.Quarry:
					inventory[Resources.Brick] += count;
					break;
				default:
					throw new Exception("why doesn't have a matching type?");
			}
        }
		public void AddResource(Resources resource, int count)
		{
			inventory[resource] += count;
		}
		public void RemoveResource(Resources resource, int count)
		{
			inventory[resource] -= count;
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
		public Resources RobOneResource()
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
			RemoveResource(list[rnd], 1);
			return list[rnd];
		}
	}
}
