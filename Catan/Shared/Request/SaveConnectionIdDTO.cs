using Catan.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Request
{
	public class SaveConnectionIdDTO
	{
		public string? ConnectionId { get; set; }
		public Guid Guid { get; set; }
		public string? Name { get; set; }

	}
}
