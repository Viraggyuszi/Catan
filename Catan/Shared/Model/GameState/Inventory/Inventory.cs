using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catan.Shared.Model.GameMap;

namespace Catan.Shared.Model.GameState
{
    public class Inventory
    {
        protected Dictionary<Resources, int> inventory;
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
        public void AddResource(TerrainType terrainType, int count)
        {
            switch (terrainType)
            {
                case TerrainType.Desert:
                    break;
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
        public int GetResourceCount(Resources resource)
        {
            return inventory[resource];
        }
        public int GetAllResourcesCount()
        {
            return inventory.Values.Sum();
        }
    }
}
