using Catan.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Request
{
	public class FetchInventoryDTO
	{
		public Dictionary<Resources,int> Inventory { get; set; }
		public Dictionary<string,int> OthersInventory { get; set; }
	}
}
