using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState
{
    public class Lobby
    {
        public Guid GuID { get; set; }
        public string? Name { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public Dictionary<GameType,bool> DLCs { get; set; }=new Dictionary<GameType,bool>();
    }
}
