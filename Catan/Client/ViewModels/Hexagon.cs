using Catan.Shared.Model.GameMap;

namespace Catan.Client.ViewModels
{
    public class Hexagon
	{
		public double Radius { get; set; }
		public Vertex Base { get; set; }
		public Vertex[] vertices;

		public Field Field { get; set; }

		public Hexagon(double x0, double y0, double radius, Field field)
		{
			Radius = radius;
			Base = new Vertex(x0, y0);
			vertices = new Vertex[6];
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i].X = x0 + radius * Math.Cos(AngleToRad(30 + i * 60));
				vertices[i].Y = y0 + radius * Math.Sin(AngleToRad(30 + i * 60));
			}
			Field = field;
		}

		public string getFieldString()
		{
			return Field.ToString();
		}
		private double AngleToRad(double angle)
		{
			return (angle * Math.PI / 180);
		}

		public string VerticesToString()
		{
			string ret = vertices[0].ToString();
			for (int i = 1; i < vertices.Length; i++)
			{
				ret += " " + vertices[i].ToString();
			}
			return ret;
		}

		public string GetVerticeString()
		{
			return "";
		}
	}
}
