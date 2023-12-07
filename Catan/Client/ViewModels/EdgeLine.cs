using Catan.Shared.Model.GameMap;
using System.ComponentModel;

namespace Catan.Client.ViewModels
{
    public class EdgeLine
	{
		public Vertex[] endPoints;
		public Edge Edge { get; set; }
		public EdgeLine(Vertex A, Vertex B, Edge edge)
		{
			Vertex vector = new Vertex() { X = A.X - B.X, Y = A.Y - B.Y };
			double length=Math.Sqrt(vector.X*vector.X + vector.Y*vector.Y);
			Vertex normalisedVector= new Vertex() { X=vector.X/length, Y=vector.Y/length };
			Vertex halfway = new Vertex() { X = (A.X + B.X) / 2, Y = (A.Y + B.Y) / 2 };
			Vertex normal = new Vertex() { X = normalisedVector.Y, Y = -normalisedVector.X };
			length *= 2 * 0.16667;
			int width = 4;
			if (edge.EdgeType == EdgeType.Ship)
			{
				width *= 2;
				endPoints = new Vertex[6];
				endPoints[0] = new Vertex()
				{
					X = halfway.X + normalisedVector.X * length,
					Y = halfway.Y + normalisedVector.Y * length
				};
				endPoints[1] = new Vertex()
				{
					X = halfway.X + normalisedVector.X * length / 3 + normal.X * width,
					Y = halfway.Y + normalisedVector.Y * length / 3 + normal.Y * width
				};
				endPoints[2] = new Vertex()
				{
					X = halfway.X - normalisedVector.X * length / 3 + normal.X * width,
					Y = halfway.Y - normalisedVector.Y * length / 3 + normal.Y * width
				};
				endPoints[3] = new Vertex()
				{
					X = halfway.X - normalisedVector.X * length,
					Y = halfway.Y - normalisedVector.Y * length
				};
				endPoints[4] = new Vertex()
				{
					X = halfway.X - normalisedVector.X * length / 3 - normal.X * width,
					Y = halfway.Y - normalisedVector.Y * length / 3 - normal.Y * width
				};
				endPoints[5] = new Vertex()
				{
					X = halfway.X + normalisedVector.X * length / 3 - normal.X * width,
					Y = halfway.Y + normalisedVector.Y * length / 3 - normal.Y * width
				};
			}
			else
			{
				endPoints = new Vertex[4];
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
			}
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
