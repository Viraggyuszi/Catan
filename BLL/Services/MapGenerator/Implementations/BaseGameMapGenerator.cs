using Catan.Shared.Model.GameMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MapGenerator.Implementations
{
	public class BaseGameMapGenerator : IMapGenerator
	{
		public Map GenerateMap()
		{
			Map map = new Map();
			map.EdgeList.Clear();
			map.FieldList.Clear();
			map.CornerList.Clear();

			GenerateFields(map);
			GenerateCorners(map);
			ResolveNeighboursCorners(map);
			GenerateEdges(map);
			ResolveNeighboursEdges(map);

			GenerateIds(map);

			return map;
		}
		private Field CreateFieldWithNumberAndTerrain(List<int> numbersList, List<TerrainType> terrains, int index)
		{
			Field field = new Field();
			field.Number = numbersList[index];
			numbersList.RemoveAt(index);
			field.Type = terrains[index];
			terrains.RemoveAt(index);
			field.IsRobbed = false;
			return field;
		}
		private void GenerateFields(Map map)
		{
			var numbersList = new List<int>() { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };
			List<TerrainType> terrains = CreateTerrainTypesList();
			var fieldLists = new List<List<Field>>();
			for (int i = 0; i < 7; i++)
			{
				var tmp = new List<Field>();
				if (i == 0 || i == 6) 
				{
					for (int j = 0; j < 4; j++)
					{
						tmp.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
					}
				}
				if (i == 1 || i == 5) 
				{
					tmp.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
					for (int j = 0; j < 3; j++)
					{
						tmp.Add(CreateFieldWithNumberAndTerrain(numbersList, terrains, new Random().Next(numbersList.Count)));
					}
					tmp.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
				}
				if (i == 2 || i == 4)
				{
					tmp.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
					for (int j = 0; j < 4; j++)
					{
						tmp.Add(CreateFieldWithNumberAndTerrain(numbersList, terrains, new Random().Next(numbersList.Count)));
					}
					tmp.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
				}
				if (i == 3)
				{
					tmp.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
					for (int j = 0; j < 2; j++)
					{
						tmp.Add(CreateFieldWithNumberAndTerrain(numbersList, terrains, new Random().Next(numbersList.Count)));
					}
					tmp.Add(new Field { Number = 0, Type = TerrainType.Desert, IsRobbed = true });
					for (int j = 0; j < 2; j++)
					{
						tmp.Add(CreateFieldWithNumberAndTerrain(numbersList, terrains, new Random().Next(numbersList.Count)));
					}
					tmp.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
				}
				fieldLists.Add(tmp);
			}
			for (int i = 0; i < 3; i++)
			{
				int j = 0;
				for (;j < fieldLists[i].Count-1; j++)
				{
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i][j + 1], 5);
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i+1][j], 1);
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i+1][j + 1], 0);
				}
				MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j], 1);
				MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j + 1], 0);
			}
			for (int i = 6; i > 3; i--)
			{
				int j = fieldLists[i].Count - 1;
				for (; j > 0; j--)
				{
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i][j - 1], 2);
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i - 1][j], 3);
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i - 1][j + 1], 4);
				}
				MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i - 1][j], 3);
				MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i - 1][j + 1], 4);
			}
			for (int i = 0; i < fieldLists[3].Count-1; i++)
			{
				MakeConnectionBetweenFields(fieldLists[3][i], fieldLists[3][i + 1], 5);
			}
			foreach (var fieldList in fieldLists)
			{
				foreach (var field in fieldList)
				{
					map.FieldList.Add(field);
				}
			}
		}
		private List<TerrainType> CreateTerrainTypesList()
		{
			List<TerrainType> ret = new List<TerrainType>();
			for (int i = 0; i < 3; i++)
			{
				ret.Add(TerrainType.Mountains);
				ret.Add(TerrainType.Grassland);
				ret.Add(TerrainType.Quarry);
				ret.Add(TerrainType.Cropfield);
				ret.Add(TerrainType.Forest);
			}
			ret.Add(TerrainType.Grassland);
			ret.Add(TerrainType.Cropfield);
			ret.Add(TerrainType.Forest);
			return ret;
		}
		private void MakeConnectionBetweenFields(Field FieldA, Field FieldB, int connectionFromFieldA)
		{
			FieldA.Neighbours[connectionFromFieldA] = FieldB;
			FieldB.Neighbours[(connectionFromFieldA + 3) % 6] = FieldA;
		}
		private void GenerateCorners(Map map)
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
		private void MergeCorners(ref Corner A, ref Corner B, Map map)
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
		private void ResolveNeighboursCorners(Map map)
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
		private void GenerateEdges(Map map)
		{
			int id = 0;
			foreach (Field field in map.FieldList)
			{
				for (int i = 0; i < 6; i++)
				{
					Edge edge = new Edge();
					field.Corners[i].Edges.Add(edge);
					field.Corners[(i + 1) % 6].Edges.Add(edge);
					edge.corners[0] = field.Corners[i];
					edge.corners[1] = field.Corners[(i + 1) % 6];
					edge.Id = id++;
					field.Edges[i] = edge;
					map.EdgeList.Add(edge);
				}
			}
		}
		private void ResolveNeighboursEdges(Map map)
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
							mergedEdge.corners[0] = field.Corners[j];
							mergedEdge.corners[1] = field.Corners[(j + 1) % 6];
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
		private void GenerateIds(Map map)
		{
			int Id = 0;
			foreach (var field in map.FieldList)
			{
				field.Id = Id++;
			}
		}
	}
}
