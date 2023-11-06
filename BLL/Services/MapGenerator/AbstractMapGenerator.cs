using Catan.Shared.Model.GameMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MapGenerator
{
    public abstract class AbstractMapGenerator
    {
        public abstract Map GenerateMap();
		protected abstract void GenerateFields(Map map);
		protected virtual void MakeConnectionBetweenFields(Field FieldA, Field FieldB, int connectionFromFieldA)
		{
			FieldA.Neighbours[connectionFromFieldA] = FieldB;
			FieldB.Neighbours[(connectionFromFieldA + 3) % 6] = FieldA;
		}
		protected virtual void GenerateRest(Map map)
		{
			GenerateCorners(map);
			ResolveNeighboursCorners(map);
			GenerateEdges(map);
			ResolveNeighboursEdges(map);
			GenerateIds(map);
		}
		protected virtual void GenerateCorners(Map map)
		{
			int id = 0;
			foreach (Field field in map.FieldList)
			{
				for (int i = 0; i < field.Corners.Length; i++)
				{
					Corner corner = new Corner();
					corner.Fields.Add(field);
					corner.Id = id++;
					field.Corners[i] = corner;
					map.CornerList.Add(corner);
				}
			}
		}
		protected virtual void MergeCorners(ref Corner A, ref Corner B, Map map)
		{
			map.CornerList.Remove(A);
			map.CornerList.Remove(B);
			Corner MergedCorner = new Corner();
			foreach (Field field in A.Fields)
			{
				if (!MergedCorner.Fields.Contains(field))
				{
					MergedCorner.Fields.Add(field);
				}
			}
			foreach (Field field in B.Fields)
			{
				if (!MergedCorner.Fields.Contains(field))
				{
					MergedCorner.Fields.Add(field);
				}
			}
			MergedCorner.Id = A.Id;
			Corner tmpA = A, tmpB = B;
			foreach (var field in map.FieldList)
			{
				for (int i = 0; i < field.Corners.Length; i++)
				{
					if (field.Corners[i] == tmpA || field.Corners[i] == tmpB)
					{
						field.Corners[i] = MergedCorner;
					}
				}
			}
			map.CornerList.Add(MergedCorner);
		}
		protected virtual void ResolveNeighboursCorners(Map map)
		{
			foreach (Field field in map.FieldList)
			{
				for (int j = 0; j < field.Neighbours.Length; j++)
				{
					if (field.Neighbours[j] != null)
					{
						if (!field.Corners[j].Equals(field.Neighbours[j].Corners[(j + 4) % 6]))
						{
							MergeCorners(ref field.Corners[j], ref field.Neighbours[j].Corners[(j + 4) % 6], map);
						}
						if (!field.Corners[(j + 1) % 6].Equals(field.Neighbours[j].Corners[(j + 3) % 6]))
						{
							MergeCorners(ref field.Corners[(j + 1) % 6], ref field.Neighbours[j].Corners[(j + 3) % 6], map);
						}
					}
				}
			}
		}
		protected virtual void GenerateEdges(Map map)
		{
			int id = 0;
			foreach (Field field in map.FieldList)
			{
				for (int i = 0; i < 6; i++)
				{
					Edge edge = new Edge();
					field.Corners[i].Edges.Add(edge);
					field.Corners[(i + 1) % 6].Edges.Add(edge);
					edge.Corners[0] = field.Corners[i];
					edge.Corners[1] = field.Corners[(i + 1) % 6];
					edge.Id = id++;
					field.Edges[i] = edge;
					map.EdgeList.Add(edge);
				}
			}
		}
		protected virtual void ResolveNeighboursEdges(Map map)
		{
			foreach (Field field in map.FieldList)
			{
				for (int j = 0; j < field.Neighbours.Length; j++)
				{
					if (field.Neighbours[j] != null)
					{
						if (field.Edges[j] != field.Neighbours[j].Edges[(j + 3) % 6])
						{
							Edge mergedEdge = new Edge();
							mergedEdge.Corners[0] = field.Corners[j];
							mergedEdge.Corners[1] = field.Corners[(j + 1) % 6];
							mergedEdge.Id = field.Edges[j].Id;
							map.EdgeList.Remove(field.Edges[j]);
							map.EdgeList.Remove(field.Neighbours[j].Edges[(j + 3) % 6]);

							field.Corners[j].Edges.Remove(field.Edges[j]);
							field.Corners[(j + 1) % 6].Edges.Remove(field.Edges[j]);
							field.Corners[j].Edges.Remove(field.Neighbours[j].Edges[(j + 3) % 6]);
							field.Corners[(j + 1) % 6].Edges.Remove(field.Neighbours[j].Edges[(j + 3) % 6]);
							field.Corners[j].Edges.Add(mergedEdge);
							field.Corners[(j + 1) % 6].Edges.Add(mergedEdge);

							field.Edges[j] = mergedEdge;
							field.Neighbours[j].Edges[(j + 3) % 6] = mergedEdge;


							map.EdgeList.Add(mergedEdge);
						}
					}
				}
			}
		}
		protected virtual void GenerateIds(Map map)
		{
			int Id = 0;
			foreach (var field in map.FieldList)
			{
				field.Id = Id++;
			}
		}
	}
}
