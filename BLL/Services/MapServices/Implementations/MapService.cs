using BLL.Services.MapGenerator;
using BLL.Services.MapGenerator.Implementations;
using Catan.Shared.Model;
using Catan.Shared.Model.GameMap;

namespace BLL.Services.MapServices.Implementations
{
	public class MapService : IMapService
	{
		public Map GenerateMap(Dictionary<GameType, bool> DLCs)
		{
			GameType gameType;
			if (DLCs.GetValueOrDefault(GameType.Seafarer))
			{
				gameType = GameType.Seafarer;
			}
			else
			{
				gameType = GameType.Base;
			}
			return gameType switch
			{
				GameType.Base => new BaseGameMapGenerator().GenerateMap(),
				GameType.Seafarer => throw new Exception("Not implemented yet"),
				_ => throw new NotImplementedException(),
			};
		}
	}
}