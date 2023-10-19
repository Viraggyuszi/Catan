using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Catan.Shared.Model.GameState;

namespace Catan.Shared.Model.GameMap
{
    public class Edge
    {
        public int Id { get; set; }
        public Player Owner { get; set; } = new Player();
		[JsonIgnore]
		public Corner[] Corners { get; set; } = new Corner[2];
        public EdgeType EdgeType { get; set; } = EdgeType.Road;
        public override string ToString()
        {
            return "sarkok: " + Corners[0].Id + " ; " + Corners[1].Id;
        }
    }
}
