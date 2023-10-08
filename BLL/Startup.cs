using BLL.Services.Implementations;
using BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class Startup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<DatabaseService>();
			services.AddScoped<IGameService, GameService>();
			services.AddScoped<IMapService, MapService>();
			services.AddScoped<ILobbyService, LobbyService>();
			services.AddSingleton<IInMemoryDatabaseLobby, InMemoryDatabaseLobby>();
			services.AddSingleton<IInMemoryDatabaseGame, InMemoryDatabaseGame>();
		}
	}
}
