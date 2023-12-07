using Catan.Shared.Model.GameMap;

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
		private int radius = 10; //TODO
        private double AngleToRad(double angle)
        {
            return (angle * Math.PI / 180);
        }
        public string GetSettlementString()
		{
            Vertex[] vertices=new Vertex[4];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].X = BaseVertex.X + radius * 0.7 * Math.Cos(AngleToRad(45 + i * 90));
                vertices[i].Y = BaseVertex.Y + radius * 0.7 * Math.Sin(AngleToRad(45 + i * 90));
            }
            string ret = vertices[0].ToString();    
            for (int i = 0; i < vertices.Length; i++)
            {
                ret += " " + vertices[(i + 1)%4].ToString() + " " + BaseVertex.ToString() + " " + vertices[i].ToString() + " " + vertices[(i + 1) % 4].ToString(); 
            }
            return ret;
        }

        public string GetCityString()
        {
			Vertex[] vertices = new Vertex[4];
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i].X = BaseVertex.X + radius * 1.3 * Math.Cos(AngleToRad(45 + i * 90));
				vertices[i].Y = BaseVertex.Y + radius * 1.3 * Math.Sin(AngleToRad(45 + i * 90));
			}
			string ret = vertices[0].ToString();
			for (int i = 0; i < vertices.Length; i++)
			{
				ret += " " + vertices[(i + 1) % 4].ToString() + " " + BaseVertex.ToString() + " " + vertices[i].ToString() + " " + vertices[(i + 1) % 4].ToString();
			}
			return ret;
		}


        public string Deprecated_GetCityString()
        {
            Vertex[][] vertices = new Vertex[3][];
            Vertex[] transpositions= new Vertex[3]; Vertex[] bases = new Vertex[3];
            transpositions[0] = new Vertex(-Math.Sqrt(5), -Math.Sqrt(5)); bases[0] = new Vertex(BaseVertex.X + transpositions[0].X, BaseVertex.Y + transpositions[0].Y);
            transpositions[1] = new Vertex(0, 5); bases[1] = new Vertex(BaseVertex.X + transpositions[1].X, BaseVertex.Y + transpositions[1].Y);
            transpositions[2] = new Vertex(Math.Sqrt(5), -Math.Sqrt(5)); bases[2] = new Vertex(BaseVertex.X + transpositions[2].X, BaseVertex.Y + transpositions[2].Y);
            vertices[0] = new Vertex[4];
            vertices[1] = new Vertex[4];
            vertices[2] = new Vertex[4];
            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = 0; j < vertices[i].Length; j++)
                {
                    vertices[i][j].X = bases[i].X + (radius / 2) * Math.Cos(AngleToRad(45 + j * 90));
                    vertices[i][j].Y = bases[i].Y + (radius / 2) * Math.Sin(AngleToRad(45 + j * 90));
                }
            }
            string ret = vertices[0][0].ToString();
            for (int i = 0; i < vertices[0].Length; i++)
            {
                ret += " " + vertices[0][(i + 1) % 4].ToString() + " " + bases[0].ToString() + " " + vertices[0][i].ToString() 
                    + " " + vertices[0][(i + 1) % 4].ToString();
            }
            for (int i = 0; i < vertices[1].Length; i++)
            {
                ret += " " + vertices[1][(i + 2) % 4].ToString() + " " + bases[1].ToString() + " " + vertices[1][(i + 1) % 4].ToString()
                    + " " + vertices[1][(i + 2) % 4].ToString();
            }
            for (int i = 0; i < vertices[2].Length; i++)
            {
                ret += " " + vertices[2][(i + 1) % 4].ToString() + " " + bases[2].ToString() + " " + vertices[2][(i + 0) % 4].ToString()
                    + " " + vertices[2][(i + 1) % 4].ToString();
            }
            return ret;
        }
	}
}
