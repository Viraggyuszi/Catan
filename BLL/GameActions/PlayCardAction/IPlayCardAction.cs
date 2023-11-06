using Catan.Shared.Model.GameState;
using Catan.Shared.Model.GameState.Inventory;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.PlayCardAction
{
	public interface IPlayCardAction
	{
		public GameServiceResponses Execute(Game game, CardType card, string name);
	}
}
