using Catan.Shared.Model;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.MoveRobberAction
{
	public interface IMoveRobberAction
	{
		public GameServiceResponses Execute(Game game, int fieldId, string name);
	}
}
