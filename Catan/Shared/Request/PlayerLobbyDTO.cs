using Catan.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Request
{
	public class PlayerLobbyDTO
	{
		public Actor? Actor { get; set; }
		public Lobby? Lobby { get; set; }
	}
}
