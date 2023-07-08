using BLL.Implementations;
using BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
	public static class Startup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<DatabaseService>();
			services.AddSingleton<IGameService, GameService>();
			services.AddScoped<IMapService, MapService>();
			services.AddScoped<ILobbyService, LobbyService>();
		}
	}
}
