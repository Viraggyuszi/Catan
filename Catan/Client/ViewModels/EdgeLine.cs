using Catan.Shared.Model;

namespace Catan.Client.ViewModels
{
	public class EdgeLine
	{
		public Vertex[] endPoints = new Vertex[4];
		public Edge Edge { get; set; }
		public EdgeLine(Vertex A, Vertex B, Edge edge)
		{
			Vertex vector = new Vertex() { X = A.X - B.X, Y = A.Y - B.Y };
			double length=Math.Sqrt(vector.X*vector.X + vector.Y*vector.Y);
			Vertex normalisedVector= new Vertex() { X=vector.X/length, Y=vector.Y/length };
			length *= 3*0.125;
			Vertex halfway = new Vertex() { X = (A.X + B.X) / 2, Y = (A.Y + B.Y) / 2 };
			Vertex normal = new Vertex() { X = normalisedVector.Y, Y = -normalisedVector.X };
			int width = 4;
			endPoints[0] = new Vertex()
			{
				X = halfway.X + normalisedVector.X * length + normal.X * width, 
				Y = halfway.Y + normalisedVector.Y * length + normal.Y * width 
			};
			endPoints[1] = new Vertex()
			{
				X = halfway.X + normalisedVector.X * length - normal.X * width,
				Y = halfway.Y + normalisedVector.Y * length - normal.Y * width
			};
			endPoints[2] = new Vertex()
			{
				X = halfway.X - normalisedVector.X * length - normal.X * width,
				Y = halfway.Y - normalisedVector.Y * length - normal.Y * width
			};
			endPoints[3] = new Vertex()
			{
				X = halfway.X - normalisedVector.X * length + normal.X * width,
				Y = halfway.Y - normalisedVector.Y * length + normal.Y * width
			};
			Edge = edge;
		}
		public string VerticesToString()
		{
			string ret = endPoints[0].ToString();
			for (int i = 1; i < endPoints.Length; i++)
			{
				ret += " " + endPoints[i].ToString();
			}
			return ret;
		}
	}
}
