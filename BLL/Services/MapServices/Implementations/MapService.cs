using BLL.Services.MapGenerator;
using BLL.Services.MapGenerator.Implementations;
using Catan.Shared.Model;
using Catan.Shared.Model.GameMap;

namespace BLL.Services.MapServices.Implementations
{
	public class MapService : IMapService
	{
		public Map GenerateMap(GameType gameType)
		{
			return gameType switch
			{
				GameType.Base => new BaseGameMapGenerator().GenerateMap(),
				GameType.Seafarer => throw new Exception("Not implemented yet"),
				_ => throw new NotImplementedException(),
			};
		}
	}
}