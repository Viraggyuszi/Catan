using Catan.Shared.Model;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.EndTurnAction
{
	public interface IEndTurnAction
	{
		public GameServiceResponses Execute(Game game, string name);
	}
}
