using Catan.Shared.Model.GameMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MapGenerator
{
    public interface IMapGenerator
    {
        public Map GenerateMap();
    }
}
