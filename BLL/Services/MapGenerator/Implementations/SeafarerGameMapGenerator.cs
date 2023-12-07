using Catan.Shared.Model.GameMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MapGenerator.Implementations
{
	public class SeafarerGameMapGenerator : AbstractMapGenerator
	{
		public override Map GenerateMap()
		{
			var map = new Map();
			map.EdgeList.Clear();
			map.FieldList.Clear();
			map.CornerList.Clear();

			GenerateFields(map);
			GenerateRest(map);


			return map;
		}
		private Field CreateFieldWithNumberAndTerrain(List<int> numbersList, List<TerrainType> terrains)
		{
			Field field = new Field();
			var terrainIndex=new Random().Next(terrains.Count);
			field.Type = terrains[terrainIndex];
			terrains.RemoveAt(terrainIndex);
			if (field.Type == TerrainType.Desert || field.Type == TerrainType.Sea)
			{
				field.Number = 0;
			}
			else
			{
				var numberIndex=new Random().Next(numbersList.Count);
				field.Number = numbersList[numberIndex];
				numbersList.RemoveAt(numberIndex);
			}
			field.IsRobbed = false;
			return field;
		}

		private void Shuffle<T>(List<T> list)
		{
			Random rng = new Random();
			int n = list.Count;

			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T temp = list[k];
				list[k] = list[n];
				list[n] = temp;
			}
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
		
		protected override void GenerateFields(Map map)
		{
			var baseIslandNumbersList = new List<int>() { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };
			List<TerrainType> BaseIslandTerrainsList = CreateTerrainTypesList();
			var fieldLists = new List<List<Field>>();
			var extraNumbersList = new List<int>();
			var extraTerrainsList = new List<TerrainType>();
			for (int i = 0; i < 3; i++)
			{
				extraTerrainsList.Add(TerrainType.Forest);
				extraTerrainsList.Add(TerrainType.Grassland);
				extraTerrainsList.Add(TerrainType.Desert);
				extraTerrainsList.Add(TerrainType.Quarry);
				extraTerrainsList.Add(TerrainType.Mountains);
				extraTerrainsList.Add(TerrainType.GoldField);
				extraTerrainsList.Add(TerrainType.Cropfield);
			}
			Shuffle(extraTerrainsList);
			Shuffle(baseIslandNumbersList);
			Shuffle(extraNumbersList);
			Shuffle(BaseIslandTerrainsList);
			for (int i = 0; i < 32; i++)
			{
				extraTerrainsList.Add(TerrainType.Sea);
			}
			for (int i = 0; i < 18; i++)
			{
				var newNumber = new Random().Next(2, 13);
				if (newNumber==7)
				{
					i--;
				}
				else
				{
					extraNumbersList.Add(newNumber);
				}
			}
			var extraFields= new List<Field>();
			var extraFieldsCount = extraTerrainsList.Count;
			for (int i = 0; i < extraFieldsCount; i++)
			{
				extraFields.Add(CreateFieldWithNumberAndTerrain(extraNumbersList, extraTerrainsList));
			}
			foreach (var eField in extraFields)
			{
				eField.HideField();
			}
			for (int i = 0; i < 9; i++)
			{
				var fieldRow = new List<Field>();
				if (i < 2)
				{
					for (int j = 0; j < 10; j++)
					{
						var index = new Random().Next(0, extraFields.Count);
						fieldRow.Add(extraFields[index]);
						extraFields.RemoveAt(index);
					}
				}
				if (i == 2 || i == 8)
				{
					for (int j = 0; j < 3; j++)
					{
						var index = new Random().Next(0, extraFields.Count);
						fieldRow.Add(extraFields[index]);
						extraFields.RemoveAt(index);
					}
					for (int j = 0; j < 4; j++)
					{
						fieldRow.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
					}
					for (int j = 0; j < 3; j++)
					{
						var index = new Random().Next(0, extraFields.Count);
						fieldRow.Add(extraFields[index]);
						extraFields.RemoveAt(index);
					}
				}
				if (i == 3 || i == 7) 
				{
					for (int j = 0; j < 2; j++)
					{
						var index = new Random().Next(0, extraFields.Count);
						fieldRow.Add(extraFields[index]);
						extraFields.RemoveAt(index);
					}
					fieldRow.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
					for (int j = 0; j < 3; j++)
					{
						fieldRow.Add(CreateFieldWithNumberAndTerrain(baseIslandNumbersList, BaseIslandTerrainsList));
					}
					fieldRow.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
					for (int j = 0; j < 3; j++)
					{
						var index = new Random().Next(0, extraFields.Count);
						fieldRow.Add(extraFields[index]);
						extraFields.RemoveAt(index);
					}
				}
				if (i == 4 || i == 6)
				{
					for (int j = 0; j < 2; j++)
					{
						var index = new Random().Next(0, extraFields.Count);
						fieldRow.Add(extraFields[index]);
						extraFields.RemoveAt(index);
					}
					fieldRow.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
					for (int j = 0; j < 4; j++)
					{
						fieldRow.Add(CreateFieldWithNumberAndTerrain(baseIslandNumbersList, BaseIslandTerrainsList));
					}
					fieldRow.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
					for (int j = 0; j < 2; j++)
					{
						var index = new Random().Next(0, extraFields.Count);
						fieldRow.Add(extraFields[index]);
						extraFields.RemoveAt(index);
					}
				}
				if (i == 5)
				{
					for (int j = 0; j < 1; j++)
					{
						var index = new Random().Next(0, extraFields.Count);
						fieldRow.Add(extraFields[index]);
						extraFields.RemoveAt(index);
					}
					fieldRow.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
					for (int j = 0; j < 2; j++)
					{
						fieldRow.Add(CreateFieldWithNumberAndTerrain(baseIslandNumbersList, BaseIslandTerrainsList));
					}
					fieldRow.Add(new Field { Number = 0, Type = TerrainType.Desert, IsRobbed = true });
					for (int j = 0; j < 2; j++)
					{
						fieldRow.Add(CreateFieldWithNumberAndTerrain(baseIslandNumbersList, BaseIslandTerrainsList));
					}
					fieldRow.Add(new Field { Number = 0, Type = TerrainType.Sea, IsRobbed = false });
					for (int j = 0; j < 2; j++)
					{
						var index = new Random().Next(0, extraFields.Count);
						fieldRow.Add(extraFields[index]);
						extraFields.RemoveAt(index);
					}
				}
				fieldLists.Add(fieldRow);
			}

			for (int i = 0; i < 8; i++)
			{
				if (i % 2 == 0) 
				{
					int j = 0;
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i][j + 1], 5);
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j], 0);
					j++;
					for (; j < fieldLists[i].Count - 1; j++)
					{
						MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i][j + 1], 5);
						MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j - 1], 1);
						MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j], 0);
					}
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j - 1], 1);
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j], 0);
				}
				if (i % 2 == 1)
				{
					int j = 0;
					for (; j < fieldLists[i].Count - 1; j++)
					{
						MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i][j + 1], 5);
						MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j], 1);
						MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j + 1], 0);
					}
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i + 1][j], 1);
				}
			}
			for (int i = 8; i < 9; i++)
			{
				int j = 0;
				for (; j < fieldLists[i].Count - 1; j++)
				{
					MakeConnectionBetweenFields(fieldLists[i][j], fieldLists[i][j + 1], 5);
				}
			}

			foreach (var fieldList in fieldLists)
			{
				foreach (var field in fieldList)
				{
					map.FieldList.Add(field);
				}
			}
		}
	}
}
