using Catan.Shared.Model.GameMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MapGenerator.Implementations
{
	public class BaseGameMapGenerator : AbstractMapGenerator
	{
		public override Map GenerateMap()
		{
			var map = new Map();
			map.EdgeList.Clear();
			map.FieldList.Clear();
			map.CornerList.Clear();

			GenerateFields(map);
			GenerateRest(map);
			
			DisableUnavailableCornersAndEdges(map);
			return map;
		}
		protected override void GenerateFields(Map map)
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
				for (; j < fieldLists[i].Count - 1; j++)
				{
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i][j + 1], 5);
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j], 1);
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j + 1], 0);
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
			for (int i = 0; i < fieldLists[3].Count - 1; i++)
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
		
		private List<TerrainType> CreateTerrainTypesList()
		{
			var ret = new List<TerrainType>();
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

		private void DisableUnavailableCornersAndEdges(Map map)
		{
			foreach (var edge in map.EdgeList)
			{
				if (edge.Corners.Any(c => c.Fields.All(f => f.Type == TerrainType.Sea)))
				{
					edge.EdgeType = EdgeType.Unavailable;
				}
			}
			foreach (var corner in map.CornerList)
			{
				if (corner.Fields.All(f => f.Type == TerrainType.Sea))
				{
					corner.Level = -1;
				}
			}
		}

	}
}
