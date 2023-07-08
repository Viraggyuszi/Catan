using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model
{
	public class Map
	{
		public List<Field> FieldList { get; set; } = new List<Field>();
		public List<Corner> CornerList { get; set; } = new List<Corner>();
		public List<Edge> EdgeList { get; set; } = new List<Edge>();
	}
}
