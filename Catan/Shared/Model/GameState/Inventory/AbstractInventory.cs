using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catan.Shared.Model.GameMap;

namespace Catan.Shared.Model.GameState.Inventory
{
    public class AbstractInventory
    {
        public Dictionary<Resources, int> Inventory { get; set; }
        public AbstractInventory()
        {
            Inventory = new Dictionary<Resources, int>
            {
                { Resources.Brick, 0 },
                { Resources.Ore, 0 },
                { Resources.Sheep, 0 },
                { Resources.Wheat, 0 },
                { Resources.Wood, 0 }
            };
        }
        public void AddResource(TerrainType terrainType, int count)
        {
            switch (terrainType)
            {
                case TerrainType.Desert:
                    break;
				case TerrainType.Unknown:
					break;
				case TerrainType.GoldField:
					break;
				case TerrainType.Sea:
                    break;
                case TerrainType.Forest:
                    Inventory[Resources.Wood] += count;
                    break;
                case TerrainType.Mountains:
                    Inventory[Resources.Ore] += count;
                    break;
                case TerrainType.Cropfield:
                    Inventory[Resources.Wheat] += count;
                    break;
                case TerrainType.Grassland:
                    Inventory[Resources.Sheep] += count;
                    break;
                case TerrainType.Quarry:
                    Inventory[Resources.Brick] += count;
                    break;
                default:
                    throw new Exception("why doesn't have a matching type?");
            }
        }
        public void AddResource(Resources resource, int count)
        {
            Inventory[resource] += count;
        }
        public void RemoveResource(Resources resource, int count)
        {
            Inventory[resource] -= count;
        }
        public int GetResourceCount(Resources resource)
        {
            return Inventory[resource];
        }
        public int GetAllResourcesCount()
        {
            return Inventory.Values.Sum();
        }
    }
}
