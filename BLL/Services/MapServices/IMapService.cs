using Catan.Shared.Model;
using Catan.Shared.Model.GameMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MapServices
{
    public interface IMapService
    {
        public Map GenerateMap(Dictionary<GameType,bool> DLCs);
    }
}
