using System.Globalization;

namespace Catan.Client.ViewModels
{
	public struct Vertex
	{
		private double x;
		private double y;
		public double X { get => x; set => this.x = value; }
		public double Y { get => y; set => this.y = value; }

		public Vertex(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return x.ToString(CultureInfo.InvariantCulture) + "," + y.ToString(CultureInfo.InvariantCulture);
		}
	}
}
