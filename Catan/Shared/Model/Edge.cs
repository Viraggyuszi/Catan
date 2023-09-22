using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catan.Shared.Model
{
	public class Edge
	{
		public int Id { get; set; }
		public Player Owner { get; set; } = new Player();
		
		public Corner[] corners { get; set; } = new Corner[2];

		public override string ToString()
		{
			return "sarkok: " + corners[0].Id + " ; " + corners[1].Id;
		}
	}
}
