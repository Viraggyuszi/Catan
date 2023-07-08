using Catan.Shared.Model;

namespace Catan.Client.ViewModels
{
	public class CornerCircle
	{
		public Vertex BaseVertex { get; set; }
		public Corner Corner { get; set; }

		public CornerCircle(Vertex baseVertex, Corner corner)
		{
			BaseVertex = baseVertex;
			Corner = corner;
		}
	}
}
