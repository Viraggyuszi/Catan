using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Catan.Shared.Model.GameState;

namespace Catan.Shared.Model.GameMap
{
    public class Corner
    {
        public int Id { get; set; }
        public Player Player { get; set; }
        public int Level { get; set; }
		[JsonIgnore]
		public List<Field> Fields { get; set; }
		public List<Edge> Edges { get; set; }
        public Corner()
        {
            Fields = new List<Field>();
            Edges = new List<Edge>();
            Player = new Player();
        }

        public override string ToString()
        {
            return Id + "";
        }
    }
}
